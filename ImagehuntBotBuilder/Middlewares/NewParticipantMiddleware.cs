using System.Threading;
using System.Threading.Tasks;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;

namespace ImageHuntBotBuilder.Middlewares
{
    public class NewParticipantMiddleware : IMiddleware
    {
        private readonly ITeamWebService _teamWebService;
        private readonly ILogger<NewParticipantMiddleware> _logger;
        private readonly ImageHuntBotAccessors _accessors;

        public NewParticipantMiddleware(ITeamWebService teamWebService, ILogger<NewParticipantMiddleware> logger,
            ImageHuntBotAccessors accessors)
        {
            _teamWebService = teamWebService;
            _logger = logger;
            _accessors = accessors;
        }

        public async Task OnTurnAsync(ITurnContext turnContext, NextDelegate next,
            CancellationToken cancellationToken = new CancellationToken())
        {
            var state = await _accessors.ImageHuntState.GetAsync(turnContext, () => new ImageHuntState());

            if (turnContext.Activity.Type == ImageHuntActivityTypes.NewPlayer)
            {
                if (!state.TeamId.HasValue)
                {
                    await turnContext.SendActivityAsync(
                        "Je ne peux ajouter un membre à une équipe que si le groupe à été initialisé! Commande /init");
                    _logger.LogError($"Unable to add an user to a team since the group had not been initialized");
                    return;
                }

                foreach (var activityAttachment in turnContext.Activity.Attachments)
                {
                    var player = activityAttachment.Content as ConversationAccount;
                    var playerRequest = new PlayerRequest() {ChatLogin = player.Name, Name = player.Name};
                    await _teamWebService.AddPlayer(state.TeamId.Value, playerRequest);
                    await turnContext.SendActivityAsync(
                        $"Le joueur {player.Name} vient d'être ajouté à l'équipe {state.Team.Name}");
                    _logger.LogInformation($"The user {player.Name} had been added to team {state.TeamId}");
                }
            }
            else
            {
                await next(cancellationToken);
            }
        }
    }
}