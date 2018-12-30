using ImageHuntCore.Model.Node;
using ImageHuntWebServiceClient.Responses;

namespace ImageHunt.Model
{
  public static class NodeFactory
  {
    public static Node CreateNode(string nodeType)
    {
      switch (nodeType)
      {
        case NodeResponse.TimerNodeType:
          return new TimerNode();
        case NodeResponse.ChoiceNodeType:
          return new ChoiceNode();
        case NodeResponse.QuestionNodeType:
          return new QuestionNode();
        case NodeResponse.PictureNodeType:
          return new PictureNode();
        case NodeResponse.FirstNodeType:
          return new FirstNode();
        case NodeResponse.LastNodeType:
          return new LastNode();
        case NodeResponse.ObjectNodeType:
          return new ObjectNode();
        case NodeResponse.HiddenNodeType:
          return new HiddenNode();
        case NodeResponse.BonusNodeType:
          return new BonusNode();
        case NodeResponse.WaypointNodeType:
          return new WaypointNode();
      }
      return null;
    }
  }
}
