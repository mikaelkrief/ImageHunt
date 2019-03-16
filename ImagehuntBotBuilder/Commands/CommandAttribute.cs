using System;

namespace ImageHuntBotBuilder.Commands
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CommandAttribute : Attribute
    {
        public CommandAttribute(string command)
        {
            Command = command.ToLowerInvariant();
        }

        public string Command { get; }
    }
}