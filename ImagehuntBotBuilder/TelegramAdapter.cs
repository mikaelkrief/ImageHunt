using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;

namespace ImageHuntBotBuilder
{
    public class TelegramAdapter : BotAdapter, IAdapterIntegration
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly IImageWebService _imageWebService;
        private readonly ILogger<TelegramAdapter> _logger;
        private readonly IMapper _mapper;
        private readonly ITelegramBotClient _telegramBotClient;

        public TelegramAdapter(
            IMapper mapper,
            ILogger<TelegramAdapter> logger,
            ITelegramBotClient telegramBotClient,
            IImageWebService imageWebService,
            IConfiguration configuration,
            ICredentialProvider credentialProvider,
            HttpClient httpClient)
        {
            _mapper = mapper;
            _logger = logger;
            _telegramBotClient = telegramBotClient;
            _imageWebService = imageWebService;
            _configuration = configuration;
            _httpClient = httpClient;
        }

        public virtual async Task<InvokeResponse> ProcessActivityAsync(
            string authHeader,
            Activity activity,
            BotCallbackHandler callback,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(activity.Type))
                activity = _mapper.Map<Activity>(activity.Properties.ToObject<Update>());

            switch (activity.Type)
            {
                case ActivityTypes.Message:
                    await DownloadPictureAsync(activity);
                    break;
            }

            using (var turnContext = new TurnContext(this, activity))
            {
                await RunPipelineAsync(turnContext, callback, cancellationToken).ConfigureAwait(false);
                return null;
            }
        }

        private async Task DownloadPictureAsync(Activity activity)
        {
            if (activity.Attachments != null && activity.Attachments.Any())
                foreach (var attachment in activity.Attachments)
                    if (!string.IsNullOrEmpty(attachment.ContentUrl))
                        switch (attachment.ContentType)
                        {
                            case string s when s.StartsWith(ImageHuntActivityTypes.Image):
                            {
                                // Try to download the image
                                using (var request =
                                    new HttpRequestMessage(HttpMethod.Get, new Uri(attachment.ContentUrl)))
                                {
                                    var response = await _httpClient.SendAsync(request);
                                    using (var contentStream = await response.Content.ReadAsStreamAsync())
                                    {
                                        var bytes = new byte[contentStream.Length];
                                        await contentStream.ReadAsync(bytes, 0, (int)contentStream.Length);
                                        attachment.Content = bytes;
                                        activity.Type = ImageHuntActivityTypes.Image;
                                    }
                                }
                            }

                                break;
                            case string s when s.StartsWith("telegram"):
                            {
                                using (var stream = new MemoryStream())
                                {
                                    var fileInfo =
                                        await _telegramBotClient.GetInfoAndDownloadFileAsync(
                                            attachment.ContentUrl,
                                            stream);
                                    stream.Seek(0, SeekOrigin.Begin);
                                    var imageId = await _imageWebService.UploadImage(stream);
                                    activity.Type = ImageHuntActivityTypes.Image;
                                    activity.Attachments = new List<Attachment>
                                    {
                                        new Attachment(ImageHuntActivityTypes.Image, content: imageId)
                                    };
                                }
                            }

                                break;
                        }
        }

        public override async Task<ResourceResponse[]> SendActivitiesAsync(
            ITurnContext turnContext,
            Activity[] activities,
            CancellationToken cancellationToken)
        {
            if (turnContext == null)
                throw new ArgumentNullException(nameof(turnContext));
            if (activities == null)
                throw new ArgumentNullException(nameof(activities));
            if (activities.Length == 0)
                throw new ArgumentException(
                    "Expecting one or more activities, but the array was empty.",
                    nameof(activities));

            var responses = new ResourceResponse[activities.Length];
            for (var index = 0; index < activities.Length; index++)
            {
                var activity = activities[index];
                var response = default(ResourceResponse);
                switch (activity.ChannelId)
                {
                    case "emulator":
                        var connectorClient =
                            turnContext.TurnState.Get<IConnectorClient>(typeof(IConnectorClient).FullName);
                        response = await connectorClient.Conversations
                            .SendToConversationAsync(activity, cancellationToken).ConfigureAwait(false);
                        if (response == null) response = new ResourceResponse(activity.Id ?? string.Empty);

                        break;
                    case "telegram":
                        var chatId = Convert.ToInt64(activity.Conversation.Id);
                        switch (activity.Type)
                        {
                            case ActivityTypes.Message:

                                Message telegramMessage = null;
                                try
                                {
                                    telegramMessage = await _telegramBotClient.SendTextMessageAsync(
                                        chatId,
                                        activity.Text);
                                }
                                catch (ApiRequestException e)
                                {
                                    _logger.LogError(e, "Error while sending message to a group");
                                }
                                finally
                                {
                                    response = new ResourceResponse(telegramMessage?.MessageId.ToString());
                                }

                                break;
                            case ImageHuntActivityTypes.Leave:
                                await _telegramBotClient.LeaveChatAsync(
                                    chatId,
                                    cancellationToken);
                                break;
                            case ImageHuntActivityTypes.GetInviteLink:
                                var inviteLink = await ExtractInviteLinkAsync(chatId, cancellationToken);
                                activity.Attachments = new List<Attachment> {new Attachment(contentUrl: inviteLink) };
                                break;

                            case ImageHuntActivityTypes.RenameChat:
                                await _telegramBotClient.SetChatTitleAsync(chatId, activity.Text, cancellationToken);
                                break;
                            case ImageHuntActivityTypes.Location:
                                var location = activity.Attachments.First().Content as GeoCoordinates;
                                await _telegramBotClient.SendLocationAsync(
                                    chatId,
                                    (float)location.Latitude.Value,
                                    (float)location.Longitude.Value);
                                break;
                            case ImageHuntActivityTypes.Wait:
                                var delay = (int)activity.Attachments.First().Content;
                                Task.Delay(delay * 1000).ContinueWith(t => WaitAsync(turnContext, activity));
                                break;
                        }

                        if (response == null) response = new ResourceResponse(activity.Id ?? string.Empty);

                        break;
                }

                responses[index] = response;
            }

            return responses;
        }

        private async Task WaitAsync(ITurnContext context, IActivity activity)
        {
        }

        public override async Task<ResourceResponse> UpdateActivityAsync(
            ITurnContext turnContext,
            Activity activity,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override async Task DeleteActivityAsync(
            ITurnContext turnContext,
            ConversationReference reference,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private async Task<string> ExtractInviteLinkAsync(ChatId chatId, CancellationToken cancellationToken)
        {
            var inviteLink = await _telegramBotClient.ExportChatInviteLinkAsync(
                chatId,
                cancellationToken);
            return inviteLink;
        }
    }
}