using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
        private DateTime? _refreshTime;

        public CommandRepository(ILogger<ICommandRepository> logger, IAdminWebService adminWebService,
            ILifetimeScope scope)
        {
            _logger = logger;
            _adminWebService = adminWebService;
            _scope = scope;
        }

        public async Task RefreshAdmins()
        {
            var span = DateTime.Now - (_refreshTime ?? DateTime.Now);
            if (span > TimeSpan.FromMinutes(5) || _admins == null)
            {
                _admins = await _adminWebService.GetAllAdmins();
                _refreshTime = DateTime.Now;
            }
        }

        public ICommand Get(ITurnContext turnContext, ImageHuntState state, string commandText)
        {
            // Remove leading '/' if any and extract command name
            var regex = new Regex(@"\/?(\S*)");
            if (!regex.IsMatch(commandText))
            {
                return null;
            }

            var group = regex.Matches(commandText);
            commandText = group[0].Groups[1].Value;
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

            if (command.IsAdmin && _admins.All(a => !turnContext.Activity.From.Name.Equals(a.Name, StringComparison.InvariantCultureIgnoreCase)))
            {
                throw new NotAuthorizedException(turnContext.Activity.From.Name);
            }

            if (state.Team != null && command.IsAdmin && state.Team.Players.Any(p => p.ChatLogin == turnContext.Activity.From.Name))
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