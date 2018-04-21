using System;
using System.Linq;
using ImageHunt.Data;
using ImageHunt.Exception;
using ImageHunt.Model;
using ImageHunt.Model.Node;
using ImageHuntCore.Services;
using Microsoft.Extensions.Logging;
using Action = ImageHunt.Model.Action;
using Game = Telegram.Bot.Types.Game;

namespace ImageHunt.Services
{
  public class PlayerService : AbstractService, IPlayerService
  {

    public PlayerService(HuntContext context, ILogger<PlayerService> logger) : base(context, logger)
    {
    }

    public Player CreatePlayer(string name, string chatLogin)
    {
      var player = new Player(){Name = name, ChatLogin = chatLogin};
      Context.Players.Add(player);
      Context.SaveChanges();
      return player;
    }

    public Player JoinTeam(int teamId, int playerId)
    {
      var team = Context.Teams.Single(t => t.Id == teamId);
      var player = GetPlayerById(playerId);
      if (team !=null && player != null)
      {
        team.TeamPlayers.Add(new TeamPlayer(){Team = team, Player = player});
        Context.SaveChanges();
      }

      return player;
    }

    private Player GetPlayer(string playerName)
    {
      var player = Context.Players.SingleOrDefault(p => p.Name == playerName);
      if (player == null)
        throw new ArgumentException($"Player {playerName} doesn't exist");

      return player;
    }
    public void StartPlayer(string name)
    {
      var player = GetPlayer(name);
      var game = player.CurrentGame;
      if (game.StartDate.Value.Date != DateTime.Today || !game.IsActive)
        throw new ArgumentException("There is no game active or today");
      player.StartTime = DateTime.Now;
      player.CurrentNode = Enumerable.FirstOrDefault<Node>(game.Nodes, n => n is FirstNode);
      Context.SaveChanges();
    }

    public Node NextNodeForPlayer(string playerName, double playerLatitude, double playerLongitude)
    {
      var player = GetPlayer(playerName);
      if (player.CurrentGame == null || !player.CurrentGame.IsActive)
        throw new InvalidGameException();
      var nextNode = Enumerable.First<Node>(player.CurrentNode.Children);
      var gameAction = new GameAction()
      {
        DateOccured = DateTime.Now,
        Player = player,
        Game = player.CurrentGame,
        Longitude = playerLongitude,
        Latitude = playerLatitude,
        Node = player.CurrentNode
      };
      player.CurrentNode = nextNode;
      Context.GameActions.Add(gameAction);
      Context.SaveChanges();
      return nextNode;
    }

    public void UploadImage(string playerName, double latitude, double longitude, byte[] image)
    {
      if (image == null)
        throw new ArgumentException("Parameter image is not provided");
      var player = GetPlayer(playerName);
      var gameAction = new GameAction()
      {
        DateOccured = DateTime.Now,
        Game = player.CurrentGame,
        Player = player,
        Latitude = latitude,
        Longitude = longitude,
        Picture = new Picture() { Image = image},
        Action = Action.SubmitPicture
      };
      Context.GameActions.Add(gameAction);
      Context.SaveChanges();
    }

    public Player GetPlayerById(int playerId)
    {
      return Context.Players.Single(p => p.Id == playerId);
    }

    public Player GetPlayerByChatId(string chatId)
    {
      return Context.Players.Single(p => p.ChatLogin == chatId);
    }
  }
}
