using System;

namespace ImageHuntWebServiceClient.Responses
{
    public class GameResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsPublic { get; set; }
        public int PictureId { get; set; }
        public string Code { get; set; }
        public TeamResponse[] Teams { get; set; }
    }
}