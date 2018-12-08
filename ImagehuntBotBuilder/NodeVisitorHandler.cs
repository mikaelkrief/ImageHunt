using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using ImageHuntBotBuilder.Commands;
using ImageHuntCore.Computation;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.Responses;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Action = ImageHuntCore.Model.Action;

namespace ImageHuntBotBuilder
{
    public interface INodeVisitorHandler
    {
        Task<NodeResponse> MatchLocationAsync(ITurnContext context, ImageHuntState state);
    }

    public class NodeVisitorHandler : INodeVisitorHandler
    {
        private readonly ILogger<NodeVisitorHandler> _logger;
        private readonly INodeWebService _nodeWebService;
        private readonly ILifetimeScope _scope;
        private readonly IConfiguration _configuration;
        private readonly IActionWebService _actionWebService;

        public NodeVisitorHandler(ILogger<NodeVisitorHandler> logger,
            INodeWebService nodeWebService,
            ILifetimeScope scope,
            IConfiguration configuration,
            IActionWebService actionWebService)
        {
            _logger = logger;
            _nodeWebService = nodeWebService;
            _scope = scope;
            _configuration = configuration;
            _actionWebService = actionWebService;
        }

        public async Task<NodeResponse> MatchLocationAsync(ITurnContext context, ImageHuntState state)
        {
            var activity = context.Activity;
            var location = activity.Attachments.First().Content as GeoCoordinates;
            // Check that location match the current node
            var distance = GeographyComputation.Distance(location.Latitude.Value, location.Longitude.Value, state.CurrentNode.Latitude,
                state.CurrentNode.Longitude);
            NodeResponse nextNode = null;
            var rangeDistance = Convert.ToDouble(_configuration["NodeSettings:RangeDistance"]);
            try
            {
                if (distance < rangeDistance)
                {
                    await context.SendActivityAsync(
                        $"Vous avez rejoint le point de controle {state.CurrentNode.Name}, bravo!");
                    var actionRequest = new GameActionRequest()
                    {
                        Action = (int)ActionFromNodeType(state.CurrentNode.NodeType),
                        Latitude = location.Latitude.Value,
                        Longitude = location.Longitude.Value,
                        GameId = state.GameId.Value,
                        TeamId = state.TeamId.Value,
                        NodeId = state.CurrentNode.Id,
                    };
                    switch (state.CurrentNode.NodeType)
                    {
                        case NodeResponse.FirstNodeType:
                        case NodeResponse.ObjectNodeType:
                        case NodeResponse.HiddenNodeType:
                            var nextNodeId = state.CurrentNode.ChildNodeIds.First();
                            nextNode = await _nodeWebService.GetNode(nextNodeId);
                            var nextActivities = ActivitiesFromNode(nextNode);
                            foreach (var nextActivity in nextActivities)
                            {
                                await context.SendActivityAsync(nextActivity);
                            }
                            actionRequest.PointsEarned = state.CurrentNode.Points;
                            break;
                        case NodeResponse.LastNodeType:
                            await context.SendActivityAsync($"Pour valider la fin de votre chasse, prévenez un orga");
                            actionRequest.PointsEarned = state.CurrentNode.Points;

                            break;
                    }

                    await _actionWebService.LogAction(actionRequest);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception while handling activity: {activity.Type}");
            }
            finally
            {
                state.CurrentNode = nextNode;

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

        public IEnumerable<Activity> ActivitiesFromNode(NodeResponse node)
        {
            var activities = new List<Activity>();
            switch (node.NodeType)
            {
                case NodeResponse.ObjectNodeType:
                    activities.Add(new Activity(text: $"Le prochain noeud {node.Name} et se trouve à l'emplacement suivant:"));
                    activities.Add(new Activity(type:ImageHuntActivityTypes.Location){Attachments = new List<Attachment>()
                        {
                            new Attachment(
                                contentType: ImageHuntActivityTypes.Location,
                                content: new GeoCoordinates(
                                    latitude: node.Latitude,
                                    longitude: node.Longitude)),
                        }
                    });
                    activities.Add(new Activity(text:$"Vous devrez effectuer l'action suivante : {node.Action}"));

                    break;
                case NodeResponse.HiddenNodeType:
                    activities.Add(new Activity(text: $"Le prochain noeud {node.Name} est un noeud mystère. L'indice suivant devrait vour permettre de deviner sa position"));
                    activities.Add(new Activity(text:node.Hint));
                    break;
                case NodeResponse.LastNodeType:
                    activities.Add(new Activity(text: $"Le prochain point de contrôle est l'arrivée! Il se trouve à la position suivante:"));
                    activities.Add(new Activity(type: ImageHuntActivityTypes.Location)
                    {
                        Attachments = new List<Attachment>()
                        {
                            new Attachment(
                                contentType: ImageHuntActivityTypes.Location,
                                content: new GeoCoordinates(
                                    latitude: node.Latitude,
                                    longitude: node.Longitude)),
                        }
                    });
                    break;
            }

            return activities;
        }
    }
}