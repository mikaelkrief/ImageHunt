using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageHunt.Request;
using ImageHunt.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ImageHunt.Controllers
{
  [Route("api/[controller]")]
  public class NodeController : Controller
  {
    private INodeService _nodeService;

    public NodeController(INodeService nodeService)
    {
      _nodeService = nodeService;
    }

    [HttpPost("AddRelationToNode")]
    public IActionResult AddRelationToNode([FromBody]NodeRelationRequest relationRequest)
    {
      _nodeService.AddChildren(relationRequest.NodeId, relationRequest.ChildrenId);
      return Ok();
    }
  }
}
