using ImageHunt.Model;
using ImageHunt.Model.Node;

namespace ImageHuntEngine
{
  public interface IPlayerService
  {
    Player CreatePlayer(string name, string chatLogin);
    Player JoinTeam(string game2, string name, string teamName);
    void StartPlayer(string name);
    Node NextNodeForPlayer(string playerName);
  }
}