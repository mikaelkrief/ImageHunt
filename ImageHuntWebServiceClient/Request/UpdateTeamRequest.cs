namespace ImageHuntWebServiceClient.Request
{
    public class UpdateTeamRequest
    {
        public int TeamId { get; set; }
        public string Name { get; set; }
        public string InviteUrl { get; set; }
    }
}