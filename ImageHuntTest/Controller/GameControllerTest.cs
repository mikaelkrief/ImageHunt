using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FakeItEasy;
using ImageHunt;
using ImageHunt.Computation;
using ImageHunt.Controllers;
using ImageHunt.Model;
using ImageHunt.Model.Node;
using ImageHunt.Response;
using ImageHunt.Services;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Logging;
using NFluent;
using TestUtilities;
using Xunit;
using NodeResponse = ImageHunt.Response.NodeResponse;

namespace ImageHuntTest.Controller
{
    [Collection("AutomapperFixture")]
    public class GameControllerTest : BaseTest
    {
        private IGameService _gameService;
        private GameController _target;
        private INodeService _nodeService;
        private IImageService _imageService;
        private IActionService _actionService;
        private ILogger<GameController> _logger;
        private IImageTransformation _imageTransformation;
        private IMapper _mapper;

        public GameControllerTest()
        {
            _gameService = A.Fake<IGameService>();
            _nodeService = A.Fake<INodeService>();
            _imageService = A.Fake<IImageService>();
            _actionService = A.Fake<IActionService>();
            _logger = A.Fake<ILogger<GameController>>();
            _imageTransformation = A.Fake<IImageTransformation>();
            _mapper = Mapper.Instance;
            _target = new GameController(_gameService, _imageService, _nodeService, _actionService, _logger, _imageTransformation, _mapper);
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
        [Fact]
        public void AddNodeFirstNode()
        {
            // Arrange
            var node = new AddNodeRequest() { NodeType = "FirstNode", Password = "toto" };
            // Act
            var result = _target.AddNode(1, node);
            // Assert
            A.CallTo(() => _gameService.AddNode(1, A<Node>.That.Matches(n => CheckFirstNode(n, node.Password)))).MustHaveHappened();
        }

        private bool CheckFirstNode(Node node, string nodePassword)
        {
            var firstNode = node as FirstNode;
            Check.That(firstNode.Password).Equals(nodePassword);
            return true;

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
            var node = new AddNodeRequest() { NodeType = "ObjectNode", Action = "Selfie", Points = 15 };
            // Act
            var result = _target.AddNode(1, node);
            // Assert
            A.CallTo(() => _gameService.AddNode(1, A<Node>.That.Matches(n => CheckObjectNode(n, node.Action)))).MustHaveHappened();
        }

        private bool CheckObjectNode(Node node, string expectedAction)
        {
            var objectNode = node as ObjectNode;
            Check.That(objectNode.Action).Equals(expectedAction);
            Check.That(objectNode.Points).Equals(15);
            return true;
        }
        [Fact]
        public void AddNodeQuestionNode()
        {
            // Arrange
            var node = new AddNodeRequest() { NodeType = "QuestionNode", Question = "Selfie", Answers = new AnswerRequest[] { new AnswerRequest() { Response = "Toto" }, new AnswerRequest() { Response = "Tata" } } };
            // Act
            var result = _target.AddNode(1, node);
            // Assert
            A.CallTo(() => _gameService.AddNode(1, A<Node>.That.Matches(n => CheckQuestionNode(n, node.Question, node.Answers)))).MustHaveHappened();
        }

        private bool CheckQuestionNode(Node node, string expectedQuestion, AnswerRequest[] nodeAnswers)
        {
            var questionNode = node as QuestionNode;
            Check.That(questionNode.Question).Equals(expectedQuestion);
            Check.That(questionNode.Answers.Extracting("Response")).ContainsExactly(nodeAnswers.Extracting("Response"));
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
            Check.That(resNodes).HasSize(1);
            Check.That(resNodes[0].NodeId).Equals(nodes[0].Id);
            Check.That(resNodes[0].ChildNodeId).Equals(nodes[0].Children[0].Id);
        }
        [Fact]
        public void AddImagesNodes()
        {
            // Arrange
            var picture = GetImageFromResource(Assembly.GetExecutingAssembly(), "ImageHuntTest.TestData.IMG_20170920_180905.jpg");
            var file = A.Fake<IFormFile>();
            A.CallTo(() => file.OpenReadStream()).ReturnsNextFromSequence(new MemoryStream(picture), new MemoryStream(picture), new MemoryStream(picture));
            var images = new List<IFormFile>() { file, file, file };
            // Act
            _target.AddImageNodes(1, images);
            // Assert
            A.CallTo(() => _gameService.AddNode(1, A<Node>._)).MustHaveHappened(Repeated.Exactly.Times(3));
        }
        [Fact]
        public void AddImagesNodes_without_Geotag()
        {
            // Arrange
            var picture = GetImageFromResource(Assembly.GetExecutingAssembly(), "ImageHuntTest.TestData.IMG_20170920_180905.jpg");
            var file = A.Fake<IFormFile>();
            A.CallTo(() => file.OpenReadStream()).ReturnsNextFromSequence(new MemoryStream(picture), new MemoryStream(picture), new MemoryStream(picture));
            A.CallTo(() => _imageService.ExtractLocationFromImage(A<Picture>._)).Returns((double.NaN, double.NaN));
            var images = new List<IFormFile>() { file, file, file };
            // Act
            _target.AddImageNodes(1, images);
            // Assert
            A.CallTo(() => _gameService.AddNode(1, A<Node>._)).MustNotHaveHappened();
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

        [Fact]
        public void GetGameFromPlayerUserName()
        {
            // Arrange
            var game = new Game();
            A.CallTo(() => _gameService.GetGameFromPlayerChatId(A<string>._)).Returns(game);
            // Act
            var result = _target.GetGameFromPlayerUserName("toto") as OkObjectResult;
            // Assert
            Check.That(result.Value).Equals(game);
            A.CallTo(() => _gameService.GetGameFromPlayerChatId(A<string>._)).MustHaveHappened();
        }

        [Fact]
        public void GetGamesfromLocation()
        {
            // Arrange

            // Act
            var result = _target.GetGamesFromLocation(15.26, 1.26) as OkObjectResult;
            // Assert
            A.CallTo(() => _gameService.GetGamesFromPosition(A<double>._, A<double>._)).MustHaveHappened();
            Check.That(result).IsNotNull();
        }

        [Fact]
        public void GetQuestionNodeOfGame()
        {
            // Arrange
            var questionNodes = new List<QuestionNode>()
          {
              new QuestionNode()
              {
                  Id = 1,
                  Question = "What",
                  Answers = new List<Answer>()
                  {
                      new Answer()
                      {
                          Id = 1,
                          Node = new PictureNode(){Id=101},
                          Response = "A"
                      },
                      new Answer()
                      {
                          Id = 2,
                          Response = "B"
                      },
                      new Answer()
                      {
                          Id = 3,
                          Node = new PictureNode(){Id=102},
                          Response = "C"
                      },

                  }
              },
              new QuestionNode()
              {
                  Id = 2,
                  Question = "Who",
                  Answers = new List<Answer>()
                  {
                      new Answer()
                      {
                          Id = 4,
                          Response = "1"
                      },
                      new Answer()
                      {
                          Id = 5,
                          Response = "2"
                      },
                      new Answer()
                      {
                          Id = 6,
                          Node = new PictureNode(){Id=103},
                          Response = "3"
                      },

                  }
              }
          };
            A.CallTo(() => _gameService.GetQuestionNodeOfGame(A<int>._)).Returns(questionNodes);
            // Act
            var result = _target.GetQuestionNodeOfGame(1) as OkObjectResult;
            // Assert
            A.CallTo(() => _gameService.GetQuestionNodeOfGame(1)).MustHaveHappened();
            var nodesResponse = result.Value as IEnumerable<QuestionNodeResponse>;
            Check.That(nodesResponse.Extracting("NodeId")).ContainsExactly(questionNodes.Extracting("Id"));
            Check.That(nodesResponse.Extracting("Question")).ContainsExactly(questionNodes.Extracting("Question"));
            Check.That(nodesResponse.First().Answers.Extracting("Id")).ContainsExactly(1, 2, 3);
        }

        [Fact]
        public void DeleteGame()
        {
            // Arrange

            // Act
            _target.DeleteGame(1);
            // Assert
            A.CallTo(() => _gameService.DeleteGame(1)).MustHaveHappened();
        }

        [Fact]
        public async Task GetGameActions()
        {
            // Arrange
            var gameActionListRequest = new GameActionListRequest()
            {
                GameId = 1,
                PageSize = 10,
                PageIndex = 0,
            };
            // Act
            var result = await _target.GetGameActions(gameActionListRequest) as OkObjectResult;
            // Assert
            A.CallTo(() => _actionService.GetGameActionsForGame(gameActionListRequest.GameId, gameActionListRequest.PageIndex, gameActionListRequest.PageSize)).MustHaveHappened();
            Check.That(result).IsNotNull();
        }
        [Fact]
        public async Task GetGameActionsWithNoImage()
        {
            // Arrange
            var gameActionListRequest = new GameActionListRequest()
            {
                GameId = 1,
                PageSize = 10,
                PageIndex = 0,
            };
            var gameActions = new List<GameAction>()
        {
            new GameAction(),
            new GameAction()
        };
            A.CallTo(() => _actionService.GetGameActionsForGame(A<int>._, A<int>._, A<int>._))
                .Returns(Task.FromResult(new PaginatedList<GameAction>(gameActions, 2, 10, 1)));
            // Act
            var result = await _target.GetGameActions(gameActionListRequest) as OkObjectResult;
            // Assert
            A.CallTo(() => _actionService.GetGameActionsForGame(gameActionListRequest.GameId, gameActionListRequest.PageIndex, gameActionListRequest.PageSize)).MustHaveHappened();
            Check.That(result).IsNotNull();
        }

        [Fact]
        public void UploadImage()
        {
            // Arrange
            var image = new byte[10];
            var file = A.Fake<IFormFile>();

            // Act
            var result = _target.UploadImage(file);
            // Assert
            Check.That(result).IsInstanceOf<CreatedAtActionResult>();
            A.CallTo(() => _imageService.AddPicture(A<Picture>._)).MustHaveHappened();
        }
        [Fact]
        public void UploadImageWithoutGeoTag()
        {
            // Arrange
            var image = new byte[10];
            var file = A.Fake<IFormFile>();

            A.CallTo(() => _imageService.ExtractLocationFromImage(A<Picture>._))
              .Returns((Double.NaN, Double.NaN));
            // Act
            var result = _target.UploadImage(file);
            // Assert
            Check.That(result).IsInstanceOf<BadRequestResult>();
            A.CallTo(() => _imageService.ExtractLocationFromImage(A<Picture>._)).MustHaveHappened();
        }
        [Fact]
        public void UploadImageImageNull()
        {
            // Arrange
            // Act
            var result = _target.UploadImage(null);
            // Assert
            Check.That(result).IsInstanceOf<BadRequestObjectResult>();
        }

        [Fact]
        public void GetGameAction()
        {
            // Arrange

            // Act
            var result = _target.GetGameAction(1) as OkObjectResult;
            // Assert
            A.CallTo(() => _actionService.GetGameAction(1)).MustHaveHappened();
        }

        [Fact]
        public void GetGameActionCountForGame()
        {
            // Arrange
            A.CallTo(() => _actionService.GetGameActionCountForGame(A<int>._)).Returns(12);
            // Act
            var result = _target.GetGameActionCountForGame(1) as OkObjectResult;
            // Assert
            Check.That(result.Value).Equals(12);
        }

        [Fact]
        public void GetImagesForGame()
        {
            // Arrange

            // Act
            var result = _target.GetImagesForGame(1);
            // Assert
            A.CallTo(() => _gameService.GetPictureNode(1)).MustHaveHappened();
        }
        [Fact]
        public void GetImagesForGame_Error()
        {
            // Arrange
            A.CallTo(() => _gameService.GetPictureNode(1)).Throws<InvalidOperationException>();

            // Act
            var result = _target.GetImagesForGame(1);
            // Assert
            A.CallTo(() => _gameService.GetPictureNode(1)).MustHaveHappened();
            Check.That(result).IsInstanceOf<BadRequestObjectResult>();
        }
        [Fact]
        public void GetGamesReviewed()
        {
            // Arrange

            // Act
            var result = _target.GetGamesReviewed();
            // Assert
            Check.That(result).IsInstanceOf<OkObjectResult>();
            A.CallTo(() => _gameService.GetGamesWithScore()).MustHaveHappened();
        }

        [Fact]
        public void GetScoreForGame()
        {
            // Arrange
            var scores = new List<Score>
            {
                new Score(){Team = new Team()}
            };
            A.CallTo(() => _actionService.GetScoresForGame(1)).Returns(scores);
            // Act
            var result = _target.GetScoreForGame(1);
            // Assert
            Check.That(result).IsInstanceOf<OkObjectResult>();
            A.CallTo(() => _actionService.GetScoresForGame(1)).MustHaveHappened();
            var response = (result as OkObjectResult).Value;
            Check.That(response).IsInstanceOf<List<ScoreResponse>>();
        }

        [Fact]
        public void GetPictureNode()
        {
            // Arrange

            // Act
            _target.GetPictureNodes(1);
            // Assert
            A.CallTo(() => _gameService.GetPictureNode(1)).MustHaveHappened();
        }


        [Fact]
        public async Task GetGameActionsToValidate()
        {
            // Arrange
            var nodes = new List<Node>()
            {
                new PictureNode() {Id = 1, Latitude = 40.0, Longitude = 5 },
                new PictureNode() {Id = 2, Latitude = 40.00005, Longitude = 5.00004 } ,
                new ObjectNode() {Id = 9, Latitude = 40.00005, Longitude = 5.00004},
                new PictureNode() {Id = 2, Latitude = 40.0001, Longitude = 5.004 } ,
                new PictureNode() {Id = 2, Latitude = 40.05, Longitude = 5.04 } ,
                new PictureNode() {Id = 2, Latitude = 40.00005, Longitude = 5.00004 } ,
            };
            //A.CallTo(() => _nodeService.GetNode())
            var gameActions = new List<GameAction>
          {
              new GameAction() {Latitude = 40, Longitude = 5},
              new GameAction() {Latitude = 50, Longitude = 5},
          };
            A.CallTo(() => _actionService.GetGameActionsForGame(A<int>._, A<int>._, A<int>._))
                .Returns(Task.FromResult(new PaginatedList<GameAction>(gameActions, 2, 1, 10)));
            A.CallTo(() => _nodeService.GetGameNodesOrderByPosition(1, 40, 5)).Returns(nodes);
            var gameActionListRequest = new GameActionListRequest(){GameId = 1, PageIndex = 1, PageSize = 1, NbPotential = 3};
            // Act
            var result = await _target.GetGameActionsToValidate(gameActionListRequest) as OkObjectResult;
            // Assert
            Check.That(result.Value).IsInstanceOf<List<GameActionToValidate>>();
            var gameActionToValidate = result.Value as List<GameActionToValidate>;
            Check.That(gameActionToValidate[0].ProbableNodes).ContainsExactly(nodes.Take(3));
            Check.That(gameActionToValidate[0].ProbableNodes.Extracting("Image")).Contains(null);
            A.CallTo(() => _actionService.GetGameActionsForGame(A<int>._, A<int>._, A<int>._)).MustHaveHappened();
            Check.That(gameActionToValidate.Extracting("Node")).IsNotNull();
        }
    } 
}
