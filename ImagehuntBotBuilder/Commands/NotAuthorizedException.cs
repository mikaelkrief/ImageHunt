using System;

namespace ImageHuntBotBuilder.Commands
{
    public class NotAuthorizedException : Exception
    {
        public NotAuthorizedException(string userName)
        {
            UserName = userName;
        }

        public string UserName { get; }
    }
}