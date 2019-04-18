using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using ImageHuntBotCore.Commands;
using ImageHuntBotCore.Commands.Interfaces;
using ImageHuntValidator;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Bot.Builder;
using Microsoft.Extensions.Logging;

namespace ImageHuntValidatorBot.Commands
{
    public class CommandRepository : CommandRepository<ImageHuntValidatorState>
    {
        public CommandRepository(ILogger<ICommandRepository<ImageHuntValidatorState>> logger, 
            IAdminWebService adminWebService, ILifetimeScope scope) 
            : base(logger, adminWebService, scope)
        {
        }

        public override ICommand<ImageHuntValidatorState> Get(ITurnContext turnContext, ImageHuntValidatorState state, string commandText)
        {
            var command = base.Get(turnContext, state, commandText);

            return command;
        }
    }
}
