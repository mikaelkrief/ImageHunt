using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;

namespace ImageHuntTelegramBot.Responses
{
  public class TeamResponse
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public PlayerResponse[] Players { get; set; }
  }

  public class PlayerResponse   
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string ChatLogin { get; set; }
  }
}