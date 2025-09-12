namespace Internal.Core.Signals
{
    public class GameEndSignal
    {
        public bool Won;

        public GameEndSignal(bool won)
        {
            Won = won;
        }
    }
}