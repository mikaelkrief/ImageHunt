using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace ImageHuntBotBuilder.Middlewares
{
    public class TeamCompositionMiddleware : IMiddleware
    {
        private readonly ITeamWebService _teamWebService;
        private readonly ILogger<TeamCompositionMiddleware> _logger;
        private IStringLocalizer _localizer;
        private readonly ImageHuntBotAccessors _accessors;

        public TeamCompositionMiddleware(ITeamWebService teamWebService, 
            ILogger<TeamCompositionMiddleware> logger,
            IStringLocalizer<TeamCompositionMiddleware> localizer,
            ImageHuntBotAccessors accessors)
        {
            _teamWebService = teamWebService;
            _logger = logger;
            _localizer = localizer;
            _accessors = accessors;
        }

        public async Task OnTurnAsync(ITurnContext turnContext, NextDelegate next,
            CancellationToken cancellationToken = new CancellationToken())
        {
            var state = await _accessors.ImageHuntState.GetAsync(turnContext, () => new ImageHuntState());
            if (state.Team != null)
            {
                _localizer = _localizer.WithCulture(new CultureInfo(state.Team.CultureInfo));
            }
            switch (turnContext.Activity.Type)
            {
                case ImageHuntActivityTypes.NewPlayer:
                if (!state.TeamId.HasValue)
                {
                    await turnContext.SendActivityAsync(_localizer["CHAT_NOT_INITIALIZED"]);
                    _logger.LogError($"Unable to add an user to a team since the group had not been initialized");
                    return;
                }

                foreach (var activityAttachment in turnContext.Activity.Attachments)
                {
                    var player = activityAttachment.Content as ConversationAccount;
                    var playerRequest = new PlayerRequest() {ChatLogin = player.Name, Name = player.Name};
                    await _teamWebService.AddPlayer(state.TeamId.Value, playerRequest);
                    await turnContext.SendActivityAsync(string.Format(_localizer["PLAYER_ADDED"], player.Name, state.Team.Name));
                    _logger.LogInformation($"The user {player.Name} had been added to team {state.TeamId}");
                }

                    break;
                case ImageHuntActivityTypes.LeftPlayer:
                    foreach (var activityAttachment in turnContext.Activity.Attachments)
                    {
                        var player = activityAttachment.Content as ConversationAccount;
                        await _teamWebService.RemovePlayerFromTeam(state.TeamId.Value, player.Name);
                        await turnContext.SendActivityAsync(string.Format(_localizer["PLAYER_REMOVED"], player.Name,
                            state.Team.Name));
                        _logger.LogInformation($"The user {player.Name} had been added to team {state.TeamId}");
                    }

                    break;
                default:
                    await next(cancellationToken);
                    break;
            }
        }
    }
}