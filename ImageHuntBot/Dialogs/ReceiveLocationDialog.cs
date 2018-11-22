using System.Linq;
using System.Threading.Tasks;
using Autofac;
using ImageHuntBot.Dialogs;
using ImageHuntCore.Computation;
using ImageHuntCore.Model;
using ImageHuntWebServiceClient;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Extensions.Logging;

namespace ImageHuntTelegramBot.Dialogs
{
    public class ReceiveLocationDialog : AbstractDialog, IReceiveLocationDialog
    {
        private readonly IActionWebService _actionWebService;
        private readonly INodeWebService _nodeWebService;
        private readonly ILifetimeScope _scope;
        public override bool IsAdmin => false;

        public override async Task Begin(ITurnContext turnContext, bool overrideAdmin = false)
        {
            try
            {
                var state = turnContext.GetConversationState<ImageHuntState>();
                if (state.Status == Status.None)
                {
                    LogInfo<ImageHuntState>(turnContext, "Game not initialized");
                    await turnContext.End();
                    return;
                }
                state.CurrentLatitude = turnContext.Activity.Location.Latitude;
                state.CurrentLongitude = turnContext.Activity.Location.Longitude;

                _logger.LogInformation($"Received position: [lat:{state.CurrentLatitude}, lng:{state.CurrentLongitude}");
                if (state.CurrentNode != null)
                {
                    var distance = GeographyComputation.Distance(state.CurrentLatitude, state.CurrentLongitude,
                        state.CurrentNode.Latitude, state.CurrentNode.Longitude);
                    if (distance <= 40.0)
                    {
                        await turnContext.ReplyActivity(
                            $"Bravo, vous avez rejoint le point de controle {state.CurrentNode.Name}, cela a fait gagner {state.CurrentNode.Points} points à votre équipe");
                        var actionRequest = new GameActionRequest()
                        {
                            GameId = state.GameId,
                            TeamId = state.TeamId,
                            Action = (int)Action.VisitWaypoint,
                            Latitude = state.CurrentLatitude,
                            Longitude = state.CurrentLongitude,
                            NodeId = state.CurrentNodeId,
                            PointsEarned = state.CurrentNode.Points
                        };
                        await _actionWebService.LogAction(actionRequest);
                        state.CurrentNode = await _nodeWebService.GetNode(state.CurrentNodeId);
                        var nextNode = await _nodeWebService.GetNode(state.CurrentNode.ChildNodeIds.First());
                        state.CurrentNode = nextNode;
                        state.CurrentNodeId = nextNode.Id;
                        var displayDialog = _scope.Resolve<IDisplayNodeDialog>();
                        await turnContext.Begin(displayDialog);
                    }
                }

                var logPositionRequest = new LogPositionRequest()
                {
                    GameId = state.GameId,
                    TeamId = state.TeamId,
                    Latitude = state.CurrentLatitude,
                    Longitude = state.CurrentLongitude
                };
                await _actionWebService.LogPosition(logPositionRequest);

            }
            finally
            {
                await turnContext.End();
            }
        }

        public override string Command => "/location";

        public ReceiveLocationDialog(IActionWebService actionWebService, 
            INodeWebService nodeWebService, ILogger<ReceiveLocationDialog> logger, ILifetimeScope scope) : base(logger)
        {
            _actionWebService = actionWebService;
            _nodeWebService = nodeWebService;
            _scope = scope;
        }
    }
}