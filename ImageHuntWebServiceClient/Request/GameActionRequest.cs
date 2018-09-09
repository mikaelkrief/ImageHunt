namespace ImageHuntWebServiceClient.Request
{
  public class GameActionRequest
  {
    public int Action { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public string Picture { get; set; }
    public int TeamId { get; set; }
    public int GameId { get; set; }
    public int NodeId { get; set; }
    public int AnswerId { get; set; }
      public int PointsEarned { get; set; }
  }
}
