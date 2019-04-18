using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using ImageHuntWebServiceClient.WebServices;

namespace ImageHuntValidatorBot
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
            builder.RegisterType<ImageWebService>().As<IImageWebService>();

            // Register commands
            builder.RegisterType<CommandRepository>().AsImplementedInterfaces().SingleInstance();

        }
    }
}
