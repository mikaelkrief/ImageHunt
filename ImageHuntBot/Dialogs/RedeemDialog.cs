using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Autofac;
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
        private readonly ILifetimeScope _scope;

        public RedeemDialog(ILogger<RedeemDialog> logger, 
            IActionWebService actionWebService, 
            IPasscodeWebService passcodeWebService,
            ITeamWebService teamWebService, 
            ILifetimeScope scope) 
            : base(logger)
        {
            _actionWebService = actionWebService;
            _passcodeWebService = passcodeWebService;
            _teamWebService = teamWebService;
            _scope = scope;
        }

        public override async Task Begin(ITurnContext turnContext)
        {
            string pass = "";
            int gameId = 0;
            try
            {
                var regEx = new Regex(@"(?i)\/redeem gameId=(\d*) pass=(\w*)");
                var activityText = turnContext.Activity.Text;
                if (regEx.IsMatch(activityText))
                {
                    var groups = regEx.Matches(activityText);
                    gameId = Convert.ToInt32(groups[0].Groups[1].Value);
                    pass = groups[0].Groups[2].Value;
                    var passcodeResponse = await _passcodeWebService.RedeemPasscode(gameId, turnContext.Username, pass);

                    string reply = "";
                    switch (passcodeResponse.RedeemStatus)
                    {
                        case RedeemStatus.UserNotFound:
                            reply =
                                $"Vous ne pouvez pas utiliser cet passcode car vous ne faites pas partie de la chasse pour laquelle il est prevu";
                            LogInfo<ImageHuntState>(turnContext, $"User {turnContext.Username} not in game {gameId}");
                            break;
                        case RedeemStatus.Ok:
                            reply =
                                $"Le passcode {pass} a été bien été utilisé, il a rapporté {passcodeResponse.Points} points à votre équipe.";
                            LogInfo<ImageHuntState>(turnContext,
                                $"User {turnContext.Username} correctly redeem passcode for game {gameId}");
                            break;
                        case RedeemStatus.WrongCode:
                            reply = $"Le passcode {pass} est inconnu";
                            LogInfo<ImageHuntState>(turnContext, $"User {turnContext.Username} used wrong passcode");
                            break;
                        case RedeemStatus.FullyRedeem:
                            reply = $"Le passcode {pass} est épuisé, désolé!";
                            LogInfo<ImageHuntState>(turnContext,
                                $"User {turnContext.Username} used full redeem passcode");
                            break;
                        case RedeemStatus.AlreadyRedeem:
                            reply = $"Le passcode {pass} a déjà été utilisé par votre équipe.";
                            LogInfo<ImageHuntState>(turnContext,
                                $"User {turnContext.Username} used already redeem passcode in game {gameId}");

                            break;
                    }

                    // Send message to team
                    turnContext.Activity.Text = $"/broadcast teamId={passcodeResponse.TeamId} L'utilisation d'un passcode vous à rapporté {passcodeResponse.Points}!";
                    var broadcastDialog = _scope.Resolve<IBroadcastDialog>();
                    await turnContext.Begin(broadcastDialog);
                    await turnContext.ReplyActivity(reply);

                }

            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Exception while handling redeem of passcode {pass} for gameId={gameId}");
            }
            finally
            {
                await turnContext.End();
            }
        }

        public override string Command => "/redeem";
    }
}