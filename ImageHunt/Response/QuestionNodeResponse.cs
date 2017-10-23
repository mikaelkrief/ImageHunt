using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageHunt.Model.Node;

namespace ImageHunt.Response
{
    public class QuestionNodeResponse : NodeResponse
    {
      public QuestionNodeResponse(QuestionNode questionNode)
      :base(questionNode)
      {
        Question = questionNode.Question;
        Answers = questionNode.Answers.Select(a => new AnswerResponse(a)).ToArray();
      }
      public string Question { get; set; }
      public AnswerResponse[] Answers { get; set; }
    }

  public class AnswerResponse
  {

    public AnswerResponse(Answer answer)
    {
      Id = answer.Id;
      Response = answer.Response;
      NodeId = answer.Node?.Id;
    }

    public int Id { get; set; }
    public string Response { get; set; }
    public int? NodeId { get; set; }
  }
}
