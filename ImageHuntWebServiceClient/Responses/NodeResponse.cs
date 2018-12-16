using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace ImageHuntWebServiceClient.Responses
{
    public class NodeResponse
    {
        public const string ObjectNodeType = "ObjectNode";
        public const string TimerNodeType = "TimerNode";
        public const string FirstNodeType = "FirstNode";
        public const string LastNodeType = "LastNode";
        public const string HiddenNodeType = "HiddenNode";
        public const string ChoiceNodeType = "ChoiceNode";
        public const string QuestionNodeType = "QuestionNode";
        public const string PictureNodeType = "PictureNode";

        public int Id { get; set; }
        public string Name { get; set; }
        public string NodeType { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int Points { get; set; }
        public string Password { get; set; }
        public IEnumerable<int> ChildNodeIds { get; set; }
        public IEnumerable<AnswerResponse> Answers { get; set; }
        public string Action { get; set; }
        public ImageResponse Image { get; set; }
        public string Question { get; set; }
        public string Hint { get; set; }
        public int Delay { get; set; }
    }
}