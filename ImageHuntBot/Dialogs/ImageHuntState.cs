using ImageHuntWebServiceClient.Responses;
using Microsoft.AspNetCore.Diagnostics;
using System;

namespace ImageHuntTelegramBot.Dialogs
{
  public class ImageHuntState
  {
    public int GameId { get; set; }
    public GameResponse Game { get; set; }
    public int TeamId { get; set; }
    public TeamResponse Team { get; set; }
    public double CurrentLongitude { get; set; }
    public double CurrentLatitude { get; set; }
    public Status Status { get; set; }
    public override string ToString()
    {
        return $"gameid={GameId} teamId={TeamId} Status={Status}";
    }
  }

    public enum Status
    {
        None,
        Initialized,
        Started,
        Ended
    }
}