using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ImageHuntTelegramBot.Dialogs;
using ImageHuntWebServiceClient.Responses;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;

namespace ImageHuntTelegramBot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpdateController : ControllerBase
    {
        private readonly ContextHub _contextHub;
        private readonly IBot _bot;
        private readonly ILogger _logger;
        private readonly IAdminWebService _adminWebService;
        private static List<AdminResponse> _admins;
        static SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);

        public UpdateController(ContextHub contextHub, IBot bot, ILogger<UpdateController> logger, IAdminWebService adminWebService)
        {
            _contextHub = contextHub;
            _bot = bot;
            _logger = logger;
            _adminWebService = adminWebService;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Update update)
        {
            _logger.LogDebug($"Received update {update}");
            var message = update.Message == null ? update.EditedMessage : update.Message;
            _logger.LogInformation(
            $"Received update from {message.Chat.Id} of type {message.Type}");
            if (!string.IsNullOrEmpty(message.Text) && message.Text.StartsWith("/") && !message.Text.StartsWith("/start"))
            {
                try
                {
                    if (_admins == null)
                    {
                        await UpdateAdmins();
                    }

                    if (!_admins.Any(a => a.Name.Equals(message.From.Username, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        _logger.LogInformation($"In Chat {message.Chat.Id}, a non admin user attempt to send a command");
                        return Ok();
                    }

                }
                catch (Exception e)
                {
                    _logger.LogError($"Error while checking if User {message.From.Username} is admin");
                }
            }
            try
            {
                var context = await _contextHub.GetContext(update);
                await _bot.OnTurn(context);

            }
            catch (Exception e)
            {
                _logger.LogError(e, $"An error while processing update {update.Id} for chat {update.Message.Chat.Id}");
                var context = await _contextHub.GetContext(update);
                await context.End();
            }

            return Ok();
        }

        public async Task UpdateAdmins()
        {
            await semaphoreSlim.WaitAsync();

            try
            {
                _admins = (await _adminWebService.GetAllAdmins()).ToList();

            }
            finally
            {
                semaphoreSlim.Release();
            }
        }
    }
}
