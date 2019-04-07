using System.ComponentModel.DataAnnotations;

namespace ImageHuntWebServiceClient.Request
{
    public class NodeUpdateRequest
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int GameId { get; set; }
        public string NodeType { get; set; }
        public string Name { get; set; }
        public int? Points { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string Hint { get; set; }
        public int? ImageId { get; set; }
        public string Action { get; set; }
        public int? Delay { get; set; }
        public int? Bonus { get; set; }
    }
}