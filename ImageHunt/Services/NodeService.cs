using System.Collections.Generic;
using System.Linq;
using ImageHunt.Computation;
using ImageHunt.Data;
using ImageHunt.Model;
using ImageHunt.Model.Node;
using Microsoft.EntityFrameworkCore;

namespace ImageHunt.Services
{
  public class NodeService : AbstractService, INodeService
  {

    public NodeService(HuntContext context) : base(context)
    {
    }
    public void AddNode(Node node)
    {
      Context.Nodes.Add(node);
      Context.SaveChanges();
    }

    public Node GetNode(int nodeId)
    {
      return Context.Nodes.Single(n => n.Id == nodeId);
    }

    public void AddChildren(int nodeId, int childrenNodeId)
    {
      var node = Context.Nodes.Single(n => n.Id == nodeId);
      var childrenNode = Context.Nodes.Single(n => n.Id == childrenNodeId);
      var parentChildren = new ParentChildren(){Parent = node, Children = childrenNode};
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
    public Node FindPictureNodeByLocation(int gameId, Picture pictureToFind)
    {
      var nodes = Context.Games.Include(g => g.Nodes).Single(g => g.Id == gameId).Nodes.Where(n => n is PictureNode);
      if (!nodes.Any())
        return null;
      var pictureCoordinates = ImageService.ExtractLocationFromImage(pictureToFind);
      var pictureNode = new PictureNode() { Latitude = pictureCoordinates.Item1, Longitude = pictureCoordinates.Item2 };
      var distances = nodes.Select(n => n.Distance(pictureNode));
      var closestNode =
        nodes.FirstOrDefault(n => n.Distance(pictureNode) < 40);
      return closestNode as PictureNode;
    }

  }
}
