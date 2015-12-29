using BotBits;
using JetBrains.Annotations;

namespace BotBitsExt.Rounds.Events
{
    /// <summary>
    ///     Event raised when new round started.
    /// </summary>
    public sealed class StartRoundEvent : Event<StartRoundEvent>
    {
        internal StartRoundEvent(Player[] players)
        {
            Players = players;
        }

        /// <summary>
        ///     Gets the players which were added to the newly started round.
        /// </summary>
        /// <value>
        ///     The players.
        /// </value>
        [UsedImplicitly]
        public Player[] Players { get; private set; }
    }
}