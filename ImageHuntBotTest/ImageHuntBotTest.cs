using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using FakeItEasy;
using ImageHuntBotBuilder;
using Microsoft.Bot.Builder;
using Microsoft.Extensions.Logging;
using TestUtilities;
using Xunit;

namespace ImageHuntBotTest
{
    public class ImageHuntBotTest : BaseTest
    {
        private ImageHuntBotBuilder.ImageHuntBot _target;
        private ILogger<ImageHuntBotBuilder.ImageHuntBot> _logger;
        private IStorage _storage;

        public ImageHuntBotTest()
        {
            _testContainerBuilder.RegisterType<ImageHuntBotBuilder.ImageHuntBot>();
            _logger = A.Fake<ILogger<ImageHuntBotBuilder.ImageHuntBot>>();
            _testContainerBuilder.RegisterInstance(_logger);
            _storage = A.Fake<IStorage>();
            _testContainerBuilder.RegisterInstance(_storage);
            _testContainerBuilder.RegisterType<ConversationState>();
            _testContainerBuilder.RegisterType<ImageHuntBotAccessors>();
            var container = _testContainerBuilder.Build();
            _target = container.Resolve<ImageHuntBotBuilder.ImageHuntBot>();
        }

        [Fact]
        public void Should_Turn_Log_position()
        {
            // Arrange
            
            // Act

            // Assert
        }
    }
}
