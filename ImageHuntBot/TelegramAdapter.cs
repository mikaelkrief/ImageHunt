using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ImageHuntTelegramBot
{
    public class TelegramAdapter : IAdapter
    {
        private readonly ITelegramBotClient _client;
        private readonly ILogger<TelegramAdapter> _logger;
        private readonly IConfiguration _configuration;

        public TelegramAdapter(ITelegramBotClient client, ILogger<TelegramAdapter> logger, IConfiguration configuration)
        {
            _client = client;
            _logger = logger;
            _configuration = configuration;
            SetWebHook().Wait();
        }
        public async Task SendActivity(IActivity activity)
        {
            switch (activity.ActivityType)
            {
                case ActivityType.Message:
                    await SendMessage(activity);
                    break;
                case ActivityType.UpdateMessage:
                    break;
                case ActivityType.CallbackQuery:
                    break;
                case ActivityType.None:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public async virtual Task<Activity> CreateActivityFromUpdate(Update update)
        {
            long chatId = 0;
            string text = null;
            ActivityType activityType = ActivityType.None;
            PhotoSize[] photoSizes = null;
            Document document = null;
            Location location = null;
            User[] newUsers = null;
            Message message = null;
            switch (update.Type)
            {
                case UpdateType.Message:
                case UpdateType.EditedMessage:
                    message = update.Message == null ? update.EditedMessage : update.Message;
                    chatId = message.Chat.Id;
                    text = message.Text;
                    activityType = ActivityType.Message;
                    if (message.Photo != null)
                    {
                        text = "/uploadphoto";
                        photoSizes = message.Photo;
                    }

                    if (message.Document != null)
                    {
                        text = "/uploaddocument";
                        document = message.Document;
                    }

                    if (message.Location != null)
                    {
                        text = "/location";
                        location = message.Location;
                    }

                    if (message.NewChatMembers != null)
                    {
                        text = "/newUser";
                        newUsers = message.NewChatMembers;
                        activityType = ActivityType.AddMember;
                    }

                    break;
                case UpdateType.CallbackQuery:
                    chatId = update.CallbackQuery.Message.Chat.Id;
                    activityType = ActivityType.CallbackQuery;
                    text = update.CallbackQuery.Message.Text;

                    break;
            }
            var activity = new Activity();
            activity.ActivityType = activityType;
            activity.ChatId = chatId;
            activity.Text = text;
            activity.Pictures = photoSizes;
            activity.Document = document;
            activity.Location = location;
            activity.NewChatMember = newUsers;
            return activity;
        }

        public async Task SetWebHook()
        {
            var botName = _configuration["BotConfiguration:BotName"];

            try
            {
                var url = _configuration["BotConfiguration:BotUrl"];
                await _client.SetWebhookAsync(url);

            }
            catch (Exception e)
            {
                _logger.LogError($"Unable to set web hook for bot {botName}");
            }
        }

        private async Task SendMessage(IActivity activity)
        {
            try
            {
                await _client.SendTextMessageAsync(activity.ChatId, activity.Text);

            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error while sending message to {activity.ChatId}");
            }
        }
    }
}
