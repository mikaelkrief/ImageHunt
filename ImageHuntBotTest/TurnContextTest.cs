using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using FakeItEasy;
using ImageHuntBot;
using ImageHuntTelegramBot;
using Microsoft.Extensions.Logging;
using NFluent;
using Telegram.Bot.Types;
using TestUtilities;
using Xunit;

namespace ImageHuntBotTest
{
    class DummyState : IBaseState
    {

        public int TheInt { get; set; }
        public string TheString { get; set; }
        public long ChatId { get; set; }
    }

    public class TurnContextTest : BaseTest
    {
        private TurnContext _target;
        private IAdapter _adapter;
        private IStorage _storage;
        private ILogger<TurnContext> _logger;

        public TurnContextTest()
        {
            _testContainerBuilder.RegisterType<TurnContext>();
            _logger = A.Fake<ILogger<TurnContext>>();
            _testContainerBuilder.RegisterInstance(_logger);
            _adapter = A.Fake<IAdapter>();
            _storage = A.Fake<IStorage>();
            _testContainerBuilder.RegisterInstance(_adapter).As<IAdapter>();
            _testContainerBuilder.RegisterInstance(_storage).As<IStorage>();

            _container = _testContainerBuilder.Build();
            _target = _container.Resolve<TurnContext>();
        }

        [Fact]
        public void GetConversationState()
        {
            // Arrange
            _target.ChatId = 156;
            // Act
            var result = _target.GetConversationState<DummyState>();
            // Assert
            Check.That(result).IsInstanceOf<DummyState>();
            Check.That(result.ChatId).Equals(156);
        }

        [Fact]
        public void GetAllConversationState()
        {
            // Arrange

            // Act
            var results = _target.GetAllConversationState<DummyState>();
            // Assert
        }
        [Fact]
        public async Task Begin()
        {
            // Arrange
            var dialog = A.Fake<IDialog>();

            // Act
            await _target.Begin(dialog);
            // Assert
            A.CallTo(() => dialog.Begin(A<ITurnContext>._, A<bool>._)).MustHaveHappened();
        }
        [Fact]
        public async Task Begin_errorInDialog()
        {
            // Arrange
            var dialog = A.Fake<IDialog>();
            A.CallTo(() => dialog.Begin(A<ITurnContext>._, A<bool>._)).Throws<Exception>();

            // Act
            await _target.Begin(dialog);
            // Assert
            A.CallTo(() => dialog.Begin(A<ITurnContext>._, A<bool>._)).MustHaveHappened();
            //A.CallTo(() => _logger.Log())
        }
        [Fact]
        public async Task Continue_Nothing_ToContinue()
        {
            // Arrange

            // Act
            Check.ThatAsyncCode(_target.Continue).DoesNotThrow();
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
            A.CallTo(() => _adapter.SendActivity(A<Activity>.That.Matches(a => a.Text == "toto"))).MustHaveHappened();
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

        [Fact]
        public async Task StorageIsRestoredWhenTurnBegin()
        {
            // Arrange
            var dialog = A.Fake<IDialog>();
            _target.ChatId = 15;
            await _target.ResetConversationStates<DummyState>();
            A.CallTo(() => _storage.Read(A<string[]>._))
              .Returns(new[] { new KeyValuePair<string, object>("15", new DummyState()), });
            // Act
            await _target.Begin(dialog);
            // Assert
            A.CallTo(() => _storage.Read(A<string[]>._)).MustHaveHappened();
        }
        [Fact]
        public async Task StorageIsRestoredWhenTurnBeginNoStateAvailable()
        {
            // Arrange
            var dialog = A.Fake<IDialog>();
            _target.ChatId = 19;
            A.CallTo(() => _storage.Read(A<string[]>._))
              .Returns(new KeyValuePair<string, object>[] { });
            // Act
            await _target.Begin(dialog);
            // Assert
            A.CallTo(() => _storage.Read(A<string[]>._)).MustHaveHappened();
        }
        [Fact]
        public async Task StorageIsSavedWhenTurnEnd()
        {
            // Arrange
            var dialog = A.Fake<IDialog>();
            _target.ChatId = 15;
            A.CallTo(() => _storage.Read(A<string[]>._))
              .Returns(new[] { new KeyValuePair<string, object>("15", new DummyState()), });

            // Act
            await _target.Begin(dialog);
            await _target.End();
            // Assert
            A.CallTo(() => _storage.Write(A<IEnumerable<KeyValuePair<string, object>>>._)).MustHaveHappened();
        }

        [Fact]
        public void NewChatMemberMessageReceived()
        {
            // Arrange
            
            // Act

            // Assert
        }

        [Fact]
        public async Task LeaveChat()
        {
            // Arrange
            var activity = new Activity() {ChatId = 15};
            _target.Activity = activity;
            // Act
            await _target.Leave();
            // Assert
            A.CallTo(() => _adapter.Leave(A<ChatId>._)).MustHaveHappened();
        }

    }
}
