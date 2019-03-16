using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using FakeItEasy;
using ImageHuntBotBuilder;
using ImageHuntBotBuilder.Commands;
using ImageHuntBotBuilder.Commands.Interfaces;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Bot.Builder;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using TestUtilities;
using Xunit;

namespace ImageHuntBotBuilderTest.Commands
{
    public class UploadTeamAvatarCommandTest : BaseTest<UploadTeamAvatarCommand>
    {
        private ILogger<IUploadTeamAvatarCommand> _logger;
        private ITeamWebService _teamWebService;
        private ITurnContext _turnContext;
        private ImageHuntState _state;

        public UploadTeamAvatarCommandTest()
        {
            _testContainerBuilder.RegisterInstance(_logger = A.Fake<ILogger<IUploadTeamAvatarCommand>>());
            _testContainerBuilder.RegisterInstance(_teamWebService = A.Fake<ITeamWebService>());
            _turnContext = A.Fake<ITurnContext>();
            _state = new ImageHuntState() { Status = Status.Started };

            Build();
        }

        //[Fact]
        public async Task Should_Upload_Avatar_Update_Team()
        {
            // Arrange
            
            // Act
            await _target.Execute(_turnContext, _state);
            // Assert
        }
    }
    [Command("upIcon")]
    public class UploadTeamAvatarCommand : AbstractCommand, IUploadTeamAvatarCommand
    {
        private readonly ITeamWebService _teamWebService;

        public UploadTeamAvatarCommand(ILogger<IUploadTeamAvatarCommand> logger, ITeamWebService teamWebService, IStringLocalizer<UploadTeamAvatarCommand> localizer) : base(logger, localizer)
        {
            _teamWebService = teamWebService;
        }

        protected override Task InternalExecute(ITurnContext turnContext, ImageHuntState state)
        {
            throw new NotImplementedException();
        }
    }
}
