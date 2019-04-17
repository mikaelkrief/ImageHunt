using System;
using System.Globalization;
using System.Threading.Tasks;
using ImageHuntBotCore.Commands.Interfaces;
using Microsoft.Bot.Builder;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace ImageHuntBotCore.Commands
{
    public abstract class AbstractCommand<TState> : ICommand<TState> 
        where TState :IState, new()
    {
        protected readonly ILogger Logger;
        protected IStringLocalizer Localizer;

        public AbstractCommand(ILogger logger, IStringLocalizer localizer)
        {
            Logger = logger;
            Localizer = localizer;
        }

        public virtual bool IsAdmin => true;
        protected abstract Task InternalExecuteAsync(ITurnContext turnContext, TState state);

        public virtual async Task ExecuteAsync(ITurnContext turnContext, TState state)
        {
            try
            {
                if (!string.IsNullOrEmpty(state.CultureInfo))
                {
                    Localizer = Localizer.WithCulture(new CultureInfo(state.CultureInfo));
                }
                await InternalExecuteAsync(turnContext, state);
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Exception occured while execute command");
            }
        }
    }
}