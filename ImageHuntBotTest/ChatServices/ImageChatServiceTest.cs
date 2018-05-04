using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using ImageHuntTelegramBot.ChatServices;
using ImageHuntTelegramBot.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Xunit;

namespace ImageHuntBotTest.ChatServices
{
    public class ImageChatServiceTest : ChatServiceBaseTest
    {
      private ImageChatService _target;
      private Dictionary<long, ChatProperties> _chatPropertiesForChatId;

      public ImageChatServiceTest()
      {

        _containerBuilder.RegisterType<ImageChatService>();
        BuildDependencies();
        _target = Resolve<ImageChatService>();
      }
      [Fact]
      public async Task GetImagesToGuess()
      {
        // Arrange
        var message = new Message(){Chat = new Chat(){Id = 15}, Text = "/images"};
        // Act
        await _target.Message(message);
        // Assert
      }
    }

  public class ImageChatService : AbstractChatService, IImageChatService
  {
    public ImageChatService(ITelegramBotClient client, 
      Dictionary<long, ChatProperties> chatPropertiesForChatId) : base(client, chatPropertiesForChatId)
    {
    }

    protected override async Task HandleMessage(Message message)
    {
      switch (message.Text)
      {
        case string s when s.StartsWith("/images"):
          break;
      }
    }

    protected override Task HandleCallbackQuery(CallbackQuery callbackQuery)
    {
      throw new NotImplementedException();
    }
  }

  public interface IImageChatService : IChatService
  {
  }
}
