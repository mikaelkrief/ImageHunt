using System;

namespace ImageHuntWebServiceClient.Responses
{
    public class GameActionResponse
    {
        public int Id { get; set; }
        public DateTime DateOccured { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public int GameId { get; set; }
        public int TeamId { get; set; }
        public int PictureId { get; set; }
        public Action Action { get; set; }
        public bool IsValidated { get; set; }
        public bool IsReviewed { get; set; }
        public int ReviewerId { get; set; }
        public double PointsEarned { get; set; }
    }
}