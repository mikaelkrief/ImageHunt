using ImageHuntWebServiceClient.Responses;

namespace ImageHuntTelegramBot.Dialogs
{
  public class ImageHuntState
  {
    public int GameId { get; set; }
    public GameResponse Game { get; set; }
    public int TeamId { get; set; }
    public TeamResponse Team { get; set; }
  }
}