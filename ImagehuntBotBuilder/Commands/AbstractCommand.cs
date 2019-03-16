using System;
using System.Globalization;
using System.Threading.Tasks;
using ImageHuntBotBuilder.Commands.Interfaces;
using Microsoft.Bot.Builder;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace ImageHuntBotBuilder.Commands
{
    public abstract class AbstractCommand : ICommand
    {
        protected readonly ILogger Logger;
        protected IStringLocalizer Localizer;

        public AbstractCommand(ILogger logger, IStringLocalizer localizer)
        {
            Logger = logger;
            Localizer = localizer;
        }

        public virtual bool IsAdmin => true;

        public virtual async Task ExecuteAsync(ITurnContext turnContext, ImageHuntState state)
        {
            try
            {
                if (state.Team != null && !string.IsNullOrEmpty(state.Team.CultureInfo))
                    Localizer = Localizer.WithCulture(new CultureInfo(state.Team.CultureInfo));
                await InternalExecuteAsync(turnContext, state);
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Exception occured while execute command");
                throw e;
            }
        }

        protected abstract Task InternalExecuteAsync(ITurnContext turnContext, ImageHuntState state);
    }
}