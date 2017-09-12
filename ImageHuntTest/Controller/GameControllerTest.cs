using System;
using System.Collections.Generic;
using System.Text;
using FakeItEasy;
using ImageHunt.Controllers;
using ImageHunt.Services;
using Xunit;

namespace ImageHuntTest.Controller
{
    public class GameControllerTest
    {
        private IGameService _gameService;
        private GameController _target;

        public GameControllerTest()
        {
            _gameService = A.Fake<IGameService>();
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
    }
}
