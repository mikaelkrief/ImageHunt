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
    public void AddNodeTimerNode()
    {
      // Arrange
      var node = new AddNodeRequest() { NodeType = "TimerNode", Duration = 1561 };
      // Act
      var result = _target.AddNode(1, node);
      // Assert
      A.CallTo(() => _gameService.AddNode(1, A<Node>.That.Matches(n => CheckTimerNode(n, node.Duration)))).MustHaveHappened();
    }

    private bool CheckTimerNode(Node node, int expectedDuration)
    {
      var timerNode = node as TimerNode;
      Check.That(timerNode.Delay).Equals(expectedDuration);
      return true;
    }

    [Fact]
    public void AddNodeObjectNode()
    {
      // Arrange
      var node = new AddNodeRequest() { NodeType = "ObjectNode", Action = "Selfie" };
      // Act
      var result = _target.AddNode(1, node);
      // Assert
      A.CallTo(() => _gameService.AddNode(1, A<Node>.That.Matches(n => CheckObjectNode(n, node.Action)))).MustHaveHappened();
    }

    private bool CheckObjectNode(Node node, string expectedAction)
    {
      var objectNode = node as ObjectNode;
      Check.That(objectNode.Action).Equals(expectedAction);
      return true;
    }
    [Fact]
    public void AddNodeQuestionNode()
    {
      // Arrange
      var node = new AddNodeRequest() { NodeType = "QuestionNode", Question = "Selfie", Answers = new[] { "Toto", "Tata" } };
      // Act
      var result = _target.AddNode(1, node);
      // Assert
      A.CallTo(() => _gameService.AddNode(1, A<Node>.That.Matches(n => CheckQuestionNode(n, node.Question, node.Answers)))).MustHaveHappened();
    }

    private bool CheckQuestionNode(Node node, string expectedQuestion, string[] nodeAnswers)
    {
      var questionNode = node as QuestionNode;
      Check.That(questionNode.Question).Equals(expectedQuestion);
      Check.That(questionNode.Answers.Extracting("Response")).ContainsExactly(nodeAnswers);
      return true;
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
      var picture = GetImageFromResource("ImageHuntTest.TestData.IMG_20170920_180905.jpg");
      var file = A.Fake<IFormFile>();
      A.CallTo(() => file.OpenReadStream()).ReturnsNextFromSequence(new MemoryStream(picture), new MemoryStream(picture), new MemoryStream(picture));
      var images = new List<IFormFile>() { file, file, file };
      //A.CallTo(() => ImageService.ExtractLocationFromImage(A<Picture>._)).Returns((15d, 16d));
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

    [Fact]
    public void UpdateZoom()
    {
      // Arrange

      // Act
      _target.UpdateZoom(1, 12);
      // Assert
      A.CallTo(() => _gameService.SetGameZoom(1, 12)).MustHaveHappened();
    }
  }
}
