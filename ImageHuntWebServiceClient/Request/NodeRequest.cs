namespace ImageHuntWebServiceClient.Request
{
    public class NodeRequest    
    {
        public string NodeType { get; set; }
        public int GameId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}