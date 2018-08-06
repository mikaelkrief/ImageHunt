using System.Collections.Generic;

namespace ImageHunt.Model
{
  public class GameActionToValidate : GameAction
  {
    public IEnumerable<Node.Node> ProbableNodes { get; set; }
  }
}
