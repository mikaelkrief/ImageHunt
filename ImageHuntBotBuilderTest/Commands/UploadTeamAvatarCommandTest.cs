using System;
using System.Threading.Tasks;
using Autofac;
using FakeItEasy;
using ImageHuntBotBuilder;
using ImageHuntBotBuilder.Commands;
using ImageHuntBotBuilder.Commands.Interfaces;
using ImageHuntBotCore.Commands;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Bot.Builder;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using TestUtilities;

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
            TestContainerBuilder.RegisterInstance(_logger = A.Fake<ILogger<IUploadTeamAvatarCommand>>());
            TestContainerBuilder.RegisterInstance(_teamWebService = A.Fake<ITeamWebService>());
            _turnContext = A.Fake<ITurnContext>();
            _state = new ImageHuntState() { Status = Status.Started };

            Build();
        }

        //[Fact]
        public async Task Should_Upload_Avatar_Update_Team()
        {
            // Arrange
            
            // Act
            await Target.ExecuteAsync(_turnContext, _state);
            // Assert
        }
    }
    [Command("upIcon")]
    public class UploadTeamAvatarCommand : AbstractCommand<ImageHuntState>, IUploadTeamAvatarCommand
    {
        private readonly ITeamWebService _teamWebService;

        public UploadTeamAvatarCommand(ILogger<IUploadTeamAvatarCommand> logger, ITeamWebService teamWebService, IStringLocalizer<UploadTeamAvatarCommand> localizer) : base(logger, localizer)
        {
            _teamWebService = teamWebService;
        }

        protected override Task InternalExecuteAsync(ITurnContext turnContext, ImageHuntState state)
        {
            throw new NotImplementedException();
        }
    }
}
