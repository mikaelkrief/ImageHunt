using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageHunt.Model
{
  public enum Action
  {
    StartGame,
    EndGame,
    SubmitPicture,
    VisitWaypoint
  }
    public class GameAction : DbObject
    {
      public DateTime DateOccured { get; set; }
      public double Latitude { get; set; }
      public double Longitude { get; set; }
      public Game Game { get; set; }
      public Player Player { get; set; }
      public Picture Picture { get; set; }
      public Action Action { get; set; }
      public Node.Node Node { get; set; }
      public bool IsValidated { get; set; }
    }
}
