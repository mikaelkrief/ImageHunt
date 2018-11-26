using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Extensions.Logging;

namespace ImageHuntBotBuilder.Commands
{
    [Command("state")]
    public class DisplayStateCommand : AbstractCommand, IDisplayStateCommand
    {
        public DisplayStateCommand(ILogger<IDisplayStateCommand> logger) : base(logger)
        {
        }

        public override bool IsAdmin => true;

        protected override async Task InternalExecute(ITurnContext turnContext, ImageHuntState state)
        {
            if (state != null)
            {
                var relyBuilder = new StringBuilder();
                relyBuilder.AppendLine($"ConversationId: {state.ConversationId}");
                relyBuilder.AppendLine($"Game Status: {state.Status}");
                if (state.Game != null)
                {
                    relyBuilder.AppendLine(
                        $"Game: (Id:{state.Game.Id}, Name: {state.Game.Name}, StartDate: {state.Game.StartDate})");
                }

                if (state.Team != null)
                {
                    relyBuilder.AppendLine($"Team: (Id: {state.Team.Id}, Name: {state.Team.Name})");
                }

                if (state.CurrentLocation != null)
                {
                    relyBuilder.AppendLine(
                        $"CurrentLocation: (Lat:{state.CurrentLocation.Latitude}, Lng: {state.CurrentLocation.Longitude})");
                }

                if (state.CurrentNode != null)
                {
                    string childs = string.Empty;
                    if (state.CurrentNode.ChildNodeIds != null)
                    {
                        childs = state.CurrentNode.ChildNodeIds.Aggregate(string.Empty, (current, next) => current.ToString() + ", " + next.ToString());
                    }

                    relyBuilder.AppendLine(
                        $"CurrentNode: (Id: {state.CurrentNode.Id}, Name: {state.CurrentNode.Name}, Location: [lat:{state.CurrentNode.Latitude}, {state.CurrentNode.Longitude}]) Childs: [{childs}]");
                }

                await turnContext.SendActivityAsync(relyBuilder.ToString());
            }
        }
    }
}