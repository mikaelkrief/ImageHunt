using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageHuntTelegramBot;
using ImageHuntTelegramBot.Dialogs;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;

namespace ImageHuntBot.Dialogs
{
    public class ListDialog: AbstractDialog, IListDialog

    {
        public ListDialog(ILogger<ListDialog> logger) : base(logger)
        {
        }

        public async override Task Begin(ITurnContext turnContext)
        {
            try
            {
                var activity = new Activity(){ActivityType = ActivityType.Message, Text = "Veuillez vous rendre à la position ci-dessous", Location = new Location(){Latitude = 0, Longitude = 0}};
                await turnContext.ReplyActivity(activity);
            }
            finally
            {
                await turnContext.End();
            }
        }

        public override string Command => "/liste";
    }
}
