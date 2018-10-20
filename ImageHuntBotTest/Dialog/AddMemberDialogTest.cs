using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using FakeItEasy;
using ImageHuntBot.Dialogs;
using ImageHuntTelegramBot;
using ImageHuntTelegramBot.Dialogs;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.Responses;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;
using TestUtilities;
using Xunit;

namespace ImageHuntBotTest.Dialog
{
    public class AddMemberDialogTest : BaseTest
    {
        private ILogger<AddMemberDialog> _logger;
        private AddMemberDialog _target;
        private ITeamWebService _teamWebService;

        public AddMemberDialogTest()
        {
            _testContainerBuilder.RegisterType<AddMemberDialog>();
            _logger = A.Fake<ILogger<AddMemberDialog>>();
            _testContainerBuilder.RegisterInstance(_logger).As<ILogger<AddMemberDialog>>();
            _teamWebService = A.Fake<ITeamWebService>();
            _testContainerBuilder.RegisterInstance(_teamWebService).As<ITeamWebService>();
            _container = _testContainerBuilder.Build();
            _target = _container.Resolve<AddMemberDialog>();

        }

        [Fact]
        public async Task Begin()
        {
            // Arrange
            var activity = new Activity()
            {
                ActivityType = ActivityType.AddMember,
                Text = "/newMember",
                NewChatMember = new User[] {new User(){Username = "toto"}, }
            };
            var context = A.Fake<ITurnContext>();
            A.CallTo(() => context.Activity).Returns(activity);
            var state = new ImageHuntState() {GameId = 15, TeamId = 1, Team = new TeamResponse()};
            A.CallTo(() => context.GetConversationState<ImageHuntState>()).Returns(state);
            A.CallTo(() => _teamWebService.AddPlayer(A<int>._, A<PlayerRequest>._))
                .Returns(new TeamResponse() {Name = "Team1"});
            // Act
            await _target.Begin(context);
            // Assert
            A.CallTo(() => _teamWebService.AddPlayer(A<int>._, A<PlayerRequest>._)).MustHaveHappened();
        }
        [Fact]
        public async Task Begin_Team_Not_Set()
        {
            // Arrange
            var activity = new Activity()
            {
                ActivityType = ActivityType.AddMember,
                Text = "/newMember",
                NewChatMember = new User[] {new User(){Username = "toto"}, }
            };
            var context = A.Fake<ITurnContext>();
            A.CallTo(() => context.Activity).Returns(activity);
            var state = new ImageHuntState() {GameId = 15};
            A.CallTo(() => context.GetConversationState<ImageHuntState>()).Returns(state);

            // Act
            await _target.Begin(context);
            // Assert
            A.CallTo(() => _teamWebService.AddPlayer(A<int>._, A<PlayerRequest>._)).MustNotHaveHappened();
            A.CallTo(() => context.ReplyActivity(A<string>._)).MustHaveHappened();
            A.CallTo(() => context.End()).MustHaveHappened();

        }
    }
}
