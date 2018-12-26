using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ImageHunt.Services;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ImageHunt.Controllers
{
  [Route("api/[controller]")]
  #if !DEBUG
  [Authorize]
  #endif
  public class NodeController : Controller
  {
    public readonly IMapper _mapper;
    private INodeService _nodeService;
    private readonly IGameService _gameService;
    private readonly ITeamService _teamService;

    public NodeController(INodeService nodeService, IGameService gameService, ITeamService teamService, IMapper mapper)
    {
      _mapper = mapper;
      _nodeService = nodeService;
      _gameService = gameService;
      _teamService = teamService;
    }

    [HttpPut("AddRelationToNode")]
    public IActionResult AddRelationToNode([FromBody]NodeRelationRequest relationRequest)
    {
      _nodeService.AddChildren(relationRequest.NodeId, relationRequest.ChildrenId);
      if (relationRequest.AnswerId != 0)
        _nodeService.LinkAnswerToNode(relationRequest.AnswerId, relationRequest.ChildrenId);
      return Ok();
    }
    [HttpPut("RemoveRelationToNode")]
    public IActionResult RemoveRelationToNode([FromBody]NodeRelationRequest relationRequest)
    {
      _nodeService.RemoveChildren(relationRequest.NodeId, relationRequest.ChildrenId);
      if (relationRequest.AnswerId != 0)
        _nodeService.UnlinkAnswerToNode(relationRequest.AnswerId);
      return Ok();
    }
    [HttpPut("AddRelationsWithAnswers")]
    public void AddRelationsWithAnswers([FromBody]IEnumerable<NodeRelationRequest> relationsRequest)
    {
      var groupsNode = relationsRequest.GroupBy(n => n.NodeId);
      foreach (var gNode in groupsNode)
      {
        var choiceNode = _nodeService.GetNode(gNode.Key);
        _nodeService.RemoveAllChildren(choiceNode);
        foreach (var nodeRelationRequest in gNode)
        {
          _nodeService.AddChildren(choiceNode.Id, nodeRelationRequest.ChildrenId);
          _nodeService.LinkAnswerToNode(nodeRelationRequest.AnswerId, nodeRelationRequest.ChildrenId);
        }
      }
    }
    [HttpDelete("RemoveNode/{nodeId}")]
    public IActionResult RemoveNode(int nodeId)
    {
      var node = _nodeService.GetNode(nodeId);
      _nodeService.RemoveNode(node);
      return Ok();
    }
    [HttpDelete("RemoveRelation/{orgNodeId}/{destNodeId}")]
    public IActionResult RemoveRelation(int orgNodeId, int destNodeId)
    {
      var orgNode = _nodeService.GetNode(orgNodeId);
      var destNode = _nodeService.GetNode(destNodeId);
      _nodeService.RemoveRelation(orgNode, destNode);
      return Ok();
    }
    [HttpGet("{nodeId}")]
    public IActionResult GetNodeById(int nodeId)
    {
      var node = _nodeService.GetNode(nodeId);
      var nodeResponse = _mapper.Map<NodeResponse>(node);
      return Ok(nodeResponse);
    }
    [HttpPatch]
    public IActionResult UpdateNode([FromBody]NodeUpdateRequest nodeRequest)
    {
      var node = _nodeService.GetNode(nodeRequest.Id);
      node.Name = nodeRequest.Name;
      node.Points = nodeRequest.Points;
      node.Longitude = nodeRequest.Longitude;
      node.Latitude = nodeRequest.Latitude;
      _nodeService.UpdateNode(node);
      return Ok();
    }
    [HttpGet("GetNextNodeForTeam/{teamId}")]
    public IActionResult GetNextNodeForTeam(int teamId)
    {
      var team = _teamService.GetTeamById(teamId);
      var node = _nodeService.GetNode(team.CurrentNode.Id);
      return Ok(node.Children);
    }
    [HttpGet("GetNodesByType/{gameId}/{nodeType}")]
    public IActionResult GetNodesByType(string nodeType, int gameId)
    {
      var eNodeType = Enum.Parse<NodeTypes>(nodeType);
      var nodes = _gameService.GetNodes(gameId, eNodeType);
      return Ok(nodes);
    }
  }
}
