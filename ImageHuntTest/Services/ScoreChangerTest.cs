using System.Collections.Generic;
using ImageHunt.Services;
using ImageHuntCore.Model;
using NFluent;
using TestUtilities;
using Xunit;

namespace ImageHuntTest.Services
{
    public class ScoreChangerTest : BaseTest<ScoreDecreaseByTeamMember>
    {
        public ScoreChangerTest()
        {
            Build();

        }

        [Fact]
        public void Should_Score_Changer_Compute_Points_No_Discount()
        {
            // Arrange
            var score = new Score(){Points = 15, Team = new Team(){TeamPlayers = new List<TeamPlayer> {new TeamPlayer(), new TeamPlayer()}}};
            var game = new Game(){ NbPlayerPenaltyValue = 0.05, NbPlayerPenaltyThreshold = 4}; 
            // Act
            var result = Target.ComputeScore(score, game);
            // Assert
            Check.That(result).Equals(score.Points);
        }
        [Fact]
        public void Should_Score_Changer_Compute_Points_With_Bonus()
        {
            // Arrange
            var team = new Team()
            {
                TeamPlayers = new List<TeamPlayer> {new TeamPlayer(), new TeamPlayer()},
                Bonus = 2
            };
            var score = new Score(){Points = 15, Team = team};
            var game = new Game(){ NbPlayerPenaltyValue = 0.05, NbPlayerPenaltyThreshold = 4}; 
            // Act
            var result = Target.ComputeScore(score, game);
            // Assert
            Check.That(result).Equals(score.Points * team.Bonus);
        }
        [Fact]
        public void Should_Score_Changer_Compute_Points_Discount()
        {
            // Arrange
            var score = new Score(){Points = 15, Team = new Team()
            {
                TeamPlayers = new List<TeamPlayer>
                {
                    new TeamPlayer(),
                    new TeamPlayer(),
                    new TeamPlayer(),
                    new TeamPlayer(),
                    new TeamPlayer(),
                    new TeamPlayer(),
                }
            }};
            var game = new Game() { NbPlayerPenaltyValue = 0.05, NbPlayerPenaltyThreshold = 4 };
            // Act
            var result = Target.ComputeScore(score, game);
            // Assert
            Check.That(result).Equals(13.5);
        }

    }
}
