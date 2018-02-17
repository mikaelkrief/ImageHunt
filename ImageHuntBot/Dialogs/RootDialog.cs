using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace ImageHuntBot.Dialogs
{
  [Serializable]
  public class RootDialog : IDialog<object>
  {
    public Task StartAsync(IDialogContext context)
    {
      context.Wait(MessageReceivedAsync);

      return Task.CompletedTask;
    }

    private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
    {
      var activity = await result as Activity;

      await context.PostAsync(
        "Bienvenue à la chasse au trésor, je suis le bot qui va vous accompagner tout le long de votre périple");

      {

        // calculate something for us to return
        int length = (activity.Text ?? string.Empty).Length;

        // return our reply to the user
        await context.PostAsync($"You sent {activity.Text} which was {length} characters");

        context.Wait(MessageReceivedAsync);
      }
    }

    private async Task LaunchWelcomeDialog(IDialogContext context, IAwaitable<bool> result)
    {
      var confirm = await result;
    }
  }
}