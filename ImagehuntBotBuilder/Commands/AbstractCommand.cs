using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Extensions.Logging;

namespace ImageHuntBotBuilder.Commands
{
    public abstract class AbstractCommand : ICommand
    {
        protected readonly ILogger _logger;

        public AbstractCommand(ILogger logger)
        {
            _logger = logger;
        }

        public abstract bool IsAdmin { get; }
        protected abstract Task InternalExecute(ITurnContext turnContext, ImageHuntState state);

        public virtual async Task Execute(ITurnContext turnContext, ImageHuntState state)
        {
            try
            {
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
            Command = command;
        }
    }
}