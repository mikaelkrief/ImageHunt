using System.Collections.Generic;

namespace ImageHuntCore.Model.Node
{
    /// <summary>
    ///     Choice node. The player should choose in a set of answers to pursue the hunt.
    /// </summary>
    public class ChoiceNode : Node
    {
        public string Choice { get; set; }
        public List<Answer> Answers { get; set; }
    }
}