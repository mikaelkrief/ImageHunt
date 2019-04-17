using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Autofac;
using ImageHuntBotCore.Commands.Interfaces;
using ImageHuntWebServiceClient.Responses;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Bot.Builder;
using Microsoft.Extensions.Logging;

namespace ImageHuntBotCore.Commands
{
    public abstract class CommandRepository<TState> : ICommandRepository<TState> 
        where TState: IState
    {
        private readonly ILogger<ICommandRepository<TState>> _logger;
        private readonly IAdminWebService _adminWebService;
        private readonly ILifetimeScope _scope;
        private IEnumerable<AdminResponse> _admins;
        private DateTime? _refreshTime;

        public CommandRepository(
            ILogger<ICommandRepository<TState>> logger,
            IAdminWebService adminWebService,
            ILifetimeScope scope)
        {
            _logger = logger;
            _adminWebService = adminWebService;
            _scope = scope;
        }

        public async Task RefreshAdminsAsync()
        {
            var span = DateTime.Now - (_refreshTime ?? DateTime.Now);
            if (span > TimeSpan.FromMinutes(5) || _admins == null)
            {
                _admins = await _adminWebService.GetAllAdmins();
                _refreshTime = DateTime.Now;
            }
        }

        public virtual ICommand<TState> Get(ITurnContext turnContext, TState state, string commandText)
        {
            // Remove leading '/' if any and extract command name
            var regex = new Regex(@"\/?(\S*)");
            if (!regex.IsMatch(commandText))
            {
                return null;
            }

            if (commandText.Contains('@'))
                commandText = commandText.Split('@')[0];
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

            ICommand<TState> command;
            try
            {
                command = _scope.ResolveNamed<ICommand<TState>>(commandText.ToLowerInvariant());
            }
            catch (Exception e)
            {
                throw new CommandNotFound(commandText);
            }
            if (command.IsAdmin && _admins.All(a => !turnContext.Activity.From.Name.Equals(a.Name, StringComparison.InvariantCultureIgnoreCase)))
            {
                throw new NotAuthorizedException(turnContext.Activity.From.Name);
            }

            return command;
        }
    }
}