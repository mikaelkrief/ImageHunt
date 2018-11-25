using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Castle.Core.Internal;
using ImageHuntBotBuilder.Commands;
using ImageHuntWebServiceClient.WebServices;

namespace ImageHuntBotBuilder
{
    public class DefaultModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ActionWebService>().As<IActionWebService>();
            builder.RegisterType<TeamWebService>().As<ITeamWebService>();
            builder.RegisterType<GameWebService>().As<IGameWebService>();
            builder.RegisterType<AdminWebService>().As<IAdminWebService>();
            builder.RegisterType<NodeWebService>().As<INodeWebService>();

            builder.RegisterType<CommandRepository>().AsImplementedInterfaces().SingleInstance();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .PublicOnly()
                .Where(t => t.Name.EndsWith("Command") && t.IsClass)
                .AsImplementedInterfaces()
                .Named<ICommand>(ct =>
                {
                    var ca = ct.GetAttribute<CommandAttribute>();
                    return ca.Command;
                });
            builder.RegisterType<NodeVisitorHandler>().AsImplementedInterfaces();
        }
    }
}
