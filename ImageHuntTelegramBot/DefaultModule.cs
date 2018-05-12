using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using ImageHuntTelegramBot.Controllers;
using ImageHuntTelegramBot.Dialogs;
using ImageHuntWebServiceClient.WebServices;

namespace ImageHuntTelegramBot
{
    public class DefaultModule : Module
    {
      protected override void Load(ContainerBuilder builder)
      {
        builder.RegisterType<GameWebService>().As<IGameWebService>();
        builder.RegisterType<TeamWebService>().As<ITeamWebService>();
        builder.RegisterType<InitDialog>().As<IInitDialog>();
        builder.RegisterType<ReceiveImageDialog>().As<IReceiveImageDialog>();
        builder.RegisterType<ReceiveLocationDialog>().As<IReceiveLocationDialog>();
        builder.Register(t =>
        {
          var bot = new TelegramBot();
          var initDialog = t.Resolve<IInitDialog>();
          bot.AddDialog("/init", initDialog);
          var receiveImageDialog = t.Resolve<IReceiveImageDialog>();
          bot.AddDialog("/uploadphoto", receiveImageDialog);
          var receiveLocationDialog = t.Resolve<IReceiveLocationDialog>();
          bot.AddDialog("/location", receiveLocationDialog);
          return bot;
        }).As<IBot>();
        builder.RegisterType<TelegramAdapter>().As<IAdapter>();
        builder.RegisterType<TurnContext>().As<ITurnContext>();
        builder.RegisterType<ContextHub>().SingleInstance();
      }
  }
}
