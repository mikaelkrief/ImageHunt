using System;
using System.Collections.Generic;
using ImageHunt.Model;
using ImageHuntCore.Model;
using ImageHuntCore.Model.Node;
using ImageHuntWebServiceClient.Responses;
using NFluent;
using Xunit;

namespace ImageHuntTest.Model
{
    public class NodeFactoryTest
    {
        [Theory]
        [InlineData(NodeResponse.TimerNodeType, typeof(TimerNode))]
        [InlineData(NodeResponse.ObjectNodeType, typeof(ObjectNode))]
        [InlineData(NodeResponse.ChoiceNodeType, typeof(ChoiceNode))]
        [InlineData(NodeResponse.FirstNodeType, typeof(FirstNode))]
        [InlineData(NodeResponse.LastNodeType, typeof(LastNode))]
        [InlineData(NodeResponse.PictureNodeType, typeof(PictureNode))]
        public void NodeFromNodeType(string nodeName, Type expectedType)
        {
            // Arrange
            var nodeType = nodeName;
            // Act
            var node = NodeFactory.CreateNode(nodeType);
            // Assert
            Check.That(node.GetType()).Equals(expectedType);
        }

        [Fact]
        public void Should_Duplicate_ObjectNode()
        {
            // Arrange
            var node = new ObjectNode() { Latitude = 5, Longitude = 6, Name = "toto", Points = 56, Action = "Action" };
            // Act
            var newNode = NodeFactory.DuplicateNode(node);
            // Assert
            Check.That(newNode.Latitude).Equals(node.Latitude);
            Check.That(newNode.Longitude).Equals(node.Longitude);
            Check.That(newNode.Name).Equals(node.Name);
            Check.That(newNode.Points).Equals(node.Points);
            Check.That(newNode).HasFieldsWithSameValues(node);
        }
        [Fact]
        public void Should_Duplicate_WaypointNode()
        {
            // Arrange
            var node = new WaypointNode() { Latitude = 5, Longitude = 6, Name = "toto", Points = 56,Id=68 };
            // Act
            var newNode = NodeFactory.DuplicateNode(node);
            // Assert
            Check.That(newNode.Latitude).Equals(node.Latitude);
            Check.That(newNode.Longitude).Equals(node.Longitude);
            Check.That(newNode.Name).Equals(node.Name);
            Check.That(newNode.Points).Equals(node.Points);
            Check.That(newNode.OrgId).Equals(node.Id);
        }
        [Fact]
        public void Should_Duplicate_FirstNode()
        {
            // Arrange
            var node = new FirstNode() { Latitude = 5, Longitude = 6, Name = "toto", Points = 56, Password = "Action" };
            // Act
            var newNode = NodeFactory.DuplicateNode(node);
            // Assert
            Check.That(newNode.Latitude).Equals(node.Latitude);
            Check.That(newNode.Longitude).Equals(node.Longitude);
            Check.That(newNode.Name).Equals(node.Name);
            Check.That(newNode.Points).Equals(node.Points);
            Check.That(newNode).HasFieldsWithSameValues(node);
        }
        [Fact]
        public void Should_Duplicate_TimerNode()
        {
            // Arrange
            var node = new TimerNode() { Latitude = 5, Longitude = 6, Name = "toto", Points = 56, Delay = 56 };
            // Act
            var newNode = NodeFactory.DuplicateNode(node);
            // Assert
            Check.That(newNode.Latitude).Equals(node.Latitude);
            Check.That(newNode.Longitude).Equals(node.Longitude);
            Check.That(newNode.Name).Equals(node.Name);
            Check.That(newNode.Points).Equals(node.Points);
            Check.That(newNode).HasFieldsWithSameValues(node);
        }
        [Fact]
        public void Should_Duplicate_QuestionNode()
        {
            // Arrange
            var node = new QuestionNode() { Latitude = 5, Longitude = 6, Name = "toto", Points = 56, Question = "Question", Answer = "Answer" };
            // Act
            var newNode = NodeFactory.DuplicateNode(node);
            // Assert
            Check.That(newNode.Latitude).Equals(node.Latitude);
            Check.That(newNode.Longitude).Equals(node.Longitude);
            Check.That(newNode.Name).Equals(node.Name);
            Check.That(newNode.Points).Equals(node.Points);
            Check.That(newNode).HasFieldsWithSameValues(node);
        }
        [Fact]
        public void Should_Duplicate_PictureNode()
        {
            // Arrange
            var node = new PictureNode() { Latitude = 5, Longitude = 6, Name = "toto", Points = 56, Image = new Picture() };
            // Act
            var newNode = NodeFactory.DuplicateNode(node);
            // Assert
            Check.That(newNode.Latitude).Equals(node.Latitude);
            Check.That(newNode.Longitude).Equals(node.Longitude);
            Check.That(newNode.Name).Equals(node.Name);
            Check.That(newNode.Points).Equals(node.Points);
            Check.That(newNode).HasFieldsWithSameValues(node);
        }
        [Fact]
        public void Should_Duplicate_HiddenNode()
        {
            // Arrange
            var node = new HiddenNode() { Latitude = 5, Longitude = 6, Name = "toto", Points = 56, LocationHint = "Location" };
            // Act
            var newNode = NodeFactory.DuplicateNode(node);
            // Assert
            Check.That(newNode.Latitude).Equals(node.Latitude);
            Check.That(newNode.Longitude).Equals(node.Longitude);
            Check.That(newNode.Name).Equals(node.Name);
            Check.That(newNode.Points).Equals(node.Points);
            Check.That(newNode).HasFieldsWithSameValues(node);
        }
        [Fact]
        public void Should_Duplicate_BonusNode()
        {
            // Arrange
            var node = new BonusNode() { Latitude = 5, Longitude = 6, Name = "toto", Points = 56, Location = "Location", BonusType = BonusNode.BONUS_TYPE.Points_x2 };
            // Act
            var newNode = NodeFactory.DuplicateNode(node);
            // Assert
            Check.That(newNode.Latitude).Equals(node.Latitude);
            Check.That(newNode.Longitude).Equals(node.Longitude);
            Check.That(newNode.Name).Equals(node.Name);
            Check.That(newNode.Points).Equals(node.Points);
            Check.That(newNode).HasFieldsWithSameValues(node);
        }
        [Fact]
        public void Should_Duplicate_ChoiceNode()
        {
            // Arrange
            var node = new ChoiceNode()
            {
                Latitude = 5,
                Longitude = 6,
                Name = "toto",
                Points = 56,
                Choice = "Choice",
                Answers = new List<Answer>(){
              new Answer()
          {
              Response = "Response1", Correct = true
          },
              new Answer()
              {
                  Response = "Response2", Correct = false,
              }
          }
            };
            // Act
            var newNode = NodeFactory.DuplicateNode(node);
            // Assert
            Check.That(newNode.Latitude).Equals(node.Latitude);
            Check.That(newNode.Longitude).Equals(node.Longitude);
            Check.That(newNode.Name).Equals(node.Name);
            Check.That(newNode.Points).Equals(node.Points);
            Check.That(newNode).HasFieldsWithSameValues(node);
            Check.That(newNode.OrgId).Equals(node.Id);
        }
    }
}