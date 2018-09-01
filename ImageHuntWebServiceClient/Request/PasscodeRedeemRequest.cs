namespace ImageHuntWebServiceClient.Request
{
  public class PasscodeRedeemRequest
  {
    public PasscodeRedeemRequest(int gameId, int teamId, string pass)
    {
      GameId = gameId;
      TeamId = teamId;
      Pass = pass;
    }

    public int GameId { get; }
    public int TeamId { get; }
    public string Pass { get; }
  }
}