using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using FakeItEasy;
using ImageHuntBotBuilder;
using ImageHuntBotBuilder.Middlewares;
using ImageHuntWebServiceClient.Responses;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using TestUtilities;
using Xunit;

namespace ImageHuntBotBuilderTest.Middlewares
{
    public class NewParticipantMiddlewareTest : BaseTest<NewParticipantMiddleware>
    {
        private ILogger<NewParticipantMiddleware> _logger;
        private ITurnContext _turnContext;
        private NextDelegate _nextDelegate;
        private ITeamWebService _teamWebService;
        private ImageHuntBotAccessors _accessor;
        private IStatePropertyAccessor<ImageHuntState> _statePropertyAccessor;

        public NewParticipantMiddlewareTest()
        {
            _testContainerBuilder.RegisterInstance(_logger = A.Fake<ILogger<NewParticipantMiddleware>>());
            _testContainerBuilder.RegisterInstance(_teamWebService = A.Fake<ITeamWebService>());
            _testContainerBuilder.RegisterInstance(_accessor = A.Fake<ImageHuntBotAccessors>());
            _statePropertyAccessor = A.Fake<IStatePropertyAccessor<ImageHuntState>>();
            A.CallTo(() => _accessor.ImageHuntState).Returns(_statePropertyAccessor);

            _turnContext = A.Fake<ITurnContext>();
            _nextDelegate = A.Fake<NextDelegate>();
            Build();
        }

        [Fact]
        public async Task Should_Add_New_Player_in_team()
        {
            // Arrange
            var activity = new Activity(type: ImageHuntActivityTypes.NewPlayer,
                attachments: new List<Attachment>()
                {
                    new Attachment()
                    {
                        ContentType = ImageHuntActivityTypes.NewPlayer,
                        Content = new ConversationAccount() {Name = "toto"}
                    }
                });
            A.CallTo(() => _turnContext.Activity).Returns(activity);
            ImageHuntState state = new ImageHuntState()
                {TeamId = 56, Team = new TeamResponse() {Name = "Team1"}};
            A.CallTo(() =>
                    _statePropertyAccessor.GetAsync(A<ITurnContext>._, A<Func<ImageHuntState>>._,
                        A<CancellationToken>._))
                .Returns(state);
            // Act
            await _target.OnTurnAsync(_turnContext, _nextDelegate);
            // Assert
            A.CallTo(() => _nextDelegate.Invoke(A<CancellationToken>._)).MustNotHaveHappened();
        }
        [Fact]
        public async Task Should_Raise_Error_If_TeamId_Not_Set()
        {
            // Arrange
            var activity = new Activity(type: ImageHuntActivityTypes.NewPlayer,
                attachments: new List<Attachment>()
                {
                    new Attachment()
                    {
                        ContentType = ImageHuntActivityTypes.NewPlayer,
                        Content = new ConversationAccount() {Name = "toto"}
                    }
                });
            A.CallTo(() => _turnContext.Activity).Returns(activity);
            ImageHuntState state = new ImageHuntState()
                {};
            A.CallTo(() =>
                    _statePropertyAccessor.GetAsync(A<ITurnContext>._, A<Func<ImageHuntState>>._,
                        A<CancellationToken>._))
                .Returns(state);
            // Act
            await _target.OnTurnAsync(_turnContext, _nextDelegate);
            // Assert
            A.CallTo(() => _nextDelegate.Invoke(A<CancellationToken>._)).MustNotHaveHappened();
        }

        [Fact]
        public async Task Should_Non_Add_User_Should_call_next()
        {
            // Arrange
            var activity = new Activity(type: ActivityTypes.Message);
            A.CallTo(() => _turnContext.Activity).Returns(activity);
            // Act
            await _target.OnTurnAsync(_turnContext, _nextDelegate);
            // Assert
            A.CallTo(() => _nextDelegate.Invoke(A<CancellationToken>._)).MustHaveHappened();
        }
    }
}