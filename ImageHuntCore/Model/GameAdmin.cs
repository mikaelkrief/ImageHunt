namespace ImageHuntCore.Model
{
  public class GameAdmin
  {
    private Game _game;
    private Admin _admin;

    public Game Game
    {
      get => _game;
      set
      {
        _game = value;
        if (GameId == 0)
          GameId = _game?.Id ?? 0;
      }
    }

    public int GameId { get; set; }

    public Admin Admin
    {
      get => _admin;
      set
      {
        _admin = value;
        if (AdminId == 0)
          AdminId = _admin?.Id ?? 0;
      }
    }

    public int AdminId { get; set; }
  }
}
