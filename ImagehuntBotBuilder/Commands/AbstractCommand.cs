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
        protected abstract Task InternalExecuteAsync(ITurnContext turnContext, ImageHuntState state);

        public virtual async Task ExecuteAsync(ITurnContext turnContext, ImageHuntState state)
        {
            try
            {
                if (state.Team != null && !string.IsNullOrEmpty(state.Team.CultureInfo))
                {
                    Localizer = Localizer.WithCulture(new CultureInfo(state.Team.CultureInfo));
                }
                await InternalExecuteAsync(turnContext, state);
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Exception occured while execute command");
            }
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class CommandAttribute : Attribute
    {
        public string Command { get; }

        public CommandAttribute(string command)
        {
            Command = command.ToLowerInvariant();
        }
    }
}