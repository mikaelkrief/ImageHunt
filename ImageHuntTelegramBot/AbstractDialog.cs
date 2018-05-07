using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace ImageHuntTelegramBot
{
  public abstract class AbstractDialog : IDialog
  {
    protected List<IDialog> _childenDialogs = new List<IDialog>();
    protected IDialog _currentDialog;

    public virtual async Task Begin(ITurnContext turnContext)
    {
      if (_childenDialogs.Any())
      {
        _currentDialog = _childenDialogs.First();
        await _currentDialog.Begin(turnContext);
      }
      else
      {
        await Reply(turnContext);
      }
    }

    public virtual async Task Continue(ITurnContext turnContext)
    {
      
    }

    public virtual async Task OnTurn(ITurnContext turnContext)
    {
      throw new NotImplementedException();
    }

    public void AddChildren(IDialog childrenDialog)
    {
      _childenDialogs.Add(childrenDialog);
    }

    public virtual async Task Reply(ITurnContext turnContext)
    {
    }
  }
}