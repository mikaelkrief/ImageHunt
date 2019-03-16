using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ImageHuntCore.Model.Node
{
    /// <summary>
    ///     Base class of all node object.
    /// </summary>
    public abstract class Node : DbObject
    {
        public Node()
        {
            ChildrenRelation = new List<ParentChildren>();
        }

        public double Longitude { get; set; }
        public double Latitude { get; set; }

        public string Name { get; set; }

        // Node that are children of the current node.
        //The application will follow up one of this node to the player if he complies with the current node
        public List<ParentChildren> ChildrenRelation { get; set; }
        public int Points { get; set; }
        [NotMapped] public string NodeType => GetType().Name;

        [NotMapped] public List<Node> Children => ChildrenRelation.Select(cr => cr.Children).ToList();
        [NotMapped] public int OrgId { get; set; }
    }

    public class ParentChildren : DbObject
    {
        public int ParentId { get; set; }
        public int ChildrenId { get; set; }
        public Node Parent { get; set; }
        public Node Children { get; set; }
    }
}