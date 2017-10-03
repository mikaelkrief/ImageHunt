using System;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using ImageHunt.Bot;
using ImageHunt.Services;
using Microsoft.Extensions.Configuration;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Xunit;
using BindingFlags = System.Reflection.BindingFlags;

namespace ImageHuntTest.Bot
{
  public class ImageHuntBotTest
  {
    private ImageHuntBotHost _botHost;
    private IConfiguration _configuration;
    private INodeService _nodeService;
    private ITelegramBotClient _telegramClient;
    private HttpMessageHandler _fakeHttpMessageHandler;
    private HttpClient _httpClient;

    public class FakeHttpMessageHandler : HttpMessageHandler
    {
      protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
      {
        return null;
      }
    }

    public ImageHuntBotTest()
    {
      _configuration = A.Fake<IConfiguration>();
      _telegramClient = A.Fake<ITelegramBotClient>();
      _fakeHttpMessageHandler = A.Fake<HttpMessageHandler>();
      _httpClient = new HttpClient(_fakeHttpMessageHandler) { BaseAddress = new Uri("http://test.com") };
      _botHost = new ImageHuntBotHost(_configuration, _telegramClient, _httpClient);
    }

    [Fact]
    public void CheckImageFromBot()
    {
      // Arrange
      _botHost.Start();
      var message = new Message() { Photo = new PhotoSize[1], Chat = new Chat()};
      MessageEventArgs messageEventArgs = typeof(MessageEventArgs)
        .GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, CallingConventions.Any, new Type[] { typeof(Message) }, null).Invoke(new[] { message }) as MessageEventArgs;
      // Act
      _telegramClient.OnMessage += Raise.With(messageEventArgs);
      // Assert
    }
  }
}
