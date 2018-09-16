namespace ImageHuntWebServiceClient.Request
{
  public class GetGameActionCountRequest
  {
    public int GameId { get; set; }
    public string IncludeAction { get; set; }
  }
}