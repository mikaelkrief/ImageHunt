using System.Threading.Tasks;
using ImageHuntCore.Model.Node;
using ImageHuntWebServiceClient.Responses;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Bot.Builder;
using Microsoft.Extensions.Logging;

namespace ImageHuntBotBuilder.Commands
{
    [Command("displayHints")]
    public class DisplayHintsCommand : AbstractCommand, IDisplayHintsCommand
    {
        private readonly INodeWebService _nodeWebService;
        public override bool IsAdmin => false;

        public DisplayHintsCommand(ILogger<IDisplayHintsCommand> logger, INodeWebService nodeWebService) : base(logger)
        {
            _nodeWebService = nodeWebService;
        }

        protected override async Task InternalExecute(ITurnContext turnContext, ImageHuntState state)
        {
            if (state.Status != Status.Started)
            {
                await turnContext.SendActivityAsync(
                    $"La partie n'as pas encore commencée, veuillez demander au maitre du jeu");
                return;
            }
            var nodes = await _nodeWebService.GetNodesByType(NodeTypes.Hidden, state.GameId.Value);
            await turnContext.SendActivityAsync("Voici des indices vous permettant de trouver les Noeuds bonus:");
            foreach (var nodeResponse in nodes)
            {
                switch (nodeResponse.NodeType)
                {
                    case NodeResponse.BonusNodeType:
                        string bonusType = string.Empty;
                        switch (nodeResponse.BonusType)
                        {
                            case BonusNode.BONUS_TYPE.Points_x2:
                                bonusType = "Multiplication du score final par 2";
                                break;
                            case BonusNode.BONUS_TYPE.Points_x3:
                                bonusType = "Multiplication du score final par 3";
                                break;
                        }
                        await turnContext.SendActivityAsync($"Nom : {nodeResponse.Name}\nIndice : {nodeResponse.Hint}\nBonus: {bonusType}");
                        break;
                    case NodeResponse.HiddenNodeType:
                        await turnContext.SendActivityAsync($"Nom : {nodeResponse.Name}\nIndice : {nodeResponse.Hint}\nBonus: {nodeResponse.Points} points");
                        break;
                }
            }
        }
    }
}