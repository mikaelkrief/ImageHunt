using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using ImageHuntCore.Computation;
using ImageHuntWebServiceClient.Responses;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

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

        public NodeVisitorHandler(ILogger<NodeVisitorHandler> logger, INodeWebService nodeWebService, ILifetimeScope scope, IConfiguration configuration)
        {
            _logger = logger;
            _nodeWebService = nodeWebService;
            _scope = scope;
            _configuration = configuration;
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
                    switch (state.CurrentNode.NodeType)
                    {
                        case NodeResponse.FirstNodeType:
                        case NodeResponse.ObjectNodeType:
                            var nextNodeId = state.CurrentNode.ChildNodeIds.First();
                            nextNode = await _nodeWebService.GetNode(nextNodeId);
                            await context.SendActivityAsync($"Le prochain point de contrôle est {nextNode.Name}");
                            var nextActivity = new Activity(type: ImageHuntActivityTypes.Location,
                                attachments: new List<Attachment>()
                                {
                                    new Attachment(ImageHuntActivityTypes.Location,
                                        content: new GeoCoordinates(latitude: nextNode.Latitude,
                                            longitude: nextNode.Longitude))
                                });
                            await context.SendActivityAsync(nextActivity);
                            break;
                    }
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
    }
}