using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageHunt.Model
{
    public class Admin : DbObject
    {
      public string Name { get; set; }
      public string Email { get; set; }
      public string Token { get; set; }
      public DateTime? ExpirationTokenDate { get; set; }
      public List<Game> Games { get; set; }
    }
}
