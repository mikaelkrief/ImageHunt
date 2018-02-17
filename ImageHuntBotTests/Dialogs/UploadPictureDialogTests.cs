using Xunit;
using ImageHuntBot.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using FakeItEasy;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Internals.Fibers;
using Microsoft.Bot.Connector;

namespace ImageHuntBot.Dialogs.Tests
{
  public class UploadPictureDialogTests
  {
    private UploadPictureDialog _target;

    public UploadPictureDialogTests()
    {
      _target = new UploadPictureDialog();
    }
    [Fact]
    public async Task MessageReceivedAsyncTest()
    {
      // Arrange
      var context = A.Fake<IDialogContext>();
      var argument = A.Fake<IAwaitable<IMessageActivity>>();
      var awaiter = A.Fake<IAwaiter<IMessageActivity>>();
      var activity = A.Fake<IMessageActivity>();
      A.CallTo(() => argument.GetAwaiter()).Returns(awaiter);
      A.CallTo(() => awaiter.GetResult()).Returns(activity);
      //
      await _target.MessageReceivedAsync(context, argument);
    }
  }
}