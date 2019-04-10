using System;

namespace ImageHuntWebServiceClient.Responses
{
    public class GameResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set }
        public DateTime StartDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsPublic { get; set; }
        public int PictureId { get; set; }
        public string Code { get; set; }
        public TeamResponse[] Teams { get; set; }
        public double? MapCenterLat { get; set; }
        public double? MapCenterLng { get; set; }
        public int? MapZoom { get; set; }

    }

    public class GameResponseEx : GameResponse
    {
        public NodeResponse[] Nodes { get; set; }
    }
}
