using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using ImageHuntTelegramBot.ChatServices;
using Microsoft.Extensions.Options;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ImageHuntTelegramBot.Services
{
  public class UpdateHub : IUpdateHub
  {
    private readonly BotConfiguration _config;
    private readonly IContainer _container;
    private static Dictionary<long, IChatService> _registeredChatServices = new Dictionary<long, IChatService>();

    public UpdateHub(IOptions<BotConfiguration> config, IContainer container)
    {
      _config = config.Value;
      _container = container;
    }
    public async Task Switch(Update update)
    {
      IChatService chatService;
      switch (update.Type)
      {
        case UpdateType.MessageUpdate:
          {
            var message = update.Message;
            var chatId = message.Chat.Id;
            chatService = GetChatService(chatId, message);

            await chatService.Message(message);
            if (!chatService.Listen)
              _registeredChatServices.Remove(chatId);
          }
          break;
        case UpdateType.CallbackQueryUpdate:
          {
            var query = update.CallbackQuery;
            var message = query.Message;
            var chatId = message.Chat.Id;
            chatService = GetChatService(chatId, message);
            await chatService.CallbackQuery(query);
          }
          break;
      }
    }

    private IChatService GetChatService(long chatId, Message message)
    {
      IChatService chatService;
      try
      {
        chatService = _registeredChatServices[chatId];
      }
      catch (KeyNotFoundException)
      {
        _registeredChatServices.Add(chatId, chatService = SwitchChatService(message.Text));
      }

      return chatService;
    }

    public static void ClearRegisteredListener()
    {
      _registeredChatServices.Clear();
    }
    private IChatService SwitchChatService(string messageText)
    {
      switch (messageText)
      {
        case var s when s.StartsWith("/init"):
          return _container.Resolve<IInitChatService>();
        case var s when s.StartsWith("/startgame"):
          return _container.Resolve<IStartChatService>();
        default:
          return _container.Resolve<IDefaultChatService>();
      }
    }
  }
}
