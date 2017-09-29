using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageHunt.Model.Node;

namespace ImageHunt.Request
{
    public class AddNodeRequest
    {
      public string NodeType { get; set; }
      public string Name { get; set; }
      public double Latitude { get; set; }
      public double Longitude { get; set; }
      public int Duration { get; set; }
      public string Action { get; set; }
      public string Question { get; set; }
      public string[] Answers { get; set; }
    }
}
