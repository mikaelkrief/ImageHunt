using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ImageHunt.Model
{
    public abstract class Node : INode
    {
        public Node(string name, Geography coordinate)
        {
            Coordinate = coordinate;
            Name = name;
        }
        [Key]
        public int Id { get; set; }
        public Geography Coordinate { get; }
        public string Name { get; }
        public List<Node> Children { get; set; }
    }
}