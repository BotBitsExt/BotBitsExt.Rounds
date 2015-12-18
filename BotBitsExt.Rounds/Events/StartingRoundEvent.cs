using BotBits;

namespace BotBitsExt.Rounds.Events
{
    public sealed class StartingRoundEvent : Event<StartingRoundEvent>
    {
        internal StartingRoundEvent(int waitTime)
        {
            WaitTime = waitTime;
        }

        public int WaitTime { get; private set; }
    }
}