using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using FakeItEasy;
using ImageHuntTelegramBot;
using NFluent;
using TestUtilities;
using Xunit;

namespace ImageHuntBotTest
{
  class DummyState
  {
    public int TheInt { get; set; }
    public string TheString { get; set; }
  }
    public class TurnContextTest : BaseTest
    {
      private TurnContext _target;
      private IAdapter _adapter;

      public TurnContextTest()
      {
        _testContainerBuilder.RegisterType<TurnContext>();
        _adapter = A.Fake<IAdapter>();
        _testContainerBuilder.RegisterInstance(_adapter).As<IAdapter>();

        _container = _testContainerBuilder.Build();
        _target = _container.Resolve<TurnContext>();
      }

      [Fact]
      public void GetConversationState()
      {
        // Arrange
        _target.ChatId = 15;
        // Act
        var result = _target.GetConversationState<DummyState>();
        // Assert
        Check.That(result).IsInstanceOf<DummyState>();
      }

      [Fact]
      public async Task Begin()
      {
      // Arrange
        var dialog = A.Fake<IDialog>();
        
        // Act
        await _target.Begin(dialog);
        // Assert
        A.CallTo(() => dialog.Begin(A<ITurnContext>._)).MustHaveHappened();
      }
      [Fact]
      public async Task Continue_Nothing_ToContinue()
      {
        // Arrange
        
        // Act
        Check.ThatAsyncCode( _target.Continue).DoesNotThrow();
        // Assert
      }

      [Fact]
      public async Task Continue_DialogPending()
      {
        // Arrange
        _target.Begin(A.Fake<IDialog>());
        // Act
        await _target.Continue();
        // Assert
        A.CallTo(() => _target.CurrentDialog.Continue(A<ITurnContext>._)).MustHaveHappened();
      }

      [Fact]
      public async Task End()
      {
        // Arrange
        await _target.Begin(A.Fake<IDialog>());
        await _target.ReplyActivity(A.Fake<IActivity>());
        // Act
        await _target.End();
        // Assert
        Check.That(_target.CurrentDialog).IsNull();
        Check.That(_target.Replied).IsFalse();
      }

      [Fact]
      public async Task ReplyActivity()
      {
        // Arrange
        var activity = new Activity();
        // Act
        await _target.ReplyActivity(activity);
        // Assert
        A.CallTo(() => _adapter.SendActivity(activity)).MustHaveHappened();
        Check.That(_target.Replied).IsTrue();
      }
      [Fact]
      public async Task ReplyActivityText()
      {
        // Arrange
        // Act
        await _target.ReplyActivity("toto");
        // Assert
        A.CallTo(() => _adapter.SendActivity(A<Activity>.That.Matches(a=>a.Text == "toto"))).MustHaveHappened();
        Check.That(_target.Replied).IsTrue();
      }

      [Fact]
      public async Task SendActivity()
      {
      // Arrange
        var activity = new Activity();
        // Act
        await _target.SendActivity(activity);
        // Assert
        A.CallTo(() => _adapter.SendActivity(activity)).MustHaveHappened();
      }
  }
}
