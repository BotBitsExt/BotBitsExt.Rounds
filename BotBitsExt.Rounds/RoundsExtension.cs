using BotBits;

namespace BotBitsExt.Rounds
{
    public sealed class RoundsExtension : Extension<RoundsExtension>
    {
        protected override void Initialize(BotBitsClient client, object args)
        {
            var settings = (Settings) args;
            RoundsManager.Of(client).MinimumPlayers = settings.MinimumPlayers;
            RoundsManager.Of(client).WaitTime = settings.WaitTime;
            RoundsManager.Of(client).FlyingPlayersCanPlay = settings.FlyingPlayersCanPlay;
        }

        public static bool LoadInto(BotBitsClient client,
            int minimumPlayers,
            int waitTime,
            bool flyingPlayersCanPlay = false)
        {
            return LoadInto(client, new Settings(minimumPlayers, waitTime, flyingPlayersCanPlay));
        }

        private class Settings
        {
            public Settings(int minimumPlayers, int waitTime, bool flyingPlayersCanPlay = false)
            {
                MinimumPlayers = minimumPlayers;
                WaitTime = waitTime;
                FlyingPlayersCanPlay = flyingPlayersCanPlay;
            }

            public int MinimumPlayers { get; }
            public int WaitTime { get; }
            public bool FlyingPlayersCanPlay { get; }
        }
    }
}