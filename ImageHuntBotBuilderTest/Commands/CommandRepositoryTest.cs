using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using FakeItEasy;
using ImageHuntBotBuilder;
using ImageHuntBotBuilder.Commands;
using ImageHuntWebServiceClient.Responses;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using NFluent;
using TestUtilities;
using Xunit;

namespace ImageHuntBotBuilderTest.Commands
{
    [Command("dummy1")]
    class Dummy1Command : ICommand
    {
        public bool IsAdmin => false;
        public Task Execute(ITurnContext turnContext, ImageHuntState state)
        {
            throw new NotImplementedException();
        }
    }
    [Command("dummy2")]
    class Dummy2Command : ICommand
    {
        public bool IsAdmin => true;
        public Task Execute(ITurnContext turnContext, ImageHuntState state)
        {
            throw new NotImplementedException();
        }
    }

    public class CommandRepositoryTest : BaseTest<CommandRepository>
    {
        private ILogger<ICommandRepository> _logger;
        private ITurnContext _turnContext;
        private IAdminWebService _adminWebService;
        private ImageHuntState _state;

        public CommandRepositoryTest()
        {
            _logger = A.Fake<ILogger<ICommandRepository>>();
            _adminWebService = A.Fake<IAdminWebService>();
            _testContainerBuilder.RegisterInstance(_adminWebService);
            _testContainerBuilder.RegisterCommand<Dummy1Command>();
            _testContainerBuilder.RegisterCommand<Dummy2Command>();
            _testContainerBuilder.RegisterInstance(_logger);
            _turnContext = A.Fake<ITurnContext>();
            _state = new ImageHuntState();
            Build();
        }

        [Fact]
        public void Should_Return_Command_With_Command_Plus_Bot_Name()
        {
            // Arrange
            var activity = new Activity() { From = new ChannelAccount(name: "toto") };

            var admins = new List<AdminResponse>
            {
                new AdminResponse() {Name = "titi"}
            };
            A.CallTo(() => _adminWebService.GetAllAdmins()).Returns(admins);
            A.CallTo(() => _turnContext.Activity).Returns(activity);

            // Act
            var commandResult = _target.Get(_turnContext, _state, "/dummy1@botname");
            // Assert
            Check.That(commandResult).IsInstanceOf<Dummy1Command>();
        }
        [Fact]
        public void Should_Get_Return_Command_By_command_string()
        {
            // Arrange
            var activity = new Activity() { From = new ChannelAccount(name: "toto") };

            var admins = new List<AdminResponse>
            {
                new AdminResponse() {Name = "titi"}
            };
            A.CallTo(() => _adminWebService.GetAllAdmins()).Returns(admins);
            A.CallTo(() => _turnContext.Activity).Returns(activity);

            // Act
            var commandResult = _target.Get(_turnContext, _state, "dummy1");
            // Assert
            Check.That(commandResult).IsInstanceOf<Dummy1Command>();
        }
        [Theory]
        [InlineData("dummy1", "Dummy1Command")]
        [InlineData("Dummy1", "Dummy1Command")]
        [InlineData("DumMy1", "Dummy1Command")]
        public void Should_Get_Return_Command_case_insensitive(string text, string expectedCommandClassName)
        {
            // Arrange
            var activity = new Activity() { From = new ChannelAccount(name: "toto") };

            var admins = new List<AdminResponse>
            {
                new AdminResponse() {Name = "titi"}
            };
            A.CallTo(() => _adminWebService.GetAllAdmins()).Returns(admins);
            A.CallTo(() => _turnContext.Activity).Returns(activity);

            // Act
            var commandResult = _target.Get(_turnContext, _state, text);
            // Assert
            Check.That(commandResult.GetType().Name).Equals(expectedCommandClassName);
        }
        [Fact]
        public void Should_Get_Return_Command_By_command_string_with_parameter()
        {
            // Arrange
            var activity = new Activity() { From = new ChannelAccount(name: "toto") };

            var admins = new List<AdminResponse>
            {
                new AdminResponse() {Name = "titi"}
            };
            A.CallTo(() => _adminWebService.GetAllAdmins()).Returns(admins);
            A.CallTo(() => _turnContext.Activity).Returns(activity);

            // Act
            var commandResult = _target.Get(_turnContext, _state, "/dummy1 toto");
            // Assert
            Check.That(commandResult).IsInstanceOf<Dummy1Command>();
        }
        [Fact]
        public void Should_Get_Return_Command_By_command_string_With_slash()
        {
            // Arrange
            var activity = new Activity() { From = new ChannelAccount(name: "toto") };

            var admins = new List<AdminResponse>
            {
                new AdminResponse() {Name = "titi"}
            };
            A.CallTo(() => _adminWebService.GetAllAdmins()).Returns(admins);
            A.CallTo(() => _turnContext.Activity).Returns(activity);

            // Act
            var commandResult = _target.Get(_turnContext, _state, "/dummy1");
            // Assert
            Check.That(commandResult).IsInstanceOf<Dummy1Command>();
        }

