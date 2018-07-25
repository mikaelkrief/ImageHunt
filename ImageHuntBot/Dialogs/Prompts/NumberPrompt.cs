using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ImageHuntTelegramBot.Dialogs.Prompts
{
  public class NumberPrompt<T> : PromptDialog where T : struct 
  {
    private string _replyUser;

    public NumberPrompt(string promptMessage, PromptResult promptResult, ILogger logger) 
      : base(promptMessage, promptResult, logger)
    {
    }

    public override Task Continue(ITurnContext turnContext)
    {
      _replyUser = turnContext.Activity.Text;
      _prompResult?.Invoke(turnContext, Value);
      return base.Continue(turnContext);
    }

      public override string Command { get; }


      public T Value
    {
      get
      {
        switch (Type.GetTypeCode(typeof(T)))
        {
          case TypeCode.Int32:
            return (T) (object) Convert.ToInt32(_replyUser);
          case TypeCode.Double:
            return (T) (object) Convert.ToDouble(_replyUser);
        }
        return default(T);
      }
    }
  }
}