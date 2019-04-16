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
            var container = TestContainerBuilder.Build();
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
            Context.Nodes.AddRange(nodes);
            var games = new List<Game> {new Game() {Nodes = nodes}};
            Context.Games.AddRange(games);
            Context.SaveChanges();
            _target = new UpdateNodePointsUpdater(Context, games[0], @"--seedPattern=\d*_(?'seed'\d)\.jpg --nodeType=PictureNode --multiplier=10");
            // Act
            _target.Execute();
            // Assert
            Check.That(Context.Nodes.Extracting("Points")).ContainsExactly(10, 20, 30, 50, 40, 10, 20, 30, 50, 0, 0);
        }
    }
}
