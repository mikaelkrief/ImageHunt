using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageHunt.Model
{
    public class Admin : DbObject
    {
      public Admin()
      {
        Games = new List<Game>();
      }
      public string Name { get; set; }
      public string Email { get; set; }
      public string Token { get; set; }
      public DateTime? ExpirationTokenDate { get; set; }
      public List<Game> Games { get; set; }
      public Role Role { get; set; }
    }

  public enum Role
  {
    Admin,
    Validator,
    Player,
    Reader
  }
}
