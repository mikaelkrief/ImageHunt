using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ImageHuntCore.Model
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
    public ImageHuntCore.Model.Node.Node CurrentNode { get; set; }
    public string Color { get; set; }
    public string CultureInfo { get; set; }
    public string Comment { get; set; }
    public Picture Picture { get; set; }
  }
}
