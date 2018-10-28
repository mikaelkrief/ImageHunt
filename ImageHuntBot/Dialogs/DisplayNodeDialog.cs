using System.Threading.Tasks;
using ImageHuntTelegramBot;
using ImageHuntTelegramBot.Dialogs;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;

namespace ImageHuntBot.Dialogs
{
    public class DisplayNodeDialog : AbstractDialog, IDisplayNodeDialog
    {
        private readonly INodeWebService _nodeWebService;

        public DisplayNodeDialog(ILogger<DisplayNodeDialog> logger, INodeWebService nodeWebService)
            : base(logger)
        {
            _nodeWebService = nodeWebService;
        }

        public async override Task Begin(ITurnContext turnContext, bool overrideAdmin = false)
        {
            try
            {
                var state = turnContext.GetConversationState<ImageHuntState>();
                if (state.Status != Status.Started)
                {
                    LogInfo<ImageHuntState>(turnContext, "Game not initialized");
                    var replyActivity = new Activity(){ActivityType = ActivityType.Message, Text = "La partie n'a pas débuté, il n'y a donc pas de noeud courant!"};
                    await turnContext.ReplyActivity(replyActivity);
                    await turnContext.End();
                    return;
                }

                if (state.CurrentNodeId == 0)
                {
                    LogInfo<ImageHuntState>(turnContext, "No current node");
                    await turnContext.ReplyActivity($"Aucun noeud courant, impossible de continuer. Prévenir les orga");
                    await turnContext.End();
                    return;
                }
                var node = await _nodeWebService.GetNode(state.CurrentNodeId);
                await turnContext.ReplyActivity(
                    $"Le prochain point de controle {node.Name} se trouve à la position suivante :");
                var activity = new Activity()
                {
                    ChatId = state.ChatId,
                    ActivityType = ActivityType.Location,
                    Location = new Location()
                    {
                        Latitude = (float)node.Latitude,
                        Longitude = (float)node.Longitude
                    }
                };

                await turnContext.SendActivity(activity);

            }
            finally
            {
                await turnContext.End();
                
            }

        }

        public override string Command => "/DisplayNode";
        public override bool IsAdmin => false;
    }
}