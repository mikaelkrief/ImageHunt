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

      public TurnContextTest()
      {
        _testContainerBuilder.RegisterType<TurnContext>();

        _container = _testContainerBuilder.Build();
        _target = _container.Resolve<TurnContext>();
      }

      [Fact]
      public void GetConversationState()
      {
        // Arrange
        _target.ChatId = "15";
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
        _target.CurrentDialog = A.Fake<IDialog>();
        // Act
        await _target.Continue();
        // Assert
        A.CallTo(() => _target.CurrentDialog.Continue(A<ITurnContext>._)).MustHaveHappened();
      }

      [Fact]
      public async Task End()
      {
        // Arrange
        _target.CurrentDialog = A.Fake<IDialog>();
        // Act
        await _target.End();
        // Assert
        Check.That(_target.CurrentDialog).IsNull();
      }
      
    }
}
