using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Core.Logging;
using ImageHunt.Model;
using ImageHunt.Services;
using Microsoft.Extensions.Logging;
using NFluent;
using TestUtilities;
using Xunit;

namespace ImageHuntTest.Services
{
    public class PasscodeServiceTest : ContextBasedTest
    {
        private ILogger<PasscodeService> _logger;
        private PasscodeService _target;

        public PasscodeServiceTest()
        {
            _target = new PasscodeService(_context, _logger);
        }

        [Fact]
        public void GetAll()
        {
            // Arrange
            var passcodes = new List<Passcode>
            {
                new Passcode(),
                new Passcode(),
                new Passcode(),
                new Passcode(),
                new Passcode(),
            };
            _context.Passcodes.AddRange(passcodes);
            var games = new List<Game>()
            {
                new Game() {Passcodes = new List<Passcode>(){passcodes[0], passcodes[2]}},
                new Game() {Passcodes = new List<Passcode>(){passcodes[1], passcodes[3], passcodes[4]}},
            };
            _context.Games.AddRange(games);
            _context.SaveChanges();
            // Act
            var result = _target.GetAll(games[1].Id);
            // Assert
            Check.That(result).Contains(passcodes[1], passcodes[3], passcodes[4]);
        }

        [Fact]
        public void Delete()
        {
            // Arrange
            var passcodes = new List<Passcode>
            {
                new Passcode(),
                new Passcode(),
                new Passcode(),
                new Passcode(),
                new Passcode(),
            };
            _context.Passcodes.AddRange(passcodes);
            var games = new List<Game>()
            {
                new Game() {Passcodes = new List<Passcode>(){passcodes[0], passcodes[2]}},
                new Game() {Passcodes = new List<Passcode>(){passcodes[1], passcodes[3], passcodes[4]}},
            };
            _context.Games.AddRange(games);
            _context.SaveChanges();
            // Act
            _target.Delete(games[1].Id, games[1].Passcodes[1].Id);
            // Assert
            Check.That(games[1].Passcodes).ContainsExactly(passcodes[1], passcodes[4]);
        }
        [Fact]
        public void Redeem()
        {
            // Arrange
            var passcodes = new List<Passcode>
            {
                new Passcode(){Pass="a", NbRedeem = 1, Points = 10},
                new Passcode(){Pass="b", NbRedeem = 2, Points = 20},
                new Passcode(){Pass="c", NbRedeem = 3, Points = 30},
                new Passcode(){Pass="d", NbRedeem = 4, Points = 40},
                new Passcode(){Pass="e", NbRedeem = 5, Points = 50},
            };
            _context.Passcodes.AddRange(passcodes);
            var teams = new List<Team>()
            {
                new Team(),
                new Team(),
                new Team(),
            };
            _context.Teams.AddRange(teams);
            var games = new List<Game>()
            {
                new Game() {Passcodes = new List<Passcode>(){passcodes[0], passcodes[2]}},
                new Game() {Passcodes = new List<Passcode>(){passcodes[1], passcodes[3], passcodes[4]}, Teams = teams},
            };
            _context.Games.AddRange(games);
            _context.SaveChanges();
            // Act
            _target.Redeem(games[1].Id, teams[1].Id, "b");
            // Assert
            Check.That(_context.GameActions.First().PointsEarned).IsEqualTo(20);
            Check.That(passcodes[1].NbRedeem).Equals(1);
            Check.That(teams[1].Passcodes).Contains(passcodes[1]);
        }
        [Fact]
        public void Redeem_infinite_code()
        {
            // Arrange
            var passcodes = new List<Passcode>
            {
                new Passcode(){Pass="a", NbRedeem = 1, Points = 10},
                new Passcode(){Pass="b", NbRedeem = -1, Points = 20},
                new Passcode(){Pass="c", NbRedeem = 3, Points = 30},
                new Passcode(){Pass="d", NbRedeem = 4, Points = 40},
                new Passcode(){Pass="e", NbRedeem = 5, Points = 50},
            };
            _context.Passcodes.AddRange(passcodes);
            var teams = new List<Team>()
            {
                new Team(),
                new Team(),
                new Team(),
            };
            _context.Teams.AddRange(teams);
            var games = new List<Game>()
            {
                new Game() {Passcodes = new List<Passcode>(){passcodes[0], passcodes[2]}},
                new Game() {Passcodes = new List<Passcode>(){passcodes[1], passcodes[3], passcodes[4]}, Teams = teams},
            };
            _context.Games.AddRange(games);
            _context.SaveChanges();
            // Act
            _target.Redeem(games[1].Id, teams[1].Id, "b");
            // Assert
            Check.That(_context.GameActions.First().PointsEarned).IsEqualTo(20);
            Check.That(passcodes[1].NbRedeem).Equals(-1);
            Check.That(teams[1].Passcodes).Contains(passcodes[1]);
        }
        [Fact]
        public void Redeem_AlreadyRedeem()
        {
            // Arrange
            var passcodes = new List<Passcode>
            {
                new Passcode(){Pass="a", NbRedeem = 1, Points = 10},
                new Passcode(){Pass="b", NbRedeem = 2, Points = 20},
                new Passcode(){Pass="c", NbRedeem = 3, Points = 30},
                new Passcode(){Pass="d", NbRedeem = 4, Points = 40},
                new Passcode(){Pass="e", NbRedeem = 5, Points = 50},
            };
            _context.Passcodes.AddRange(passcodes);
            var teams = new List<Team>()
            {
                new Team(),
                new Team(),
                new Team(),
            };
            teams[1].TeamPasscodes.Add(new TeamPasscode(){Passcode = passcodes[1], Team = teams[1]});
            _context.Teams.AddRange(teams);
            var games = new List<Game>()
            {
                new Game() {Passcodes = new List<Passcode>(){passcodes[0], passcodes[2]}},
                new Game() {Passcodes = new List<Passcode>(){passcodes[1], passcodes[3], passcodes[4]}, Teams = teams},
            };
            _context.Games.AddRange(games);
            _context.SaveChanges();
            // Act
            var result = _target.Redeem(games[1].Id, teams[1].Id, "b");
            // Assert
            Check.That(result).Equals(RedeemStatus.AlreadyRedeem );
        }
        [Fact]
        public void Redeem_CodeFullyRedeem()
        {
            // Arrange
            var passcodes = new List<Passcode>
            {
                new Passcode(){Pass="a", NbRedeem = 1, Points = 10},
                new Passcode(){Pass="b", NbRedeem = 0, Points = 20},
                new Passcode(){Pass="c", NbRedeem = 3, Points = 30},
                new Passcode(){Pass="d", NbRedeem = 4, Points = 40},
                new Passcode(){Pass="e", NbRedeem = 5, Points = 50},
            };
            _context.Passcodes.AddRange(passcodes);
            var teams = new List<Team>()
            {
                new Team(),
                new Team(),
                new Team(),
            };
            _context.Teams.AddRange(teams);
            var games = new List<Game>()
            {
                new Game() {Passcodes = new List<Passcode>(){passcodes[0], passcodes[2]}},
                new Game() {Passcodes = new List<Passcode>(){passcodes[1], passcodes[3], passcodes[4]}, Teams = teams},
            };
            _context.Games.AddRange(games);
            _context.SaveChanges();
            // Act
            var result = _target.Redeem(games[1].Id, teams[1].Id, "b");
            // Assert
            Check.That(result).IsEqualTo(RedeemStatus.FullyRedeem);
        }
        [Fact]
        public void Redeem_CodeNotExist()
        {
            // Arrange
            var passcodes = new List<Passcode>
            {
                new Passcode(){Pass="a", NbRedeem = 1, Points = 10},
                new Passcode(){Pass="b", NbRedeem = 2, Points = 20},
                new Passcode(){Pass="c", NbRedeem = 3, Points = 30},
                new Passcode(){Pass="d", NbRedeem = 4, Points = 40},
                new Passcode(){Pass="e", NbRedeem = 5, Points = 50},
            };
            _context.Passcodes.AddRange(passcodes);
            var teams = new List<Team>()
            {
                new Team(),
                new Team(),
                new Team(),
            };
            _context.Teams.AddRange(teams);
            var games = new List<Game>()
            {
                new Game() {Passcodes = new List<Passcode>(){passcodes[0], passcodes[2]}},
                new Game() {Passcodes = new List<Passcode>(){passcodes[1], passcodes[3], passcodes[4]}, Teams = teams},
            };
            _context.Games.AddRange(games);
            _context.SaveChanges();
            // Act
            var result = _target.Redeem(games[1].Id, teams[1].Id, "bsdsd");
            // Assert
            Check.That(result).Equals(RedeemStatus.WrongCode);
        }

        [Fact]
        public void Add()
        {
            // Arrange
            var passcodes = new List<Passcode>
            {
                new Passcode(){Pass="a", NbRedeem = 1, Points = 10},
                new Passcode(){Pass="b", NbRedeem = 2, Points = 20},
                new Passcode(){Pass="c", NbRedeem = 3, Points = 30},
                new Passcode(){Pass="d", NbRedeem = 4, Points = 40},
                new Passcode(){Pass="e", NbRedeem = 5, Points = 50},
            };
            _context.Passcodes.AddRange(passcodes);
            var games = new List<Game>()
            {
                new Game() {Passcodes = new List<Passcode>(){passcodes[0], passcodes[2]}},
                new Game() {Passcodes = new List<Passcode>(){passcodes[1], passcodes[3], passcodes[4]}},
            };
            _context.Games.AddRange(games);
            _context.SaveChanges();
            var passcode = new Passcode(){Pass="Z"};
            // Act
            var result = _target.Add(games[1].Id, passcode);
            // Assert
            Check.That(result.Id).IsNotEqualTo(0);
            Check.That(games[1].Passcodes).Contains(passcode);
            Check.That(_context.Passcodes.Any(p => p.Pass == passcode.Pass)).IsTrue();
        }
    }
}
