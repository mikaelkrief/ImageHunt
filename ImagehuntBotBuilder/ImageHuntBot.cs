using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ImageHuntBotBuilder.Commands;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using Action = ImageHuntCore.Model.Action;

namespace ImageHuntBotBuilder
{
    /// <summary>
    /// Represents a bot that processes incoming activities.
    /// For each user interaction, an instance of this class is created and the OnTurnAsync method is called.
    /// This is a Transient lifetime service.  Transient lifetime services are created
    /// each time they're requested. For each Activity received, a new instance of this
    /// class is created. Objects that are expensive to construct, or have a lifetime
    /// beyond the single turn, should be carefully managed.
    /// For example, the <see cref="MemoryStorage"/> object and associated
    /// <see cref="IStatePropertyAccessor{T}"/> object are created with a singleton lifetime.
    /// </summary>
    /// <seealso cref="https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-2.1"/>
    public class ImageHuntBot : IBot
    {
        private readonly ImageHuntBotAccessors _accessors;
        private readonly IActionWebService _actionWebService;
        private readonly ITeamWebService _teamWebService;
        private readonly ICommandRepository _commandRepository;
        private readonly INodeVisitorHandler _nodeVisitorHandler;
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="EchoWithCounterBot"/> class.
        /// </summary>
        /// <param name="accessors">A class containing <see cref="IStatePropertyAccessor{T}"/> used to manage state.</param>
        /// <param name="logger">Logger provided by injection</param>
        /// <seealso cref="https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-2.1#windows-eventlog-provider"/>
        public ImageHuntBot(ImageHuntBotAccessors accessors,
            IActionWebService actionWebService,
            ITeamWebService teamWebService,
            ICommandRepository commandRepository,
            INodeVisitorHandler nodeVisitorHandler,
            ILogger<ImageHuntBot> logger)
        {
            _logger = logger;
            _logger.LogTrace("ImageHuntBot turn start.");
            _accessors = accessors ?? throw new System.ArgumentNullException(nameof(accessors));
            _actionWebService = actionWebService;
            _teamWebService = teamWebService;
            _commandRepository = commandRepository;
            _nodeVisitorHandler = nodeVisitorHandler;
        }

        public async Task OnTurnAsync(
            ITurnContext turnContext,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            // Get the conversation state from the turn context.
            var state = await _accessors.ImageHuntState.GetAsync(turnContext, () => new ImageHuntState());
            state.ConversationId = turnContext.Activity.Conversation.Id;
            switch (turnContext.Activity.Type)
            {
                case ImageHuntActivityTypes.Image:
                    if (state.Status == Status.Started &&
                        state.GameId.HasValue &&
                        state.TeamId.HasValue)
                    {
                        var gameActionRequest = new GameActionRequest()
                        {
                            Action = (int)Action.SubmitPicture,
                            GameId = state.GameId.Value,
                            TeamId = state.TeamId.Value,
                            Latitude = state.CurrentLocation.Latitude.Value,
                            Longitude = state.CurrentLocation.Longitude.Value,
                            PictureId = (int)turnContext.Activity.Attachments.First().Content
                        };
                        await _actionWebService.LogAction(gameActionRequest);
                        _logger.LogInformation(
                            $"Image {turnContext.Activity.Attachments.First().Name} had been uploaded");
                        await turnContext.SendActivityAsync(
                            $"Votre image a bien été téléchargée, un validateur l'examinera pour vous attribuer les points", cancellationToken: cancellationToken);
                    }
                    else
                    {
                        await turnContext.SendActivityAsync(
                            $"La chasse n'a pas encore commencée ou le groupe n'est pas encore initialisé, merci de m'envoyer les photos plus tard!", cancellationToken: cancellationToken);
                    }

                    break;
                case ActivityTypes.Message:
                    if (!string.IsNullOrEmpty(turnContext.Activity.Text) &&
                        turnContext.Activity.Text.StartsWith('/'))
                    {
                        await _commandRepository.RefreshAdmins();

                        try
                        {
                            var command = _commandRepository.Get(turnContext, state, turnContext.Activity.Text);
                            await command.Execute(turnContext, state);

                        }
                        catch (NotAuthorizedException e)
                        {
                            _logger.LogError(e, $"User {turnContext.Activity.From.Name} not authorized to use this command");
                            await turnContext.SendActivityAsync("Vous n'êtes pas autorisé à utiliser cette commande");
                        }

                    }

                    break;
                case ImageHuntActivityTypes.Location:
                    await _nodeVisitorHandler.MatchHiddenNodesLocationAsync(turnContext, state);
                    await _nodeVisitorHandler.MatchLocationAsync(turnContext, state);
                    break;
            }

            // Set the property using the accessor.
            await _accessors.ImageHuntState.SetAsync(turnContext, state);
            // Save the new turn count into the conversation state.
            await _accessors.ConversationState.SaveChangesAsync(turnContext);
        }
    }
}