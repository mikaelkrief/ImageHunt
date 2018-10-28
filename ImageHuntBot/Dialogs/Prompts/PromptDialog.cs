﻿using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ImageHuntTelegramBot.Dialogs.Prompts
{
  public abstract class PromptDialog : AbstractDialog
  {
    private readonly string _promptMessage;
    protected readonly PromptResult _prompResult;

    public delegate Task PromptResult(ITurnContext context, object result);

    protected PromptDialog(string promptMessage, PromptResult prompResult, ILogger logger) : base(logger)
    {
      _promptMessage = promptMessage;
      _prompResult = prompResult;
    }

    public override async Task Begin(ITurnContext turnContext, bool overrideAdmin)
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