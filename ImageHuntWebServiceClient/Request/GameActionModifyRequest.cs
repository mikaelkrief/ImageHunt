using Microsoft.AspNetCore.Mvc;

namespace ImageHuntWebServiceClient.Request
{
    public class GameActionModifyRequest
    {
        [FromBody]
        public bool Validated { get; set; }
        [FromBody]
        public bool Reviewed { get; set; }
        [FromBody]
        public int PointsEarned { get; set; }
        [FromBody]
        public int Id { get; set; }
    }
}