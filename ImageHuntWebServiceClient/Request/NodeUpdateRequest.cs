namespace ImageHuntWebServiceClient.Request
{
    public class NodeUpdateRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Points { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}