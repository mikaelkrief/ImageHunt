using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ImageHunt.Model
{
  public class Team : DbObject
  {
    public Team()
    {
      TeamPlayers = new List<TeamPlayer>();
    }
    public string Name { get; set; }
    public List<TeamPlayer> TeamPlayers { get; set; }
    [NotMapped]
    public IEnumerable<Player> Players => TeamPlayers.Select(tp => tp.Player);

    public string ChatId { get; set; }
  }
}
