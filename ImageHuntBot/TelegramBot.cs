using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ImageHuntTelegramBot.Controllers;
using Microsoft.Extensions.Logging;

namespace ImageHuntTelegramBot
{
    public class TelegramBot : IBot
    {
        private readonly ILogger<TelegramBot> _logger;
        private Dictionary<string, IDialog> _dialogs = new Dictionary<string, IDialog>();
        private static readonly SemaphoreSlim Padlock = new SemaphoreSlim(1, 1);

        public TelegramBot(ILogger<TelegramBot> logger)
        {
            _logger = logger;
        }
        public void AddDialog(IDialog dialog)
        {
            _dialogs.Add(dialog.Command.ToLowerInvariant(), dialog);
        }

        public async Task OnTurn(ITurnContext context)
        {
            await Padlock.WaitAsync();
            // Start critical section
            try
            {
                if (context.Activity.Command == "/reset")
                {
                    await context.Reset();
                    return;
                }

                await context.Continue();
                IDialog dialog = null;
                if (!context.Replied)
                {
                    if (context.CurrentDialog == null && _dialogs.Any(d => context.Activity.Command == d.Key))
                    {
                        dialog = _dialogs[context.Activity.Command.ToLowerInvariant()];
                    }
                }

                if (dialog != null)
                {
                    await context.Begin(dialog);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occured: {ex.Message}");
                await context.End();
            }
            finally
            {
                // End critical section
                Padlock.Release();
            }
        }
    }
}