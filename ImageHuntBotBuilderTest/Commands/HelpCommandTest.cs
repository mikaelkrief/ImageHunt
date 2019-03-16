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
using Microsoft.Bot.Builder;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using TestUtilities;
using Xunit;

namespace ImageHuntBotBuilderTest.Commands
{
    public class HelpCommandTest : BaseTest<HelpCommand>
    {
        private ILogger<IHelpCommand> _logger;
        private ITurnContext _turnContext;
        private ImageHuntState _state;
        private IStringLocalizer<HelpCommand> _localizer;

        public HelpCommandTest()
        {
            _testContainerBuilder.RegisterInstance(_logger = A.Fake<ILogger<IHelpCommand>>());
            _testContainerBuilder.RegisterInstance(_localizer = A.Fake<IStringLocalizer<HelpCommand>>());

            _turnContext = A.Fake<ITurnContext>();
            _state = new ImageHuntState();
            Build();
        }

        [Fact]
        public async Task Should_Reply_help_message()
        {
            // Arrange
            
            // Act
            await _target.ExecuteAsync(_turnContext, _state);
            // Assert
            A.CallTo(
                    () => _turnContext.SendActivityAsync(A<string>._, A<string>._, A<string>._, A<CancellationToken>._))
                .MustHaveHappened();
        }
    }
}
