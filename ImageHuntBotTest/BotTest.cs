using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using FakeItEasy;
using ImageHuntTelegramBot;
using ImageHuntTelegramBot.Dialogs;
using ImageHuntWebServiceClient.Responses;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Extensions.Logging;
using NFluent;
using TestUtilities;
using Xunit;

namespace ImageHuntBotTest
{
    public class BotTest : BaseTest
    {
        private TelegramBot _target;
        private ILogger<TelegramBot> _logger;
        private IAdminWebService _adminService;
        private ITeamWebService _teamService;

        public BotTest()
        {
            _logger = A.Fake<ILogger<TelegramBot>>();
            _adminService = A.Fake<IAdminWebService>();
            _teamService = A.Fake<ITeamWebService>();
            _testContainerBuilder.RegisterInstance(_logger);
            _testContainerBuilder.RegisterType<TelegramBot>();
            _testContainerBuilder.RegisterInstance(_adminService);
            _testContainerBuilder.RegisterInstance(_teamService);
            var admin = new List<AdminResponse> { new AdminResponse() { Name = "tata" } };
            A.CallTo(() => _adminService.GetAllAdmins()).Returns(admin);

            _container = _testContainerBuilder.Build();
            _target = _container.Resolve<TelegramBot>();
        }

        [Fact]
        public async Task OnTurn_NoDialog()
        {
            // Arrange
            var turnContext = A.Fake<ITurnContext>();
            // Act
            await _target.OnTurn(turnContext);
            // Assert
        }
        [Fact]
        public async Task OnTurn_Init()
        {
            // Arrange
            var turnContext = A.Fake<ITurnContext>();
            var activity = new Activity() { ActivityType = ActivityType.Message, Text = "/init", ChatId = 15};
            var state = new ImageHuntState() { GameId = 15, TeamId = 6 };
            A.CallTo(() => turnContext.GetConversationState<ImageHuntState>()).Returns(state);
            var teamResponse = new TeamResponse() { Players = new PlayerResponse[] { new PlayerResponse() { ChatLogin = "tita" }, new PlayerResponse() { ChatLogin = "titi" }, } };
            A.CallTo(() => _teamService.GetTeamById(state.TeamId)).Returns(teamResponse);

            A.CallTo(() => turnContext.Activity).Returns(activity);
            A.CallTo(() => turnContext.CurrentDialog).Returns(null);
            A.CallTo(() => turnContext.Username).Returns("tata");
            var initDialog = A.Fake<IDialog>();
            A.CallTo(() => initDialog.Command).Returns("/init");
            A.CallTo(() => initDialog.IsAdmin).Returns(true);
            _target.AddDialog(initDialog);
            // Act
            await _target.OnTurn(turnContext);
            // Assert
            A.CallTo(() => turnContext.Begin(initDialog)).MustHaveHappened();
            A.CallTo(() => turnContext.Continue()).MustHaveHappened();

        }
        [Fact]
        public async Task OnTurn_Uploadphoto()
        {
            // Arrange
            var turnContext = A.Fake<ITurnContext>();
            var activity = new Activity() { ActivityType = ActivityType.Message, Text = "/uploadphoto", ChatId = 15};
            A.CallTo(() => turnContext.Activity).Returns(activity);
            A.CallTo(() => turnContext.CurrentDialog).Returns(null);
            var uploadPhotoDialog = A.Fake<IDialog>();
            A.CallTo(() => uploadPhotoDialog.IsAdmin).Returns(false);

            A.CallTo(() => uploadPhotoDialog.Command).Returns("/uploadphoto");

            _target.AddDialog(uploadPhotoDialog);
            // Act
            await _target.OnTurn(turnContext);
            // Assert
            A.CallTo(() => turnContext.Begin(uploadPhotoDialog)).MustHaveHappened();
            A.CallTo(() => turnContext.Continue()).MustHaveHappened();

        }
        [Fact]
        public async Task OnTurn_UploadDocument()
        {
            // Arrange
            var turnContext = A.Fake<ITurnContext>();
            var activity = new Activity() { ActivityType = ActivityType.Message, Text = "/uploaddocument", ChatId = 15 };
            A.CallTo(() => turnContext.Activity).Returns(activity);
            A.CallTo(() => turnContext.CurrentDialog).Returns(null);
            var uploadDocumentDialog = A.Fake<IDialog>();
            A.CallTo(() => uploadDocumentDialog.Command).Returns("/uploaddocument");
            _target.AddDialog(uploadDocumentDialog);
            // Act
            await _target.OnTurn(turnContext);
            // Assert
            A.CallTo(() => turnContext.Begin(uploadDocumentDialog)).MustHaveHappened();
            A.CallTo(() => turnContext.Continue()).MustHaveHappened();

        }
        [Fact]
        public async Task OnTurn_ResetBot()
        {
            // Arrange
            var turnContext = A.Fake<ITurnContext>();
            var activity = new Activity() { ActivityType = ActivityType.Message, Text = "/reset", ChatId = 15 };
            A.CallTo(() => turnContext.Activity).Returns(activity);
            A.CallTo(() => turnContext.CurrentDialog).Returns(null);
            var resetDialog = A.Fake<IDialog>();
            A.CallTo(() => resetDialog.Command).Returns("/reset");
            _target.AddDialog(resetDialog);
            // Act
            await _target.OnTurn(turnContext);
            // Assert
            A.CallTo(() => turnContext.Begin(resetDialog)).MustHaveHappened();
            A.CallTo(() => turnContext.Continue()).MustHaveHappened();

        }

