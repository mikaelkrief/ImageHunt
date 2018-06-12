using System;
using System.Linq;
using ImageHunt.Data;
using ImageHunt.Exception;
using ImageHunt.Model;
using ImageHunt.Model.Node;
using ImageHuntCore.Services;
using Microsoft.Extensions.Logging;
using Action = ImageHunt.Model.Action;

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
    private Player GetPlayer(int playerId)
    {
      var player = Context.Players.SingleOrDefault(p => p.Id == playerId);
      if (player == null)
        throw new ArgumentException($"Player {playerId} doesn't exist");

      return player;
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
