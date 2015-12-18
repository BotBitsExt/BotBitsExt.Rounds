using System.Linq;
using BotBits;
using BotBits.Events;
using BotBitsExt.Afk;
using BotBitsExt.Rounds.Events;

namespace BotBitsExt.Rounds
{
    public sealed class RoundPlayers : EventListenerPackage<RoundPlayers>
    {
        private Players players;
        private RoundsManager roundsManager;

        public RoundPlayers()
        {
            InitializeFinish += (sender, e) =>
            {
                players = Players.Of(BotBits);
                roundsManager = RoundsManager.Of(BotBits);
            };
        }

        /// <summary>
        ///     Gets the playing players.
        /// </summary>
        /// <value>The playing players.</value>
        public Player[] Playing => (from player in players
            where player.IsPlaying()
            select player)
            .ToArray();

        /// <summary>
        ///     Gets the players that can join new round.
        /// </summary>
        /// <value>The players that can join new round.</value>
        public Player[] Potential => (from player in players
            where (!player.Flying || roundsManager.FlyingPlayersCanPlay)
                  && player != players.OwnPlayer
                  && !player.IsAfk()
            select player)
            .ToArray();

        [EventListener]
        private void OnJoin(JoinEvent e)
        {
            e.Player.MetadataChanged += (sender, ev) =>
            {
                if (ev.Key != PlayerExtensions.Playing) return;

                if ((bool) ev.NewValue)
                {
                    new JoinRoundEvent(e.Player)
                        .RaiseIn(BotBits);
                }
                else
                {
                    new LeaveRoundEvent(e.Player)
                        .RaiseIn(BotBits);
                }
            };
        }

        internal void AddAllToRound()
        {
            foreach (var player in Potential)
            {
                player.AddToRound();
            }

            if (AfkExtension.IsLoadedInto(BotBits))
            {
                AfkService.Of(BotBits).ResetAutoAfk();
            }
        }

        internal void RemoveAllFromRound()
        {
            foreach (var player in Playing)
            {
                player.RemoveFromRound();
            }
        }

        [EventListener]
        private void OnFly(FlyEvent e)
        {
            if (e.Flying && !roundsManager.FlyingPlayersCanPlay)
                RemovePlayerFromRound(e.Player);
        }

        [EventListener]
        private void OnLeave(LeaveEvent e)
        {
            RemovePlayerFromRound(e.Player);
        }

        private void RemovePlayerFromRound(Player player)
        {
            player.RemoveFromRound();

            if (Playing.Length < roundsManager.MinimumPlayers &&
                (roundsManager.Running || roundsManager.Starting))
            {
                RoundsManager.Of(BotBits).ForceStop();
            }
        }
    }
}