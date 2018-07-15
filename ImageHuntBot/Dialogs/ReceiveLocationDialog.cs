using System.Threading.Tasks;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Extensions.Logging;

namespace ImageHuntTelegramBot.Dialogs
{
  public class ReceiveLocationDialog : AbstractDialog, IReceiveLocationDialog
  {
      private readonly IActionWebService _actionWebService;

      public override async Task Begin(ITurnContext turnContext)
    {
      var state = turnContext.GetConversationState<ImageHuntState>();
      state.CurrentLatitude = turnContext.Activity.Location.Latitude;
      state.CurrentLongitude = turnContext.Activity.Location.Longitude;
        
      _logger.LogInformation($"Received position: [lat:{state.CurrentLatitude}, lng:{state.CurrentLongitude}");
      await base.Begin(turnContext);
        var logPositionRequest = new LogPositionRequest()
        {
            GameId = state.GameId,
            TeamId = state.TeamId,
            Latitude = state.CurrentLatitude,
            Longitude = state.CurrentLongitude
        };
        await _actionWebService.LogPosition(logPositionRequest);
      //await turnContext.ReplyActivity(
      //  $"J'ai enregistré votre nouvelle position {state.CurrentLatitude}, {state.CurrentLongitude}");
      await turnContext.End();
    }

    public ReceiveLocationDialog(IActionWebService actionWebService, ILogger<ReceiveLocationDialog> logger) : base(logger)
    {
        _actionWebService = actionWebService;
    }
  }
}