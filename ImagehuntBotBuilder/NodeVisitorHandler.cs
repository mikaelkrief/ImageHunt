using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using ImageHuntCore.Computation;
using ImageHuntCore.Model.Node;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.Responses;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Action = ImageHuntCore.Model.Action;
using Attachment = Microsoft.Bot.Schema.Attachment;

namespace ImageHuntBotBuilder
{
    public interface INodeVisitorHandler
    {
        Task<NodeResponse> MatchLocationAsync(ITurnContext context, ImageHuntState state);
        Task MatchHiddenNodesLocationAsync(ITurnContext turnContext, ImageHuntState state);
    }

    public class NodeVisitorHandler : INodeVisitorHandler
    {
        private readonly ILogger<NodeVisitorHandler> _logger;
        private IStringLocalizer _localizer;
        private readonly INodeWebService _nodeWebService;
        private readonly ILifetimeScope _scope;
        private readonly IConfiguration _configuration;
        private readonly IActionWebService _actionWebService;

        public NodeVisitorHandler(ILogger<NodeVisitorHandler> logger,
            IStringLocalizer<NodeVisitorHandler> localizer, 
            INodeWebService nodeWebService,
            ILifetimeScope scope,
            IConfiguration configuration,
            IActionWebService actionWebService)
        {
            _logger = logger;
            _localizer = localizer;
            _nodeWebService = nodeWebService;
            _scope = scope;
            _configuration = configuration;
            _actionWebService = actionWebService;
        }

