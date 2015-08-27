using System;
using System.Linq;
using BotBits;
using BotBits.Events;
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
        /// Gets the playing players.
        /// </summary>
        /// <value>The playing players.</value>
        public Player[] Playing
        {
            get
            {
                return (from player in players
                                    where player.IsPlaying()
                                    select player)
                        .ToArray();
            }
        }

        /// <summary>
        /// Gets the players that can join new round.
        /// </summary>
        /// <value>The players that can join new round.</value>
        public Player[] Potential
        {
            get
            {
                return (from player in players
                                    where (!player.Flying || roundsManager.FlyingPlayersCanPlay)
                                        && player != players.OwnPlayer
                                    select player)
                        .ToArray();
            }
        }

        [EventListener]
        private void OnJoin(JoinEvent e)
        {
            e.Player.MetadataChanged += (sender, ev) =>
                {
                    if (ev.Key == PlayerExtensions.Playing)
                    {
                        if ((bool)ev.NewValue)
                        {
                            new JoinRoundEvent(e.Player)
                                .RaiseIn(BotBits);
                        }
                        else
                        {
                            new LeaveRoundEvent(e.Player)
                                .RaiseIn(BotBits);
                        }
                    }
                };
        }

        internal void AddAllToRound()
        {
            foreach (var player in Potential)
            {
                player.AddToRound();
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
        private void OnLeaveRound(LeaveRoundEvent e)
        {
            if (Playing.Length < roundsManager.MinimumPlayers && roundsManager.Running)
            {
                RoundsManager.Of(BotBits).ForceStop();
            }
        }

        [EventListener]
        private void OnFly(FlyEvent e)
        {
            if (e.Flying && !roundsManager.FlyingPlayersCanPlay)
                e.Player.RemoveFromRound();
        }

        [EventListener]
        private void OnLeave(LeaveEvent e)
        {
            e.Player.RemoveFromRound();
        }
    }
}

