using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace ImageHuntWebServiceClient.Request
{
    public class BatchUpdateNodeRequest
    {
        [FromBody]
        public int GameId { get; set; }
        [FromBody]
        public string UpdaterType { get; set; }
        [FromBody]
        public string UpdaterArgument { get; set; }

    }
}