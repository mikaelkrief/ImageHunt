using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace ImageHuntTelegramBot.Services
{
  public interface IUpdateHub
    {
      Task Switch(Update update);
    }
}
