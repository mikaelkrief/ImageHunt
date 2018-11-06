using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using ImageHuntWebServiceClient.Responses;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Bot.Builder;
using Microsoft.Extensions.Logging;

namespace ImageHuntBotBuilder.Commands
{
    public class CommandRepository : ICommandRepository
    {
        private readonly ILogger<ICommandRepository> _logger;
        private readonly IAdminWebService _adminWebService;
        private readonly ILifetimeScope _scope;
        private IEnumerable<AdminResponse> _admins;

        public CommandRepository(ILogger<ICommandRepository> logger, IAdminWebService adminWebService,
            ILifetimeScope scope)
        {
            _logger = logger;
            _adminWebService = adminWebService;
            _scope = scope;
        }

        public async Task RefreshAdmins()
        {
            _admins = await _adminWebService.GetAllAdmins();
        }

        public ICommand Get(ITurnContext turnContext, string commandText)
        {
            // Remove leading '/' if any
            commandText = commandText.Replace('/', ' ').Trim(' ');
            if (turnContext.Activity == null)
            {
                throw new ArgumentNullException("turnContext.Activity");
            }

            var from = turnContext.Activity.From;
            if (from == null)
            {
                throw new NotAuthorizedException("no User");
            }

            var command = _scope.ResolveNamed<ICommand>(commandText);

            if (command.IsAdmin && _admins.All(a => a.Name != turnContext.Activity.From.Name))
            {
                throw new NotAuthorizedException(turnContext.Activity.From.Name);
            }

            return command;
        }
    }

    public class NotAuthorizedException : Exception
    {
        public string UserName { get; }

        public NotAuthorizedException(string userName)
        {
            UserName = userName;
        }
    }
}