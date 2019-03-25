namespace ImageHuntWebServiceClient.Request
{
    public class UpdateUserRequest
    {
        public string Id { get; set; }
        public string Role { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string Telegram { get; set; }
    }
}