using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageHunt.Model
{
  public class Score
  {
    public Team Team { get; set; }
    public double Points { get; set; }
    public TimeSpan TravelTime { get; set; }
  }
}
