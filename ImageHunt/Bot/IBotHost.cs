using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace ImageHuntBot
{
  public interface IBotHost : IDisposable
  {
    // Lifecycle managment
    void Start();
    Task StartAsync(CancellationToken cancellationToken = default(CancellationToken));
    void StopAsync(CancellationToken cancellationToken = default(CancellationToken));

    // Messages managment
    void OnMessage(object sender, MessageEventArgs messageEventArgs);

    // Specific messages
    Task OnTextMessage(Message message);

  }
}