        public async Task<NodeResponse> MatchLocationAsync(ITurnContext context, ImageHuntState state)
        {
            var activity = context.Activity;
            var location = activity.Attachments.First().Content as GeoCoordinates;
            _localizer = _localizer.WithCulture(new CultureInfo(state.Team.CultureInfo));
            if (state.Status != Status.Started)
                return null;
            if (state.CurrentNode == null)
                return null;
            // Check that location match the current node
            var distance = GeographyComputation.Distance(location.Latitude.Value, location.Longitude.Value, state.CurrentNode.Latitude,
                state.CurrentNode.Longitude);
            NodeResponse nextNode = null;
            var rangeDistance = Convert.ToDouble(_configuration["NodeSettings:RangeDistance"]);
            try
            {
                if (distance <= rangeDistance)
                {
                    await context.SendActivityAsync(
                        _localizer["WAYPOINT_REACHED", state.CurrentNode.Name]);
                    var actionRequest = new GameActionRequest()
                    {
                        Action = (int)ActionFromNodeType(state.CurrentNode.NodeType),
                        Latitude = location.Latitude.Value,
                        Longitude = location.Longitude.Value,
                        GameId = state.GameId.Value,
                        TeamId = state.TeamId.Value,
                        NodeId = state.CurrentNode.Id,
                    };
                    IList<Activity> nextActivities = null;
                    switch (state.CurrentNode.NodeType)
                    {
                        case NodeResponse.FirstNodeType:
                        case NodeResponse.ObjectNodeType:
                        case NodeResponse.WaypointNodeType:
                            var nextNodeId = state.CurrentNode.ChildNodeIds.First();
                            nextNode = await _nodeWebService.GetNode(nextNodeId);
                            nextActivities = ActivitiesFromNode(nextNode);
                            actionRequest.PointsEarned = state.CurrentNode.Points;
                            state.CurrentNode = nextNode;
                            state.CurrentNodeId = nextNode.Id;
                          break;
                        case NodeResponse.LastNodeType:
                            nextActivities = new List<Activity>();
                            nextActivities.Add(new Activity(text:_localizer["LASTNODE_REACHED"], type: ActivityTypes.Message));
                            actionRequest.PointsEarned = state.CurrentNode.Points;

                            break;
                    }
                    foreach (var nextActivity in nextActivities)
                    {
                        await context.SendActivityAsync(nextActivity);
                    }

                    await _actionWebService.LogAction(actionRequest);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception while handling activity: {0}", activity.Type);
            }
            return nextNode;
        }

        private Action ActionFromNodeType(string currentNodeNodeType)
        {
            switch (currentNodeNodeType)
            {
                case NodeResponse.FirstNodeType:
                    return Action.StartGame;
                case NodeResponse.ObjectNodeType:
                    return Action.DoAction;
                case NodeResponse.LastNodeType:
                    return Action.EndGame;
                default:
                    return Action.None;
            }
        }

        public IList<Activity> ActivitiesFromNode(NodeResponse node)
        {
            var activities = new List<Activity>();
            switch (node.NodeType)
            {
                case NodeResponse.ObjectNodeType:
                    activities.Add(new Activity(text: _localizer["NEXT_NODE_LOCATION", node.Name], type:ActivityTypes.Message));
                    var item = new Activity(type:ImageHuntActivityTypes.Location);
                    item.Attachments = new List<Attachment>()
                    {
                        new Attachment(
                            contentType: ImageHuntActivityTypes.Location,
                            content: new GeoCoordinates(
                                latitude: node.Latitude,
                                longitude: node.Longitude)),
                    };
                    activities.Add(item);
                    var imageActivity = new Activity(text: _localizer["DO_ACTION_REQUEST", node.Action], type: ImageHuntActivityTypes.Image);
                    var apiBaseAddress = _configuration["ImageHuntApi:Url"];
                    if (node.Image != null)
                    {
                        imageActivity.Attachments = new List<Attachment>
                        {
                            new Attachment(
                                contentType: ImageHuntActivityTypes.Image,
                                contentUrl: $"{apiBaseAddress}/api/Image/{node.Image?.Id}"),
                        };
                    }

                    activities.Add(imageActivity);

                    break;
                case NodeResponse.WaypointNodeType:
                    activities.Add(new Activity(text: _localizer["NEXT_NODE_LOCATION", node.Name], type: ActivityTypes.Message));
                    var activity = new Activity(type:ImageHuntActivityTypes.Location);
                    activity.Attachments = new List<Attachment>()
                    {
                        new Attachment(
                            contentType: ImageHuntActivityTypes.Location,
                            content: new GeoCoordinates(
                                latitude: node.Latitude,
                                longitude: node.Longitude)),
                    };
                    activities.Add(activity);

                    break;
                case NodeResponse.HiddenNodeType:
                    activities.Add(new Activity(text: _localizer["HIDDEN_NODE", node.Name], type: ActivityTypes.Message));
                    activities.Add(new Activity(text:node.Hint, type: ActivityTypes.Message));
                    break;
                case NodeResponse.LastNodeType:
                    activities.Add(new Activity(text: _localizer["NEXT_LAST_NODE"], type: ActivityTypes.Message));
                    var item1 = new Activity(type: ImageHuntActivityTypes.Location);
                    item1.Attachments = new List<Attachment>()
                    {
                        new Attachment(
                            contentType: ImageHuntActivityTypes.Location,
                            content: new GeoCoordinates(
                                latitude: node.Latitude,
                                longitude: node.Longitude)),
                    };
                    activities.Add(item1);

                    break;
                case NodeResponse.TimerNodeType:
                    activities.Add(new Activity(text: _localizer["TIMER_NODE", node.Delay], type: ActivityTypes.Message));
                    activities.Add(new Activity(type: ImageHuntActivityTypes.Wait, attachments: new List<Attachment>(){new Attachment(content: node.Delay)}));
                    break;
            }

            return activities;
        }

        public async Task MatchHiddenNodesLocationAsync(ITurnContext turnContext, ImageHuntState state)
        {
            if (state.Status != Status.Started)
                return ;

            var rangeDistance = Convert.ToDouble(_configuration["NodeSettings:RangeDistance"]);
            if (state.HiddenNodes == null || !state.HiddenNodes.Any())
                return;
            foreach (var hiddenNode in state.HiddenNodes)
            {
                var activity = turnContext.Activity;
                var location = activity.Attachments.First().Content as GeoCoordinates;
                // Check that location match the current node
                var distance = GeographyComputation.Distance(location.Latitude.Value, location.Longitude.Value, hiddenNode.Latitude,
                    hiddenNode.Longitude);
                if (distance <= rangeDistance)
                {
                    await turnContext.SendActivityAsync(_localizer["WAYPOINT_REACHED", hiddenNode.Name]);
                    var actionRequest = new GameActionRequest()
                    {
                        Latitude = location.Latitude,
                        Longitude = location.Longitude,
                        GameId = state.GameId.Value,
                        TeamId = state.TeamId.Value,
                        PointsEarned = hiddenNode.Points,
                        NodeId = hiddenNode.Id,
                    };
                    state.HiddenNodes = state.HiddenNodes.Where(n => n.Id != hiddenNode.Id).ToArray();
                    switch (hiddenNode.NodeType)
                    {
                        case NodeResponse.BonusNodeType:
                            string multi;
                            actionRequest.Action = (int) Action.BonusNode;
                            switch (hiddenNode.BonusType)
                            {
                                case BonusNode.BONUS_TYPE.Points_x2:
                                    multi = "2 fois";
                                    actionRequest.PointsEarned = 2;
                                    break;
                                case BonusNode.BONUS_TYPE.Points_x3:
                                    multi = "3 fois";
                                    actionRequest.PointsEarned = 3;
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }

                            actionRequest.Action = (int)Action.BonusNode;
                            await _actionWebService.LogAction(actionRequest);
                            await turnContext.SendActivityAsync(
                                _localizer["BONUS_NODE", multi]);

                            break;
                        case NodeResponse.HiddenNodeType:
                            actionRequest.Action = (int)Action.HiddenNode;
                            actionRequest.PointsEarned = hiddenNode.Points;
                            await turnContext.SendActivityAsync(
                                _localizer["EARN_POINTS", hiddenNode.Points]);
                            await _actionWebService.LogAction(actionRequest);
                            break;
                    }

                    await turnContext.SendActivityAsync(_localizer["ASK_SELFIE"]);
                }
            }
        }

        public async Task MatchLocationDialogAsync(NodeResponse node, IStatePropertyAccessor<DialogState> conversationDialogState)
        {
            var dialogSet = new DialogSet(conversationDialogState);
        }
    }
}
