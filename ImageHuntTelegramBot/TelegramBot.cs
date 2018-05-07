using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageHuntTelegramBot.Controllers;

namespace ImageHuntTelegramBot
{
  public class TelegramBot : IBot
  {
    private Dictionary<string, IDialog> _dialogs = new Dictionary<string, IDialog>();

    public void AddDialog(string dialogInitText, IDialog dialog)
    {
      _dialogs.Add(dialogInitText, dialog);
    }
    public async Task OnTurn(ITurnContext context)
    {
      await context.Continue();
      if (!context.Replied)
      {
        if (_dialogs.Any())
        {
          var dialog = _dialogs[context.Activity.Text];
          await dialog.Begin(context);
        }
      }
    }
  }
}