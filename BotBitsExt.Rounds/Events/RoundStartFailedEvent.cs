using System;
using BotBits;

namespace BotBitsExt.Rounds.Events
{
    public sealed class RoundStartFailedEvent : Event<RoundStartFailedEvent>
    {
        internal RoundStartFailedEvent(bool enoughPlayers)
        {
            EnoughPlayers = enoughPlayers;
        }

        /// <summary>
        /// Gets a value indicating whether there were enough players to start new round.
        /// If it's set to false it means that round start was stopped because eligble
        /// players left the world.
        /// </summary>
        /// <value><c>true</c> if there were enough players; otherwise, <c>false</c>.</value>
        public bool EnoughPlayers { get; private set; }
    }
}

