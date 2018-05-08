using System.Threading.Tasks;

namespace ImageHuntTelegramBot.Dialogs.Prompts
{
  public abstract class PromptDialog : AbstractDialog
  {
    private readonly string _promptMessage;

    protected PromptDialog(string promptMessage)
    {
      _promptMessage = promptMessage;
    }

    public override async Task Begin(ITurnContext turnContext)
    {
      var activity = new Activity()
      {
        ChatId = turnContext.ChatId, ActivityType = ActivityType.Message, Text = _promptMessage
      };
      await turnContext.ReplyActivity(activity);
    }

    public override async Task Continue(ITurnContext turnContext)
    {
      await turnContext.End();
    }

  }
}