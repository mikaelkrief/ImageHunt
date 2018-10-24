using ImageHuntWebServiceClient.Responses;
using Microsoft.AspNetCore.Diagnostics;
using System;
using ImageHuntBot;
using Telegram.Bot.Types;

namespace ImageHuntTelegramBot.Dialogs
{
    public class ImageHuntState : IBaseState
    {
        public int GameId { get; set; }
        public GameResponse Game { get; set; }
        public int TeamId { get; set; }
        public TeamResponse Team { get; set; }
        public double CurrentLongitude { get; set; }
        public double CurrentLatitude { get; set; }
        public Status Status { get; set; }
        public int CurrentNodeId { get; set; }
        public NodeResponse CurrentNode { get; set; }
        public override string ToString()
        {
            return $"chatId={ChatId} gameid={GameId} teamId={TeamId} Status={Status}";
        }

        public long ChatId { get; set; }
    }

    public enum Status
    {
        None,
        Initialized,
        Started,
        Ended
    }
}