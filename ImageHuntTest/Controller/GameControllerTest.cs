using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using FakeItEasy;
using ImageHunt.Controllers;
using ImageHunt.Model;
using ImageHunt.Model.Node;
using ImageHunt.Request;
using ImageHunt.Response;
using ImageHunt.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using NFluent;
using Xunit;

namespace ImageHuntTest.Controller
{
    public class GameControllerTest : BaseTest
    {
        private IGameService _gameService;
        private GameController _target;
        private INodeService _nodeService;
        private IImageService _imageService;

        public GameControllerTest()
        {
            _gameService = A.Fake<IGameService>();
            _nodeService = A.Fake<INodeService>();
            _imageService = A.Fake<IImageService>();
            _target = new GameController(_gameService, _imageService);
        }

        [Fact]
        public void GetGameById()
        {
            // Arrange

            // Act
            _target.GetGameById(2);
            // Assert
            A.CallTo(() => _gameService.GetGameById(A<int>._)).MustHaveHappened();
        }

        [Fact]
        public void GetGames()
        {
            // Arrange

            // Act
            var result = _target.GetGames(1) as OkObjectResult;
            // Assert
            A.CallTo(() => _gameService.GetGamesForAdmin(A<int>._)).MustHaveHappened();
            Check.That(result).IsNotNull();
        }

        [Fact]
        public void CreateGame()
        {
            // Arrange
            var game = new Game();
            // Act
            var result = _target.CreateGame(1, game);
            // Assert
            A.CallTo(() => _gameService.CreateGame(1, game)).MustHaveHappened();
        }

        [Fact]
        public void AddNode()
        {
            // Arrange
            var node = new AddNodeRequest(){NodeType = "TimerNode"};
            // Act
            var result = _target.AddNode(1, node);
            // Assert
            A.CallTo(() => _gameService.AddNode(1, A<Node>._)).MustHaveHappened();
        }

      [Fact]
      public void GetNodes()
      {
        // Arrange
        var nodes = new List<Node>()
        {
          new TimerNode()
          {
            Id = 1,
            ChildrenRelation = new List<ParentChildren>() {new ParentChildren()
            {
              Children = new QuestionNode()
              {
                Id = 2,
                ChildrenRelation = new List<ParentChildren>()
                {
                  new ParentChildren() { Children = new FirstNode(){Id = 3}}
                }
              }
              
            }}
          }
        };
        A.CallTo(() => _gameService.GetNodes(1)).Returns(nodes);
        // Act
        var result = _target.GetNodesRelations(1) as OkObjectResult;
        // Assert
        Check.That(result).IsNotNull();
        A.CallTo(() => _gameService.GetNodes(1)).MustHaveHappened();
        var resNodes = result.Value as List<NodeResponse>;
        // Check that only first level nodes are populated
        Check.That(resNodes[0].NodeId).Equals(1);
        Check.That(resNodes[0].ChildNodeId).HasSize(1).And.ContainsExactly(2);
      }
        [Fact]
        public void AddImagesNodes()
        {
            // Arrange
            var file = A.Fake<IFormFile>();
            var images = new List<IFormFile>() { file, file, file };
            A.CallTo(() => _imageService.ExtractLocationFromImage(A<Picture>._)).Returns((15d, 16d));
            // Act
            _target.AddImageNodes(1, images);
            // Assert
            A.CallTo(() => _gameService.AddNode(1, A<Node>._)).MustHaveHappened(Repeated.Exactly.Times(3));
        }

      [Fact]
      public void SetCenterOfGameByNodes()
      {
        // Arrange
        
        // Act
        _target.SetCenterOfGameByNodes(1);
        // Assert
        A.CallTo(() => _gameService.SetCenterOfGameByNodes(A<int>._)).MustHaveHappened();
      }
    }
}
