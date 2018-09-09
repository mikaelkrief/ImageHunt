using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Autofac;
using ImageHuntTelegramBot;
using ImageHuntTelegramBot.Dialogs;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Extensions.Logging;
using Remotion.Linq.Parsing.Structure.IntermediateModel;

namespace ImageHuntBot.Dialogs
{
    public class StartDialog : AbstractDialog, IStartDialog
    {
        private readonly IPasscodeWebService _passcodeWebService;
        private readonly ILifetimeScope _scope;
        private readonly IContainer _container;

        public StartDialog(ILogger<StartDialog> logger, IPasscodeWebService passcodeWebService, ILifetimeScope scope) : base(logger)
        {
            _passcodeWebService = passcodeWebService;
            _scope = scope;
        }

        public override async Task Begin(ITurnContext turnContext)
        {
            // extract the payload
            var command = turnContext.Activity.Command;
            var payload = turnContext.Activity.Payload;
            payload = $"/{payload}";
            var regex = new Regex(@"(\w*)#(.*)");
            if (regex.IsMatch(payload))
            {
                var group = regex.Matches(payload);
                var subCommand = $"/{group[0].Groups[1].Value}";
                IDialog subDialog = null;
                var subpayload = payload;
                // Replace # by space
                subpayload = payload.Replace('#', ' ');
                turnContext.Activity.Text = subpayload;
                switch (subCommand)
                {
                    case "/redeem":
                        subDialog = _scope.Resolve<IRedeemDialog>();
                        await turnContext.Begin(subDialog);
                        break;
                }
            }

            await turnContext.End();
        }

        public override string Command => "/start";
    }
}