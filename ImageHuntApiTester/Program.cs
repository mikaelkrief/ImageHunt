using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.WebServices;
using Newtonsoft.Json;

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
        }
        static HttpClient httpClient = new HttpClient();
        private static IActionWebService _actionWebService;
        static void Main(string[] args)
        {
            RunAsync(args).GetAwaiter().GetResult();
        }

        static async Task InsertRandomPosition(int gameId, int minTeamId, int maxTeamId, double latitude, double longitude)
        {
            var random = new Random((int)DateTime.Now.Ticks);
            var positionRequest = new LogPositionRequest()
            {
                GameId = gameId,
                TeamId = minTeamId,
                Latitude = latitude,
                Longitude = longitude
            };
            do
            {
                positionRequest.TeamId = random.Next(minTeamId, maxTeamId + 1);
                positionRequest.Latitude += 0.001 * (random.NextDouble() - 0.5);
                positionRequest.Longitude += 0.001 * (random.NextDouble() - 0.5);
                await _actionWebService.LogPosition(positionRequest);
                Thread.Sleep(10000);
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
                        await InsertRandomPosition(o.GameId, o.MinTeamId, o.MaxTeamId, o.SeedLatitude, o.SeedLongitude);
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
