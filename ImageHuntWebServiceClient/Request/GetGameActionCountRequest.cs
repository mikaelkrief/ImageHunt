namespace ImageHuntWebServiceClient.Request
{
  public class GameActionCountRequest
  {
    public int GameId { get; set; }
    public int? TeamId { get; set; }
    public string IncludeAction { get; set; }
  }
}