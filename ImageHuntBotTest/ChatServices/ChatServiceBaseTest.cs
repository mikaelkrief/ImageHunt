using System.Collections.Generic;
using ImageHuntTelegramBot.ChatServices;
using TestUtilities;

namespace ImageHuntBotTest.ChatServices
{
  public class ChatServiceBaseTest : BaseTest
  {
    protected Dictionary<long, ChatProperties> ChatPropertiesForChatId = new Dictionary<long, ChatProperties>();
  }
}