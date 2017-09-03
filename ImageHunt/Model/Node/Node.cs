using System.Collections.Generic;

namespace ImageHunt.Model.Node
{
    public abstract class Node : DbObject
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string Name { get; set; }
        public List<Node> Children { get; set; }
    }
}
