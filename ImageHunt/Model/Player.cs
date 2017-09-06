using System;

namespace ImageHunt.Model
{
    public class Player : DbObject
    {
        public string Name { get; set; }
      public Node.Node CurrentNode { get; set; }
      public DateTime? StartTime { get; set; }
    }
}
