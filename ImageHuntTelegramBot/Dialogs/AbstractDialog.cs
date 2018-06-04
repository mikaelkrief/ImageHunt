using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telegram.Bot;

namespace ImageHuntTelegramBot.Dialogs
{
  public abstract class AbstractDialog : IDialog
  {
    protected readonly ILogger _logger;
    protected List<IDialog> _childenDialogs = new List<IDialog>();
    protected IDialog _currentDialog;

    protected AbstractDialog(ILogger logger)
    {
      _logger = logger;
    }


    public virtual async Task Begin(ITurnContext turnContext)
    {
      turnContext.EndCalled += TurnContextOnEndCalled;
      if (_childenDialogs.Any())
      {
        _currentDialog = _childenDialogs.First();
        await turnContext.Begin(_currentDialog);
      }
    }

    private async void TurnContextOnEndCalled(object sender, EventArgs eventArgs)
    {
      var turnContext = sender as ITurnContext;
      if (_currentDialog != null)
      {
        _currentDialog = _childenDialogs.SkipWhile(c => c != _currentDialog).Skip(1).FirstOrDefault();

        if (_currentDialog != null)
        {
          await _currentDialog.Begin(turnContext);
        }
        else
        {
          await turnContext.End();
          turnContext.EndCalled -= TurnContextOnEndCalled;
        }
      }

    }

    public virtual async Task Continue(ITurnContext turnContext)
    {
      // continue only if the current dialog is not self (to avoid infinite loop)
      if (_currentDialog != this && _currentDialog!= null)
        await _currentDialog.Continue(turnContext);
    }


    public virtual void AddChildren(IDialog childrenDialog)
    {
      _childenDialogs.Add(childrenDialog);
    }

    public virtual async Task Reply(ITurnContext turnContext)
    {
    }
  }
}