using BotBits;
using JetBrains.Annotations;

namespace BotBitsExt.Rounds
{
    public static class PlayerExtensions
    {
        internal const string PlayingMetadata = "BotBitsExt.Rounds.Playing";
        internal const string CanPlayMetadata = "BotBitsExt.Rounds.CanPlay";

        /// <summary>
        ///     Determines if the specified player is playing.
        /// </summary>
        /// <returns><c>true</c> if the specified player is playing; otherwise, <c>false</c>.</returns>
        /// <param name="player">Player.</param>
        public static bool IsPlaying(this Player player)
        {
            return player.Get<bool>(PlayingMetadata);
        }

        /// <summary>
        ///     Adds the specified player to round.
        /// </summary>
        /// <param name="player">Player.</param>
        public static void AddToRound(this Player player)
        {
            if (!player.IsPlaying())
                player.Set(PlayingMetadata, true);
        }

        /// <summary>
        ///     Removes the specified player from round.
        /// </summary>
        /// <param name="player">Player.</param>
        public static void RemoveFromRound(this Player player)
        {
            if (player.IsPlaying())
                player.Set(PlayingMetadata, false);
        }

        /// <summary>
        ///     Determines whether this player is added to rounds.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <returns>Whether specified player is added to rounds.</returns>
        public static bool CanPlay(this Player player)
        {
            return player.Get<bool>(CanPlayMetadata);
        }

        /// <summary>
        ///     Sets whether the player can be added to rounds.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <param name="canPlay">if set to <c>true</c> player is added to rounds.</param>
        [UsedImplicitly]
        public static void SetCanPlay(this Player player, bool canPlay)
        {
            if (player.CanPlay() != canPlay)
                player.Set(CanPlayMetadata, canPlay);
        }
    }
}