        [Fact]
        public async Task OnTurn_ErrorOccured()
        {
            // Arrange
            var turnContext = A.Fake<ITurnContext>();
            var activity = new Activity() { ActivityType = ActivityType.Message, Text = "/uploaddocument", ChatId = 15 };
            A.CallTo(() => turnContext.Activity).Returns(activity);
            A.CallTo(() => turnContext.CurrentDialog).Returns(null);
            A.CallTo(() => turnContext.Begin(A<IDialog>._)).Throws<Exception>();
            var uploadDocumentDialog = A.Fake<IDialog>();
            A.CallTo(() => uploadDocumentDialog.Command).Returns("/uploaddocument");
            _target.AddDialog(uploadDocumentDialog);
            // Act
            await _target.OnTurn(turnContext);
            // Assert
            A.CallTo(() => turnContext.End()).MustHaveHappened();

        }
        [Fact]
        public async Task OnTurn_DialogPending()
        {
            // Arrange
            var turnContext = A.Fake<ITurnContext>();
            var activity = new Activity() { ActivityType = ActivityType.Message, Text = "15" };
            A.CallTo(() => turnContext.Activity).Returns(activity);
            A.CallTo(() => turnContext.Replied).Returns(true);
            var initDialog = A.Fake<IDialog>();
            A.CallTo(() => initDialog.Command).Returns("/init");
            _target.AddDialog(initDialog);
            // Act
            await _target.OnTurn(turnContext);
            // Assert
            A.CallTo(() => initDialog.Begin(turnContext, A<bool>._)).MustNotHaveHappened();
            A.CallTo(() => turnContext.Continue()).MustHaveHappened();
        }