        [Fact]
        public void Should_Get_Throw_if_user_not_authorized()
        {
            // Arrange
            var activity = new Activity() { From = new ChannelAccount(name: "toto") };

            var admins = new List<AdminResponse>
            {
                new AdminResponse() {Name = "titi"}
            };
            A.CallTo(() => _adminWebService.GetAllAdmins()).Returns(admins);
            A.CallTo(() => _turnContext.Activity).Returns(activity);
            _target.RefreshAdmins();
            // Act
            Check.ThatCode(() => _target.Get(_turnContext, _state, "dummy2")).Throws<NotAuthorizedException>();
            // Assert
        }
        [Fact]
        public void Should_Get_Throw_if_user_is_admin_but_in_a_team()
        {
            // Arrange
            var activity = new Activity() { From = new ChannelAccount(name: "titi") };

            var admins = new List<AdminResponse>
            {
                new AdminResponse() {Name = "titi"}
            };
            A.CallTo(() => _adminWebService.GetAllAdmins()).Returns(admins);
            //            _turnContext.TurnState.Get<ImageHuntState>()
            A.CallTo(() => _turnContext.Activity).Returns(activity);
            _target.RefreshAdmins();
            _state.Team = new TeamResponse() { Players = new PlayerResponse[] { new PlayerResponse() { ChatLogin = "titi" }, } };
            // Act
            Check.ThatCode(() => _target.Get(_turnContext, _state, "dummy2")).Throws<NotAuthorizedException>();
            // Assert
        }
        [Fact]
        public void Should_Get_Return_Command_If_User_Authorized()
        {
            // Arrange
            var activity = new Activity() { From = new ChannelAccount(name: "toto") };

            var admins = new List<AdminResponse>
            {
                new AdminResponse() {Name = "toto"}
            };
            A.CallTo(() => _adminWebService.GetAllAdmins()).Returns(admins);
            A.CallTo(() => _turnContext.Activity).Returns(activity);
            _target.RefreshAdmins();
            // Act
            Check.ThatCode(() => _target.Get(_turnContext, _state, "dummy2")).DoesNotThrow();
            // Assert
        }
        [Fact]
        public void Should_Get_Throw_if_user_not_set_int_turnContext()
        {
            // Arrange
            A.CallTo(() => _turnContext.Activity).Returns(new Activity());
            // Act
            Check.ThatCode(() => _target.Get(_turnContext, _state, "dummy2")).Throws<NotAuthorizedException>();
            // Assert
        }

        [Fact]
        public async Task Should_Refresh_Every_5_Minutes()
        {
            // Arrange

            // Act
            await _target.RefreshAdmins();
            await _target.RefreshAdmins();
            // Assert
            A.CallTo(() => _adminWebService.GetAllAdmins()).MustHaveHappened(Repeated.Exactly.Once);
        }
    }
}
