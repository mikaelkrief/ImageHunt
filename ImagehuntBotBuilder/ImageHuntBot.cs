using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;

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
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="EchoWithCounterBot"/> class.
        /// </summary>
        /// <param name="accessors">A class containing <see cref="IStatePropertyAccessor{T}"/> used to manage state.</param>
        /// <param name="logger">Logger provided by injection</param>
        /// <seealso cref="https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-2.1#windows-eventlog-provider"/>
        public ImageHuntBot(ImageHuntBotAccessors accessors, IActionWebService actionWebService, ILogger<ImageHuntBot> logger)
        {
            _logger = logger;
            _logger.LogTrace("ImageHuntBot turn start.");
            _accessors = accessors ?? throw new System.ArgumentNullException(nameof(accessors));
            _actionWebService = actionWebService;
        }

        public async Task OnTurnAsync(
            ITurnContext turnContext,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            // Get the conversation state from the turn context.
            var state = await _accessors.ImageHuntState.GetAsync(turnContext, () => new ImageHuntState());

            switch (turnContext.Activity.Type)
            {
                case "location":
                    if (state.Status == Status.Started && state.GameId.HasValue && state.TeamId.HasValue)
                    {
                        var location = turnContext.Activity.Attachments.Single().Content as GeoCoordinates;
                        var logPositionRequest = new LogPositionRequest()
                        {
                            GameId = state.GameId.Value,
                            TeamId = state.TeamId.Value,
                            Latitude = location.Latitude ?? 0d,
                            Longitude = location.Longitude ?? 0d,
                        };
                        await _actionWebService.LogPosition(logPositionRequest, cancellationToken);
                        state.CurrentLocation = location;
                    }

                    break;
                case ActivityTypes.Message:
                    break;
            }
            // Set the property using the accessor.
            await _accessors.ImageHuntState.SetAsync(turnContext, state);
            // Save the new turn count into the conversation state.
            await _accessors.ConversationState.SaveChangesAsync(turnContext);


        }
    }
}
