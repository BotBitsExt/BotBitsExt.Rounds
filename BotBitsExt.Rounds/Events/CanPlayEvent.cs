using BotBits;
using JetBrains.Annotations;

namespace BotBitsExt.Rounds.Events
{
    /// <summary>
    ///     Event raised when player is allowed or disallowed to join rounds.
    /// </summary>
    public sealed class CanPlayEvent : Event<CanPlayEvent>
    {
        internal CanPlayEvent(Player player, bool canPlay)
        {
            Player = player;
            CanPlay = canPlay;
        }

        /// <summary>
        ///     Gets the player.
        /// </summary>
        /// <value>
        ///     The player.
        /// </value>
        [UsedImplicitly]
        public Player Player { get; private set; }

        /// <summary>
        ///     Gets a value indicating whether the player is allowed to join rounds.
        /// </summary>
        /// <value>
        ///     <c>true</c> if the player is allowed to join rounds; otherwise, <c>false</c>.
        /// </value>
        [UsedImplicitly]
        public bool CanPlay { get; private set; }
    }
}