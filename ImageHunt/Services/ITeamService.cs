using System.Collections.Generic;
using ImageHunt.Model;
using ImageHunt.Model.Node;
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
      IEnumerable<Team> GetTeamsForPlayer(Player player);
      Node NextNodeForTeam(int teamId, double playerLatitude, double playerLongitude);
      Node StartGame(int gameId, int teamId);

      void UploadImage(int gameId, int teamId, double latitude, double longitude, byte[] image, string imageName=null);
    }
}
