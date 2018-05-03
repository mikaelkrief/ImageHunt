using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace ImageHuntTelegramBot
{
    class Program
    {
      public static void Main(string[] args)
      {
        BuildWebHost(args).Run();
      }

      public static IWebHost BuildWebHost(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
          .UseStartup<Startup>()
          .Build();
    
  //    static void Main(string[] args)
  //    {
  //      var configurationBuilder = new ConfigurationBuilder();
  //      configurationBuilder
  //        .SetBasePath(Directory.GetCurrentDirectory())
  //        .AddJsonFile("appsettings.json", optional:true, reloadOnChange:true)
  //        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")??"Production"}.json", optional:true);
  //      var configuration = configurationBuilder.Build();

  //      var afContainer = new ContainerBuilder();
  //      afContainer.RegisterInstance(configuration);

  //      var telegramBot = new TelegramBotClient(configuration["Telegram:APIKey"]);
  //      afContainer.RegisterInstance(telegramBot);
  //      var me = telegramBot.GetMeAsync().Result;
  //      Console.Title = me.Username;

  //      //telegramBot.OnMessage += BotOnMessageReceived;

  //      telegramBot.StartReceiving();
  //      Console.WriteLine($"Start listening for @{me.Username}");
  //      Console.ReadLine();
  //      telegramBot.StopReceiving();
  //}

  //  public static void Startup()
  //  {

  //  }
  //private static void BotOnMessageReceived(object sender, MessageEventArgs e)
  //{
  //  var message = e.Message;
  //  if (message == null) return;
  //  if (message.Type == MessageType.TextMessage)
  //  {
  //    switch (message.Text)
  //    {
  //      case "/init":
  //        await 
  //      case "/start":
  //        break;
  //        default:

  //          break;
  //    }

  //  }
  //}
  }
}
