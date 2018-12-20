namespace ImageHuntWebServiceClient.Request
{
    public class AddNodeRequest
    {
        public string NodeType { get; set; }
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int Duration { get; set; }
        public string Action { get; set; }
        public string Question { get; set; }
        public int Points { get; set; }
        public string Answer { get; set; }
        public AnswerRequest[] Choices { get; set; }
        public string Password { get; set; }
        public string Hint { get; set; }
        public int Bonus { get; set; }
        public string Location { get; set; }
    }
}
