using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ImageHuntBot
{
  public abstract class BotHost : IBotHost
  {
    protected readonly IConfiguration _configuration;
    protected ITelegramBotClient _bot;
    private ManualResetEvent _shutdownEvent;
    private CancellationTokenSource _cancelationTokenSource;

    public BotHost(IConfiguration configuration)
    {
      _configuration = configuration;
      _shutdownEvent = new ManualResetEvent(false);
      _bot = new TelegramBotClient(_configuration["Telegram:APIKey"]);
      _cancelationTokenSource = new CancellationTokenSource();
    }
    public void Dispose()
    {
      _cancelationTokenSource.Dispose();
    }

    public void Start()
    {
      StartAsync().GetAwaiter().GetResult();
    }

    public Task StartAsync(CancellationToken cancellationToken = new CancellationToken())
    {
      _bot.OnMessage += OnMessage;
      return Task.Factory.StartNew(() => _bot.StartReceiving(cancellationToken: cancellationToken), cancellationToken);
    }

    public void StopAsync(CancellationToken cancellationToken = new CancellationToken())
    {
      throw new NotImplementedException();
    }

    public async void OnMessage(object sender, MessageEventArgs messageEventArgs)
    {
      var message = messageEventArgs.Message;
      switch (message.Type)
      {
        case MessageType.UnknownMessage:
          break;
        case MessageType.TextMessage:
          await OnTextMessage(message);
          break;
        case MessageType.PhotoMessage:
          break;
        case MessageType.AudioMessage:
          break;
        case MessageType.VideoMessage:
          break;
        case MessageType.VoiceMessage:
          break;
        case MessageType.DocumentMessage:
          break;
        case MessageType.StickerMessage:
          break;
        case MessageType.LocationMessage:
          break;
        case MessageType.ContactMessage:
          break;
        case MessageType.ServiceMessage:
          break;
        case MessageType.VenueMessage:
          break;
        case MessageType.GameMessage:
          break;
        case MessageType.VideoNoteMessage:
          break;
        case MessageType.Invoice:
          break;
        case MessageType.SuccessfulPayment:
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    public abstract Task OnTextMessage(Message message);

    public void Stop(CancellationToken cancellationToken = default(CancellationToken))
    {
      _cancelationTokenSource.Cancel();
      
    }
  }
}
