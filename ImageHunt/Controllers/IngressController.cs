using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ImageHunt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngressController : ControllerBase
    {
      [HttpPost("{kindVisit}")]
      public IActionResult ParseGameAction(string kindVisit, List<IFormFile> ingressFiles)
      {
        switch (kindVisit)
        {
        case "upc":
          break;
        case "upv":
          break;
        }
        return Ok();  
      }
    }
}
