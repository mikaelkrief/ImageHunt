using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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
    public class DisplayNodeDialogTest : BaseTest
    {
        private ILogger<DisplayNodeDialog> _logger;
        private DisplayNodeDialog _target;
        private INodeWebService _nodeWebService;

        public DisplayNodeDialogTest()
        {
            _testContainerBuilder.RegisterType<DisplayNodeDialog>();
            _logger = A.Fake<ILogger<DisplayNodeDialog>>();
            _nodeWebService = A.Fake<INodeWebService>();
            _testContainerBuilder.RegisterInstance(_nodeWebService);
            _testContainerBuilder.RegisterInstance(_logger).As<ILogger<DisplayNodeDialog>>();

            _container = _testContainerBuilder.Build();
            _target = _container.Resolve<DisplayNodeDialog>();
        }

        [Fact]
        public async Task Begin()
        {
            // Arrange
            var activity = new Activity()
            {
                ActivityType = ActivityType.Message,
                ChatId = 15,
            };
            var turnContext = A.Fake<ITurnContext>();
            A.CallTo(() => turnContext.Activity).Returns(activity);
            var imageHuntState = new ImageHuntState() { Status = Status.Started, CurrentLatitude = 45.8, CurrentNodeId = 15};
            A.CallTo(() => turnContext.GetConversationState<ImageHuntState>()).Returns(imageHuntState);
            // Act
            await _target.Begin(turnContext);
            // Assert
        }
    }

    public class DisplayNodeDialog : AbstractDialog, IDisplayNodeDialog
    {
        private readonly INodeWebService _nodeWebService;

        public DisplayNodeDialog(ILogger<DisplayNodeDialog> logger, INodeWebService nodeWebService) 
            : base(logger)
        {
            _nodeWebService = nodeWebService;
        }

        public override string Command => "/DisplayNode";
    }
}
