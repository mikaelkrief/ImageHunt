using System.Collections.Generic;
using ImageHunt.Model;
using ImageHuntCore.Services;

namespace ImageHunt.Services
{
    public interface ITeamService : IService
    {
        void CreateTeam(int gameId, Team team);
        void DeleteTeam(Team team);
        IEnumerable<Team> GetTeams(int adminId);
      void AddMemberToTeam(Team team, List<Player> players);
      void DelMemberToTeam(Team team, Player playerToDelete);
      Team GetTeamByName(string teamName);
      Team GetTeamById(int teamId);
      Player GetPlayer(string playerLogin, int gameId);
    }
}
