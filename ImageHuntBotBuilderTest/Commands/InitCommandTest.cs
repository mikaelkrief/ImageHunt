using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ImageHuntBotBuilder;
using ImageHuntBotBuilder.Commands;
using Microsoft.Bot.Builder;
using Microsoft.Extensions.Logging;
using TestUtilities;

namespace ImageHuntBotBuilderTest.Commands
{
    public class InitCommandTest : BaseTest<InitCommand>
    {
    }
    [Command("init")]
    public class InitCommand : AbstractCommand, IInitCommand
    {
        public InitCommand(ILogger logger) : base(logger)
        {
        }

        public override bool IsAdmin { get; }
        protected override Task InternalExecute(ITurnContext turnContext, ImageHuntState state)
        {
            throw new NotImplementedException();
        }
    }
}
