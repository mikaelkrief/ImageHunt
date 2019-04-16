using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace ImageHuntBotBuilder.Commands
{
    [Command("actions")]
    public class ActionCommand : AbstractCommand, IActionCommand
    {
        public override bool IsAdmin => false;

        public ActionCommand(ILogger<IActionCommand> logger, IStringLocalizer<ActionCommand> localizer)
            : base(logger, localizer)
        {
        }

        protected override async Task InternalExecuteAsync(ITurnContext turnContext, ImageHuntState state)
        {
            if (state.Status != Status.Started)
            {
                await turnContext.SendActivityAsync(
                    Localizer["GAME_NOT_STARTED"]);
                return;
            }

            var nodes = state.ActionNodes;
            await turnContext.SendActivityAsync(Localizer["ACTION_NODES_TITLE"]);
            var activities = new List<IActivity>();
            foreach (var nodeResponse in nodes)
            {
                var activity = new Activity()
                {
                    Type = ImageHuntActivityTypes.Location,
                    Text = nodeResponse.Action,
                    Attachments = new List<Attachment>()
                    {
                        new Attachment(
                            contentType: ImageHuntActivityTypes.Location,
                            content: new GeoCoordinates(
                                latitude: nodeResponse.Latitude,
                                longitude: nodeResponse.Longitude
                            )),
                    },
                };
                activities.Add(activity);
            }

            await turnContext.SendActivitiesAsync(activities.ToArray());
        }
    }
}