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
  }
}
