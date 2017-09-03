using System.Collections.Generic;
using System.Linq;
using ImageHunt.Data;
using ImageHunt.Model;
using ImageHunt.Model.Node;

namespace ImageHunt.Services
{
    public class NodeService : AbstractService, INodeService
    {

        public NodeService(HuntContext context) : base(context)
        {
        }
        public void AddNode(Node node)
        {
            Context.Nodes.Add(node);
            Context.SaveChanges();
        }

        public Node GetNode(int nodeId)
        {
            return Context.Nodes.Single(n => n.Id == nodeId);
        }

        public void AddChildren(int nodeId, Node childrenNode)
        {
            var node = Context.Nodes.Single(n => n.Id == nodeId);
            node.Children = node.Children ?? new List<Node>();
            node.Children.Add(childrenNode);

            Context.SaveChanges();
        }

        public void AddChildren(Node parentNode, TimerNode childrenNode)
        {
            var node = Context.Nodes.Single(n => n == parentNode);
            node.Children = node.Children ?? new List<Node>();
            node.Children.Add(childrenNode);
            Context.SaveChanges();
        }
    }
}