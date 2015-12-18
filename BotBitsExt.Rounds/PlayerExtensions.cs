using BotBits;

namespace BotBitsExt.Rounds
{
    public static class PlayerExtensions
    {
        internal const string Playing = "Playing";

        /// <summary>
        ///     Determines if the specified player is playing.
        /// </summary>
        /// <returns><c>true</c> if the specified player is playing; otherwise, <c>false</c>.</returns>
        /// <param name="player">Player.</param>
        public static bool IsPlaying(this Player player)
        {
            return player.Get<bool>(Playing);
        }

        /// <summary>
        ///     Adds the specified player to round.
        /// </summary>
        /// <param name="player">Player.</param>
        public static void AddToRound(this Player player)
        {
            if (!player.IsPlaying())
                player.Set(Playing, true);
        }

        /// <summary>
        ///     Removes the specified player from round.
        /// </summary>
        /// <param name="player">Player.</param>
        public static void RemoveFromRound(this Player player)
        {
            if (player.IsPlaying())
                player.Set(Playing, false);
        }
    }
}