﻿using System.Linq;
using BotBits;
using BotBits.Events;
using BotBitsExt.Afk;
using BotBitsExt.Afk.Events;
using BotBitsExt.Rounds.Events;
using JetBrains.Annotations;

namespace BotBitsExt.Rounds
{
    /// <summary>
    ///     Round players controller.
    /// </summary>
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
        [UsedImplicitly]
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
                  && player.CanPlay()
            select player)
            .ToArray();

        [EventListener]
        private void On(JoinEvent e)
        {
            e.Player.SetCanPlay(true);

            e.Player.MetadataChanged += (sender, ev) =>
            {
                switch (ev.Key)
                {
                    case PlayerExtensions.PlayingMetadata:
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
                        break;

                    case PlayerExtensions.CanPlayMetadata:
                        new CanPlayEvent(e.Player, (bool) ev.NewValue).RaiseIn(BotBits);
                        break;
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
        private void On(FlyEvent e)
        {
            if (e.Flying && !roundsManager.FlyingPlayersCanPlay)
                RemovePlayerFromRound(e.Player);
        }

        [EventListener]
        private void On(LeaveEvent e)
        {
            RemovePlayerFromRound(e.Player);
        }

        [EventListener]
        private void On(AfkEvent e)
        {
            if (e.Afk)
                RemovePlayerFromRound(e.Player);
        }

        private void RemovePlayerFromRound(Player player)
        {
            player.RemoveFromRound();

            if (Playing.Length < roundsManager.MinimumPlayers && roundsManager.Running ||
                Potential.Length < roundsManager.MinimumPlayers && roundsManager.Starting)
            {
                RoundsManager.Of(BotBits).ForceStop();
            }
        }
    }
}