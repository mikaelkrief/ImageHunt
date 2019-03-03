using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using FakeItEasy;
using ImageHuntBotBuilder;
using ImageHuntBotBuilder.Commands;
using ImageHuntBotBuilder.Commands.Interfaces;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using TestUtilities;
using Xunit;

namespace ImageHuntBotBuilderTest.Commands
{
    public class ExtractInviteLinkCommandTest : BaseTest<ExtractInviteLinkCommand>
    {
        private ILogger<IExtractInviteLinkCommand> _logger;
        private IStringLocalizer<ExtractInviteLinkCommand> _localizer;
        private ITurnContext _turnContext;
        private ImageHuntState _state;
        private ITeamWebService _teamWebService;

        public ExtractInviteLinkCommandTest()
        {
            _testContainerBuilder.RegisterInstance(_logger = A.Fake<ILogger<IExtractInviteLinkCommand>>());
            _testContainerBuilder.RegisterInstance(_localizer = A.Fake<IStringLocalizer<ExtractInviteLinkCommand>>());
            _testContainerBuilder.RegisterInstance(_teamWebService = A.Fake<ITeamWebService>());
            _turnContext = A.Fake<ITurnContext>();
            _state = new ImageHuntState();

            Build();
        }

        [Fact]
        public async Task Should_ExtractCommand_Fail_if_Group_not_initialized()
        {
            // Arrange
            
            // Act
            await _target.Execute(_turnContext, _state);
            // Assert
            A.CallTo(
                    () => _turnContext.SendActivityAsync(A<string>._, A<string>._, A<string>._, A<CancellationToken>._))
                .MustHaveHappened();
            A.CallTo(() => _logger.Log(LogLevel.Error, A<EventId>._, A<object>._, A<Exception>._,
                A<Func<object, Exception, string>>._)).MustHaveHappened();
        }

        [Fact]
        public async Task Should_ExtractInviteUrlCommand_Succeed()
        {
            // Arrange
            _state.Status = Status.Initialized;
            _state.TeamId = 15;
            var activity = new Activity(type: ImageHuntActivityTypes.GetInviteLink);
            A.CallTo(() => _turnContext.Activity).Returns(activity);
            A.CallTo(() => _turnContext.SendActivitiesAsync(A<IActivity[]>._, A<CancellationToken>._))
                .Invokes(x =>
                    x.GetArgument<Activity[]>(0)[0].Attachments = new List<Attachment>() {new Attachment(contentUrl: "https://toto")});
            // Act
            await _target.Execute(_turnContext, _state);
            // Assert
            A.CallTo(() => _turnContext.SendActivitiesAsync(A<Activity[]>._, A<CancellationToken>._))
                .MustHaveHappened();
            A.CallTo(() => _teamWebService.UpdateTeam(A<UpdateTeamRequest>._)).MustHaveHappened();
        }
    }

    public class ExtractInviteLinkCommand : AbstractCommand, IExtractInviteLinkCommand
    {
        private readonly ITeamWebService _teamWebService;

        public ExtractInviteLinkCommand(ILogger<IExtractInviteLinkCommand> logger, 
            IStringLocalizer<ExtractInviteLinkCommand> localizer, ITeamWebService teamWebService) : base(logger, localizer)
        {
            _teamWebService = teamWebService;
        }

        protected async override Task InternalExecute(ITurnContext turnContext, ImageHuntState state)
        {
            if (state.Status != Status.Initialized)
            {
                _logger.LogError($"Group not initialized, unable to extract Invite Url from non-initialzed group");
                await turnContext.SendActivityAsync(_localizer["NON_INITIALIZED_GROUP"]);
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
                InviteUrl = activities[0].Attachments[0].ContentUrl
            };
            await _teamWebService.UpdateTeam(updateTeamRequest);
        }
    }
}
