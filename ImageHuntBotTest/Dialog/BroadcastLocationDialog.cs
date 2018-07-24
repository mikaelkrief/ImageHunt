using System.Text;
using ImageHuntTelegramBot.Dialogs;
using Microsoft.Extensions.Logging;

namespace ImageHuntBotTest.Dialog
{
    public class BroadcastLocationDialog : AbstractDialog, IBroadcastLocationDialog
    {
        public BroadcastLocationDialog(ILogger<BroadcastLocationDialog> logger) 
            : base(logger)
        {
        }
    }
}
