using System.Threading.Tasks;
using ImageHuntTelegramBot;
using ImageHuntTelegramBot.Dialogs;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.WebServices;

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

        public override async Task Begin(ITurnContext turnContext)
        {
            try
            {
                var state = turnContext.GetConversationState<ImageHuntState>();
                if (state.TeamId == 0)
                {
                    await turnContext.ReplyActivity(
                        "Je ne peux ajouter un membre à une équipe que si le groupe à été initialisé! Commande /init");
                    await turnContext.End();
                    return;
                }
                foreach (var user in turnContext.Activity.NewChatMember)
                {
                    var playerRequest = new PlayerRequest(){Name = user.LastName, Username=user.Username};
                    await _teamWebService.AddPlayer(state.TeamId, playerRequest);
                    await turnContext.ReplyActivity(
                        $"Le joueur {user.Username} vient d'être ajouté à l'équipe {state.Team.Name}");
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