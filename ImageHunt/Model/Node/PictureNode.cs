using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageHunt.Model.Node
{
    /// <summary>
    /// Mystery picture node. The player validate this node by uploading a picture through the chatbot. 
    /// It compares the metadata of the picture with the one of the player
    /// </summary>
    public class PictureNode : ImageHuntCore.Model.Node.Node
    {
      public Picture Image { get; set; }
    }
}
