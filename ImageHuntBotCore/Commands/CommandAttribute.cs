using System;

namespace ImageHuntBotCore.Commands
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CommandAttribute : Attribute
    {
        public string Command { get; }

        public CommandAttribute(string command)
        {
            Command = command.ToLowerInvariant();
        }
    }
}