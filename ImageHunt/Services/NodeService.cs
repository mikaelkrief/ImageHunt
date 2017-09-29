using System.Collections.Generic;
using System.Linq;
using ImageHunt.Data;
using ImageHunt.Model;
using ImageHunt.Model.Node;

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
  }
}
