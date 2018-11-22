using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ImageHuntCore.Model
{
    public class Admin : DbObject
    {
      public Admin()
      {
        GameAdmins = new List<GameAdmin>();
      }
      public string Name { get; set; }
      public string Email { get; set; }
      public string Token { get; set; }
      public DateTime? ExpirationTokenDate { get; set; }
      [NotMapped] public IEnumerable<Game> Games => GameAdmins.Select(ga => ga.Game);
      public List<GameAdmin> GameAdmins { get; set; }
      public Role Role { get; set; }
    }

  public enum Role
  {
    Admin,
    Validator,
    GameMaster,
    Reader,
    Bot
  }
}