        [Theory]
        [InlineData("toto", false)]
        [InlineData("tata", true)]
        public async Task OnTurn_Check_Player_Cannot_Use_Admin_Command(string userName, bool mustBegin)
        {
            // Arrange
            var turnContext = A.Fake<ITurnContext>();
            var activity = new Activity() { ActivityType = ActivityType.Message, Text = "/init" };
            var state = new ImageHuntState() { GameId = 15, TeamId = 6 };
            A.CallTo(() => turnContext.GetConversationState<ImageHuntState>()).Returns(state);
            var teamResponse = new TeamResponse() { Players = new PlayerResponse[] { new PlayerResponse() { ChatLogin = "tita" }, new PlayerResponse() { ChatLogin = "titi" }, } };
            A.CallTo(() => _teamService.GetTeamById(state.TeamId)).Returns(teamResponse);
            A.CallTo(() => turnContext.Activity).Returns(activity);
            A.CallTo(() => turnContext.Replied).Returns(false);
            A.CallTo(() => turnContext.CurrentDialog).Returns(null);
            A.CallTo(() => turnContext.Username).Returns(userName);
            var admin = new List<AdminResponse> { new AdminResponse() { Name = "tata" } };
            A.CallTo(() => _adminService.GetAllAdmins()).Returns(admin);
            var initDialog = A.Fake<IDialog>();
            A.CallTo(() => initDialog.IsAdmin).Returns(true);
            A.CallTo(() => initDialog.Command).Returns("/init");
            _target.AddDialog(initDialog);

            // Act
            await _target.OnTurn(turnContext);
            // Assert
            if (mustBegin)
                A.CallTo(() => turnContext.Begin(A<IDialog>._)).MustHaveHappened();
            else
                A.CallTo(() => turnContext.Begin(A<IDialog>._)).MustNotHaveHappened();
        }
        [Theory]
        [InlineData("toto", false)]
        [InlineData("tata", true)]
        public async Task OnTurn_Check_Player_Cannot_Use_Admin_Command_Team_Not_Defined(string userName, bool mustBegin)
        {
            // Arrange
            var turnContext = A.Fake<ITurnContext>();
            var activity = new Activity() { ActivityType = ActivityType.Message, Text = "/init" };
            var state = new ImageHuntState();
            TeamResponse teamResponse = null;
            A.CallTo(() => _teamService.GetTeamById(state.TeamId)).Returns(teamResponse);

            A.CallTo(() => turnContext.GetConversationState<ImageHuntState>()).Returns(state);
            A.CallTo(() => turnContext.Activity).Returns(activity);
            A.CallTo(() => turnContext.Replied).Returns(false);
            A.CallTo(() => turnContext.CurrentDialog).Returns(null);
            A.CallTo(() => turnContext.Username).Returns(userName);
            var admin = new List<AdminResponse> { new AdminResponse() { Name = "tata" } };
            A.CallTo(() => _adminService.GetAllAdmins()).Returns(admin);
            var initDialog = A.Fake<IDialog>();
            A.CallTo(() => initDialog.IsAdmin).Returns(true);
            A.CallTo(() => initDialog.Command).Returns("/init");
            _target.AddDialog(initDialog);

            // Act
            await _target.OnTurn(turnContext);
            // Assert
            if (mustBegin)
                A.CallTo(() => turnContext.Begin(A<IDialog>._)).MustHaveHappened();
            else
                A.CallTo(() => turnContext.Begin(A<IDialog>._)).MustNotHaveHappened();
        }
        [Fact]
        public async Task OnTurn_Check_Admin_Cannot_Use_Admin_Command_If_They_Belongs_to_Game()
        {
            // Arrange
            var turnContext = A.Fake<ITurnContext>();
            var activity = new Activity() { ActivityType = ActivityType.Message, Text = "/init" };
            var state = new ImageHuntState(){GameId = 15, TeamId = 6};
            A.CallTo(() => turnContext.GetConversationState<ImageHuntState>()).Returns(state);
            A.CallTo(() => turnContext.Activity).Returns(activity);
            A.CallTo(() => turnContext.Replied).Returns(false);
            A.CallTo(() => turnContext.CurrentDialog).Returns(null);
            A.CallTo(() => turnContext.Username).Returns("Toto");
            var teamResponse = new TeamResponse(){Players = new PlayerResponse[]{new PlayerResponse(){ChatLogin = "toto"}, new PlayerResponse(){ChatLogin = "titi"},  }};
            A.CallTo(() => _teamService.GetTeamById(state.TeamId)).Returns(teamResponse);
            var admin = new List<AdminResponse> { new AdminResponse() { Name = "Toto" } };
            A.CallTo(() => _adminService.GetAllAdmins()).Returns(admin);
            var initDialog = A.Fake<IDialog>();
            A.CallTo(() => initDialog.IsAdmin).Returns(true);
            A.CallTo(() => initDialog.Command).Returns("/init");
            _target.AddDialog(initDialog);

            // Act
            await _target.OnTurn(turnContext);
            // Assert
            A.CallTo(() => turnContext.Begin(A<IDialog>._)).MustNotHaveHappened();
            A.CallTo(() => turnContext.ReplyActivity(A<string>._)).MustHaveHappened();
        }

        [Fact]
        public async Task OnTurn_Should_Log_All_Commands()
        {
            // Arrange
            var turnContext = A.Fake<ITurnContext>();
            var activity = new Activity() { ActivityType = ActivityType.Message, Text = "/init" };
            // Act
            await _target.OnTurn(turnContext);
            // Assert
            A.CallTo(() => _logger.Log(LogLevel.Trace, A<EventId>._, A<object>._, A<Exception>._,
                A<Func<object, Exception, string>>._)).MustHaveHappened();
        }
    }
}
