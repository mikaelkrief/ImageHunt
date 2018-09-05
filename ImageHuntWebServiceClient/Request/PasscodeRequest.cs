namespace ImageHuntWebServiceClient.Request
{
    public class PasscodeRequest
    {
        public string Pass { get; set; }
        public int Points { get; set; }
        public int NbRedeem { get; set; }
        public int GameId { get; set; }
    }
}