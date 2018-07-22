using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using FakeItEasy;
using ImageHuntTelegramBot;
using ImageHuntTelegramBot.Controllers;
using ImageHuntWebServiceClient.Responses;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NFluent;
using Telegram.Bot.Types;
using TestUtilities;
using Xunit;

namespace ImageHuntBotTest
{
    public class UpdateControllerTest : BaseTest
    {
      private UpdateController _target;
      private IBot _bot;
      private ContextHub _contextHub;
      private ILogger _logger;
        private IAdminWebService _adminService;

        public UpdateControllerTest()
      {
        _testContainerBuilder.RegisterType<UpdateController>();
        _bot = A.Fake<IBot>();
        _logger = A.Fake<ILogger<UpdateController>>();
        _testContainerBuilder.RegisterInstance(_logger).As<ILogger<UpdateController>>();
          _adminService = A.Fake<IAdminWebService>();
          var admins = new List<AdminResponse>
          {
              new AdminResponse(){Name = "admin"},
              new AdminResponse(){Name = "toto"},
          };
          A.CallTo(() => _adminService.GetAllAdmins()).Returns(admins);

            _testContainerBuilder.RegisterInstance(_adminService).As<IAdminWebService>();
        _testContainerBuilder.RegisterInstance(_bot);
        _contextHub = A.Fake<ContextHub>();
        _testContainerBuilder.RegisterInstance(_contextHub).SingleInstance();
        _container  = _testContainerBuilder.Build();
        _target = _container.Resolve<UpdateController>();
      }
      [Fact]
      public async Task Update_Message()
      {
        // Arrange
        var update = new Update() {Message = new Message()
        {
            Chat = new Chat() {Id = 15},
            Text = "/init",
            From = new User() { Username = "admin"}
        }};
        // Act
        var result = await _target.Post(update);
        // Assert
        A.CallTo(() => _contextHub.GetContext(update)).MustHaveHappened();
        A.CallTo(() => _bot.OnTurn(A<ITurnContext>._)).MustHaveHappened();
        Check.That(result).IsInstanceOf<OkResult>();
        A.CallTo(() => _logger.Log<object>(A<LogLevel>._, A<EventId>._, 
            A<string>._, A<Exception>._, A<Func<object, Exception, string>>._))
          .WithAnyArguments()
          .MustHaveHappened();
      }
      [Fact]
      public async Task Update_UpdateMessage()
      {
        // Arrange
        var update = new Update() {EditedMessage = new Message()
        {
            Chat = new Chat() {Id = 15},
            Text = "/start",
            From = new User() { Username = "admin" }
        }
        };
        // Act
        var result = await _target.Post(update);
        // Assert
        A.CallTo(() => _contextHub.GetContext(update)).MustHaveHappened();
        A.CallTo(() => _bot.OnTurn(A<ITurnContext>._)).MustHaveHappened();
        Check.That(result).IsInstanceOf<OkResult>();
        A.CallTo(() => _logger.Log<object>(A<LogLevel>._, A<EventId>._, 
            A<string>._, A<Exception>._, A<Func<object, Exception, string>>._))
          .WithAnyArguments()
          .MustHaveHappened();
      }
      [Fact]
      public async Task Update_Message_Exception_Occured()
      {
        // Arrange
        var update = new Update() {Message = new Message()
        {
            Chat = new Chat() {Id = 15},
            From = new User() { Username = "admin" },
            Text = "/start"
        }};
        A.CallTo(() => _bot.OnTurn(A<ITurnContext>._)).Throws<Exception>();
        // Act
        var result = await _target.Post(update);
        // Assert
        A.CallTo(() => _contextHub.GetContext(update)).MustHaveHappened();
        Check.That(result).IsInstanceOf<OkResult>();
        A.CallTo(() => _logger.Log<object>(A<LogLevel>._, A<EventId>._, 
            A<string>._, A<Exception>._, A<Func<object, Exception, string>>._))
          .WithAnyArguments()
          .MustHaveHappened(Repeated.Exactly.Times(3));
      }

        [Fact]
        public async Task UpdateAdmins()
        {
            // Arrange
            
            // Act
            await _target.UpdateAdmins();
            // Assert
            A.CallTo(() => _adminService.GetAllAdmins()).MustHaveHappened(Repeated.Exactly.Twice);
        }

        [Fact]
        public async Task Update_CheckUser_ShouldBeAdmin()
        {
            // Arrange
            var update = new Update() { Message = new Message()
            {
                Chat = new Chat() { Id = 15 },
                From = new User() { Username = "titi"},
                Text = "/start"
            } };
            await _target.UpdateAdmins();
            // Act
            await _target.Post(update);
            // Assert
            A.CallTo(() => _bot.OnTurn(A<ITurnContext>._)).MustNotHaveHappened();

        }
    }
}
