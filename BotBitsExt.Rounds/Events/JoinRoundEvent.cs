using BotBits;
using JetBrains.Annotations;

namespace BotBitsExt.Rounds.Events
{
    /// <summary>
    ///     Event raised when player is added to round.
    /// </summary>
    public sealed class JoinRoundEvent : Event<JoinRoundEvent>
    {
        internal JoinRoundEvent(Player player)
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