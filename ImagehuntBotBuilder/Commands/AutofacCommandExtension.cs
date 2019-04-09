using Autofac;
using Castle.Core.Internal;
using ImageHuntBotBuilder.Commands.Interfaces;

namespace ImageHuntBotBuilder.Commands
{
    public static class AutofacCommandExtension
    {
        public static void RegisterCommand<TC>(this ContainerBuilder containerBuilder)
            where TC : ICommand
        {
            var commandType = typeof(TC);
            var commandAttribute = commandType.GetAttribute<CommandAttribute>();

            containerBuilder.RegisterType<TC>().Named<ICommand>(commandAttribute.Command);
        }
    }
}