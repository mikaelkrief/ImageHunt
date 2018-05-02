using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ImageHuntTelegramBot.Services;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace ImageHuntTelegramBot.Controllers
{
  [Route("api/[controller]")]
    public class UpdateController : Controller
    {
      private readonly IUpdateService _updateService;

      public UpdateController(IUpdateService updateService)
      {
        _updateService = updateService;
      }

      [HttpGet]
      public IActionResult Get()
      {
        return Ok("Toto");
      }
    [HttpPost]
      public async Task<IActionResult> Post([FromBody] Update update)
      {
        await _updateService.Root(update);
        return Ok();
      }
    }
}
