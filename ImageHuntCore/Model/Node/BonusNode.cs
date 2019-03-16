using System;
using System.Collections.Generic;
using System.Text;

namespace ImageHuntCore.Model.Node
{
    public class BonusNode : Node
    {
        public enum BONUS_TYPE
        {
            Points_x2,
            Points_x3,
        }

        public string Location { get; set; }
        public BONUS_TYPE BonusType { get; set; }
    }
}
