using System.Threading;
using System.Threading.Tasks;
using Autofac;
using FakeItEasy;
using ImageHuntBotBuilder;
using ImageHuntBotBuilder.Commands;
using ImageHuntBotBuilder.Commands.Interfaces;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using TestUtilities;
using Xunit;

namespace ImageHuntBotBuilderTest.Commands
{
    public class RenameChatCommandTest : BaseTest<RenameChatCommand>
    {
        private ILogger<IRenameChatCommand> _logger;
        private IGameWebService _gameWebService;
        private ITurnContext _turnContext;
        private ImageHuntState _state;
        private ITeamWebService _teamWebService;
        private IStringLocalizer<RenameChatCommand> _localizer;

        public RenameChatCommandTest()
        {
            _testContainerBuilder.RegisterInstance(_logger = A.Fake<ILogger<IRenameChatCommand>>());
            _testContainerBuilder.RegisterInstance(_gameWebService = A.Fake<IGameWebService>());
            _testContainerBuilder.RegisterInstance(_teamWebService = A.Fake<ITeamWebService>());
            _testContainerBuilder.RegisterInstance(_localizer = A.Fake<IStringLocalizer<RenameChatCommand>>());
            _turnContext = A.Fake<ITurnContext>();
            _state = new ImageHuntState() { Status = Status.Initialized };
            Build();

        }

        [Fact]
        public async Task Should_RenameCommand_Rename_Chat()
        {
            // Arrange
            _state.GameId = 15;
            _state.TeamId = 6;
            // Act
            await _target.Execute(_turnContext, _state);
            // Assert
            A.CallTo(() => _gameWebService.GetGameById(A<int>._, A<CancellationToken>._)).MustHaveHappened();
            A.CallTo(() => _teamWebService.GetTeamById(A<int>._)).MustHaveHappened();
            A.CallTo(() => _turnContext.SendActivityAsync(A<IActivity>._, A<CancellationToken>._)).MustHaveHappened();
        }
        [Fact]
        public async Task Should_RenameCommand_Fail_If_chat_not_init()
        {
            // Arrange
            _state.Status = Status.None;
            // Act
            await _target.Execute(_turnContext, _state);
            // Assert
            A.CallTo(() => _gameWebService.GetGameById(A<int>._, A<CancellationToken>._)).MustNotHaveHappened();
            A.CallTo(() => _teamWebService.GetTeamById(A<int>._)).MustNotHaveHappened();
            A.CallTo(() => _turnContext.SendActivityAsync(A<IActivity>._, A<CancellationToken>._)).MustNotHaveHappened();
        }

    }
}
