using System.Linq;

namespace ImageHuntWebServiceClient.Responses
{
    public class QuestionNodeResponse : NodeResponse
    {
      public string Question { get; set; }
      public AnswerResponse[] Answers { get; set; }
    }

  public class AnswerResponse
  {


    public int Id { get; set; }
    public string Response { get; set; }
    public int? NodeId { get; set; }
  }
}
