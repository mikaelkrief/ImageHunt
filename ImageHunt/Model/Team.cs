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
      TeamPasscodes = new List<TeamPasscode>();
    }
    public string Name { get; set; }
    public List<TeamPlayer> TeamPlayers { get; set; }
    public List<TeamPasscode> TeamPasscodes { get; set; }
    [NotMapped]
    public IEnumerable<Player> Players => TeamPlayers.Select(tp => tp.Player);
    [NotMapped]
    public IEnumerable<Passcode> Passcodes => TeamPasscodes.Select(tp => tp.Passcode);

    public string ChatId { get; set; }
    public Node.Node CurrentNode { get; set; }
    public string Color { get; set; }
  }
}
