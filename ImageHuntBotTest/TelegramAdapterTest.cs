using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using FakeItEasy;
using ImageHuntTelegramBot;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NFluent;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
using TestUtilities;
using Xunit;

namespace ImageHuntBotTest
{
    public class TelegramAdapterTest : BaseTest
    {
        private IAdapter _target;
        private ITelegramBotClient _telegramClient;
        private ILogger<TelegramAdapter> _logger;
        private IConfiguration _configuration;

        public TelegramAdapterTest()
        {
            _testContainerBuilder.RegisterType<TelegramAdapter>().As<IAdapter>();
            _configuration = A.Fake<IConfiguration>();
            A.CallTo(() => _configuration["BotConfiguration:BotUrl"]).Returns("localhost:51515");

            _logger = A.Fake<ILogger<TelegramAdapter>>();
            _testContainerBuilder.RegisterInstance(_logger);
            _telegramClient = A.Fake<ITelegramBotClient>();
            _testContainerBuilder.RegisterInstance(_configuration);
            _testContainerBuilder.RegisterInstance(_telegramClient).As<ITelegramBotClient>();
            _container = _testContainerBuilder.Build();
            _target = _container.Resolve<IAdapter>();
        }

        [Fact]
        public async Task SendActivity_Message()
        {
            // Arrange
            var activity = new Activity() { ChatId = 15, ActivityType = ActivityType.Message };
            // Act
            await _target.SendActivity(activity);
            // Assert
            A.CallTo(() => _telegramClient.SendTextMessageAsync(A<ChatId>._, A<string>._, A<ParseMode>._, A<bool>._, A<bool>._, A<int>._, A<IReplyMarkup>._, A<CancellationToken>._)).MustHaveHappened();
        }

        [Fact]
        public async Task SendActivity_Picture()
        {
            // Arrange
            var activity = new Activity(){ChatId = 15, ActivityType = ActivityType.Picture};
            activity.Pictures = new PhotoSize[]{new PhotoSize(), };
            // Act
            await _target.SendActivity(activity);
            // Assert
            A.CallTo(() => _telegramClient.SendPhotoAsync(A<ChatId>._, A<InputOnlineFile>._, A<string>._,
                A<ParseMode>._, A<bool>._, A<int>._, A<IReplyMarkup>._, A<CancellationToken>._)).MustHaveHappened();
        }
        [Fact]
        public async Task CreateActivityFromUpdate_Message()
        {
            // Arrange
            var update = new Update() { Message = new Message() { Text = "toto", Chat = new Chat() { Id = 15 } } };
            // Act
            var activity = await _target.CreateActivityFromUpdate(update);
            // Assert
            Check.That(activity.ActivityType).Equals(ActivityType.Message);
            Check.That(activity.Text).Equals(update.Message.Text);
            Check.That(activity.ChatId).Equals(update.Message.Chat.Id);
        }
        [Fact]
        public async Task CreateActivityFromUpdate_CallbackQuery()
        {
            // Arrange
            var update = new Update() { CallbackQuery = new CallbackQuery() { Message = new Message() { Text = "toto", Chat = new Chat() { Id = 15 } } } };
            // Act
            var activity = await _target.CreateActivityFromUpdate(update);
            // Assert
            Check.That(activity.ActivityType).Equals(ActivityType.CallbackQuery);
            Check.That(activity.Text).Equals(update.CallbackQuery.Message.Text);
            Check.That(activity.ChatId).Equals(update.CallbackQuery.Message.Chat.Id);
        }

        [Fact]
        public async Task CreateActivityFromUpdate_Message_Photo()
        {
            // Arrange
            var photoSize1 = new PhotoSize() { FileId = "fileId1", FileSize = 15 };
            var photoSize2 = new PhotoSize() { FileId = "fileId2", FileSize = 150 };
            var photoSize3 = new PhotoSize() { FileId = "fileId3", FileSize = 1500 };
            var update = new Update()
            {
                Message = new Message()
                {
                    Chat = new Chat()
                    {
                        Id = 15
                    },
                    Photo = new[]
                {
            photoSize1, photoSize2, photoSize3
          }
                }
            };
            // Act                                                                                       
            var activity = await _target.CreateActivityFromUpdate(update);
            // Assert
            Check.That(activity.ActivityType).Equals(ActivityType.Message);
            Check.That(activity.Pictures).HasSize(3).And.Contains(update.Message.Photo);
            Check.That(activity.ChatId).Equals(update.Message.Chat.Id);
            Check.That(activity.Text).Equals("/uploadphoto");
        }

