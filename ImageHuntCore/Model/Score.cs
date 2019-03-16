using System;

namespace ImageHuntCore.Model
{
    public class Score
    {
        public Team Team { get; set; }
        public double Points { get; set; }
        public TimeSpan TravelTime { get; set; }
    }
}