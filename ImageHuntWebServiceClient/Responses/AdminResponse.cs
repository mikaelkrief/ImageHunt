namespace ImageHuntWebServiceClient.Responses
{
    public class AdminResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int[] GameIds { get; set; }
        public GameResponse[] GameResponses { get; set; }
    }
}