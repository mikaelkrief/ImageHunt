using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using ImageHuntTelegramBot.ChatServices;
using Microsoft.Extensions.Options;
using Telegram.Bot;
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
        if (update.Type != UpdateType.MessageUpdate)
          return;
        var message = update.Message;
        IChatService chatService;
        var chatId = message.Chat.Id;
        try
        {
          chatService = _registeredChatServices[chatId];
 
        }
      catch (KeyNotFoundException )
        {
          _registeredChatServices.Add(chatId, chatService = SwitchChatService(message.Text));
        }
           await chatService.Update(update);
        if (!chatService.Listen)
          _registeredChatServices.Remove(chatId);
      }

      public static void ClearRegisteredListener()
      {
        _registeredChatServices.Clear();
      }
      private IChatService SwitchChatService(string messageText)
      {
        switch (messageText)
        {
        case "/init":
          return _container.Resolve<IInitChatService>();
        case "/startgame":
          return _container.Resolve<IStartChatService>();
        default:
          return _container.Resolve<IDefaultChatService>();
        }
      }
    }
}
