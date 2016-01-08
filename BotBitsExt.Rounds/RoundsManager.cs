using System.Threading;
using System.Threading.Tasks;
using BotBits;
using BotBits.Events;
using BotBitsExt.Afk.Events;
using BotBitsExt.Rounds.Events;
using JetBrains.Annotations;

namespace BotBitsExt.Rounds
{
    /// <summary>
    ///     The rounds manager. Used to control round starting and stopping.
    /// </summary>
    public sealed class RoundsManager : EventListenerPackage<RoundsManager>
    {
        private CancellationTokenSource cts = new CancellationTokenSource();
        private bool enabled;
        private RoundPlayers players;

        public RoundsManager()
        {
            InitializeFinish += (sender, e) =>
                players = RoundPlayers.Of(BotBits);
        }

        /// <summary>
        ///     Gets or sets the minimum amount of players required to start new round.
        /// </summary>
        /// <value>The minimum amount of players required to start new round.</value>
        public int MinimumPlayers { get; internal set; }

        /// <summary>
        ///     Gets or sets the wait time before starting each round. (in seconds)
        /// </summary>
        /// <value>The wait time before each round.</value>
        [UsedImplicitly]
        public int WaitTime { get; internal set; }

        /// <summary>
        ///     Gets or sets a value indicating whether flying players can join game.
        /// </summary>
        /// <value><c>true</c> if flying players can join game; otherwise, <c>false</c>.</value>
        public bool FlyingPlayersCanPlay { get; internal set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="RoundsManager" /> is enabled.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        public bool Enabled
        {
            get { return enabled; }
            set
            {
                if (enabled == value)
                    return;

                enabled = value;

                if (enabled)
                {
                    new EnabledEvent().RaiseIn(BotBits);
                }
                else
                {
                    new DisabledEvent().RaiseIn(BotBits);
                }
            }
        }

        /// <summary>
        ///     Gets a value indicating whether round is running.
        /// </summary>
        /// <value><c>true</c> if round is running; otherwise, <c>false</c>.</value>
        public bool Running { get; private set; }

        /// <summary>
        ///     Gets a value indicating whether new round is starting.
        /// </summary>
        /// <value><c>true</c> if new round is starting; otherwise, <c>false</c>.</value>
        public bool Starting { get; private set; }

        /// <summary>
        ///     Forces the start of new round.
        /// </summary>
        public void ForceStart()
        {
            if (!Enabled)
                return;

            // Stop already running round
            ForceStop(false);

            Running = true;
            players.AddAllToRound();
            new StartRoundEvent(players.Playing).RaiseIn(BotBits);
        }

        /// <summary>
        ///     Forces the stop of the currently running round.
        /// </summary>
        /// <param name="restart">
        ///     Tells whether new round should start after stopping currently
        ///     running one.
        /// </param>
        public void ForceStop(bool restart = true)
        {
            if (!Running)
            {
                if (!Starting) return;

                // Cancel starting round
                cts.Cancel();
                cts = new CancellationTokenSource();

                if (restart)
                    CheckRoundStart();

                return;
            }

            Running = false;

            var leftPlayers = players.Playing;
            players.RemoveAllFromRound();
            new StopRoundEvent(leftPlayers).RaiseIn(BotBits);

            if (restart)
                CheckRoundStart();
        }

        #region Round start/stop checking

        [EventListener]
        private void On(JoinEvent e)
        {
            CheckRoundStart();
        }

        [EventListener]
        private void On(FlyEvent e)
        {
            if (!e.Flying && !FlyingPlayersCanPlay)
                CheckRoundStart();
        }

        [EventListener]
        private void On(MoveEvent e)
        {
            CheckRoundStart();
        }

        [EventListener]
        private void On(AfkEvent e)
        {
            if (!e.Afk)
                CheckRoundStart();
        }

        [EventListener]
        private void On(AutoAfkEvent e)
        {
            if (!e.AutoAfk)
                CheckRoundStart();
        }

        [EventListener]
        private void On(EnabledEvent e)
        {
            if (!Running)
            {
                CheckRoundStart();
            }
        }

        [EventListener]
        private void On(DisabledEvent e)
        {
            if (Running)
            {
                ForceStop(false);
            }
        }

        private async void CheckRoundStart()
        {
            if (Running || Starting || !Enabled || players.Potential.Length < MinimumPlayers) return;

            if (WaitTime > 0)
            {
                // Notify about newly starting round
                new StartingRoundEvent(players.Potential, WaitTime).RaiseIn(BotBits);

                try
                {
                    Starting = true;
                    await Task.Delay(WaitTime*1000, cts.Token);
                }
                catch (TaskCanceledException)
                {
                    // Notify about round start failure
                    new RoundStartFailedEvent(players.Potential.Length >= MinimumPlayers).RaiseIn(BotBits);
                    return;
                }
                finally
                {
                    Starting = false;
                }
            }

            ForceStart();
        }

        #endregion
    }
}