using System.Collections.Generic;
using ImageHuntCore.Model;
using ImageHuntCore.Model.Node;
using ImageHuntCore.Services;
using ImageHuntWebServiceClient.Responses;

namespace ImageHunt.Services
{
  public interface IGameService : IService
  {
    Game CreateGame(int adminId, Game newGame);
    Game GetGameById(int gameId);
    IEnumerable<Game> GetGamesForAdmin(int adminId);
    void AddNode(int gameId, Node node);
    void SetCenterOfGameByNodes(int gameId);
    IEnumerable<Node> GetNodes(int gameId, NodeTypes nodeType = NodeTypes.All);
    void SetGameZoom(int gameId, int zoom);
    Game GetGameFromPlayerChatId(string playerChatUserName);
    IEnumerable<Game> GetGamesFromPosition(double lat, double lng);
    IEnumerable<ChoiceNode> GetChoiceNodeOfGame(int gameId);
    void DeleteGame(int gameId);
    IEnumerable<PictureNode> GetPictureNode(int gameId);
    IEnumerable<Game> GetGamesWithScore();
    Game GetActiveGameForPlayer(Player player);
    IEnumerable<Game> GetAllGame();
    string GameCode(int gameId);
    Game Duplicate(Game orgGame, Admin admin);
    Game GetGameByCode(string gameCode);
    IEnumerable<Game> GetAllGameForValidation(Admin user);
    Game Toogle(int gameId, Flag flagToChange = Flag.Active);
  }

  public enum Flag
  {
    Public,
    Active
  }
}
