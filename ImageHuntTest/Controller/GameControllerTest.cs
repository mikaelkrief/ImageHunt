using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using FakeItEasy;
using ImageHunt.Computation;
using ImageHunt.Controllers;
using ImageHunt.Helpers;
using ImageHunt.Services;
using ImageHuntCore.Model;
using ImageHuntCore.Model.Node;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NFluent;
using TestUtilities;
using Xunit;

namespace ImageHuntTest.Controller
{
    [Collection("AutomapperFixture")]
    public class GameControllerTest : BaseTest<GameController>
    {
        private IGameService _gameService;
        private INodeService _nodeService;
        private IImageService _imageService;
        private IActionService _actionService;
        private ILogger<GameController> _logger;
        private IImageTransformation _imageTransformation;
        private IMapper _mapper;
        private UserManager<Identity> _userManager;
        private IAdminService _adminService;

        public GameControllerTest()
        {
            _testContainerBuilder.RegisterInstance(_gameService = A.Fake<IGameService>());
            _testContainerBuilder.RegisterInstance(_nodeService = A.Fake<INodeService>());
            _testContainerBuilder.RegisterInstance(_imageService = A.Fake<IImageService>());
            _testContainerBuilder.RegisterInstance(_actionService = A.Fake<IActionService>());
            _testContainerBuilder.RegisterInstance(_adminService = A.Fake<IAdminService>());
            _testContainerBuilder.RegisterInstance(_logger = A.Fake<ILogger<GameController>>());
            _testContainerBuilder.RegisterInstance(_imageTransformation = A.Fake<IImageTransformation>());
            _testContainerBuilder.RegisterInstance(_mapper = Mapper.Instance);
            _testContainerBuilder.RegisterInstance(_userManager = A.Fake<UserManager<Identity>>());
            //_target = new GameController(_gameService, _imageService, _nodeService, _actionService, _logger, _imageTransformation, _mapper);
            Build();
            var identities = new List<Identity> {new Identity() {Id = "15", AppUserId = 15}};
            A.CallTo(() => _userManager.Users).Returns(identities.AsQueryable());
            _target.ControllerContext = new ControllerContext() {HttpContext = new DefaultHttpContext()};
            _target.User.AddIdentity(new ClaimsIdentity(new[]
                {new Claim(new ClaimsIdentityOptions().UserIdClaimType, "15"),}));
        }

        [Fact]
        public void GetAllGame()
        {
            // Arrange

            // Act
            var response = _target.GetAllGame();
            // Assert
            A.CallTo(() => _gameService.GetAllGame()).MustHaveHappened();
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
            var result = _target.GetGames() as OkObjectResult;
            // Assert
            A.CallTo(() => _gameService.GetGamesForAdmin(A<int>._)).MustHaveHappened();
            Check.That(result).IsNotNull();
        }

        [Fact]
        public async Task CreateGame()
        {
            // Arrange
            var gameRequest = new GameRequest();
            // Act
            var result = await _target.CreateGame(gameRequest);
            // Assert
            A.CallTo(() => _gameService.CreateGame(15, A<Game>.That.Matches(g => CheckCreateGame(g, false))))
                .MustHaveHappened();
        }

        private bool CheckCreateGame(Game game, bool havePicture)
        {
            if (havePicture)
                Check.That(game.Picture).IsNotNull();
            else
                Check.That(game.Picture).IsNull();
            return true;
        }

        [Fact]
        public async Task CreateGame_WithPicture()
        {
            // Arrange
            var gameRequest = new GameRequest() {PictureId = 15};
            // Act
            var result = await _target.CreateGame(gameRequest);
            // Assert
            A.CallTo(() => _gameService.CreateGame(15, A<Game>.That.Matches(g => CheckCreateGame(g, true))))
                .MustHaveHappened();
        }

        [Fact]
        public void AddNodeTimerNode()
        {
            // Arrange
            var node = new AddNodeRequest() {NodeType = "TimerNode", Duration = 1561};
            // Act
            var result = _target.AddNode(1, node);
            // Assert
            A.CallTo(() => _gameService.AddNode(1, A<Node>.That.Matches(n => CheckTimerNode(n, node.Duration))))
                .MustHaveHappened();
        }

