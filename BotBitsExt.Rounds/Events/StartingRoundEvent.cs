using BotBits;
using JetBrains.Annotations;

namespace BotBitsExt.Rounds.Events
{
    /// <summary>
    ///     Event raised when new round is starting.
    /// </summary>
    public sealed class StartingRoundEvent : Event<StartingRoundEvent>
    {
        internal StartingRoundEvent(Player[] players, int waitTime)
        {
            Players = players;
            WaitTime = waitTime;
        }

        /// <summary>
        ///     Gets the players that would be added to the round if it started
        ///     at the time when this event was raised.
        /// </summary>
        /// <value>
        ///     The players.
        /// </value>
        [UsedImplicitly]
        public Player[] Players { get; private set; }

        /// <summary>
        ///     Gets the number of seconds of delay before the new round will start.
        /// </summary>
        /// <value>
        ///     The wait time in seconds.
        /// </value>
        [UsedImplicitly]
        public int WaitTime { get; private set; }
    }
}