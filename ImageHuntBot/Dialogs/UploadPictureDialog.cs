using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace ImageHuntBot.Dialogs
{
  [Serializable]
  public class UploadPictureDialog : IDialog<object>
  {
    public Task StartAsync(IDialogContext context)
    {
      context.Wait(MessageReceivedAsync);

      return Task.CompletedTask;
    }

    public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
    {
      var activity = await argument;

      if (activity?.Attachments != null && activity.Attachments.Any())
      {

      }

      context.Wait(MessageReceivedAsync);
    }
  }
}