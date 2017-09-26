using System;
using System.Collections.Generic;
using ImageHunt.Model;
using ImageHunt.Model.Node;

namespace ImageHunt.Services
{
  public interface IGameService : IService
  {
    Game CreateGame(int adminId, Game newGame);
    Game GetGameById(int gameId);
    IEnumerable<Game> GetGamesForAdmin(int adminId);
    void AddNode(int gameId, Node node);
    void SetCenterOfGameByNodes(int gameId);
  }
}
