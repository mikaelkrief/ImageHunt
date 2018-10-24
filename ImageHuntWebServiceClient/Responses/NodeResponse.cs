using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace ImageHuntWebServiceClient.Responses
{
    public class NodeResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NodeType { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int Points { get; set; }
        public string Password { get; set; }
        public IEnumerable<int> ChildNodeIds { get; set; }
        public string Action { get; set; }
        public ImageResponse Image { get; set; }
    }
}