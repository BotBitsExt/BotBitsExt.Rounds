using BotBits;
using JetBrains.Annotations;

namespace BotBitsExt.Rounds.Events
{
    /// <summary>
    ///     Event raised when new round is starting.
    /// </summary>
    public sealed class StartingRoundEvent : Event<StartingRoundEvent>
    {
        internal StartingRoundEvent(int waitTime)
        {
            WaitTime = waitTime;
        }

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