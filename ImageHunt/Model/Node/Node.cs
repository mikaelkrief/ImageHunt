using System.Collections.Generic;

namespace ImageHunt.Model.Node
{
    public abstract class Node : DbObject
    {
        public Geography Coordinate { get; set; }
        public string Name { get; set; }
        public List<Node> Children { get; set; }
    }
}