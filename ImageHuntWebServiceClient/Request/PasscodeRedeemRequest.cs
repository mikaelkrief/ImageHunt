namespace ImageHuntWebServiceClient.Request
{
  public class PasscodeRedeemRequest
  {

    public int GameId { get; set; }
    public int TeamId { get; set; }
    public string Pass { get; set; }
  }
}