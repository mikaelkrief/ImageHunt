using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImageHunt.Model
{
    public class Game : DbObject
    {
        public bool IsActive { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public List<Node.Node> Nodes { get; set; }
    }
}
