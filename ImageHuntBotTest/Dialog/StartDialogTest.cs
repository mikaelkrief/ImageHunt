using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using FakeItEasy;
using ImageHuntBot.Dialogs;
using ImageHuntTelegramBot;
using ImageHuntTelegramBot.Dialogs;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Extensions.Logging;
using TestUtilities;
using Xunit;

namespace ImageHuntBotTest.Dialog
{
    public class StartDialogTest : BaseTest
    {
        private IPasscodeWebService _passcodeWebService;
        private ILogger<StartDialog> _logger;
        private StartDialog _target;

        public StartDialogTest()
        {
            _testContainerBuilder.RegisterType<StartDialog>();
            _passcodeWebService = A.Fake<IPasscodeWebService>();
            _testContainerBuilder.RegisterInstance(_passcodeWebService);
            _logger = A.Fake<ILogger<StartDialog>>();
            _testContainerBuilder.RegisterInstance(_logger).As<ILogger<StartDialog>>();
            _container = _testContainerBuilder.Build();
            _target = _container.Resolve<StartDialog>();

        }

        [Fact]
        public void Begin()
        {
            // Arrange
            
            // Act

            // Assert
        }
    }

    public class StartDialog : AbstractDialog, IStartDialog
    {
        private readonly IPasscodeWebService _passcodeWebService;

        public StartDialog(ILogger logger, IPasscodeWebService passcodeWebService) : base(logger)
        {
            _passcodeWebService = passcodeWebService;
        }

        public override string Command => "/start";
    }

    public interface IStartDialog : IDialog
    {
    }
}
