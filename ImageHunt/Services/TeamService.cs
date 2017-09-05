using System;
using System.Collections.Generic;
using System.Linq;
using ImageHunt.Data;
using ImageHunt.Model;
using Microsoft.EntityFrameworkCore;

namespace ImageHunt.Services
{
    public class TeamService : AbstractService, ITeamService
    {
        public TeamService(HuntContext context) 
            : base(context)
        {
            
        }

        public void CreateTeam(Team team)
        {
            Context.Teams.Add(team);
            Context.SaveChanges();
        }

        public void DeleteTeam(Team team)
        {
            var teamToDelete = Context.Teams.Single(t => t.Id == team.Id);
            Context.Teams.Remove(teamToDelete);
            Context.SaveChanges();
        }

        public IEnumerable<Team> GetTeams()
        {
            return Context.Teams.Include(t=>t.Players);
        }

      public void AddMemberToTeam(Team team, List<Player> players)
      {
        var teamToAddPlayers = Context.Teams.Single(t => t.Id == team.Id);
        var playerList = teamToAddPlayers.Players ?? new List<Player>();
        playerList.AddRange(players);
        teamToAddPlayers.Players = playerList;
        Context.SaveChanges();
      }

      public void DelMemberToTeam(Team team, Player playerToDelete)
      {
        var teamToModify = Context.Teams.Single(t => t.Id == team.Id);
        var playerToRemove = Context.Players.Single(p => p.Id == playerToDelete.Id);
        teamToModify.Players.Remove(playerToRemove);
        Context.SaveChanges();
      }

      public Team GetTeamByName(string teamName)
      {
        return Context.Teams.Include(t=>t.Players).Single(t => t.Name == teamName);
      }

      public Team GetTeamById(int teamId)
      {
        return Context.Teams.Include(t => t.Players).Single(t => t.Id == teamId);
      }
    }
}
