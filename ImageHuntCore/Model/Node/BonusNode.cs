namespace ImageHuntCore.Model.Node
{
    public class BonusNode : Node
    {
        public enum BONUSTYPE
        {
            PointsX2,
            PointsX3,
        }

        public string Location { get; set; }
        public BONUSTYPE BonusType { get; set; }
    }
}
