using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Autofac;
using FakeItEasy;
using ImageHuntBotBuilder;
using ImageHuntBotBuilder.Commands;
using ImageHuntBotBuilder.Commands.Interfaces;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using TestUtilities;
using Xunit;

namespace ImageHuntBotBuilderTest.Commands
{
    public class DeleteStateCommandTest : BaseTest<DeleteStateCommand>
    {
        private ILogger<IDeleteStateCommand> _logger;
        private IStringLocalizer<DeleteStateCommand> _localizer;
        private ITurnContext _turnContext;
        private ImageHuntState _state;
        private ImageHuntBotAccessors _accessors;

        public DeleteStateCommandTest()
        {
            _testContainerBuilder.RegisterInstance(_logger = A.Fake<ILogger<IDeleteStateCommand>>());
            _testContainerBuilder.RegisterInstance(_localizer = A.Fake<IStringLocalizer<DeleteStateCommand>>());
            _testContainerBuilder.RegisterInstance(_accessors = A.Fake<ImageHuntBotAccessors>());
            _turnContext = A.Fake<ITurnContext>();
            _state = new ImageHuntState() { Status = Status.Started };
            Build();
        }

        [Fact]
        public async Task Should_Delete_All_States_For_Game()
        {
            // Arrange
            Activity activity = new Activity(text:"/delState gameid=20");
            A.CallTo(() => _turnContext.Activity).Returns(activity);
            // Act
            await _target.Execute(_turnContext, _state);
            // Assert
        }
    }

    public class DeleteStateCommand : AbstractCommand, IDeleteStateCommand
    {
        private readonly ImageHuntBotAccessors _accessors;

        public DeleteStateCommand(ILogger<IDeleteStateCommand> logger, 
            IStringLocalizer<DeleteStateCommand> localizer,
            ImageHuntBotAccessors accessors) : base(logger, localizer)
        {
            _accessors = accessors;
        }

        protected override async Task InternalExecute(ITurnContext turnContext, ImageHuntState state)
        {
            var regex = new Regex(@"\/delState \s*(gameid\=(?'gameid'\d*)|teamid\=(?'teamid'\d*))");
            if (regex.IsMatch(turnContext.Activity.Text))
            {
                var gameIdAsString = regex.Matches(turnContext.Activity.Text)[0].Groups["gameid"].Value;
                var teamIdAsString = regex.Matches(turnContext.Activity.Text)[0].Groups["teamid"].Value;
            }
            
        }
    }
}
