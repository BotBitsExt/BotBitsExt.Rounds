using BotBits;
using JetBrains.Annotations;

namespace BotBitsExt.Rounds.Events
{
    /// <summary>
    ///     Event raised when round starting failed due to user interaction or too low number of players.
    /// </summary>
    public sealed class RoundStartFailedEvent : Event<RoundStartFailedEvent>
    {
        internal RoundStartFailedEvent(bool enoughPlayers)
        {
            EnoughPlayers = enoughPlayers;
        }

        /// <summary>
        ///     Gets a value indicating whether there were enough players to start new round.
        ///     If it's set to <c>false</c> it means that round start was stopped because
        ///     there weren't enough players to continue starting new round.
        /// </summary>
        /// <value><c>true</c> if there were enough players; otherwise, <c>false</c>.</value>
        [UsedImplicitly]
        public bool EnoughPlayers { get; private set; }
    }
}