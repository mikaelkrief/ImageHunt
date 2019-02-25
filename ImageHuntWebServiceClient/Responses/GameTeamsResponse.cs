using System;
using System.Collections.Generic;
using System.Text;

namespace ImageHuntWebServiceClient.Responses
{
    public class GameTeamsResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public bool IsActive { get; set; }
        public int PictureId { get; set; }

        public TeamResponse[] Teams { get; set; }
    }
}
