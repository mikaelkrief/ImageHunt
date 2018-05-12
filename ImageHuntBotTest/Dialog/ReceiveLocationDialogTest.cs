using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using FakeItEasy;
using ImageHuntTelegramBot;
using ImageHuntTelegramBot.Dialogs;
using TestUtilities;
using Xunit;

namespace ImageHuntBotTest.Dialog
{
    public class ReceiveLocationDialogTest : BaseTest
    {
      private IReceiveLocationDialog _target;

      public ReceiveLocationDialogTest()
      {
        _testContainerBuilder.RegisterType<ReceiveLocationDialog>().As<IReceiveLocationDialog>();
        _container = _testContainerBuilder.Build();
        _target = _container.Resolve<IReceiveLocationDialog>();
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

  public class ReceiveLocationDialog : AbstractDialog, IReceiveLocationDialog
  {
    public override Task Begin(ITurnContext turnContext)
    {
      return base.Begin(turnContext);
    }
  }

  public interface IReceiveLocationDialog : IDialog
  {
  }
}
