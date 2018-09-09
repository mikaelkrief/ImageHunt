namespace ImageHuntWebServiceClient.Request
{
  public class PasscodeRedeemRequest
  {

    public int GameId { get; set; }
    public string UserName { get; set; }
    public string Pass { get; set; }
  }
}