using System.Collections.Generic;
using System.Linq;
using ImageHunt.Data;
using ImageHuntCore.Computation;
using ImageHuntCore.Model.Node;
using ImageHuntCore.Services;
using ImageHuntWebServiceClient.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ImageHunt.Services
{
  public class NodeService : AbstractService, INodeService
  {

    public NodeService(HuntContext context, ILogger<NodeService> logger) : base(context, logger)
    {
    }
    public void AddNode(Node node)
    {
      Context.Nodes.Add(node);
      Context.SaveChanges();
    }

    public Node GetNode(int nodeId)
    {
      Node node = Context.Nodes
        .Include(n=>n.ChildrenRelation).ThenInclude(cr=>cr.Children)
        .Include(n=>n.Image)
        .SingleOrDefault(n => n.Id == nodeId);
      if (node == null)
      {
        _logger.LogError("Node Id: {0} not found", nodeId);
        return null;
      }

      switch (node.NodeType)
      {
        case NodeResponse.ChoiceNodeType:
          node = Context.ChoiceNodes.Include(q => q.Answers).SingleOrDefault(n => n.Id == nodeId);
          break;
      }
      return node;
    }

    public void AddChildren(int nodeId, int childrenNodeId)
    {
      var node = Context.Nodes.Single(n => n.Id == nodeId);
      var childrenNode = Context.Nodes.Single(n => n.Id == childrenNodeId);
      var parentChildren = new ParentChildren() { Parent = node, Children = childrenNode };
      node.ChildrenRelation.Add(parentChildren);

      Context.SaveChanges();
    }

    public void AddChildren(Node parentNode, Node childrenNode)
    {
      var node = Context.Nodes.Single(n => n == parentNode);
      var children = Context.Nodes.Single(n => n == childrenNode);
      var parentChildren = new ParentChildren() { Parent = node, Children = children };
      node.ChildrenRelation.Add(parentChildren);
      Context.SaveChanges();
    }

    public void RemoveChildren(int nodeId, int childrenNodeId)
    {
      var node = Context.Nodes.Include(n => n.ChildrenRelation).Single(n => n.Id == nodeId);
      var childrenNode = Context.Nodes.Single(n => n.Id == childrenNodeId);
      var parentChildren = node.ChildrenRelation.Single(pc => pc.Parent == node && pc.Children == childrenNode);
      node.ChildrenRelation.Remove(parentChildren);
      Context.SaveChanges();

    }

    public void LinkAnswerToNode(int answerId, int targetNodeId)
    {
      var answer = Context.Answers.SingleOrDefault(a => a.Id == answerId);
      var childNode = Context.Nodes.SingleOrDefault(n => n.Id == targetNodeId);
      if (answer == null || childNode == null) return;
      answer.Node = childNode;
      Context.SaveChanges();
    }

    public void UnlinkAnswerToNode(int answerId)
    {
      var answer = Context.Answers.SingleOrDefault(a => a.Id == answerId);
      if (answer != null)
        answer.Node = null;
      Context.SaveChanges();
    }

    public void RemoveAllChildren(Node node)
    {
      var theNode = Context.Nodes.Include(n => n.ChildrenRelation).Single(n => n.Id == node.Id);
      theNode.ChildrenRelation.RemoveAll(n => true);
      Context.SaveChanges();
    }

    public Answer GetAnswer(int answerId)
    {
      return Context.Answers.SingleOrDefault(a => a.Id == answerId);
    }

    public Node FindPictureNodeByLocation(int gameId, (double, double) pictureCoordinates)
    {
      var nodes = Context.Games.Include(g => g.Nodes).Single(g => g.Id == gameId).Nodes.Where(n => n is PictureNode);
      if (!nodes.Any())
        return null;
      var pictureNode = new PictureNode() { Latitude = pictureCoordinates.Item1, Longitude = pictureCoordinates.Item2 };
      var closestNode =
        nodes.FirstOrDefault(n => n.Distance(pictureNode) < 40);
      return closestNode as PictureNode;
    }

    public void RemoveNode(Node nodeToRemove)
    {
      // Remove answers if node is QuestionNode
      if (nodeToRemove is ChoiceNode questionNode && questionNode.Answers != null)
      {
        Context.Answers.RemoveRange(questionNode.Answers);
        questionNode.Answers.Clear();
      }

      var childrenOfNode = nodeToRemove.Children.FirstOrDefault();

      // remove all children of the node to remove
      nodeToRemove.ChildrenRelation.Clear();
      // Retrieve relations of node to remove
      var parentsOfNode = Context.ParentChildren.Where(pc => pc.Children == nodeToRemove);
      Context.ParentChildren.RemoveRange(parentsOfNode);
      if (childrenOfNode != null)
      {
        foreach (var parent in parentsOfNode)
        {
          parent.Parent = Context.Nodes.Single(n=>n.Id == parent.ParentId);
          parent.Parent.ChildrenRelation.Add(new ParentChildren(){Parent = parent.Parent, Children = childrenOfNode});
          //Context.ParentChildren.Add(new ParentChildren() {Parent = parent.Parent, Children = childrenOfNode});
        }
      }
      Context.Nodes.Remove(nodeToRemove);
      Context.SaveChanges();
    }

    public void RemoveRelation(Node orgNode, Node destNode)
    {
      orgNode = Context.Nodes.Include(n => n.ChildrenRelation)
        .Single(n => n == orgNode);
      // Remove answers for QuestionNode
      if (orgNode is ChoiceNode questionNode)
      {
        questionNode = Context.ChoiceNodes.Include(n => n.Answers).Single(n => n == questionNode);
        var answerToRemove = questionNode.Answers.Single(a => a.Node == destNode);
        questionNode.Answers.Remove(answerToRemove);
        Context.Answers.Remove(answerToRemove);
      }
      // Remove relation
      var relationToRemove = orgNode.ChildrenRelation.Single(pc => pc.Children == destNode);
      orgNode.ChildrenRelation.Remove(relationToRemove);
      Context.SaveChanges();
    }

    public void UpdateNode(Node node)
    {
      var oldNode = Context.Nodes.Single(n => n.Id == node.Id);
      oldNode.Name = node.Name;
      oldNode.Points = node.Points;
      oldNode.Latitude = node.Latitude;
      oldNode.Longitude = node.Longitude;
      Context.SaveChanges();
    }

    public IEnumerable<Node> GetGameNodesOrderByPosition(int gameId, double latitude, double longitude,
      NodeTypes nodeTypes = NodeTypes.All)
    {
      var nodes = Context.Games.Include(g => g.Nodes).Single(g => g.Id == gameId).Nodes;
      IEnumerable<Node> selectedNodes = new List<Node>();
      if (nodeTypes.HasFlag(NodeTypes.All))
        return nodes.OrderBy(n => GeographyComputation.Distance(latitude, longitude, n.Latitude, n.Longitude));
      if (nodeTypes.HasFlag(NodeTypes.Picture))
      {
        selectedNodes = selectedNodes.Union(nodes.Where(n => n.NodeType == NodeResponse.PictureNodeType));
      }
      if (nodeTypes.HasFlag(NodeTypes.Hidden))
      {
        selectedNodes = selectedNodes.Union(nodes.Where(n => n.NodeType == NodeResponse.HiddenNodeType || n.NodeType == NodeResponse.BonusNodeType));
      }
      if (nodeTypes.HasFlag(NodeTypes.Path))
      {
        selectedNodes = selectedNodes.Union(nodes.Where(n => n.NodeType == NodeResponse.FirstNodeType ||
                                                             n.NodeType == NodeResponse.LastNodeType ||
                                                             n.NodeType == NodeResponse.ChoiceNodeType ||
                                                             n.NodeType == NodeResponse.ObjectNodeType ||
                                                             n.NodeType == NodeResponse.QuestionNodeType ||
                                                             n.NodeType == NodeResponse.TimerNodeType ||
                                                             n.NodeType == NodeResponse.WaypointNodeType));
      }

      return selectedNodes.OrderBy(n => GeographyComputation.Distance(latitude, longitude, n.Latitude, n.Longitude));
    }

  }
}
