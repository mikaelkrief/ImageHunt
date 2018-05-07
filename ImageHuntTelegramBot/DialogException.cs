using System;

namespace ImageHuntTelegramBot
{
  public class DialogException : Exception
  {
    public DialogException(string message) : base(message)
    {
      
    }
  }
}