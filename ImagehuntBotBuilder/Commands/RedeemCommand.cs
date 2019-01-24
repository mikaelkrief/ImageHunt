using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Autofac;
using ImageHuntWebServiceClient;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Bot.Builder;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace ImageHuntBotBuilder.Commands
{
    [Command("redeem")]
    public class RedeemCommand : AbstractCommand, IRedeemCommand
    {
        private readonly IPasscodeWebService _passcodeWebService;
        private readonly ILifetimeScope _scope;

        public RedeemCommand(ILogger<IRedeemCommand> logger, IPasscodeWebService passcodeWebService, ILifetimeScope scope, IStringLocalizer<RedeemCommand> localizer) : base(logger, localizer)
        {
            _passcodeWebService = passcodeWebService;
            _scope = scope;
        }

        protected async override Task InternalExecute(ITurnContext turnContext, ImageHuntState state)
        {
            int gameId = 0;
            string pass;
            var regEx = new Regex(@"(?i)\/redeem gameId=(\d*) pass=(\w*)");
            var activityText = turnContext.Activity.Text;
            if (regEx.IsMatch(activityText))
            {
                var groups = regEx.Matches(activityText);
                gameId = Convert.ToInt32(groups[0].Groups[1].Value);
                pass = groups[0].Groups[2].Value;
                var userName = turnContext.Activity.From.Name;
                var passcodeResponse = await _passcodeWebService.RedeemPasscode(gameId, userName, pass);
                string reply = "";
                switch (passcodeResponse.RedeemStatus)
                {
                    case RedeemStatus.UserNotFound:
                        reply =
                            $"Vous ne pouvez pas utiliser cet passcode car vous ne faites pas partie de la chasse pour laquelle il est prevu";
                        _logger.LogInformation($"User {userName} not in game {gameId}");
                        break;
                    case RedeemStatus.Ok:
                        reply =
                            $"Le passcode {pass} a été bien été utilisé, il a rapporté {passcodeResponse.Points} points à votre équipe.";
                        _logger.LogInformation($"User {userName} correctly redeem passcode for game {gameId}");
                        break;
                    case RedeemStatus.WrongCode:
                        reply = $"Le passcode {pass} est inconnu";
                        _logger.LogInformation($"User {userName} used wrong passcode");
                        break;
                    case RedeemStatus.FullyRedeem:
                        reply = $"Le passcode {pass} est épuisé, désolé!";
                        _logger.LogInformation($"User {userName} used full redeem passcode");
                        break;
                    case RedeemStatus.AlreadyRedeem:
                        reply = $"Le passcode {pass} a déjà été utilisé par votre équipe.";
                        _logger.LogInformation($"User {userName} used already redeem passcode in game {gameId}");

                        break;
                }

                await turnContext.SendActivityAsync(
                    $"/broadcast teamId={passcodeResponse.TeamId} L'utilisation d'un passcode vous à rapporté {passcodeResponse.Points}!");
                var broadcastCommand = _scope.Resolve<IBroadcastCommand>();
                turnContext.Activity.Text = reply;
                await broadcastCommand.Execute(turnContext, state);
            }
        }
    }
}