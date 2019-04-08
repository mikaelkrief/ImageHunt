using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Autofac;
using ImageHuntBotBuilder.Commands.Interfaces;
using Microsoft.Bot.Builder;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace ImageHuntBotBuilder.Commands
{
    [Command("start")]
    public class StartCommand : AbstractCommand, IStartCommand
    {
        private readonly ILifetimeScope _scope;

        public StartCommand(ILogger<IStartCommand> logger, ILifetimeScope scope, IStringLocalizer<StartCommand> localizer) : base(logger, localizer)
        {
            _scope = scope;
        }

        protected async override Task InternalExecute(ITurnContext turnContext, ImageHuntState state)
        {
            var regex = new Regex(@"(\/\w*) (.*)");
            if (regex.IsMatch(turnContext.Activity.Text))
            {
                var payload = regex.Matches(turnContext.Activity.Text)[0].Groups[2].Value;
                payload = $"/{payload}";
                regex = new Regex(@"(\w*)_(.*)");
                if (regex.IsMatch(payload))
                {
                    var group = regex.Matches(payload);
                    var subCommand = $"/{group[0].Groups[1].Value}";
                    ICommand subDialog = null;
                    var subpayload = payload;
                    // Replace # by space
                    subpayload = payload.Replace('_', ' ');
                    turnContext.Activity.Text = subpayload;
                    switch (subCommand)
                    {
                        case "/redeem":
                            subDialog = _scope.Resolve<IRedeemCommand>();
                            await subDialog.Execute(turnContext, state);
                            break;
                    }
                }
            }
        }
    }
}