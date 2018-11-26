using System.Collections.Generic;

namespace ImageHuntCore.Model.Node
{
  /// <summary>
  /// Question node. The player should choose in a set of answers to pursue the hunt. 
  /// </summary>
    public class QuestionNode : ImageHuntCore.Model.Node.Node
    {
        public string Question { get; set; }
        public List<Answer> Answers { get; set; }
    }
}