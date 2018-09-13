using System;

namespace ImageHuntWebServiceClient.Responses
{
    public class ScoreResponse
    {
        public TeamResponse Team { get; set; }
        public double Points { get; set; }
        public TimeSpan TravelTime { get; set; }
    }
}