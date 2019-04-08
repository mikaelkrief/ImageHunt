using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using ImageHunt.Model;
using ImageHunt.Services;
using ImageHunt.Updater;
using ImageHuntCore.Model.Node;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.Responses;
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
    private readonly ILifetimeScope _scope;
    private INodeService _nodeService;
    private readonly IGameService _gameService;
    private readonly ITeamService _teamService;
    private readonly IImageService _imageService;

    public NodeController(INodeService nodeService, IGameService gameService,
      ITeamService teamService, IImageService imageService, IMapper mapper, ILifetimeScope scope)
    {
      _mapper = mapper;
      _scope = scope;
      _nodeService = nodeService;
      _gameService = gameService;
      _teamService = teamService;
      _imageService = imageService;
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
    public async Task<IActionResult> UpdateNode([FromBody]NodeUpdateRequest nodeRequest)
    {
      var node = _nodeService.GetNode(nodeRequest.Id);
      if (node.NodeType != nodeRequest.NodeType)
      {
        _nodeService.RemoveNode(node);
        node = NodeFactory.UpdateNode(node, nodeRequest.NodeType);
        _gameService.AddNode(nodeRequest.GameId, node);
      }
      node.Name = nodeRequest.Name;
      if (nodeRequest.Points.HasValue)
        node.Points = nodeRequest.Points.Value;
      if (nodeRequest.Longitude.HasValue)
        node.Longitude = nodeRequest.Longitude.Value;
      if (nodeRequest.Latitude.HasValue)
        node.Latitude = nodeRequest.Latitude.Value;
      if (nodeRequest.ImageId.HasValue)
      {
        if (node.Image == null)
        {
          node.Image = await _imageService.GetPictureById(nodeRequest.ImageId.Value);
        }
        else if (node.Image.Id != nodeRequest.ImageId.Value)
        {
          var image = await _imageService.GetPictureById(nodeRequest.ImageId.Value);
          node.Image = image;
        }

      }

      if (!string.IsNullOrEmpty(nodeRequest.Action) && nodeRequest.NodeType == NodeResponse.ObjectNodeType)
      {
        ((ObjectNode) node).Action = nodeRequest.Action;
      }
      if (!string.IsNullOrEmpty(nodeRequest.Hint) && nodeRequest.NodeType == NodeResponse.HiddenNodeType)
      {
        ((HiddenNode) node).LocationHint= nodeRequest.Hint;
      }
      if (!string.IsNullOrEmpty(nodeRequest.Hint) && nodeRequest.NodeType == NodeResponse.BonusNodeType)
      {
        ((BonusNode) node).Location= nodeRequest.Hint;
        ((BonusNode) node).BonusType = (BonusNode.BONUS_TYPE) nodeRequest.Bonus;
      }

      if (nodeRequest.Delay.HasValue && nodeRequest.NodeType == NodeResponse.TimerNodeType)
      {
        ((TimerNode) node).Delay = nodeRequest.Delay.Value;
      }
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
      return Ok(_mapper.Map<IEnumerable<NodeResponse>>(nodes));
    }
    [HttpPost("BatchUpdate")]
    public IActionResult BatchUpdateNode([FromBody]BatchUpdateNodeRequest batchUpdateNodeRequest)
    {
      var game = _gameService.GetGameById(batchUpdateNodeRequest.GameId);
      //var arguments = JsonConvert.DeserializeObject<string>(batchUpdateNodeRequest.UpdaterArgument);
      var updater = _scope.ResolveNamed<IUpdater>(batchUpdateNodeRequest.UpdaterType,
        new NamedParameter("arguments", batchUpdateNodeRequest.UpdaterArgument),
        new NamedParameter("game", game));
      updater.Execute();
      return Ok();
    }
  }
}
