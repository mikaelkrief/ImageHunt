using System;
using System.Threading.Tasks;
using Autofac;
using FakeItEasy;
using Telegram.Bot;
using Telegram.Bot.Args;
using Xunit;

namespace ImageHuntBotTest
{
    public class TelegramBotTest
    {
      private ContainerBuilder _containerBuilder;
      private ITelegramBotClient _telegramBotClient;
      private TelegramBot _target;

      public TelegramBotTest()
      {
        _containerBuilder = new ContainerBuilder();
        _telegramBotClient = A.Fake<ITelegramBotClient>();
        _containerBuilder.RegisterInstance(_telegramBotClient);
        _containerBuilder.RegisterType<TelegramBot>();
        _target = _containerBuilder.Build().Resolve<TelegramBot>();
      }
        [Fact]
        public async Task InitMessage_Received()
        {
      // Arrange

          // Act
          await _target.Run();
          
          // Assert

        }
    }

  public class TelegramBot
  {
    private readonly ITelegramBotClient _telegramBotClient;
    private bool _run;

    public TelegramBot(ITelegramBotClient telegramBotClient)
    {
      _telegramBotClient = telegramBotClient;
      
    }

    protected void Init()
    {
      _telegramBotClient.OnMessage += TelegramBotClientOnOnMessage;
    }

    private void TelegramBotClientOnOnMessage(object sender, MessageEventArgs messageEventArgs)
    {
      throw new NotImplementedException();
    }

    public async Task Stop()
    {
      _telegramBotClient.StopReceiving();
      _run = false;
    }
    public async Task Run()
    {

      await Task.Run(() => _telegramBotClient.StartReceiving());

    }
  }
}
