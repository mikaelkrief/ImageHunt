using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageHunt.Model
{
    public class Game : DbObject
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public List<Node.Node> Nodes { get; set; }
        public Node.Node FirstNode { get; set; }
    }
}
