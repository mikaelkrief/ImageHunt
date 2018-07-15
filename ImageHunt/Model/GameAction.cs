using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using ImageHunt.Model.Node;

namespace ImageHunt.Model
{
  public enum Action
  {
    StartGame,
    EndGame,
    SubmitPicture,
    VisitWaypoint,
    ReplyQuestion,
    DoAction,
    SubmitPosition
  }
    public class GameAction : DbObject
    {
      public DateTime DateOccured { get; set; }
      public double Latitude { get; set; }
      public double Longitude { get; set; }
      public Game Game { get; set; }
      public Team Team { get; set; }
      public Picture Picture { get; set; }
      public Action Action { get; set; }
      public Node.Node Node { get; set; }
      public bool IsValidated { get; set; }
      public Answer SelectedAnswer { get; set; }
      public Answer CorrectAnswer { get; set; }
      public bool IsReviewed { get; set; }
      public Admin Reviewer { get; set; }
      public DateTime DateReviewed { get; set; }
      public int PointsEarned { get; set; }
      [NotMapped]
      public double Delta { get; set; }
    }
}
