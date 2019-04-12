namespace ImageHuntCore.Model
{
    public class Picture : DbObject
    {
      public byte[] Image { get; set; }
        public string CloudUrl { get; set; }
    }
}
