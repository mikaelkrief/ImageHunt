using System;
using System.Globalization;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.WebServices;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Action = ImageHuntWebServiceClient.Action;

namespace ImageHuntApiTester
{
    class Program
    {
        public class Options
        {
            [Option("API", Required = true)]
            public string APIToTest { get; set; }
            [Option('g', "gameId", Required = false)]
            public int GameId { get; set; }
            [Option('t', "minTeamId", Required = false)]
            public int MinTeamId { get; set; }
            [Option('T', "maxTeamId", Required = false)]
            public int MaxTeamId { get; set; }
            [Option('a', "latitude", Required = false)]
            public double SeedLatitude { get; set; }
            [Option('n', "longitude", Required = false)]
            public double SeedLongitude { get; set; }
            [Option('u', "APIUrl", Required = true)]
            public string APIUrl { get; set; }
            [Option('i', "Timer", Required = false, Default = 15000)]
            public int TimerInterval { get; set; }
            [Option("Name")]
            public string Name { get; set; }
            [Option("ChatLogin")]
            public string ChatLogin { get; set; }
        }
        static HttpClient httpClient = new HttpClient();
        private static IActionWebService _actionWebService;
        private static ITeamWebService _teamWebService;
        private static LoggerFactory _loggerFactory;

        static void Main(string[] args)
        {
            _loggerFactory = new LoggerFactory();
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            RunAsync(args).GetAwaiter().GetResult();
        }

        static async Task InsertRandomPosition(int gameId, int minTeamId, int maxTeamId, double latitude, double longitude, int interval = 15000)
        {
            var random = new Random((int)DateTime.Now.Ticks);
            var gameActionRequest = new GameActionRequest()
            {
                GameId = gameId,
                Latitude = latitude,
                Longitude = longitude,
            };
            do
            {
                gameActionRequest.Action = random.Next(0, 7);
                gameActionRequest.TeamId = random.Next(minTeamId, maxTeamId + 1);
                gameActionRequest.TeamId = random.Next(minTeamId, maxTeamId + 1);
                gameActionRequest.Latitude += 0.001 * (random.NextDouble() - 0.5);
                gameActionRequest.Longitude += 0.001 * (random.NextDouble() - 0.5);
                try
                {
                    await _actionWebService.LogAction(gameActionRequest);

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                Thread.Sleep(interval);
            } while (true);
        }

        static async Task InsertPlayerToTeam(int teamId, string playerName, string playerChatLogin)
        {
            var playerRequest = new PlayerRequest(){Name = playerName, ChatLogin = playerChatLogin};
            var result =await _teamWebService.AddPlayer(teamId, playerRequest);
            Console.WriteLine($"Player {playerName} with {playerChatLogin} inserted in team {teamId}");
        }
        static async Task RunAsync(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(async o =>
                {
                    httpClient.BaseAddress = new Uri(o.APIUrl);
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    try
                    {
                        switch (o.APIToTest)
                        {
                            case "LogAction":
                                _actionWebService = new ActionWebService(httpClient, _loggerFactory.CreateLogger<IActionWebService>());
                                await InsertRandomPosition(o.GameId, o.MinTeamId, o.MaxTeamId, o.SeedLatitude, o.SeedLongitude, o.TimerInterval);
                                break;
                            case "AddPlayer":
                                _teamWebService = new TeamWebService(httpClient, _loggerFactory.CreateLogger<ITeamWebService>());
                                await InsertPlayerToTeam(o.MinTeamId, o.Name, o.ChatLogin);
                                break;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                });

            Console.ReadLine();
        }
    }
}