        [Fact]
        public void AddNodeFirstNode()
        {
            // Arrange
            var node = new AddNodeRequest() {NodeType = "FirstNode", Password = "toto"};
            // Act
            var result = _target.AddNode(1, node);
            // Assert
            A.CallTo(() => _gameService.AddNode(1, A<Node>.That.Matches(n => CheckFirstNode(n, node.Password))))
                .MustHaveHappened();
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
            var node = new AddNodeRequest() {NodeType = "ObjectNode", Action = "Selfie", Points = 15};
            // Act
            var result = _target.AddNode(1, node);
            // Assert
            A.CallTo(() => _gameService.AddNode(1, A<Node>.That.Matches(n => CheckObjectNode(n, node.Action))))
                .MustHaveHappened();
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
            var node = new AddNodeRequest()
            {
                NodeType = NodeResponse.ChoiceNodeType,
                Question = "Selfie",
                Choices = new AnswerRequest[]
                    {new AnswerRequest() {Response = "Toto"}, new AnswerRequest() {Response = "Tata"}}
            };
            // Act
            var result = _target.AddNode(1, node);
            // Assert
            A.CallTo(() =>
                    _gameService.AddNode(1,
                        A<Node>.That.Matches(n => CheckQuestionNode(n, node.Question, node.Choices))))
                .MustHaveHappened();
        }

        private bool CheckQuestionNode(Node node, string expectedQuestion, AnswerRequest[] nodeAnswers)
        {
            var questionNode = node as ChoiceNode;
            Check.That(questionNode.Choice).Equals(expectedQuestion);
            Check.That(questionNode.Answers.Extracting("Response")).ContainsExactly(nodeAnswers.Extracting("Response"));
            return true;
        }

