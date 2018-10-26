using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ImageHuntTelegramBot.Controllers;
using ImageHuntTelegramBot.Dialogs;
using ImageHuntWebServiceClient.Responses;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Extensions.Logging;

namespace ImageHuntTelegramBot
{
    public class TelegramBot : IBot
    {
        private readonly ILogger<TelegramBot> _logger;
        private readonly IAdminWebService _adminWebService;
        private readonly ITeamWebService _teamWebService;
        private Dictionary<string, IDialog> _dialogs = new Dictionary<string, IDialog>();
        private static readonly SemaphoreSlim Padlock = new SemaphoreSlim(1, 1);
        private static List<AdminResponse> _admins;

        public TelegramBot(ILogger<TelegramBot> logger, IAdminWebService adminWebService, ITeamWebService teamWebService)
        {
            _logger = logger;
            _adminWebService = adminWebService;
            _teamWebService = teamWebService;
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
            var state = context.GetConversationState<ImageHuntState>();
            var team = await _teamWebService.GetTeamById(state.TeamId);
            // Start critical section
            try
            {
                _logger.LogTrace($"Command {context.Activity.Command} had occured in {context.ChatId}");
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
                    if (context.CurrentDialog == null && _dialogs.Any(d => context.Activity.Command.Equals(d.Key, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        dialog = _dialogs[context.Activity.Command.ToLowerInvariant()];
                    }
                }
                if (!dialog.IsAdmin || (_admins.Any(a=>a.Name.Equals(context.Username, StringComparison.InvariantCultureIgnoreCase)) &&
                    (team == null || !team.Players.Any(p=>p.ChatLogin.Equals(context.Username, StringComparison.InvariantCultureIgnoreCase)))))
                {
                    if (dialog != null)
                    {
                        _logger.LogInformation($"Pass control to {nameof(dialog)}");
                        await context.Begin(dialog);
                    }
                }
                else
                {
                    _logger.LogError($"Attempt to use {nameof(dialog)} by {context.Username}");
                    await context.ReplyActivity(
                        $"Cette commande est réservée aux orgas, vous ne pouvez pas l'utiliser");
                    await context.End();
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