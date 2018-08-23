using System;
using System.Globalization;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.WebServices;
using Newtonsoft.Json;
using Action = ImageHuntWebServiceClient.Action;

namespace ImageHuntApiTester
{
    class Program
    {
        public class Options
        {
            [Option('g', "gameId", Required = true)]
            public int GameId { get; set; }
            [Option('t', "minTeamId", Required = true)]
            public int MinTeamId { get; set; }
            [Option('T', "maxTeamId", Required = true)]
            public int MaxTeamId { get; set; }
            [Option('a', "latitude", Required = true)]
            public double SeedLatitude { get; set; }
            [Option('n', "longitude", Required = true)]
            public double SeedLongitude { get; set; }
            [Option('u', "APIUrl", Required = true)]
            public string APIUrl { get; set; }
            [Option('i', "Timer", Required = false, Default = 15000)]
            public int TimerInterval { get; set; }
        }
        static HttpClient httpClient = new HttpClient();
        private static IActionWebService _actionWebService;
        static void Main(string[] args)
        {
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
        static async Task RunAsync(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(async o => {
                    httpClient.BaseAddress = new Uri(o.APIUrl);
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    _actionWebService = new ActionWebService(httpClient);

                    try
                    {
                        await InsertRandomPosition(o.GameId, o.MinTeamId, o.MaxTeamId, o.SeedLatitude, o.SeedLongitude, o.TimerInterval);
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
