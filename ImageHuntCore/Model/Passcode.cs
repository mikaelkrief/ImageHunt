namespace ImageHuntCore.Model
{
    public class Passcode : DbObject
    {
      public string Pass { get; set; }
      public int NbRedeem { get; set; }
      public int Points { get; set; }
    }
}
