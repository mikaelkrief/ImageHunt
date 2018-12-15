using System.Collections.Generic;

namespace ImageHuntCore.Model.Node
{
  /// <summary>
  /// Choice node. The player should choose in a set of answers to pursue the hunt. 
  /// </summary>
    public class ChoiceNode : ImageHuntCore.Model.Node.Node
    {
        public string Question { get; set; }
        public List<Answer> Answers { get; set; }
    }
}
