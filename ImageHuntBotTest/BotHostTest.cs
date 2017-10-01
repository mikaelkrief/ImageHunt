using ImageHuntBot;
using NFluent;
using Xunit;

namespace ImageHuntBotTest
{
    public class BotHostTest  
    {
        [Fact]
        public void CreateDefaultBuilder()
        {
          // Arrange

          // Act
          var botHostBuilder = BotHost.CreateDefaultBuilder(null);
          // Assert
          Check.That(botHostBuilder).IsInstanceOf<BotHostBuilder>();
        }
    }
}
