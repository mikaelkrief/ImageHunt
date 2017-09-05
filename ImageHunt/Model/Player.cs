using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImageHunt.Model
{
    public class Player : DbObject
    {
        public string Name { get; set; }
      public Node.Node CurrentNode { get; set; }
      public DateTime? StartTime { get; set; }
    }
}
