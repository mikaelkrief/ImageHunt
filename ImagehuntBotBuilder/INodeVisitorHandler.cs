using System.Threading.Tasks;
using ImageHuntWebServiceClient.Responses;
using Microsoft.Bot.Builder;

namespace ImageHuntBotBuilder
{
    public interface INodeVisitorHandler
    {
        Task<NodeResponse> MatchLocationAsync(ITurnContext context, ImageHuntState state);

        Task MatchHiddenNodesLocationAsync(ITurnContext turnContext, ImageHuntState state);
    }
}