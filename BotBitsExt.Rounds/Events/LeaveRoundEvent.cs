using BotBits;

namespace BotBitsExt.Rounds.Events
{
    public sealed class LeaveRoundEvent : Event<LeaveRoundEvent>
    {
        internal LeaveRoundEvent(Player player)
        {
            Player = player;
        }

        public Player Player { get; private set; }
    }
}