namespace ImageHuntWebServiceClient.Request
{
  public class PasscodeRedeemRequest
  {
    public PasscodeRedeemRequest(int gameId, int teamId, string passcode)
    {
      GameId = gameId;
      TeamId = teamId;
      Passcode = passcode;
    }

    public int GameId { get; private set; }
    public int TeamId { get; private set; }
    public string Passcode { get; private set; }
  }
}