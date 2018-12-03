using ImageHuntCore.Model;

namespace ImageHunt.Services
{
    public interface IScoreChanger
    {
      double ComputeScore(Score score, Game game);
    }
}
