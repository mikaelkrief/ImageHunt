
namespace ImageHuntWebServiceClient.Responses
{
    public class TeamResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public PlayerResponse[] Players { get; set; }
        public int GameId { get; set; }
        public string ChatId { get; set; }
        public string CultureInfo { get; set; }
        public string Code { get; set; }
    }
}