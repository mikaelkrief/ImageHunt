using System.Collections.Generic;
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

    public static Node DuplicateNode(Node orgNode)
    {
      var newNode = CreateNode(orgNode.NodeType);
      newNode.Latitude = orgNode.Latitude;
      newNode.Longitude = orgNode.Longitude;
      newNode.Name = orgNode.Name;
      newNode.Points = orgNode.Points;
      switch (orgNode.NodeType)
      {
        case NodeResponse.FirstNodeType:
          ((FirstNode) newNode).Password = ((FirstNode) orgNode).Password;
          break;
        case NodeResponse.ObjectNodeType:
          ((ObjectNode) newNode).Action = ((ObjectNode) orgNode).Action;
          break;
        case NodeResponse.PictureNodeType:
          ((PictureNode) newNode).Image = ((PictureNode) orgNode).Image;
          break;
        case NodeResponse.QuestionNodeType:
          ((QuestionNode) newNode).Question = ((QuestionNode) orgNode).Question;
          ((QuestionNode) newNode).Answer = ((QuestionNode) orgNode).Answer;
          break;
        case NodeResponse.TimerNodeType:
          ((TimerNode) newNode).Delay = ((TimerNode) orgNode).Delay;
          break;
        case NodeResponse.HiddenNodeType:
          ((HiddenNode) newNode).LocationHint = ((HiddenNode) orgNode).LocationHint;
          break;
        case NodeResponse.ChoiceNodeType:
          ((ChoiceNode) newNode).Choice = ((ChoiceNode) orgNode).Choice;
          ((ChoiceNode)newNode).Answers = new List<Answer>();
          ((ChoiceNode)orgNode).Answers.ForEach(a=>((ChoiceNode)newNode).Answers.Add(new Answer(){Response = a.Response, Correct = a.Correct})); 
          break;
        case NodeResponse.BonusNodeType:
          ((BonusNode) newNode).Location = ((BonusNode) orgNode).Location;
          ((BonusNode) newNode).BonusType = ((BonusNode) orgNode).BonusType;
          break;
      }
      return newNode;
    }
  }
}
