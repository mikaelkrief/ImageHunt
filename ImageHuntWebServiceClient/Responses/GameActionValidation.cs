namespace ImageHuntWebServiceClient.Responses
{
    public class GameActionValidationResponse
    {
        public int Id { get; set; }
        public NodeResponse[] ProbableNodes { get; set; }
    }
}
