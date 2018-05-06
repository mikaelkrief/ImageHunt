using System.Collections.Generic;
using Autofac;
using FakeItEasy;
using ImageHuntTelegramBot.ChatServices;
using Telegram.Bot;
using TestUtilities;

namespace ImageHuntBotTest.ChatServices
{
  public class ChatServiceBaseTest : BaseTest
  {
    protected Dictionary<long, ChatProperties> ChatPropertiesForChatId = new Dictionary<long, ChatProperties>();
    protected ContainerBuilder _containerBuilder;
    protected IContainer _container;
    private ITelegramBotClient _telegramClient;

    public ChatServiceBaseTest()
    {
      _containerBuilder = new ContainerBuilder();
      _telegramClient = A.Fake<ITelegramBotClient>();
      _containerBuilder.RegisterInstance(_telegramClient);
      _containerBuilder.RegisterType<Dictionary<long, ChatProperties>>().SingleInstance();

    }

    public void BuildDependencies()
    {
      _container = _containerBuilder.Build();
    }

    protected T Resolve<T>()
    {
      return _container.Resolve<T>();
    }
  }
}