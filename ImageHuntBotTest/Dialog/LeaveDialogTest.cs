using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using FakeItEasy;
using ImageHuntBot.Dialogs;
using ImageHuntTelegramBot;
using Microsoft.Extensions.Logging;
using TestUtilities;
using Xunit;

namespace ImageHuntBotTest.Dialog
{
    public class LeaveDialogTest : BaseTest
    {
        private ILeaveDialog _target;
        private ILogger<LeaveDialog> _logger;

        public LeaveDialogTest()
        {
            _testContainerBuilder.RegisterType<LeaveDialog>().As<ILeaveDialog>();
            _logger = A.Fake<ILogger<LeaveDialog>>();
            _testContainerBuilder.RegisterInstance(_logger).As<ILogger<LeaveDialog>>();
            var container = _testContainerBuilder.Build();
            _target = container.Resolve<ILeaveDialog>();
        }

        [Fact]
        public async Task Begin()
        {
            // Arrange
            var turnContext = A.Fake<ITurnContext>();
            // Act
            await _target.Begin(turnContext);
            // Assert
            A.CallTo(() => turnContext.Leave()).MustHaveHappened();
        }
    }
}
