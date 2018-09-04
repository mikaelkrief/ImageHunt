using Autofac;
using ImageHuntBot.Dialogs;
using Microsoft.Extensions.Logging;
using TestUtilities;

namespace ImageHuntBotTest
{
    public class DisplayDialogTest : BaseTest
    {
        private DisplayDialog _target;

        public DisplayDialogTest()
        {
            _testContainerBuilder.RegisterType<DisplayDialog>();
            _container = _testContainerBuilder.Build();
            _target = _container.Resolve<DisplayDialog>();
        }

        //[Fact]
        //public async Task DisplayMessage()
        //{
        //  // Arrange
        //  var turnContext = A.Fake<ITurnContext>();
        //  // Act
        //  await _target.Begin(turnContext);
        //  // Assert
        //  A.CallTo(() => turnContext.ReplyActivity(A<IActivity>.That.Matches(a => a.Text == "toto")));
        //  A.CallTo(() => turnContext.End()).MustHaveHappened();
        //}
    }
}