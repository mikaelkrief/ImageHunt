using ImageHunt.Model.Node;

namespace ImageHunt.Model
{
  public static class NodeFactory
  {
    public static ImageHuntCore.Model.Node.Node CreateNode(string nodeType)
    {
      switch (nodeType)
      {
        case "TimerNode":
          return new TimerNode();
        case "QuestionNode":
          return new QuestionNode();
        case "PictureNode":
          return new PictureNode();
        case "FirstNode":
          return new FirstNode();
        case "LastNode":
          return new LastNode();
        case "ObjectNode":
          return new ObjectNode();
      }
      return null;
    }
  }
}
