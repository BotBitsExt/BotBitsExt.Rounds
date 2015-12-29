using BotBits;
using JetBrains.Annotations;

namespace BotBitsExt.Rounds.Events
{
    /// <summary>
    ///     Event raised when player is removed from round.
    /// </summary>
    public sealed class LeaveRoundEvent : Event<LeaveRoundEvent>
    {
        internal LeaveRoundEvent(Player player)
        {
            Player = player;
        }

        /// <summary>
        ///     Gets the player.
        /// </summary>
        /// <value>
        ///     The player.
        /// </value>
        [UsedImplicitly]
        public Player Player { get; private set; }
    }
}