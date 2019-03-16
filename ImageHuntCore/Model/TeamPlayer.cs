namespace ImageHuntCore.Model
{
    public class TeamPlayer
    {
        private Player _player;
        private Team _team;
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
            get => _player;
            set
            {
                _player = value;
                if (PlayerId == 0)
                    PlayerId = _player?.Id ?? 0;
            }
        }
    }
}