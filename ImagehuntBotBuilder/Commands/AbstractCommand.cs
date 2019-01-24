using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace ImageHuntBotBuilder.Commands
{
    public abstract class AbstractCommand : ICommand
    {
        protected readonly ILogger _logger;
        protected IStringLocalizer _localizer;

        public AbstractCommand(ILogger logger, IStringLocalizer localizer)
        {
            _logger = logger;
            _localizer = localizer;
        }

        public virtual bool IsAdmin => true;
        protected abstract Task InternalExecute(ITurnContext turnContext, ImageHuntState state);

        public virtual async Task Execute(ITurnContext turnContext, ImageHuntState state)
        {
            try
            {
                if (state.Team != null && !string.IsNullOrEmpty(state.Team.CultureInfo))
                {
                    _localizer = _localizer.WithCulture(new CultureInfo(state.Team.CultureInfo));
                }
                await InternalExecute(turnContext, state);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception occured while execute command");
                throw e;
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