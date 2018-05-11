using System.Threading;
using System.Threading.Tasks;
using Autofac;
using FakeItEasy;
using ImageHuntTelegramBot;
using NFluent;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TestUtilities;
using Xunit;

namespace ImageHuntBotTest
{
  public class TelegramAdapterTest : BaseTest
  {
    private IAdapter _target;
    private ITelegramBotClient _telegramClient;

    public TelegramAdapterTest()
    {
      _testContainerBuilder.RegisterType<TelegramAdapter>().As<IAdapter>();
      _telegramClient = A.Fake<ITelegramBotClient>();
      _testContainerBuilder.RegisterInstance(_telegramClient);
      _container = _testContainerBuilder.Build();
      _target = _container.Resolve<IAdapter>();
    }

    [Fact]
    public async Task SendActivity_Message()
    {
      // Arrange
      var activity = new Activity(){ChatId=15, ActivityType = ActivityType.Message};
      // Act
      await _target.SendActivity(activity);
      // Assert
      A.CallTo(() => _telegramClient.SendTextMessageAsync(A<ChatId>._, A<string>._, A<ParseMode>._, A<bool>._, A<bool>._, A<int>._, A<IReplyMarkup>._, A<CancellationToken>._)).MustHaveHappened();
    }

    [Fact]
    public async Task CreateActivityFromUpdate_Message()
    {
      // Arrange
      var update = new Update(){Message = new Message(){Text = "toto", Chat = new Chat(){Id = 15}}};
      // Act
      var activity = await _target.CreateActivityFromUpdate(update);
      // Assert
      Check.That(activity.ActivityType).Equals(ActivityType.Message);
      Check.That(activity.Text).Equals(update.Message.Text);
      Check.That(activity.ChatId).Equals(update.Message.Chat.Id);
    }
    [Fact]
    public async Task CreateActivityFromUpdate_CallbackQuery()
    {
      // Arrange
      var update = new Update(){CallbackQuery = new CallbackQuery() {Message = new Message(){Text="toto", Chat = new Chat(){Id = 15}}}};
      // Act
      var activity = await _target.CreateActivityFromUpdate(update);
      // Assert
      Check.That(activity.ActivityType).Equals(ActivityType.CallbackQuery);
      Check.That(activity.Text).Equals(update.CallbackQuery.Message.Text);
      Check.That(activity.ChatId).Equals(update.CallbackQuery.Message.Chat.Id);
    }
  }
}