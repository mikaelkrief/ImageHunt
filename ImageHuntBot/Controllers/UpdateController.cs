using System;
using System.Threading.Tasks;
using ImageHuntTelegramBot.Dialogs;
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

    public UpdateController(ContextHub contextHub, IBot bot, ILogger<UpdateController> logger)
    {
      _contextHub = contextHub;
      _bot = bot;
      _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Post(Update update)
    {
        _logger.LogDebug($"Received update {update}");
        var message = update.Message == null ? update.EditedMessage : update.Message;
      _logger.LogInformation(
        $"Received update from {message.Chat.Id} of type {message.Type}");
      try
      {
        var context = await _contextHub.GetContext(update);
        if (message.Text == "/reset")
        {
            await context.ResetConversationStates<ImageHuntState>();
            
            return Ok();
        }
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
  }
}
