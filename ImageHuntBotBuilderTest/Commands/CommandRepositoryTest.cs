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

        public CommandRepositoryTest()
        {
            _logger = A.Fake<ILogger<ICommandRepository>>();
            _adminWebService = A.Fake<IAdminWebService>();
            _testContainerBuilder.RegisterInstance(_adminWebService);
            _testContainerBuilder.RegisterCommand<Dummy1Command>();
            _testContainerBuilder.RegisterCommand<Dummy2Command>();
            _testContainerBuilder.RegisterInstance(_logger);
            _turnContext = A.Fake<ITurnContext>();

            Build();
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
            var commandResult = _target.Get(_turnContext, "dummy1");
            // Assert
            Check.That(commandResult).IsInstanceOf<Dummy1Command>();
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
            var commandResult = _target.Get(_turnContext, "/dummy1 toto");
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
            var commandResult = _target.Get(_turnContext, "/dummy1");
            // Assert
            Check.That(commandResult).IsInstanceOf<Dummy1Command>();
        }

        [Fact]
        public void Should_Get_Throw_if_user_not_authorized()
        {
            // Arrange
            var activity = new Activity() {From = new ChannelAccount(name: "toto")};
            
            var admins = new List<AdminResponse>
            {
                new AdminResponse() {Name = "titi"}
            };
            A.CallTo(() => _adminWebService.GetAllAdmins()).Returns(admins);
            A.CallTo(() => _turnContext.Activity).Returns(activity);
            _target.RefreshAdmins();
            // Act
            Check.ThatCode(() => _target.Get(_turnContext, "dummy2")).Throws<NotAuthorizedException>();
            // Assert
        }
        [Fact]
        public void Should_Get_Return_Command_If_User_Authorized()
        {
            // Arrange
            var activity = new Activity() {From = new ChannelAccount(name: "toto")};
            
            var admins = new List<AdminResponse>
            {
                new AdminResponse() {Name = "toto"}
            };
            A.CallTo(() => _adminWebService.GetAllAdmins()).Returns(admins);
            A.CallTo(() => _turnContext.Activity).Returns(activity);
            _target.RefreshAdmins();
            // Act
            Check.ThatCode(() => _target.Get(_turnContext, "dummy2")).DoesNotThrow();
            // Assert
        }
        [Fact]
        public void Should_Get_Throw_if_user_not_set_int_turnContext()
        {
            // Arrange
            A.CallTo(() => _turnContext.Activity).Returns(new Activity());
            // Act
            Check.ThatCode(() => _target.Get(_turnContext, "dummy2")).Throws<NotAuthorizedException>();
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
