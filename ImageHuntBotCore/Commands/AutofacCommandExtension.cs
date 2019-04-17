using System.Linq;
using Autofac;
using ImageHuntBotCore.Commands.Interfaces;

namespace ImageHuntBotCore.Commands
{
    public static class AutofacCommandExtension
    {
        public static void RegisterCommand<TC>(this ContainerBuilder containerBuilder)
            where TC : ICommand<IState>
        {
            var commandType = typeof(TC);
            var commandAttribute = commandType.GetCustomAttributes(false).Single(a=> a is CommandAttribute) as CommandAttribute;

            containerBuilder.RegisterType<TC>().Named<ICommand<IState>>(commandAttribute.Command);
        }
    }
}