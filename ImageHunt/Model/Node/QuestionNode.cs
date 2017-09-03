using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ImageHunt.Model.Node
{
    public class QuestionNode : Node
    {
        public string Question { get; set; }
        public List<Answer> Answers { get; set; }
    }

    public class Answer
    {
        [Key]
        public int Id { get; set; }
        public string Response { get; set; }
        public Node Node { get; set; }
    }
}
