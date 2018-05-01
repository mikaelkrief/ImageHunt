using System;
using System.Collections.Generic;
using ImageHunt.Model;
using ImageHunt.Model.Node;
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
    IEnumerable<QuestionNode> GetQuestionNodeOfGame(int gameId);
    void DeleteGame(int gameId);
    IEnumerable<Error> ValidateGame(Game game);
  }

  public class Error
  {
    public Error(Game game, Node node, string message)
    {
      Game = game;
      Node = node;
      Message = message;
    }
    public Game Game { get; set; }
    public Node Node { get; set; }
    public string Message { get; set; }
  }
}
