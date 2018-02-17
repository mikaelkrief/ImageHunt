using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ImageHunt.Model.Node
{
  /// <summary>
  /// Question node. The player should choose in a set of answers to pursue the hunt. 
  /// </summary>
    public class QuestionNode : Node
    {
        public string Question { get; set; }
        public List<Answer> Answers { get; set; }
    }
}
