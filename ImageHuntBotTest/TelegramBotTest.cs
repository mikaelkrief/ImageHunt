using System;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using FakeItEasy;
using ImageHuntBotTest.ChatServices;
using ImageHuntTelegramBot;
using ImageHuntTelegramBot.ChatServices;
using ImageHuntTelegramBot.Services;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Xunit;

namespace ImageHuntBotTest
{
  public class TelegramBotTest
  {
    private ContainerBuilder _containerBuilder;
    private ITelegramBotClient _telegramBotClient;
    private IUpdateHub _target;
    private IDefaultChatService _defaultChatService;
    private IInitChatService _initChatService;
    private IStartChatService _startChatService;

    public TelegramBotTest()
    {
      _containerBuilder = new ContainerBuilder();
      var optionConfig = A.Fake<IOptions<BotConfiguration>>();
      _containerBuilder.RegisterInstance(optionConfig);
      var botConfig = new BotConfiguration() { BotToken = "Toto" };
      A.CallTo(() => optionConfig.Value).Returns(botConfig);
      _telegramBotClient = A.Fake<ITelegramBotClient>();
      _containerBuilder.RegisterInstance(_telegramBotClient);
      _defaultChatService = A.Fake<IDefaultChatService>();
      _containerBuilder.RegisterInstance(_defaultChatService);
      _initChatService = A.Fake<IInitChatService>();
      _containerBuilder.RegisterInstance(_initChatService);
      _startChatService = A.Fake<IStartChatService>();
      _containerBuilder.RegisterInstance(_startChatService);


      var container = _containerBuilder.Build();
      _target = new UpdateHub(container.Resolve<IOptions<BotConfiguration>>(), container);
    }
    [Fact]
    public async Task UnknownMessage_Received()
    {
      // Arrange
      var update = new Update() { Message = new Message() { Text = "toto", Chat = new Chat() { Id = 15 } } };
      UpdateHub.ClearRegisteredListener();
      // Act
      await _target.Switch(update);
      // Assert
      A.CallTo(() => _defaultChatService.Message(A<Message>._)).MustHaveHappened();
    }

    [Fact]
    public async Task InitMessage_Received()
    {
      // Arrange
      var update = new Update() { Message = new Message() { Text = "/init", Chat = new Chat() { Id = 15 } } };
      A.CallTo(() => _initChatService.Listen).Returns(true);
      UpdateHub.ClearRegisteredListener();
      // Act
      await _target.Switch(update);
      // Assert
      A.CallTo(() => _initChatService.Message(update.Message)).MustHaveHappened();
    }
    [Fact]
    public async Task StartMessage_Received()
    {
      // Arrange
      var update = new Update() { Message = new Message() { Text = "/startgame", Chat = new Chat() { Id = 15 } } };
      A.CallTo(() => _startChatService.Listen).Returns(true);
      UpdateHub.ClearRegisteredListener();
      // Act
      await _target.Switch(update);
      // Assert
      A.CallTo(() => _startChatService.Message(update.Message)).MustHaveHappened();
    }

    [Fact]
    public async Task InitThenUnknown_MessageReceived()
    {
      // Arrange
      var update1 = new Update() { Message = new Message() { Text = "/init", Chat = new Chat() { Id = 15 } } };
      var update2 = new Update() { Message = new Message() { Text = "toto", Chat = new Chat() { Id = 15 } } };
      UpdateHub.ClearRegisteredListener();
      // Act
      await _target.Switch(update1);
      await _target.Switch(update2);
      // Assert
      A.CallTo(() => _initChatService.Message(A<Message>._)).MustHaveHappened();
      A.CallTo(() => _defaultChatService.Message(A<Message>._)).MustHaveHappened();
    }
    [Fact]
    public async Task Init2TimesThenUnknown_MessageReceived()
    {
      // Arrange
      var update1 = new Update() { Message = new Message() { Text = "/init", Chat = new Chat() { Id = 15 } } };
      var update2 = new Update() { Message = new Message() { Text = "/init", Chat = new Chat() { Id = 15 } } };
      var update3 = new Update() { Message = new Message() { Text = "toto", Chat = new Chat() { Id = 15 } } };
      A.CallTo(() => _initChatService.Listen).ReturnsNextFromSequence(new[] { true, false });
      UpdateHub.ClearRegisteredListener();
      // Act
      await _target.Switch(update1);
      await _target.Switch(update2);
      await _target.Switch(update3);
      // Assert
      A.CallTo(() => _initChatService.Message(A<Message>._)).MustHaveHappened(Repeated.Exactly.Twice);
      A.CallTo(() => _defaultChatService.Message(A<Message>._)).MustHaveHappened(Repeated.Exactly.Once);
    }

    [Fact]
    public async Task Callback_QueryReceived()
    {
      // Arrange
      var update1 = new Update() { Message = new Message() { Text = "/init", Chat = new Chat() { Id = 15 } } };
      var update2 = new Update() { CallbackQuery = new CallbackQuery() { Message = new Message() { Chat = new Chat() { Id = 15 } } } };
      A.CallTo(() => _initChatService.Listen).ReturnsNextFromSequence(new[] { true, false });
      // Act
      // Call with text to get a chatService
      await _target.Switch(update1);
      // call on query
      await _target.Switch(update2);

      // Assert
      A.CallTo(() => _initChatService.Message(A<Message>._)).MustHaveHappened();
      A.CallTo(() => _initChatService.CallbackQuery(A<CallbackQuery>._)).MustHaveHappened();
    }
  }

}
