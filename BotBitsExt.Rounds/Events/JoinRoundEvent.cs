using System;
using BotBits;

namespace BotBitsExt.Rounds.Events
{
    public sealed class JoinRoundEvent : Event<JoinRoundEvent>
    {
        internal JoinRoundEvent(Player player)
        {
            Player = player;
        }

        public Player Player { get; private set; }
    }
}

