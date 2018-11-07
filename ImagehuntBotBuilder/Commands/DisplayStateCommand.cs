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
        protected async override Task InternalExecute(ITurnContext turnContext, ImageHuntState state)
        {
            if (state != null)
            {
                var relyBuilder = new StringBuilder();
                relyBuilder.AppendLine($"Game Status: {state.Status}");
                relyBuilder.AppendLine(
                    $"Game: (Id:{state.Game.Id}, Name: {state.Game.Name}, StartDate: {state.Game.StartDate})");
                relyBuilder.AppendLine($"Team: (Id: {state.Team.Id}, Name: {state.Team.Name})");
                relyBuilder.AppendLine(
                    $"CurrentLocation: (Lat:{state.CurrentLocation.Latitude}, Lng: {state.CurrentLocation.Longitude})");
                await turnContext.SendActivityAsync(relyBuilder.ToString());
            }
        }
    }
}