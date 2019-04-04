using System.Collections.Generic;
using AutoMapper;
using ImageHuntCore.Model.Node;
using ImageHuntWebServiceClient.Responses;
using NFluent;
using Xunit;

namespace ImageHuntTest.Model.Node
{
    [Collection("AutomapperFixture")]
    public class NodeMappingTest
    {
        private IMapper _mapper;

        public NodeMappingTest()
        {
            _mapper = AutoMapper.Mapper.Instance;

        }

        [Fact]
        public void MapNodeToNodeResponse()
        {
            // Arrange
            var node = new ObjectNode() {Action = "Toto",Name = "titi"};
            // Act
            var response = _mapper.Map<NodeResponse>(node);
            // Assert
            Check.That(response.Name).Equals(node.Name);
            Check.That(response.Action).Equals(node.Action);
        }

        [Fact]
        public void MapDelayToNodeResponse()
        {
            // Arrange
            var node = new TimerNode() { Delay = 56, Name = "titi" };
            // Act
            var response = _mapper.Map<NodeResponse>(node);
            // Assert
            Check.That(response.Name).Equals(node.Name);
            Check.That(response.Delay).Equals(node.Delay);
        }
        [Fact]
        public void MapBonusNodeToNodeResponse()
        {
            // Arrange
            var node = new BonusNode() { Location = "Location", BonusType = BonusNode.BONUS_TYPE.Points_x2, Name = "titi" };
            // Act
            var response = _mapper.Map<NodeResponse>(node);
            // Assert
            Check.That(response.Name).Equals(node.Name);
            Check.That(response.Hint).Equals(node.Location);
            Check.That(response.BonusType).Equals(node.BonusType);
        }
        [Fact]
        public void MapHiddenNodeToNodeResponse()
        {
            // Arrange
            var node = new HiddenNode() { LocationHint = "Location", Points = 56, Name = "titi" };
            // Act
            var response = _mapper.Map<NodeResponse>(node);
            // Assert
            Check.That(response.Name).Equals(node.Name);
            Check.That(response.Hint).Equals(node.LocationHint);
            Check.That(response.Points).Equals(node.Points);
        }

        [Fact]
        public void Should_Map_Children_To_ChildrenIds()
        {
            // Arrange
            var childrenNodes = new List<ImageHuntCore.Model.Node.Node>(){new ObjectNode(){Id=2}, new BonusNode(){Id = 3}, new ChoiceNode(){Id = 4}};
            var node = new ObjectNode();
            var parentChildren = new List<ParentChildren>()
            {
                new ParentChildren() {Parent = node, Children = childrenNodes[0]},
                new ParentChildren() {Parent = node, Children = childrenNodes[1]},
                new ParentChildren() {Parent = node, Children = childrenNodes[2]},

            };
            node.ChildrenRelation = parentChildren;
            // Act
            var response = _mapper.Map<NodeResponse>(node);
            // Assert
            Check.That(response.ChildNodeIds).ContainsExactly(2, 3, 4);
        }
    }
}
