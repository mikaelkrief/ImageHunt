using System.Collections.Generic;
using System.Threading.Tasks;
using ImageHuntBotBuilder.Commands.Interfaces;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace ImageHuntBotBuilder.Commands
{
    [Command("next")]
    public class DisplayNodeCommand : AbstractCommand, IDisplayNodeCommand
    {
        private readonly INodeWebService _nodeWebService;

        public DisplayNodeCommand(ILogger<IDisplayNodeCommand> logger, INodeWebService nodeWebService,
            IStringLocalizer<DisplayNodeCommand> localizer)
            : base(logger, localizer)
        {
            _nodeWebService = nodeWebService;
        }

        public override bool IsAdmin => false;

        protected override async Task InternalExecuteAsync(ITurnContext turnContext, ImageHuntState state)
        {
            if (state.Status != Status.Started)
            {
                Logger.LogInformation("Game not initialized");
                await turnContext.SendActivityAsync("La partie n'a pas débuté, il n'y a donc pas de noeud courant!");
                return;
            }

            if (!state.CurrentNodeId.HasValue)
            {
                Logger.LogInformation("No current node");
                await turnContext.SendActivityAsync($"Aucun noeud courant, impossible de continuer. Prévenir les orga");
                return;
            }

            var node = await _nodeWebService.GetNode(state.CurrentNodeId.Value);
            var activity = new Activity
            {
                Type = ImageHuntActivityTypes.Location,
                Text = $"Le prochain point de controle {node.Name} se trouve à la position suivante :",
                Attachments = new List<Attachment>
                {
                    new Attachment
                    {
                        Content = new GeoCoordinates(latitude: node.Latitude, longitude: node.Longitude),
                        ContentType = ImageHuntActivityTypes.Location
                    }
                }
            };
            await turnContext.SendActivityAsync(activity);
        }
    }
}