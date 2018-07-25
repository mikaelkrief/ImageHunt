using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using Telegram.Bot;

namespace ImageHuntTelegramBot.Dialogs
{
    public class ReceiveDocumentDialog : AbstractDialog, IReceiveDocumentDialog
    {
        private readonly ITeamWebService _teamWebService;
        private readonly ITelegramBotClient _telegramBotClient;

        public ReceiveDocumentDialog(ITeamWebService teamWebService, ITelegramBotClient telegramBotClient, ILogger<ReceiveDocumentDialog> logger) : base(logger)
        {
            _teamWebService = teamWebService;
            _telegramBotClient = telegramBotClient;
        }
        public override async Task Begin(ITurnContext turnContext)
        {
            var state = turnContext.GetConversationState<ImageHuntState>();
            if (state.Status != Status.Started)
            {
                LogInfo<ImageHuntState>(turnContext, "Attempt to record image before start!");
                await turnContext.ReplyActivity($"Vous ne pouvez m'envoyer de images qu'après le début de la chasse!");
                await turnContext.End();
                return;
            }

            if (state.GameId == 0 ||
                state.TeamId == 0)
            {
                LogInfo<ImageHuntState>(turnContext, "Game not initialized!");

                await turnContext.ReplyActivity(
          $"La chasse n'a pas été correctement initalisée, veuillez demander de l'assistance à l'orga");
                await turnContext.End();
                return;
            }

            if (turnContext.Activity.Document == null)
            {
                LogInfo<ImageHuntState>(turnContext, "No document received!");

                await turnContext.ReplyActivity(
                $"Aucun document ne m'est envoyé, veuillez recommencer");
                await turnContext.End();
                return;
            }

            var document = turnContext.Activity.Document;
            if (document.MimeType != "image/jpeg")
            {
                LogInfo<ImageHuntState>(turnContext, "Document is not image");

                await turnContext.ReplyActivity(
                $"Vous ne m'avez pas envoyé une image, je ne peux accepter d'autres documents");
                await turnContext.End();
                return;
            }
            byte[] imageBytes = null;

            using (var stream = new MemoryStream())
            {
                await _telegramBotClient.GetInfoAndDownloadFileAsync(document.FileId, stream);
                if (stream.Length == 0)
                {
                    LogInfo<ImageHuntState>(turnContext, "Problem while downloading image from Telegram");

                    await turnContext.ReplyActivity(
                    $"Un problème est apparu dans l'image que vous m'avez envoyé.");
                    await turnContext.End();
                    return;
                }
                imageBytes = new byte[stream.Length];
                stream.Read(imageBytes, 0, (int)stream.Length);
                var uploadRequest = new UploadImageRequest()
                {
                    GameId = state.GameId,
                    TeamId = state.TeamId,
                    Latitude = state.CurrentLatitude,
                    Longitude = state.CurrentLongitude,
                    ImageName = turnContext.Activity.Text
                };
                uploadRequest.FormFile = new FormFile(stream, 0, stream.Length, "formFile", document.FileName);
                await _teamWebService.UploadImage(uploadRequest);

            }

            await base.Begin(turnContext);
            var activity = new Activity()
            {
                ActivityType = ActivityType.Message,
                ChatId = turnContext.ChatId,
                Text = "Votre image a bien été téléchargée, un validateur l'examinera pour vous attribuer les points"
            };
            LogInfo<ImageHuntState>(turnContext, "Image uploaded");

            await turnContext.ReplyActivity(activity);
            await turnContext.End();

        }

        public override string Command => "/uploaddocument";
    }
}