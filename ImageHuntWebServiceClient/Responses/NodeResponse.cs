namespace ImageHuntWebServiceClient.Responses
{
  public class NodeResponse
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string NodeType { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int Points { get; set; }
    public string Password { get; set; }
  }
}