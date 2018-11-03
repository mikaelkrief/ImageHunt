using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using FakeItEasy;
using ImageHunt;
using ImageHuntBotBuilder;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using TestUtilities;
using Xunit;

namespace ImageHuntBotBuilderTest
{
    public class ImageHuntBotTest : BaseTest<ImageHuntBot>
    {
        private ILogger<ImageHuntBot> _logger;
        private ImageHuntBotAccessors _accessor;
        private ITurnContext _turnContext;

        public ImageHuntBotTest()
        {
            Startup.ConfigureMappings();
            _logger = A.Fake<ILogger<ImageHuntBot>>();
            _accessor = A.Fake<ImageHuntBotAccessors>();
            _testContainerBuilder.RegisterInstance(_logger);
            _turnContext = A.Fake<ITurnContext>();
            Build();
        }

        [Fact]
        public void Should_Turn_Record_Position()
        {
            // Arrange
            //var activity = new Activity(type:"location", attachments:new )
            // Act

            // Assert
        }
    }
}
