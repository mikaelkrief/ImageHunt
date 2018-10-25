using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageHunt.Model.Node
{
  /// <summary>
  /// Represent a real life object on which the player should find someting to answer the question
  /// </summary>
  public class ObjectNode : ImageHuntCore.Model.Node.Node
  {
    public string Action { get; set; }
  }
}
