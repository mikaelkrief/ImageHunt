using System.Threading.Tasks;
using Autofac;
using FakeItEasy;
using ImageHuntTelegramBot;
using ImageHuntTelegramBot.Dialogs;
using ImageHuntTelegramBot.Dialogs.Prompts;
using TestUtilities;
using Xunit;

namespace ImageHuntBotTest
{
    public class InitDialogTest : BaseTest
    {
      private IInitDialog _target;

      public InitDialogTest()
      {
        _testContainerBuilder.RegisterType<InitDialog>().As<IInitDialog>();
        _container = _testContainerBuilder.Build();
        _target = _container.Resolve<IInitDialog>();
      }
      [Fact]
      public async Task Begin_Call_Sub_Dialogs()
      {
        // Arrange
        var context = A.Fake<ITurnContext>();
        // Act
        await _target.Begin(context);
        // Assert
        A.CallTo(() => context.Begin(A<NumberPrompt<int>>._)).MustHaveHappened();
      }

  }
}
