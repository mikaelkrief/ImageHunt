using System;
using System.Collections.Generic;
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
            TestContainerBuilder.RegisterInstance(_logger = A.Fake<ILogger<IExtractInviteLinkCommand>>());
            TestContainerBuilder.RegisterInstance(_localizer = A.Fake<IStringLocalizer<ExtractInviteLinkCommand>>());
            TestContainerBuilder.RegisterInstance(_teamWebService = A.Fake<ITeamWebService>());
            _turnContext = A.Fake<ITurnContext>();
            _state = new ImageHuntState();

            Build();
        }

        [Fact]
        public async Task Should_ExtractCommand_Fail_if_Group_not_initialized()
        {
            // Arrange
            
            // Act
            await Target.ExecuteAsync(_turnContext, _state);
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
            await Target.ExecuteAsync(_turnContext, _state);
            // Assert
            A.CallTo(() => _turnContext.SendActivitiesAsync(A<Activity[]>._, A<CancellationToken>._))
                .MustHaveHappened();
            A.CallTo(() => _teamWebService.UpdateTeam(A<UpdateTeamRequest>._)).MustHaveHappened();
        }
    }
}
