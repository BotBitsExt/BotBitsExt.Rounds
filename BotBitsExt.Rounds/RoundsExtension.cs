using System;
using BotBits;

namespace BotBitsExt.Rounds
{
    public sealed class RoundsExtension : Extension<RoundsExtension>
    {
        private class Settings
        {
            public int MinimumPlayers { get; private set; }
            public int WaitTime { get; private set; }
            public bool FlyingPlayersCanPlay { get; private set; }

            public Settings(int minimumPlayers, int waitTime, bool flyingPlayersCanPlay = false)
            {
                MinimumPlayers = minimumPlayers;
                WaitTime = waitTime;
                FlyingPlayersCanPlay = flyingPlayersCanPlay;
            }
        }

        protected override void Initialize(BotBitsClient client, object args)
        {
            var settings = (Settings)args;
            RoundsManager.Of(client).MinimumPlayers = settings.MinimumPlayers;
            RoundsManager.Of(client).WaitTime = settings.WaitTime;
            RoundsManager.Of(client).FlyingPlayersCanPlay = settings.FlyingPlayersCanPlay;
        }

        public static bool LoadInto(BotBitsClient client, int minimumPlayers, int waitTime)
        {
            return LoadInto(client, new Settings(minimumPlayers, waitTime));
        }
    }
}

