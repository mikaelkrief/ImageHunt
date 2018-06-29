using System;
using System.Threading.Tasks;
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
    public async Task<IActionResult> Post([FromBody] dynamic update)
    {
      _logger.LogInformation(
        $"UpdateController::Post: ChatId: {update.Message.Chat.Id}, Text: {update.Message.Text}, Type: {update.Type}");
      try
      {
        var context = await _contextHub.GetContext(update);
        await _bot.OnTurn(context);

      }
      catch (Exception e)
      {
        _logger.LogError(e, $"An error while processing update {update.Id} for chat {update.Message.Chat.Id}");
      }

      return Ok();
    }
  }
}
