namespace ImageHuntCore.Model
{
  public class TeamPasscode
  {
    private Team _team;
    private Passcode _passcode;
    public int TeamId { get; set; }
    public int PasscodeId { get; set; }

    public Team Team
    {
      get => _team;
      set
      {
        _team = value;
        if (TeamId == 0)
          TeamId = _team?.Id ?? 0;
      }
    }

    public Passcode Passcode
    {
      get => _passcode;
      set
      {
        _passcode = value;
        if (PasscodeId == 0)
          PasscodeId = _passcode?.Id ?? 0;
      }
    }
  }
}
