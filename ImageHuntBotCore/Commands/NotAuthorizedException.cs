using System;

namespace ImageHuntBotCore.Commands
{
    public class NotAuthorizedException : Exception
    {
        public string UserName { get; }

        public NotAuthorizedException(string userName)
        {
            UserName = userName;
        }
    }
}