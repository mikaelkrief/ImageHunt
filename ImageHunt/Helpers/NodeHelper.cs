using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageHuntCore.Model.Node;

namespace ImageHunt.Helpers
{
  public static class NodeHelper
  {
    public static Node HaveChild(this Node parent, Node child)
    {
      parent.ChildrenRelation.Add(new ParentChildren(){Parent = parent, Children = child});
      return parent;
    }
    public static IEnumerable<Node> DuplicatePath(this Node firstNode, IEnumerable<Node> orgNodes, IEnumerable<Node> newNodes)
    {
      var firstOldNode = orgNodes.Single(o=>o.Id == firstNode.OrgId);
      var nextOldNode = firstOldNode.Children.FirstOrDefault();
      if (nextOldNode == null)
        return newNodes;
      var nextNewNode = newNodes.Single(n => n.OrgId == nextOldNode.Id);
      firstNode.HaveChild(nextNewNode);
      nextNewNode.DuplicatePath(orgNodes, newNodes);
      return newNodes;
    }

  }
}
