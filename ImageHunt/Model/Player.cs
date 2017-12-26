using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImageHunt.Model
{
  public class Player : DbObject
  {
    public string Name { get; set; }
    public Node.Node CurrentNode { get; set; }
    public DateTime? StartTime { get; set; }
    public string ChatLogin { get; set; }
    [NotMapped]
    public Team Team { get; set; }
  }
}
