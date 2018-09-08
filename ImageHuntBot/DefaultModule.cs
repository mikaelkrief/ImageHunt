using System.Collections.Generic;
using Autofac;
using ImageHuntTelegramBot;
using ImageHuntTelegramBot.Controllers;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Extensions.Logging;

namespace ImageHuntBot
{
    public class DefaultModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(ThisAssembly)
                .PublicOnly()
                .Where(t => t.Name.EndsWith("Dialog"))
                .AsImplementedInterfaces();
            builder.RegisterType<GameWebService>().As<IGameWebService>();
            builder.RegisterType<TeamWebService>().As<ITeamWebService>();
            builder.RegisterType<ActionWebService>().As<IActionWebService>();
            builder.RegisterType<AdminWebService>().As<IAdminWebService>();
            builder.RegisterType<PasscodeWebService>().As<IPasscodeWebService>();
            builder.Register(t =>
            {
                var bot = new TelegramBot(t.Resolve<ILogger<TelegramBot>>());
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
