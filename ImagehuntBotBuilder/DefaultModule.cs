using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
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

            builder.RegisterType<CommandRepository>().AsImplementedInterfaces().SingleInstance();

            builder.RegisterCommand<ResetCommand>();
            builder.RegisterCommand<InitCommand>();
            builder.RegisterCommand<BeginCommand >();
            builder.RegisterCommand<DisplayStateCommand>();
        }
    }
}
