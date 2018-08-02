using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ImageHunt.Controllers
{
  [Route("api/[controller]")]
  //[ApiController]
  public class IngressController : ControllerBase
  {
    [HttpPost("{kindVisit}")]
    [DisableRequestSizeLimit]

    public IActionResult ParseGameAction(string kindVisit, IFormFile ingressFiles)
    {
      ISet<string> a = null;
      var file = ingressFiles;
      //foreach (var file in ingressFiles)
      {
        using (var fileStream = file.OpenReadStream())
        {
          var actions = new Dictionary<string, ISet<string>>();
          var reader = new StreamReader(fileStream);
          // Strip header
          string line = null;
          reader.ReadLine();
          while ((line = reader.ReadLine()) != null)
          {
            var splittedLine = line.Split('\t');
            if (!actions.ContainsKey(splittedLine[3]))
            {
              actions.Add(splittedLine[3], new HashSet<string>());
            }
            actions[splittedLine[3]].Add($"{splittedLine[1]};{splittedLine[2]}"); // for kml it's {line[2]},{line[1]}
          }
          switch (kindVisit)
          {
            case "upc":
              a = actions["captured portal"];
              break;
            case "upv":
              a = actions["hacked friendly portal"];
              a.UnionWith(actions["created link"]);
              a.UnionWith(actions["captured portal"]);
              a.UnionWith(actions["resonator deployed"]);
              a.UnionWith(actions["resonator upgraded"]);
              a.UnionWith(actions["hacked enemy portal"]);
              a.UnionWith(actions["hacked neutral portal"]);
              break;
          }
        }
      }

      return Ok(a);
    }
  }
}
