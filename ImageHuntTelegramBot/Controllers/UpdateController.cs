using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace ImageHuntTelegramBot.Controllers
{
  [Route("api/[controller]")]
    public class UpdateController : Controller
    {
      private readonly ContextHub _contextHub;
      private readonly IBot _bot;

      public UpdateController(ContextHub contextHub, IBot bot)
      {
        _contextHub = contextHub;
        _bot = bot;
      }

    [HttpPost]
      public async Task<IActionResult> Post([FromBody] Update update)
      {
        var context = _contextHub.GetContext(update);
        await _bot.OnTurn(context);
        return Ok();
      }
    }

  public interface IBot
  {
    Task OnTurn(ITurnContext context);
  }
}
