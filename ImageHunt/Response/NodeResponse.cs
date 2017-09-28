using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageHunt.Response
{
    public class NodeResponse
    {
      public int NodeId { get; set; }
      public int[] ChildNodeId { get; set; }  
    }
}
