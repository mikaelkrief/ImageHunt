using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using ImageHuntWebServiceClient.WebServices;

namespace ImageHuntBotBuilder
{
    public class DefaultModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ActionWebService>().As<IActionWebService>();
            builder.RegisterType<TeamWebService>().As<ITeamWebService>();
        }
    }
}
