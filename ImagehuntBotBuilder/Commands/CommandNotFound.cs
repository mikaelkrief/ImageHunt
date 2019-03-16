using System;

namespace ImageHuntBotBuilder.Commands
{
    public class CommandNotFound : Exception
    {
        public CommandNotFound(string command)
        {
            Command = command;
        }

        public string Command { get; set; }
    }
}