using System;
using System.Collections.Generic;
using ImageHunt.Model;
using ImageHuntCore.Model;
using ImageHuntCore.Model.Node;
using ImageHuntCore.Services;

namespace ImageHunt.Services
{
  public interface IGameService : IService
  {
    Game CreateGame(int adminId, Game newGame);
    Game GetGameById(int gameId);
    IEnumerable<Game> GetGamesForAdmin(int adminId);
    void AddNode(int gameId, Node node);
    void SetCenterOfGameByNodes(int gameId);
    IEnumerable<Node> GetNodes(int gameId);
    void SetGameZoom(int gameId, int zoom);
    Game GetGameFromPlayerChatId(string playerChatUserName);
    IEnumerable<Game> GetGamesFromPosition(double lat, double lng);
    IEnumerable<ChoiceNode> GetChoiceNodeOfGame(int gameId);
    void DeleteGame(int gameId);
    IEnumerable<PictureNode> GetPictureNode(int gameId);
    IEnumerable<Game> GetGamesWithScore();
    Game GetActiveGameForPlayer(Player player);
    IEnumerable<Game> GetAllGame();
  }
}
