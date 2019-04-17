using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using ImageHuntBotCore.Commands;
using ImageHuntBotCore.Commands.Interfaces;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Bot.Builder;
using Microsoft.Extensions.Logging;

namespace ImageHuntBotBuilder.Commands
{
    public class CommandRepository : CommandRepository<ImageHuntState>
    {
        public CommandRepository(ILogger<ICommandRepository<ImageHuntState>> logger, IAdminWebService adminWebService, ILifetimeScope scope) 
            : base(logger, adminWebService, scope)
        {
        }

        public override ICommand<ImageHuntState> Get(ITurnContext turnContext, ImageHuntState state, string commandText)
        {
            var command = base.Get(turnContext, state, commandText);
            if (state.Team != null && command.IsAdmin && state.Team.Players.Any(p => p.ChatLogin == turnContext.Activity.From.Name))
            {
                throw new NotAuthorizedException(turnContext.Activity.From.Name);
            }

            return command;
        }
    }
}
