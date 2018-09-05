using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ImageHuntTelegramBot;
using ImageHuntTelegramBot.Dialogs;
using ImageHuntWebServiceClient;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Extensions.Logging;

namespace ImageHuntBot.Dialogs
{
    public class RedeemDialog : AbstractDialog, IRedeemDialog
    {
        private readonly IActionWebService _actionWebService;
        private readonly IPasscodeWebService _passcodeWebService;
        private readonly ITeamWebService _teamWebService;

        public RedeemDialog(ILogger<RedeemDialog> logger, 
            IActionWebService actionWebService, 
            IPasscodeWebService passcodeWebService,
            ITeamWebService teamWebService) 
            : base(logger)
        {
            _actionWebService = actionWebService;
            _passcodeWebService = passcodeWebService;
            _teamWebService = teamWebService;
        }

        public override async Task Begin(ITurnContext turnContext)
        {
            var regEx = new Regex(@"(?i)\/redeem=(\w*)");
            var activityText = turnContext.Activity.Text;
            if (regEx.IsMatch(activityText))
            {
                var groups = regEx.Matches(activityText);
                var pass = groups[0].Groups[1].Value;
                var teamResponse = await _teamWebService.GetTeamForUserName(turnContext.Username);
                var passcodeResponse = await _passcodeWebService.RedeemPasscode(teamResponse.GameId, teamResponse.Id, pass);
                string reply = "";
                switch (passcodeResponse.RedeemStatus)
                {
                    case RedeemStatus.Ok:
                        reply =
                            $"Le passcode {pass} a été bien été utilisé, il a rapporté {passcodeResponse.Points} points à votre équipe.";
                        break;
                    case RedeemStatus.WrongCode:
                        reply = $"Le passcode {pass} est inconnu";
                        break;
                    case RedeemStatus.FullyRedeem:
                        reply = $"Le passcode {pass} est épuisé, désolé!";
                        break;
                    case RedeemStatus.AlreadyRedeem:
                        reply = $"Le passcode {pass} a déjà été utilisé par votre équipe.";
                        break;
                }

                await turnContext.ReplyActivity(reply);
                var gameActionRequest = new GameActionRequest()
                {
                    GameId = teamResponse.GameId,
                    TeamId = teamResponse.Id,
                    Action = (int) ImageHuntWebServiceClient.Action.RedeemPasscode,
                };
                await _actionWebService.LogAction(gameActionRequest);

            }

            await turnContext.End();
        }

        public override string Command => "/redeem";
    }
}