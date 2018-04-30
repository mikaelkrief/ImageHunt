using ImageHunt.Model;
using ImageHunt.Model.Node;
using ImageHuntCore.Services;

namespace ImageHunt.Services
{
  public interface INodeService : IService
  {
    void AddNode(Node node);
    Node GetNode(int nodeId);
    void AddChildren(int nodeId, int childrenNodeId);
    void AddChildren(Node parentNode, Node childrenNode);
    void RemoveChildren(int nodeId, int childrenNodeId);
    void LinkAnswerToNode(int answerId, int targetNodeId);
    void UnlinkAnswerToNode(int answerId);
    void RemoveAllChildren(Node node);
    Answer GetAnswer(int answerId);
    void RemoveNode(Node nodeToRemove);
    void RemoveRelation(Node orgNode, Node destNode);
  }
}
