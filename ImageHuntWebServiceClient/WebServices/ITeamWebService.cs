using System;
using System.Threading;
using System.Threading.Tasks;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.Responses;

namespace ImageHuntWebServiceClient.WebServices
{
  public interface ITeamWebService
  {
    Task<TeamResponse> GetTeamById(int teamId);
    Task<NodeResponse> UploadImage(UploadImageRequest request);
      Task<TeamResponse> GetTeamForUserName(int gameId, string userName);
      Task<TeamResponse> CreateTeam(int gameId, TeamRequest teamRequest);
      Task<TeamResponse> AddPlayer(int teamId, PlayerRequest playerRequest);
      Task<NodeResponse> StartGameForTeam(int gameId, int teamId, CancellationToken cancellationToken = default(CancellationToken));

      Task RemovePlayerFromTeam(int teamId, string chatId);
      Task UpdateTeam(UpdateTeamRequest updateTeamRequest);
  }
}