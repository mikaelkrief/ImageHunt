using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using FakeItEasy;
using ImageHuntBotBuilder;
using ImageHuntBotBuilder.Middlewares;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using NFluent;
using TestUtilities;
using Xunit;

namespace ImageHuntBotBuilderTest.Middlewares
{
    public class LogFakePositionMiddlewareTest : BaseTest<LogFakePositionMiddleware>
    {
        private LogFakePositionMiddleware _logger;
        private ITurnContext _turnContext;
        private NextDelegate _nextDelegate;
        private IStorage _storage;
        private ConversationState _conversationState;
        private ImageHuntBotAccessors _accessor;
        private IStatePropertyAccessorExtended<ImageHuntState> _statePropertyAccessor;

        public LogFakePositionMiddlewareTest()
        {
            _testContainerBuilder.RegisterInstance(_logger = A.Fake<LogFakePositionMiddleware>());
            _statePropertyAccessor = A.Fake<IStatePropertyAccessorExtended<ImageHuntState>>();

            _storage = A.Fake<IStorage>();
            _conversationState = new ConversationState(_storage);
            _accessor = new ImageHuntBotAccessors(_conversationState);
            _accessor.ImageHuntState = _statePropertyAccessor;
            _testContainerBuilder.RegisterInstance(_accessor);

            _turnContext = A.Fake<ITurnContext>();
            _nextDelegate = A.Fake<NextDelegate>();
            Build();
        }

        [Fact]
        public async Task Should_Intercept_Location_Command_And_Store_Location()
        {
            // Arrange
            var activity = new Activity(type: ActivityTypes.Message, text: "/location lat=54.7 lng=5.87");

            A.CallTo(() => _turnContext.Activity).Returns(activity);
            // Act
            await _target.OnTurnAsync(_turnContext, _nextDelegate);
            // Assert
        }
    }
}
