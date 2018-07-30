using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using ImageHuntBot.Dialogs;
using ImageHuntTelegramBot.Controllers;
using ImageHuntTelegramBot.Dialogs;
using ImageHuntWebServiceClient.WebServices;

namespace ImageHuntTelegramBot
{
    public class DefaultModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(this.ThisAssembly)
                .PublicOnly()
                .Where(t => t.Name.EndsWith("Dialog"))
                .AsImplementedInterfaces();
            builder.RegisterType<GameWebService>().As<IGameWebService>();
            builder.RegisterType<TeamWebService>().As<ITeamWebService>();
            builder.RegisterType<ActionWebService>().As<IActionWebService>();
            builder.RegisterType<AdminWebService>().As<IAdminWebService>();
            builder.Register(t =>
            {
                var bot = new TelegramBot();
                var dialogs = t.Resolve<IEnumerable<IDialog>>();
                foreach (var dialog in dialogs)
                {
                    bot.AddDialog(dialog);
                }
                return bot;
            }).As<IBot>();
            builder.RegisterType<TelegramAdapter>().As<IAdapter>();
            builder.RegisterType<TurnContext>().As<ITurnContext>();
            builder.RegisterType<ContextHub>().SingleInstance();
        }
    }
}
