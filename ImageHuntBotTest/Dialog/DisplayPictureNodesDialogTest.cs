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
    public class DisplayPictureNodesDialogTest : BaseTest
    {
        private ILogger<DisplayPictureNodesDialog> _logger;
        private DisplayPictureNodesDialog _target;
        private IGameWebService _gameService;

        public DisplayPictureNodesDialogTest()
        {
            _testContainerBuilder.RegisterType<DisplayPictureNodesDialog>();
            _logger = A.Fake<ILogger<DisplayPictureNodesDialog>>();
            _gameService = A.Fake<IGameWebService>();
            _testContainerBuilder.RegisterInstance(_logger).As<ILogger<DisplayPictureNodesDialog>>();
            _testContainerBuilder.RegisterInstance(_gameService).As<IGameWebService>();
            var container = _testContainerBuilder.Build();
            _target = container.Resolve<DisplayPictureNodesDialog>();
        }

        [Fact]
        public async Task Begin()
        {
            // Arrange
            var turnContext = A.Fake<ITurnContext>();
            // Act
            await _target.Begin(turnContext);
            // Assert
        }
    }

    public class DisplayPictureNodesDialog : AbstractDialog, IDisplayPictureNodesDialog
    {
        private readonly IGameWebService _gameWebService;

        public DisplayPictureNodesDialog(ILogger<DisplayPictureNodesDialog> logger, IGameWebService gameWebService) : base(logger)
        {
            _gameWebService = gameWebService;
        }

        public override async Task Begin(ITurnContext turnContext)
        {
            try
            {
                var state = turnContext.GetConversationState<ImageHuntState>();
                var pictureNodes = _gameWebService.GetPictureNodesForGame(state.GameId);
            }
            finally
            {
                await turnContext.End();
            }
        }

        public override string Command => "/displayPictures";
        public override bool IsAdmin => false;
    }
}
