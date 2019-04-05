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
        public override bool IsAdmin => false;

        public DisplayNodeCommand(ILogger<IDisplayNodeCommand> logger, INodeWebService nodeWebService, IStringLocalizer<DisplayNodeCommand> localizer) 
            : base(logger, localizer)
        {
            _nodeWebService = nodeWebService;
        }

        protected override async Task InternalExecute(ITurnContext turnContext, ImageHuntState state)
        {
            if (state.Status != Status.Started)
            {
                _logger.LogInformation("Game not initialized");
                await turnContext.SendActivityAsync(_localizer["GAME_NOT_STARTED"]);
                return;
            }


            if (!state.CurrentNodeId.HasValue)
            {
                _logger.LogInformation("No current node");
                await turnContext.SendActivityAsync(_localizer["NO_CURRENT_NODE"]);
                return;
            }

            var node = await _nodeWebService.GetNode(state.CurrentNode.Id);
            var activity = new Activity()
            {
                Type = ImageHuntActivityTypes.Location,
                Text = _localizer["NEXT_NODE_POSITION", node.Name],
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