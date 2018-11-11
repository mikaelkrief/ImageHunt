using System.Collections.Generic;
using System.Threading.Tasks;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;

namespace ImageHuntBotBuilder.Commands
{
    [Command("displayNode")]
    public class DisplayNodeCommand : AbstractCommand, IDisplayNodeCommand
    {
        private readonly INodeWebService _nodeWebService;

        public DisplayNodeCommand(ILogger<IDisplayNodeCommand> logger, INodeWebService nodeWebService) 
            : base(logger)
        {
            _nodeWebService = nodeWebService;
        }

        protected override async Task InternalExecute(ITurnContext turnContext, ImageHuntState state)
        {
            if (state.Status != Status.Started)
            {
                _logger.LogInformation("Game not initialized");
                await turnContext.SendActivityAsync("La partie n'a pas débuté, il n'y a donc pas de noeud courant!");
                return;
            }


            if (!state.CurrentNodeId.HasValue)
            {
                _logger.LogInformation("No current node");
                await turnContext.SendActivityAsync($"Aucun noeud courant, impossible de continuer. Prévenir les orga");
                return;
            }

            var node = await _nodeWebService.GetNode(state.CurrentNodeId.Value);
            var activity = new Activity()
            {
                Type = ImageHuntActivityTypes.Location,
                Text = $"Le prochain point de controle {node.Name} se trouve à la position suivante :",
                Attachments = new List<Attachment>() { new Attachment()
                {
                    Content = new GeoCoordinates(latitude:node.Latitude, longitude:node.Longitude),
                    ContentType = ImageHuntActivityTypes.Location
                } }
            };
            await turnContext.SendActivityAsync(activity);
        }
    }
}