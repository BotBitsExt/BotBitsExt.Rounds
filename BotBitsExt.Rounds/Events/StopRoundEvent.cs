using BotBits;
using JetBrains.Annotations;

namespace BotBitsExt.Rounds.Events
{
    /// <summary>
    ///     Event raised when currently running round stopped.
    /// </summary>
    public sealed class StopRoundEvent : Event<StopRoundEvent>
    {
        internal StopRoundEvent(Player[] players)
        {
            Players = players;
        }

        /// <summary>
        ///     Gets the players which were still playing when the round finished.
        /// </summary>
        /// <value>
        ///     The players.
        /// </value>
        [UsedImplicitly]
        public Player[] Players { get; private set; }
    }
}