using System.Collections.Generic;
using ImageHunt.Data;
using ImageHunt.Updater;
using ImageHuntCore.Model;
using ImageHuntCore.Model.Node;
using NFluent;
using TestUtilities;
using Xunit;

namespace ImageHuntTest.Updaters
{
    public class UpdateNodePointsUpdaterTest : ContextBasedTest<HuntContext>
    {
        private UpdateNodePointsUpdater _target;

        public UpdateNodePointsUpdaterTest()
        {
            var container = _testContainerBuilder.Build();
        }

        [Fact]
        public void Should_()
        {
            // Arrange
            var nodes = new List<Node>
            {
                new PictureNode() {Name = "15_1.jpg"},
                new PictureNode() {Name = "155_2.jpg"},
                new PictureNode() {Name = "141_3.jpg"},
                new PictureNode() {Name = "1155_5.jpg"},
                new PictureNode() {Name = "25_4.jpg"},
                new PictureNode() {Name = "95_1.jpg"},
                new PictureNode() {Name = "97_2.jpg"},
                new PictureNode() {Name = "85_3.jpg"},
                new PictureNode() {Name = "74_5.jpg"},
                new ObjectNode(),
                new TimerNode()
            };
            _context.Nodes.AddRange(nodes);
            var games = new List<Game> {new Game() {Nodes = nodes}};
            _context.Games.AddRange(games);
            _context.SaveChanges();
            _target = new UpdateNodePointsUpdater(_context, games[0], @"--seedPattern=\d*_(?'seed'\d)\.jpg --nodeType=PictureNode --multiplier=10");
            // Act
            _target.Execute();
            // Assert
            Check.That(_context.Nodes.Extracting("Points")).ContainsExactly(10, 20, 30, 50, 40, 10, 20, 30, 50, 0, 0);
        }
    }
}
