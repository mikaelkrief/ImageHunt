using System.Threading.Tasks;
using ImageHuntTelegramBot;
using ImageHuntTelegramBot.Dialogs;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Extensions.Logging;

namespace ImageHuntBot.Dialogs
{
    public class AddMemberDialog : AbstractDialog, IAddMemberDialog
    {
        private readonly ITeamWebService _teamWebService;

        public AddMemberDialog(ILogger<AddMemberDialog> logger, ITeamWebService teamWebService) 
            : base(logger)
        {
            _teamWebService = teamWebService;
        }

        public override async Task Begin(ITurnContext turnContext, bool overrideAdmin = false)
        {
            try
            {
                var state = turnContext.GetConversationState<ImageHuntState>();
                if (state.TeamId == 0)
                {
                    await turnContext.ReplyActivity(
                        "Je ne peux ajouter un membre à une équipe que si le groupe à été initialisé! Commande /init");
                    _logger.LogError($"Unable to add an user to a team since the group had not been initialized");
                    await turnContext.End();
                    return;
                }
                foreach (var user in turnContext.Activity.NewChatMember)
                {
                    var playerRequest = new PlayerRequest(){Name = user.LastName, ChatLogin= user.Username};
                    await _teamWebService.AddPlayer(state.TeamId, playerRequest);
                    await turnContext.ReplyActivity(
                        $"Le joueur {user.Username} vient d'être ajouté à l'équipe {state.Team.Name}");
                    _logger.LogInformation($"The user {user.Username} had been added to team {state.TeamId}");
                }
            }
            finally
            {
                await turnContext.End();
            }
        }

        public override string Command => "/newUser";
    }
}