using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageHuntCore.Model;

namespace ImageHunt.Model
{
    public class Passcode : DbObject
    {
      public string Pass { get; set; }
      public int NbRedeem { get; set; }
      public int Points { get; set; }
    }
}
