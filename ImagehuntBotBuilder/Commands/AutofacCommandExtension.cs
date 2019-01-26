using Autofac;
using Castle.Core.Internal;
using ImageHuntBotBuilder.Commands.Interfaces;

namespace ImageHuntBotBuilder.Commands
{
    public static class AutofacCommandExtension
    {
        public static void RegisterCommand<C>(this ContainerBuilder containerBuilder) where C : ICommand
        {
            var commandType = typeof(C);
            var commandAttribute = commandType.GetAttribute<CommandAttribute>();

            containerBuilder.RegisterType<C>().Named<ICommand>(commandAttribute.Command);
        }
    }
}