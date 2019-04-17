using System.Threading.Tasks;
using ImageHuntBotBuilder.Commands.Interfaces;
using ImageHuntBotCore;
using ImageHuntBotCore.Commands;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace ImageHuntBotBuilder.Commands
{
    [Command("inviteLink")]
    public class ExtractInviteLinkCommand : AbstractCommand<ImageHuntState>, IExtractInviteLinkCommand
    {
        private readonly ITeamWebService _teamWebService;

        public ExtractInviteLinkCommand(
            ILogger<IExtractInviteLinkCommand> logger, 
            IStringLocalizer<ExtractInviteLinkCommand> localizer, ITeamWebService teamWebService) : base(logger, localizer)
        {
            _teamWebService = teamWebService;
        }

        protected async override Task InternalExecuteAsync(ITurnContext turnContext, ImageHuntState state)
        {
            if (state.Status != Status.Initialized)
            {
                Logger.LogError("Group not initialized, unable to extract Invite Url from non-initialzed group");
                await turnContext.SendActivityAsync(Localizer["NON_INITIALIZED_GROUP"]);
                return;
            }

            var activities = new Activity[]
            {
                new Activity(type: ImageHuntActivityTypes.GetInviteLink),
            };
            await turnContext.SendActivitiesAsync(activities);
            var updateTeamRequest = new UpdateTeamRequest()
            {
                TeamId = state.TeamId.Value,
                InviteUrl = activities[0].Attachments[0].ContentUrl,
            };
            await _teamWebService.UpdateTeam(updateTeamRequest);
        }
    }
}