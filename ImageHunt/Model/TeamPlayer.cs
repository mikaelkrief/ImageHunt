using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageHunt.Model
{
    public class TeamPlayer
    {
      private Team _team;
      private Player _player;
      public int TeamId { get; set; }
      public int PlayerId { get; set; }

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

      public Player Player  
      {
        get { return _player; }
        set
        {
          _player = value;
          if (PlayerId == 0)
            PlayerId = _player?.Id ?? 0;
        }
      }
    }
}
