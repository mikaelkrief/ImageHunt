using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FakeItEasy;
using ImageHuntTelegramBot;
using ImageHuntTelegramBot.Dialogs;
using ImageHuntTelegramBot.Dialogs.Prompts;
using NFluent;
using TestUtilities;
using Xunit;

namespace ImageHuntBotTest
{
    public class NumberPromptTest : BaseTest
    {


      [Fact]
      public async Task ContinueGetValue()
      {
      // Arrange
        var target1 = new NumberPrompt<int>("");
        var target2 = new NumberPrompt<double>("");
      var context = A.Fake<ITurnContext>();
        var activity = new Activity(){ActivityType = ActivityType.Message, Text = "15"};
        A.CallTo(() => context.Activity).Returns(activity);
        // Act
        await target1.Continue(context);
        await target2.Continue(context);
        // Assert
        Check.That(target1.Value).Equals(15);
        Check.That(target2.Value).Equals(15.0);
      }
    }
}
