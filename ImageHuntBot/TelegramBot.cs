using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ImageHuntTelegramBot.Controllers;
using ImageHuntWebServiceClient.Responses;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Extensions.Logging;

namespace ImageHuntTelegramBot
{
    public class TelegramBot : IBot
    {
        private readonly ILogger<TelegramBot> _logger;
        private readonly IAdminWebService _adminWebService;
        private Dictionary<string, IDialog> _dialogs = new Dictionary<string, IDialog>();
        private static readonly SemaphoreSlim Padlock = new SemaphoreSlim(1, 1);
        private static List<AdminResponse> _admins;

        public TelegramBot(ILogger<TelegramBot> logger, IAdminWebService adminWebService)
        {
            _logger = logger;
            _adminWebService = adminWebService;
            
        }
        public void AddDialog(IDialog dialog)
        {
            _dialogs.Add(dialog.Command.ToLowerInvariant(), dialog);
        }

        public async Task OnTurn(ITurnContext context)
        {
            await Padlock.WaitAsync();
            if (_admins==null)
                _admins = await _adminWebService.GetAllAdmins() as List<AdminResponse>;
            // Start critical section
            try
            {
                if (context.Activity.Command == "/reset")
                {
                    await context.Reset();
                    var resetDialog = _dialogs[context.Activity.Command.ToLowerInvariant()];
                    await context.Begin(resetDialog);
                    await context.Continue();
                    await context.End();
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
                if (_admins.Any(a=>a.Name == context.Username) || !dialog.IsAdmin)
                {
                    if (dialog != null)
                    {
                        await context.Begin(dialog);
                    }
                }
                else
                {
                    _logger.LogError($"Attempt to use {nameof(dialog)} by {context.Username}");
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