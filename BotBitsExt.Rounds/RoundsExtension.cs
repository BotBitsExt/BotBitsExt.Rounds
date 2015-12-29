using BotBits;
using JetBrains.Annotations;

namespace BotBitsExt.Rounds
{
    /// <summary>
    ///     Rounds extension used to manage automatic and manual starting of
    ///     game rounds.
    /// </summary>
    [UsedImplicitly]
    public sealed class RoundsExtension : Extension<RoundsExtension>
    {
        protected override void Initialize(BotBitsClient client, object args)
        {
            var settings = (Settings) args;
            RoundsManager.Of(client).MinimumPlayers = settings.MinimumPlayers;
            RoundsManager.Of(client).WaitTime = settings.WaitTime;
            RoundsManager.Of(client).FlyingPlayersCanPlay = settings.FlyingPlayersCanPlay;
        }

        /// <summary>
        ///     Loads the rounds extension into the specified <see cref="BotBitsClient" />.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="minimumPlayers">
        ///     The minimum amount of players required to start new rounds.
        /// </param>
        /// <param name="waitTime">The wait time before start of each round.</param>
        /// <param name="flyingPlayersCanPlay">
        ///     if set to <c>true</c> players in god mode are also allowed to
        ///     play the game.
        /// </param>
        /// <returns>Whether the load succeeded.</returns>
        [UsedImplicitly]
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