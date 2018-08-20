using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.WebServices;

namespace ImageHuntApiTester
{
    class Program
    {
        static HttpClient httpClient = new HttpClient();
        private static IActionWebService _actionWebService;
        static void Main(string[] args)
        {
            RunAsync().GetAwaiter().GetResult();
        }

        static async Task InsertRandomPosition()
        {
            var random = new Random((int)DateTime.Now.Ticks);
            var positionRequest = new LogPositionRequest()
            {
                GameId = 3,
                TeamId = 1,
                Latitude = 46.04139569600664,
                Longitude = 4.066337343144121
            };
            do
            {
                positionRequest.Latitude += 0.001 * (random.NextDouble() - 0.5);
                positionRequest.Longitude += 0.001 * (random.NextDouble() - 0.5);
                await _actionWebService.LogPosition(positionRequest);
                Thread.Sleep(1000);
            } while (true);
        }
        static async Task RunAsync()
        {
            httpClient.BaseAddress = new Uri("http://localhost:55955/");
            httpClient.DefaultRequestHeaders.Accept.Clear();
            _actionWebService = new ActionWebService(httpClient);
            
            try
            {
                await InsertRandomPosition();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.ReadLine();
        }
    }
}
