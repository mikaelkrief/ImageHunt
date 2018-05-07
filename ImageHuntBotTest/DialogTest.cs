using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using FakeItEasy;
using ImageHuntTelegramBot;
using TestUtilities;
using Xunit;

namespace ImageHuntBotTest
{
  public class DialogTest : BaseTest
    {
      private IDialog _target;

      public DialogTest()
      {

        _container = _testContainerBuilder.Build();
        _target = A.Fake<AbstractDialog>();
      }

    [Fact]
      public async Task AddChildren_And_Begin()
      {
      // Arrange
        _target = A.Fake<AbstractDialog>(options=>options.CallsBaseMethods());

      var childrenDialog = A.Fake<IDialog>();
        _target.AddChildren(childrenDialog);
        var context = A.Fake<ITurnContext>();
        // Act
        await _target.Begin(context);
        // Assert
        A.CallTo(() => childrenDialog.Begin(context)).MustHaveHappened();
      }
    [Fact]
      public async Task AddChildren_And_Begin_And_Continue()
      {
      // Arrange
        _target = A.Fake<AbstractDialog>(options=>options.CallsBaseMethods());

      var childrenDialog = A.Fake<IDialog>();
        _target.AddChildren(childrenDialog);
        var context = A.Fake<ITurnContext>();
        var activity1 = A.Fake<IActivity>();
        // Act
        A.CallTo(() => context.Activity).Returns(activity1);
        await _target.Begin(context);
        await _target.Continue(context);
        // Assert
        A.CallTo(() => childrenDialog.Begin(context)).MustHaveHappened();
      }

      class DummyDialog : AbstractDialog
      {
        public override async Task Reply(ITurnContext turnContext)
        {
          var activity = A.Fake<IActivity>();
          await turnContext.ReplyActivity(activity);
        }
      }

      [Fact]
      public void NoChildren_Begin_Should_Reply()
      {
        // Arrange
        _target = new DummyDialog();
        var context = A.Fake<ITurnContext>();
        // Act
        _target.Begin(context);
        // Assert
        A.CallTo(() => context.ReplyActivity(A<IActivity>._)).MustHaveHappened();
      }
    }
}
