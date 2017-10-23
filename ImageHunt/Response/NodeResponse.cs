using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageHunt.Model.Node;

namespace ImageHunt.Response
{
    public class NodeResponse
    {
      public NodeResponse(Node node)
      {
        NodeId = node.Id;
        Name = node.Name;
        NodeType = node.NodeType;
        ChildNodeId = node.ChildrenRelation.Select(ch => ch.Children.Id).ToArray();
      }

      public string NodeType { get; set; }
      public string Name { get; set; }
      public int NodeId { get; set; }
      public int[] ChildNodeId { get; set; }  
    }
}
