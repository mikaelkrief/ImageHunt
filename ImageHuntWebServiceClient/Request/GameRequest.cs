using System;

namespace ImageHuntWebServiceClient.Request
{
    public class GameRequest
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public double MapCenterLat { get; set; }
        public double MapCenterLng { get; set; }
        public int MapZoom { get; set; }
        public int PictureId { get; set; }
        public string Description { get; set; }
    }
}