using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ImageHuntCore.Model
{
    public class Player : DbObject
    {
        public Player()
        {
            TeamPlayers = new List<TeamPlayer>();
        }

        public string Name { get; set; }
        public Node.Node CurrentNode { get; set; }
        public DateTime? StartTime { get; set; }
        public string ChatLogin { get; set; }

        [NotMapped] public List<TeamPlayer> TeamPlayers { get; set; }

        [NotMapped] public IEnumerable<Team> Teams => TeamPlayers.Select(tp => tp.Team);

        public Game CurrentGame { get; set; }
    }
}