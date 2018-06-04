using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Threading.Tasks;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using Microsoft.Extensions.Logging;
using Telegram.Bot;

namespace ImageHuntTelegramBot.Dialogs
{
  public class ReceiveImageDialog : AbstractDialog, IReceiveImageDialog
  {
    private readonly ITeamWebService _teamWebService;
    private readonly ITelegramBotClient _telegramBotClient;

    public ReceiveImageDialog(ITeamWebService teamWebService, ITelegramBotClient telegramBotClient, ILogger logger) : base(logger)
    {
      _teamWebService = teamWebService;
      _telegramBotClient = telegramBotClient;
    }

    public override async Task Begin(ITurnContext turnContext)
    {
      var state = turnContext.GetConversationState<ImageHuntState>();
      if (state.GameId == 0 ||
          state.TeamId == 0 ||
          state.CurrentLatitude == 0.0 ||
          state.CurrentLongitude == 0.0)
      {
        var errorMessage = $"La chasse n'a pas été correctement initalisée ou je ne sais pas où vous êtes, veuillez demander de l'assistance à l'orga";
        await turnContext.ReplyActivity(errorMessage);
        _logger.LogWarning(errorMessage);
        await turnContext.End();
        return;
      }
      byte[] imageBytes = null;
      var photoSizes = turnContext.Activity.Pictures;
      var biggestPhoto = photoSizes.OrderByDescending(p => p.FileSize).First();
      using (Stream stream = new MemoryStream())
      {
        var fileInfo = await _telegramBotClient.GetInfoAndDownloadFileAsync(biggestPhoto.FileId, stream);
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
        uploadRequest.FormFile = new FormFile(stream, 0, stream.Length, "formFile", "image.jpg");
        await _teamWebService.UploadImage(uploadRequest);
        _logger.LogInformation($"Image {turnContext.Activity.Pictures.First().FileId} had been uploaded");
      }

      await base.Begin(turnContext);
      var activity = new Activity() { ActivityType = ActivityType.Message, ChatId = turnContext.ChatId, Text = "Votre image a bien été téléchargée, un validateur l'examinera pour vous attribuer les points" };
      await turnContext.ReplyActivity(activity);
      await turnContext.End();
    }
  }
}
