using System;
using ImageHuntCore.Model;

namespace ImageHunt.Services
{
  public class ScoreDecreaseByTeamMember : IScoreChanger
  {
    public double ComputeScore(Score score, Game game)
    {
      var points = score.Points;
      points *= 1 - game.NbPlayerPenaltyValue *
                Math.Max(0, score.Team.TeamPlayers.Count - game.NbPlayerPenaltyThreshold);
      if (score.Team.Bonus.HasValue)
        points *= score.Team.Bonus.Value;
      return points;
    }
  }
}
