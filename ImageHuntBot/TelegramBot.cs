using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ImageHuntTelegramBot.Controllers;
using Microsoft.Extensions.Logging;

namespace ImageHuntTelegramBot
{
  public class TelegramBot : IBot
  {
    private Dictionary<string, IDialog> _dialogs = new Dictionary<string, IDialog>();
    private static readonly SemaphoreSlim Padlock = new SemaphoreSlim(1,1);

    public void AddDialog(string dialogInitText, IDialog dialog)
    {
      _dialogs.Add(dialogInitText, dialog);
    }

    public async Task OnTurn(ITurnContext context)
    {
      await Padlock.WaitAsync();
      // Start critical section
      try
      {
        await context.Continue();
        IDialog dialog = null;
        if (!context.Replied)
        {
          if (context.CurrentDialog == null && _dialogs.Any(d => context.Activity.Command == d.Key))
          {
            dialog = _dialogs[context.Activity.Command];
          }
        }
        if (dialog != null)
        {
          await context.Begin(dialog);
        }
      }
      finally
      {
        // End critical section
        Padlock.Release();
      }
    }
  }
}