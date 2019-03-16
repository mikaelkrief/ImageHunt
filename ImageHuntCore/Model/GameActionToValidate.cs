using System.Collections.Generic;

namespace ImageHuntCore.Model
{
    public class GameActionToValidate : GameAction
    {
        public IEnumerable<Node.Node> ProbableNodes { get; set; }
    }
}