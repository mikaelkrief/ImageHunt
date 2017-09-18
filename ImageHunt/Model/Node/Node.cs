using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImageHunt.Model.Node
{
  /// <summary>
  /// Base class of all node object. 
  /// </summary>
    public abstract class Node : DbObject
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string Name { get; set; }
        // Node that are children of the current node. The application will follow up one of this node to the player if he complies with the current node
        public List<Node> Children { get; set; }
      [NotMapped]
      public string NodeType {
        get { return GetType().Name; }
    }
    }
}
