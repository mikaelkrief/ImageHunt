using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageHuntBotBuilder.Commands.Interfaces;
using Microsoft.Bot.Builder;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace ImageHuntBotBuilder.Commands
{
    [Command("state")]
    public class DisplayStateCommand : AbstractCommand, IDisplayStateCommand
    {
        private readonly ImageHuntBotAccessors _accessors;

        public DisplayStateCommand(
            ILogger<IDisplayStateCommand> logger, 
            IStringLocalizer<DisplayStateCommand> localizer,
            ImageHuntBotAccessors accessors) : base(logger, localizer)
        {
            _accessors = accessors;
        }

        public override bool IsAdmin => true;

        protected override async Task InternalExecuteAsync(ITurnContext turnContext, ImageHuntState state)
        {
            if (state != null)
            {
                if (turnContext.Activity.Text.Contains("All", StringComparison.InvariantCultureIgnoreCase))
                {
                    var states = await _accessors.AllStates.GetAllAsync();
                    foreach (var imageHuntState in states)
                    {
                        await ComposeReplyAsync(turnContext, imageHuntState);
                    }
                }
                else
                {
                    await ComposeReplyAsync(turnContext, state);
                }
            }
        }

        private async Task ComposeReplyAsync(ITurnContext turnContext, ImageHuntState state)
        {
            var relyBuilder = new StringBuilder();
            relyBuilder.AppendLine($"ConversationId: {state.ConversationId}");
            if (state.Game != null)
            {
                relyBuilder.AppendLine(
                    $"Game: (Id:{state.Game.Id}, Name: {state.Game.Name}, StartDate: {state.Game.StartDate})");
            }
            relyBuilder.AppendLine($"Game Status: {state.Status}");

            if (state.Team != null)
            {
                relyBuilder.AppendLine(
                    $"Team: (Id: {state.Team.Id}, Name: {state.Team.Name}, Culture:{state.Team.CultureInfo})");
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
                    childs = string.Join(',', state.CurrentNode.ChildNodeIds);
                }

                relyBuilder.AppendLine(
                    $"CurrentNode: (Id: {state.CurrentNode.Id}, Name: {state.CurrentNode.Name}, Location: [lat:{state.CurrentNode.Latitude}, {state.CurrentNode.Longitude}]) Childs: [{childs}]");
            }

            if (state.HiddenNodes != null && state.HiddenNodes.Any())
            {
                var hiddenNodes = string.Empty;
                hiddenNodes = string.Join(',', state.HiddenNodes.Select(n => n.Name));
                relyBuilder.AppendLine($"Hidden nodes: [{hiddenNodes}]");
            }

            await turnContext.SendActivityAsync(relyBuilder.ToString());
        }
    }
}