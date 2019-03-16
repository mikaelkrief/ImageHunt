using System.Collections.Generic;

namespace ImageHuntCore.Model
{
  public class GameActionToValidate : GameAction
  {
    public IEnumerable<ImageHuntCore.Model.Node.Node> ProbableNodes { get; set; }
  }
}
