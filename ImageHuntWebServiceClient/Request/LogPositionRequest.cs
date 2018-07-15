namespace ImageHuntWebServiceClient.Request
{
    public class LogPositionRequest
    {
        public int GameId { get; set; }
        public int TeamId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}