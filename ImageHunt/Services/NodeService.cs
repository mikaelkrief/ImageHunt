using System.Collections.Generic;
using System.Linq;
using ImageHunt.Computation;
using ImageHunt.Data;
using ImageHunt.Model;
using ImageHunt.Model.Node;
using ImageHuntCore.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ImageHunt.Services
{
    public class NodeService : AbstractService, INodeService
    {

        public NodeService(HuntContext context, ILogger<NodeService> logger) : base(context, logger)
        {
        }
        public void AddNode(Node node)
        {
            Context.Nodes.Add(node);
            Context.SaveChanges();
        }

        public Node GetNode(int nodeId)
        {
            Node node = Context.Nodes.SingleOrDefault(n => n.Id == nodeId);
            switch (node.NodeType)
            {
                case "QuestionNode":
                    node = Context.QuestionNodes.Include(q => q.Answers).SingleOrDefault(n => n.Id == nodeId);
                    break;
                case "PictureNode":
                    node = Context.PictureNodes.Include(p => p.Image).SingleOrDefault(n => n.Id == nodeId);
                    break;
            }
            return node;
        }

        public void AddChildren(int nodeId, int childrenNodeId)
        {
            var node = Context.Nodes.Single(n => n.Id == nodeId);
            var childrenNode = Context.Nodes.Single(n => n.Id == childrenNodeId);
            var parentChildren = new ParentChildren() { Parent = node, Children = childrenNode };
            node.ChildrenRelation.Add(parentChildren);

            Context.SaveChanges();
        }

        public void AddChildren(Node parentNode, Node childrenNode)
        {
            var node = Context.Nodes.Single(n => n == parentNode);
            var children = Context.Nodes.Single(n => n == childrenNode);
            var parentChildren = new ParentChildren() { Parent = node, Children = children };
            node.ChildrenRelation.Add(parentChildren);
            Context.SaveChanges();
        }

        public void RemoveChildren(int nodeId, int childrenNodeId)
        {
            var node = Context.Nodes.Include(n => n.ChildrenRelation).Single(n => n.Id == nodeId);
            var childrenNode = Context.Nodes.Single(n => n.Id == childrenNodeId);
            var parentChildren = node.ChildrenRelation.Single(pc => pc.Parent == node && pc.Children == childrenNode);
            node.ChildrenRelation.Remove(parentChildren);
            Context.SaveChanges();

        }

        public void LinkAnswerToNode(int answerId, int targetNodeId)
        {
            var answer = Context.Answers.SingleOrDefault(a => a.Id == answerId);
            var childNode = Context.Nodes.SingleOrDefault(n => n.Id == targetNodeId);
            if (answer == null || childNode == null) return;
            answer.Node = childNode;
            Context.SaveChanges();
        }

        public void UnlinkAnswerToNode(int answerId)
        {
            var answer = Context.Answers.SingleOrDefault(a => a.Id == answerId);
            if (answer != null)
                answer.Node = null;
            Context.SaveChanges();
        }

        public void RemoveAllChildren(Node node)
        {
            var theNode = Context.Nodes.Include(n => n.ChildrenRelation).Single(n => n.Id == node.Id);
            theNode.ChildrenRelation.RemoveAll(n => true);
            Context.SaveChanges();
        }

        public Answer GetAnswer(int answerId)
        {
            return Context.Answers.Single(a => a.Id == answerId);
        }

        public Node FindPictureNodeByLocation(int gameId, (double, double) pictureCoordinates)
        {
            var nodes = Context.Games.Include(g => g.Nodes).Single(g => g.Id == gameId).Nodes.Where(n => n is PictureNode);
            if (!nodes.Any())
                return null;
            var pictureNode = new PictureNode() { Latitude = pictureCoordinates.Item1, Longitude = pictureCoordinates.Item2 };
            var closestNode =
              nodes.FirstOrDefault(n => n.Distance(pictureNode) < 40);
            return closestNode as PictureNode;
        }

        public void RemoveNode(Node nodeToRemove)
        {
            // Remove answers if node is QuestionNode
            if (nodeToRemove is QuestionNode questionNode && questionNode.Answers != null)
            {
                Context.Answers.RemoveRange(questionNode.Answers);
                questionNode.Answers.Clear();
            }
            // remove all children of the node to remove
            nodeToRemove.ChildrenRelation.Clear();
            // Retrieve relations of node to remove
            var parentsOfNode = Context.ParentChildren.Where(pc => pc.Children == nodeToRemove);
            Context.ParentChildren.RemoveRange(parentsOfNode);
            Context.Nodes.Remove(nodeToRemove);
            Context.SaveChanges();
        }

        public void RemoveRelation(Node orgNode, Node destNode)
        {
            orgNode = Context.Nodes.Include(n => n.ChildrenRelation)
              .Single(n => n == orgNode);
            // Remove answers for QuestionNode
            if (orgNode is QuestionNode questionNode)
            {
                questionNode = Context.QuestionNodes.Include(n => n.Answers).Single(n => n == questionNode);
                var answerToRemove = questionNode.Answers.Single(a => a.Node == destNode);
                questionNode.Answers.Remove(answerToRemove);
                Context.Answers.Remove(answerToRemove);
            }
            // Remove relation
            var relationToRemove = orgNode.ChildrenRelation.Single(pc => pc.Children == destNode);
            orgNode.ChildrenRelation.Remove(relationToRemove);
            Context.SaveChanges();
        }

      public void UpdateNode(Node node)
      {
        var oldNode = Context.Nodes.Single(n => n.Id == node.Id);
        oldNode.Name = node.Name;
        oldNode.Points = node.Points;
        oldNode.Latitude = node.Latitude;
        oldNode.Longitude = node.Longitude;
        Context.SaveChanges();
      }
    }
}