        [Fact]
        public void GetHiddenNodes()
        {
            // Arrange
            var nodes = new List<Node> {new HiddenNode(), new BonusNode()};
            // Act

            // Assert
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
                    ChildrenRelation = new List<ParentChildren>()
                    {
                        new ParentChildren()
                        {
                            Children = new ChoiceNode()
                            {
                                Id = 2,
                                ChildrenRelation = new List<ParentChildren>()
                                {
                                    new ParentChildren() {Children = new FirstNode() {Id = 3}}
                                }
                            }
                        }
                    }
                }
            };
            A.CallTo(() => _gameService.GetNodes(1, NodeTypes.All)).Returns(nodes);
            // Act
            var result = _target.GetNodesRelations(1) as OkObjectResult;
            // Assert
            Check.That(result).IsNotNull();
            A.CallTo(() => _gameService.GetNodes(1, NodeTypes.All)).MustHaveHappened();
            var resNodes = result.Value as List<NodeResponse>;
            // Check that only first level nodes are populated
            Check.That(resNodes).HasSize(1);
            Check.That(resNodes[0].Id).Equals(nodes[0].Id);
            Check.That(resNodes[0].ChildNodeIds.First()).Equals(nodes[0].Children[0].Id);
        }

        [Fact]
        public void AddImagesNodes()
        {
            // Arrange
            var picture = GetImageFromResource(Assembly.GetExecutingAssembly(),
                "ImageHuntTest.TestData.IMG_20170920_180905.jpg");
            var file = A.Fake<IFormFile>();
            A.CallTo(() => file.OpenReadStream()).ReturnsNextFromSequence(new MemoryStream(picture),
                new MemoryStream(picture), new MemoryStream(picture));
            var images = new List<IFormFile>() {file, file, file};
            // Act
            _target.AddImageNodes(1, images);
            // Assert
            A.CallTo(() => _imageService.AddPicture(A<Picture>._)).MustHaveHappened();
            A.CallTo(() => _gameService.AddNode(1, A<Node>.That.Matches(n => CheckImageNode(n))))
                .MustHaveHappened(Repeated.Exactly.Times(3));
        }

        private bool CheckImageNode(Node node)
        {
            Check.That(node.Name).IsNullOrEmpty();
            return true;
        }

        [Fact]
        public void AddImagesNodes_without_Geotag()
        {
            // Arrange
            var picture = GetImageFromResource(Assembly.GetExecutingAssembly(),
                "ImageHuntTest.TestData.IMG_20170920_180905.jpg");
            var file = A.Fake<IFormFile>();
            A.CallTo(() => file.OpenReadStream()).ReturnsNextFromSequence(new MemoryStream(picture),
                new MemoryStream(picture), new MemoryStream(picture));
            A.CallTo(() => _imageService.ExtractLocationFromImage(A<Picture>._)).Returns((double.NaN, double.NaN));
            var images = new List<IFormFile>() {file, file, file};
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
            var questionNodes = new List<ChoiceNode>()
            {
                new ChoiceNode()
                {
                    Id = 1,
                    Choice = "What",
                    Answers = new List<Answer>()
                    {
                        new Answer()
                        {
                            Id = 1,
                            Node = new PictureNode() {Id = 101},
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
                            Node = new PictureNode() {Id = 102},
                            Response = "C"
                        },
                    }
                },
                new ChoiceNode()
                {
                    Id = 2,
                    Choice = "Who",
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
                            Node = new PictureNode() {Id = 103},
                            Response = "3"
                        },
                    }
                }
            };
            A.CallTo(() => _gameService.GetChoiceNodeOfGame(A<int>._)).Returns(questionNodes);
            // Act
            var result = _target.GetChoiceNodeOfGame(1) as OkObjectResult;
            // Assert
            A.CallTo(() => _gameService.GetChoiceNodeOfGame(1)).MustHaveHappened();
            var nodesResponse = result.Value as IEnumerable<NodeResponse>;
            Check.That(nodesResponse.Extracting("Id")).ContainsExactly(questionNodes.Extracting("Id"));
            //Check.That(nodesResponse.Extracting("Question")).ContainsExactly(questionNodes.Extracting("Question"));
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
                new Score() {Team = new Team()}
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
        public void Should_return_Game_Code()
        {
            // Arrange

            // Act
            var result = _target.GetGameCode(1);
            // Assert
            Check.That(result).IsInstanceOf<OkObjectResult>();
            A.CallTo(() => _gameService.GameCode(A<int>._)).MustHaveHappened();
        }

        [Fact]
        public void Should_Import_kml_file()
        {
            // Arrange
            var kmlFile = GetStringFromResource(Assembly.GetExecutingAssembly(), "ImageHuntTest.TestData.Parcours.kml");
            var file = A.Fake<IFormFile>();
            using (var stream = new MemoryStream())
            {
                using (var writter = new StreamWriter(stream))
                {
                    writter.Write(kmlFile);
                    writter.Flush();
                    stream.Seek(0, SeekOrigin.Begin);
                    var expectedNodeCount = 35;

                    A.CallTo(() => file.OpenReadStream()).ReturnsNextFromSequence(stream);

                    // Act
                    _target.ImportKmlFile(1, false, file);
                    // Assert
                    A.CallTo(() => _gameService.AddNode(A<int>._, A<FirstNode>._))
                        .MustHaveHappened(Repeated.Exactly.Once);
                    A.CallTo(() => _gameService.AddNode(A<int>._, A<Node>._))
                        .MustHaveHappened(Repeated.Exactly.Times(expectedNodeCount));
                    A.CallTo(() => _gameService.AddNode(A<int>._, A<LastNode>._))
                        .MustHaveHappened(Repeated.Exactly.Once);
                    A.CallTo(() => _nodeService.AddChildren(A<Node>._, A<Node>._))
                        .MustHaveHappened(Repeated.Exactly.Times(expectedNodeCount - 1));
                }
            }
        }
        [Fact]
        public void Should_Import_non_closed_kml_file()
        {
            // Arrange
            var kmlFile = GetStringFromResource(Assembly.GetExecutingAssembly(), "ImageHuntTest.TestData.non_closed.kml");
            var file = A.Fake<IFormFile>();
            using (var stream = new MemoryStream())
            {
                using (var writter = new StreamWriter(stream))
                {
                    writter.Write(kmlFile);
                    writter.Flush();
                    stream.Seek(0, SeekOrigin.Begin);
                    var expectedNodeCount = 28;

                    A.CallTo(() => file.OpenReadStream()).ReturnsNextFromSequence(stream);

                    // Act
                    _target.ImportKmlFile(1, false, file);
                    // Assert
                    A.CallTo(() => _gameService.AddNode(A<int>._, A<FirstNode>._))
                        .MustHaveHappened(Repeated.Exactly.Once);
                    A.CallTo(() => _gameService.AddNode(A<int>._, A<Node>._))
                        .MustHaveHappened(Repeated.Exactly.Times(expectedNodeCount));
                    A.CallTo(() => _gameService.AddNode(A<int>._, A<LastNode>._))
                        .MustHaveHappened(Repeated.Exactly.Once);
                    A.CallTo(() => _nodeService.AddChildren(A<Node>._, A<Node>._))
                        .MustHaveHappened(Repeated.Exactly.Times(expectedNodeCount - 1));
                }
            }
        }

        [Fact]
        public void Should_return_Nodes_close_to()
        {
            // Arrange
            NodeRequest nodeRequest = new NodeRequest()
            {
                GameId = 1,
                Longitude = 1,
                Latitude = 1,
                NodeType = NodeTypes.Path.ToString()
            };

            // Act
            var result = _target.GetPathNodesCloseTo(nodeRequest);
            // Assert
            Check.That(result).IsInstanceOf<OkObjectResult>();
            A.CallTo(() =>
                _nodeService.GetGameNodesOrderByPosition(1, nodeRequest.Latitude, nodeRequest.Longitude,
                    NodeTypes.Path)).MustHaveHappened();
        }

        [Fact]
        public void Should_duplicate_Game_Succeed()
        {
            // Arrange
            var duplicateGameRequest = new DuplicateGameRequest()
            {
                GameId = 15,
            };
            var nodes = new List<Node>
            {
                new FirstNode() {Id = 1},
                new ObjectNode() {Id = 2},
                new TimerNode() {Id = 3},
                new QuestionNode() {Id = 4},
                new LastNode() {Id = 5}
            };
            nodes[0].HaveChild(nodes[1]);
            nodes[1].HaveChild(nodes[2]);
            nodes[2].HaveChild(nodes[3]);
            nodes[3].HaveChild(nodes[4]);
            var orgGame = new Game() {Nodes = nodes, Picture = new Picture() {Id = 56}};
            A.CallTo(() => _gameService.GetGameById(duplicateGameRequest.GameId)).Returns(orgGame);
            A.CallTo(() => _gameService.GetNodes(A<int>._, A<NodeTypes>._)).Returns(nodes);
            // Act
            var result = _target.DuplicateGame(duplicateGameRequest);
            // Assert
            A.CallTo(() => _gameService.Duplicate(orgGame, A<Admin>._)).MustHaveHappened();
            Check.That(result).IsInstanceOf<OkObjectResult>();
            var newGame = ((OkObjectResult) result).Value as GameResponse;
            Check.That(newGame.Name).Equals(orgGame.Name);
            Check.That(newGame.IsActive).Equals(orgGame.IsActive);
            A.CallTo(() => _gameService.AddNode(A<int>._, A<Node>._))
                .MustHaveHappenedANumberOfTimesMatching(i => i == nodes.Count);
            A.CallTo(() => _nodeService.AddChildren(A<Node>._, A<Node>._))
                .MustHaveHappenedANumberOfTimesMatching(i => i == 4);
        }

        [Fact]
        public void Should_duplicate_Game_Fail_if_ChoiceNode()
        {
            // Arrange
            var duplicateGameRequest = new DuplicateGameRequest()
            {
                GameId = 15,
            };
            var nodes = new List<Node>
            {
                new FirstNode() {Id = 1},
                new ObjectNode() {Id = 2},
                new ChoiceNode() {Id = 3},
                new QuestionNode() {Id = 4},
                new LastNode() {Id = 5}
            };
            nodes[0].HaveChild(nodes[1]);
            nodes[1].HaveChild(nodes[2]);
            nodes[2].HaveChild(nodes[3]);
            nodes[3].HaveChild(nodes[4]);
            var orgGame = new Game() {Nodes = nodes, Picture = new Picture() {Id = 56}};
            A.CallTo(() => _gameService.GetGameById(duplicateGameRequest.GameId)).Returns(orgGame);
            // Act
            var result = _target.DuplicateGame(duplicateGameRequest);
            // Assert
            Check.That(result).IsInstanceOf<BadRequestObjectResult>();
        }

        [Fact]
        public void Should_GetByCode_Return_Game_with_team()
        {
            // Arrange

            // Act
            var result = _target.GetGameByCode("HJGJHJ");
            // Assert
            Check.That(result).IsInstanceOf<OkObjectResult>();
            A.CallTo(() => _gameService.GetGameByCode(A<string>._)).MustHaveHappened();
        }

        [Fact]

        public void Should_GetForValidation_Returns_Games_to_validate_for_user()
        {
            // Arrange

            // Act
            var result = _target.GetForValidation();
            // Assert
            A.CallTo(() => _adminService.GetAdminById(A<int>._)).MustHaveHappened();
            A.CallTo(() => _gameService.GetAllGameForValidation(A<Admin>._)).MustHaveHappened();
        }

        [Fact]
        public void Should_ToggleGame_Activate_Game()
        {
            // Arrange
            
            // Act
            var result = _target.ToggleGame(17, "Active");
            // Assert
            A.CallTo(() => _gameService.Toogle(A<int>._, Flag.Active)).MustHaveHappened();
            Check.That(result).IsInstanceOf<OkObjectResult>();
        }

        [Fact]
        public void Should_Make_Game_Public()
        {
            // Arrange
            
            // Act
            var result = _target.ToggleGame(17, "Public");
            // Assert
            A.CallTo(() => _gameService.Toogle(A<int>._, Flag.Public)).MustHaveHappened();
            Check.That(result).IsInstanceOf<OkObjectResult>();

        }
    }
}