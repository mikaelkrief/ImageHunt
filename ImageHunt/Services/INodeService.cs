using ImageHunt.Model;
using ImageHunt.Model.Node;

namespace ImageHunt.Services
{
    public interface INodeService : IService
    {
        void AddNode(Node node);
        Node GetNode(int nodeId);
        void AddChildren(int nodeId, int childrenNodeId);
        void AddChildren(Node parentNode, Node childrenNode);
    }
}
