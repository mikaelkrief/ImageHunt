using System.Linq;
using ImageHunt.Data;
using ImageHuntCore.Model;
using ImageHuntCore.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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



    public Player GetPlayerById(int playerId)
    {
      return Context.Players.Single(p => p.Id == playerId);
    }

    public Player GetPlayerByChatId(string chatId)
    {
      return Context.Players
        .Include(p=>p.TeamPlayers).ToList()
        .SingleOrDefault(p => p.ChatLogin == chatId);
    }
  }
}
