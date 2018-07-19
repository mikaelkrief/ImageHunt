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
        builder.RegisterType<GameWebService>().As<IGameWebService>();
        builder.RegisterType<TeamWebService>().As<ITeamWebService>();
        builder.RegisterType<ActionWebService>().As<IActionWebService>();
        builder.RegisterType<InitDialog>().As<IInitDialog>();
        builder.RegisterType<ReceiveImageDialog>().As<IReceiveImageDialog>();
        builder.RegisterType<ReceiveDocumentDialog>().As<IReceiveDocumentDialog>();
        builder.RegisterType<ReceiveLocationDialog>().As<IReceiveLocationDialog>();
        builder.RegisterType<ResetDialog>().As<IResetDialog>();
        builder.Register(t =>
        {
          var bot = new TelegramBot();
          var initDialog = t.Resolve<IInitDialog>();
          bot.AddDialog("/init", initDialog);
          var receiveImageDialog = t.Resolve<IReceiveImageDialog>();
          bot.AddDialog("/uploadphoto", receiveImageDialog);
          var receiveDocumentDialog = t.Resolve<IReceiveDocumentDialog>();
          bot.AddDialog("/uploaddocument", receiveDocumentDialog);
          var receiveLocationDialog = t.Resolve<IReceiveLocationDialog>();
          bot.AddDialog("/location", receiveLocationDialog);
          var resetDialog = t.Resolve<IResetDialog>();
          bot.AddDialog("/reset", resetDialog);
          return bot;
        }).As<IBot>();
        builder.RegisterType<TelegramAdapter>().As<IAdapter>();
        builder.RegisterType<TurnContext>().As<ITurnContext>();
        builder.RegisterType<ContextHub>().SingleInstance();
      }
  }
}
