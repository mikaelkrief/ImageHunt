namespace ImageHunt.Model
{
    public class TimerNode : Node
    {
        private readonly int _delay;

        public TimerNode(string name, Geography coordinate, int delay)
            : base(name, coordinate)
        {
            _delay = delay;
        }

    }
}