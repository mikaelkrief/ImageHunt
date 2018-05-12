using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using FakeItEasy;
using ImageHuntTelegramBot;
using ImageHuntTelegramBot.Dialogs;
using TestUtilities;
using Xunit;

namespace ImageHuntBotTest
{
    public class DisplayDialogTest : BaseTest
    {
      private DisplayDialog _target;

      public DisplayDialogTest()
      {
        _testContainerBuilder.RegisterType<DisplayDialog>().WithParameter("displayMessage", "toto");
        _container = _testContainerBuilder.Build();
        _target = _container.Resolve<DisplayDialog>();
      }

      [Fact]
      public async Task DisplayMessage()
      {
        // Arrange
        var turnContext = A.Fake<ITurnContext>();
        // Act
        await _target.Begin(turnContext);
        // Assert
        A.CallTo(() => turnContext.ReplyActivity(A<IActivity>.That.Matches(a => a.Text == "toto")));
        A.CallTo(() => turnContext.End()).MustHaveHappened();
      }
    }

  public class DisplayDialog : AbstractDialog, IDisplayDialog
  {
    private readonly string _displayMessage;

    public DisplayDialog(string displayMessage)
    {
      _displayMessage = displayMessage;
    }
    public override async Task Begin(ITurnContext turnContext)
    {
      var activity = new Activity(){};
      await turnContext.ReplyActivity(activity);
      await turnContext.End();
    }
  }
}
