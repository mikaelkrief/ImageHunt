using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
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
            var state = new ImageHuntState(){GameId = 12};
            A.CallTo(() => turnContext.GetConversationState<ImageHuntState>()).Returns(state);
            // Act
            await _target.Begin(turnContext);
            // Assert
            A.CallTo(() => _gameService.GetPictureNodesForGame(state.GameId, A<CancellationToken>._)).MustHaveHappened();
        }
    }

    public class DisplayPictureNodesDialog : AbstractDialog, IDisplayPictureNodesDialog
    {
        private readonly IGameWebService _gameWebService;

        public DisplayPictureNodesDialog(ILogger<DisplayPictureNodesDialog> logger, IGameWebService gameWebService) : base(logger)
        {
            _gameWebService = gameWebService;
        }

        public override async Task Begin(ITurnContext turnContext, bool overrideAdmin = false)
        {
            try
            {
                var state = turnContext.GetConversationState<ImageHuntState>();
                var pictureNodes = await _gameWebService.GetPictureNodesForGame(state.GameId);
                foreach (var pictureNode in pictureNodes)
                {
                    var activity = new Activity(){};
                    //await turnContext.SendActivity()
                }
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
