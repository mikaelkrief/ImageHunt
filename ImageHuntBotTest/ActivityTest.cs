using System;
using System.Collections.Generic;
using System.Text;
using ImageHuntTelegramBot;
using NFluent;
using Xunit;

namespace ImageHuntBotTest
{
    public class ActivityTest
    {
        [Fact]
        public void ActivityText_Extract_Command()
        {
            // Arrange
            var activity = new Activity() {Text = "/start /redeem pass=GFGFG"};
            // Act
            var command = activity.Command;
            // Assert
            Check.That(command).Equals("/start");
        }

        [Fact]
        public void Activity_Extract_Payload()
        {
            // Arrange
            var activity = new Activity() { Text = "/start /redeem pass=GFGFG" };
            // Act
            var payload = activity.Payload;
            // Assert
            Check.That(payload).Equals("/redeem pass=GFGFG");
        }
    }
}
