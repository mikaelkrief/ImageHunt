using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using FakeItEasy;
using ImageHuntTelegramBot;
using ImageHuntTelegramBot.Dialogs;
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
      public void Begin_Call_Sub_Dialogs()
      {
        // Arrange
        var subDialog = new[] {A.Fake<IDialog>(), A.Fake<IDialog>()};
        var context = A.Fake<ITurnContext>();
        foreach (var dialog in subDialog)
        {
          _target.AddChildren(dialog);
        }
        // Act
        _target.Begin(context);
        // Assert
        A.CallTo(() => subDialog[0].Begin(context)).MustHaveHappened();
      }
    }
}
