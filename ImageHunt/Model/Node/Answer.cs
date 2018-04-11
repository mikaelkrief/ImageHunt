namespace ImageHunt.Model.Node
{
  public class Answer : DbObject
  {
    public string Response { get; set; }
    public Node Node { get; set; }
    public bool Correct { get; set; }
  }
}
