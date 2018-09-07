
namespace ImageHuntWebServiceClient.Responses
{
  public class TeamResponse
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public PlayerResponse[] Players { get; set; }
      public int GameId { get; set; }
  }
}