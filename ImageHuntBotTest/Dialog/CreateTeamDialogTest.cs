using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using FakeItEasy;
using ImageHuntBot.Dialogs;
using ImageHuntTelegramBot;
using ImageHuntTelegramBot.Dialogs;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Extensions.Logging;
using TestUtilities;
using Xunit;

namespace ImageHuntBotTest.Dialog
{
    public class CreateTeamDialogTest : BaseTest
    {
        private ITeamWebService _teamWebService;
        private ILogger<CreateTeamDialog> _logger;
        private CreateTeamDialog _target;

        public CreateTeamDialogTest()
        {
            _testContainerBuilder.RegisterType<CreateTeamDialog>();
            _teamWebService = A.Fake<ITeamWebService>();
            _testContainerBuilder.RegisterInstance(_teamWebService).As<ITeamWebService>();
            _logger = A.Fake<ILogger<CreateTeamDialog>>();
            _testContainerBuilder.RegisterInstance(_logger).As<ILogger<CreateTeamDialog>>();

            _container = _testContainerBuilder.Build();
            _target = _container.Resolve<CreateTeamDialog>();

        }

        [Fact]
        public async Task CreateTeam()
        {
            // Arrange
            var activity = new Activity()
            {
                ActivityType = ActivityType.Message,
                ChatId = 15,
                Text = "/createTeam gameId=11"
            };
            var turnContext = A.Fake<ITurnContext>();
            A.CallTo(() => turnContext.Activity).Returns(activity);
            var imageHuntState = new ImageHuntState() { Status = Status.None};
            A.CallTo(() => turnContext.GetConversationState<ImageHuntState>()).Returns(imageHuntState);
            // Act
            await _target.Begin(turnContext);
            // Assert
        }
    }

    public class CreateTeamDialog : AbstractDialog, ICreateTeamDialog
    {
        public CreateTeamDialog(ILogger<CreateTeamDialog> logger) : base(logger)
        {
        }

        public override string Command => "/createTeam";
    }

    public interface ICreateTeamDialog : IDialog
    {
    }
}
