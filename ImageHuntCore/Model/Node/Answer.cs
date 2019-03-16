namespace ImageHuntCore.Model.Node
{
  public class Answer : DbObject
  {
    public string Response { get; set; }
    public ImageHuntCore.Model.Node.Node Node { get; set; }
    public bool Correct { get; set; }
  }
}
