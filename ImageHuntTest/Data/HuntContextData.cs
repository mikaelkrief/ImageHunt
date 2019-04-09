using System.Collections.Generic;
using ImageHunt.Data;
using ImageHuntCore.Model;
using Microsoft.EntityFrameworkCore;
using NFluent;
using TestUtilities;
using Xunit;

namespace ImageHuntTest.Data
{
    public class HuntContextTest : ContextBasedTest<HuntContext>
    {

        [Fact]
        public void SoftDeleteTest()
        {
            // Arrange
            var players = new List<Player>() { new Player(), new Player(), new Player(), new Player()};
            Context.Players.AddRange(players);
            Context.SaveChanges();
            // Act
            Context.Players.Remove(players[2]);
            Context.SaveChanges();
            // Assert
            long countPlayerActive, countAllPlayers;
            var connection = Context.Database.GetDbConnection();
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT count(*) FROM Players";
                countAllPlayers = (long) command.ExecuteScalar();
                command.CommandText = "SELECT count(*) FROM Players where [IsDeleted] = 0";
                countPlayerActive = (long) command.ExecuteScalar();
            }
            Check.That(countAllPlayers).IsEqualTo(4);
            Check.That(countPlayerActive).IsEqualTo(3);
            Check.That(Context.Players).HasSize(3);
        }
    }
}
