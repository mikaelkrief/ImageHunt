using System.Linq;

namespace ImageHuntWebServiceClient.Responses
{

    public class AnswerResponse
    {


        public int Id { get; set; }
        public string Response { get; set; }
        public int? NodeId { get; set; }
        public bool Correct { get; set; }
    }
}
