using Microsoft.AspNetCore.Mvc;

namespace ImageHuntWebServiceClient.Request
{
    public class GameActionListRequest
    {
        [FromQuery]
        public int GameId { get; set; }
        [FromQuery]
        public int? TeamId { get; set; }
        [FromQuery]
        public int PageSize { get; set; }
        [FromQuery]
        public int PageIndex { get; set; }
        [FromQuery]
        public int NbPotential { get; set; }
        [FromQuery]
        public string IncludeAction { get; set; }

    }
}
