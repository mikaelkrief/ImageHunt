using System.Threading.Tasks;
using ImageHuntTelegramBot;
using ImageHuntTelegramBot.Dialogs;
using Microsoft.Extensions.Logging;

namespace ImageHuntBot.Dialogs
{
    public class DisplayDialog : AbstractDialog, IDisplayDialog
  {
    private readonly string _displayMessage;

    public DisplayDialog(ILogger<DisplayDialog> logger) : base(logger)
    {
    }
    public override async Task Begin(ITurnContext turnContext)
    {
      var activity = new Activity(){};
      await turnContext.ReplyActivity(activity);
      await turnContext.End();
    }

      public override string Command => "/display";
  }
}