        [Fact]
        public async Task CreateActivityFromUpdate_Message_Location()
        {
            // Arrange
            var update = new Update()
            {
                Message = new Message()
                {
                    Chat = new Chat()
                    {
                        Id = 15
                    },
                    Location = new Location() { Latitude = 15.2f, Longitude = 25.2f }
                }
            };
            // Act
            var activity = await _target.CreateActivityFromUpdate(update);
            // Assert
            Check.That(activity.ActivityType).Equals(ActivityType.Message);
            Check.That(activity.ChatId).Equals(update.Message.Chat.Id);
            Check.That(activity.Text).Equals("/location");
            Check.That(activity.Location).IsNotNull();
            Check.That(activity.Location.Latitude).Equals(15.2f);
            Check.That(activity.Location.Longitude).Equals(25.2f);
        }

        [Fact]
        public async Task SetWebHook()
        {
            // Arrange
            A.CallTo(() => _configuration["BotConfiguration:BotUrl"]).Returns("localhost:51515");
            // Act
            await _target.SetWebHook();
            // Assert
            A.CallTo(() => _telegramClient.SetWebhookAsync(A<string>.That.Matches(u => CheckSetHookUrl(u, _configuration["BotConfiguration:BotUrl"])), A<InputFileStream>._, A<int>._,
                A<IEnumerable<UpdateType>>._, A<CancellationToken>._)).MustHaveHappened();
        }


        [Fact]
        public async Task SetWebHook_Error()
        {
            // Arrange
            A.CallTo(() => _configuration["BotConfiguration:BotUrl"]).Returns("localhost:51515");
            A.CallTo(() => _telegramClient.SetWebhookAsync(
                A<string>.That.Matches(u => CheckSetHookUrl(u, _configuration["BotConfiguration:BotUrl"])),
                A<InputFileStream>._, A<int>._,
                A<IEnumerable<UpdateType>>._, A<CancellationToken>._)).Throws<Exception>();
            // Act
            await _target.SetWebHook();
            // Assert
            A.CallTo(() => _telegramClient.SetWebhookAsync(A<string>.That.Matches(u => CheckSetHookUrl(u, _configuration["BotConfiguration:BotUrl"])), A<InputFileStream>._, A<int>._,
                A<IEnumerable<UpdateType>>._, A<CancellationToken>._)).MustHaveHappened();
            A.CallTo(() => _logger.Log(A<LogLevel>._, A<EventId>._, A<object>._, A<Exception>._,
                A<Func<object, Exception, string>>._)).MustHaveHappened();
        }

        private bool CheckSetHookUrl(string url, string token)
        {
            Check.That(url).IsNotEmpty();
            return true;
        }

        [Fact]
        public async Task CreateActivityFromUpdate_NewMembers()
        {
            // Arrange
            var update = new Update()
            {

                Message = new Message()
                {
                    Chat = new Chat()
                    {
                        Id = 155151
                    },
                    From = new User()
                    {
                        Username = "tata"
                    },
                    NewChatMembers = new User[]
                    {
                        new User(){Username = "toto"}
                    }
                }
            };
            // Act
            var activity = await _target.CreateActivityFromUpdate(update);
            // Assert
            Check.That(activity.ActivityType).Equals(ActivityType.AddMember);
            Check.That(activity.Text).Equals("/newUser");
        }

        [Fact]
        public async Task Leave()
        {
            // Arrange
            var chatId = new ChatId(15);
            // Act
            await _target.Leave(chatId);
            // Assert
            A.CallTo(() => _telegramClient.LeaveChatAsync(chatId, A<CancellationToken>._)).MustHaveHappened();
        }
    }
}