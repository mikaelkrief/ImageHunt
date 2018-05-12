using System.Threading.Tasks;

namespace ImageHuntTelegramBot.Dialogs
{
  public class ReceiveLocationDialog : AbstractDialog, IReceiveLocationDialog
  {
    public override async Task Begin(ITurnContext turnContext)
    {
      var state = turnContext.GetConversationState<ImageHuntState>();
      state.CurrentLatitude = turnContext.Activity.Location.Latitude;
      state.CurrentLongitude = turnContext.Activity.Location.Longitude;
      await base.Begin(turnContext);
      await turnContext.ReplyActivity(
        $"J'ai enregistré votre nouvelle position {state.CurrentLatitude}, {state.CurrentLongitude}");
      await turnContext.End();
    }
  }
}