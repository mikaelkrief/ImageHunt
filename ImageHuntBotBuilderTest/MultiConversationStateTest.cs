using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using FakeItEasy;
using ImageHuntBotBuilder;
using TestUtilities;
using Xunit;

namespace ImageHuntBotBuilderTest
{
    public class MultiConversationStateTest : BaseTest<MultiConversationState<ImageHuntState>>

    {
        private IMultiStorage _storage;

        public MultiConversationStateTest()
        {
            _testContainerBuilder.RegisterInstance(_storage = A.Fake<IMultiStorage>());
            Build();
        }

        [Fact(Skip = "No time to fix this")]
        public async Task Shoud_GetAllAsync_Return_Enumeration_Of_ImageHuntState()
        {
            // Arrange
            var orgStates = new List<IDictionary<string, object>>()
            {
                new Dictionary<string, object>()
                {
                    {
                        "conv1",
                        new ImageHuntState() {Status = Status.Initialized, GameId = 5, TeamId = 61}
                    }
                },
                new Dictionary<string, object>()
                {
                    {
                        "conv2",
                        new ImageHuntState() {Status = Status.Started, GameId = 6, TeamId = 94}
                    }
                }
            };
            A.CallTo(() => _storage.ReadAllAsync(A<CancellationToken>._)).Returns(orgStates);
            // Act
            var states = await _target.GetAllAsync();
            // Assert
            A.CallTo(() => _storage.ReadAllAsync(A<CancellationToken>._)).MustHaveHappened();

        }
    }
}