using System;
using System.Collections.Generic;
using System.Text;
using FakeItEasy;
using ImageHunt.Controllers;
using ImageHunt.Model;
using ImageHunt.Model.Node;
using ImageHunt.Services;
using Microsoft.AspNetCore.Mvc;
using NFluent;
using Xunit;

namespace ImageHuntTest.Controller
{
    public class GameControllerTest
    {
        private IGameService _gameService;
        private GameController _target;
        private INodeService _nodeService;

        public GameControllerTest()
        {
            _gameService = A.Fake<IGameService>();
            _nodeService = A.Fake<INodeService>();
            _target = new GameController(_gameService);
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
            var node = new TimerNode();
            // Act
            var result = _target.AddNode(1, node);
            // Assert
            A.CallTo(() => _gameService.AddNode(1, node)).MustHaveHappened();
        }
    }
}
