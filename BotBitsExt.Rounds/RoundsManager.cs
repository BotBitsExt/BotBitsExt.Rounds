using System.Threading;
using System.Threading.Tasks;
using BotBits;
using BotBits.Events;
using BotBitsExt.Afk.Events;
using BotBitsExt.Rounds.Events;
using JetBrains.Annotations;

namespace BotBitsExt.Rounds
{
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
                enabled = value;

                if (enabled)
                {
                    if (!Running)
                    {
                        CheckGameStart();
                    }
                }
                else
                {
                    if (Running)
                    {
                        ForceStop();
                    }
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

        [EventListener]
        private void OnJoin(JoinEvent e)
        {
            CheckGameStart();
        }

        [EventListener]
        private void OnFly(FlyEvent e)
        {
            if (!e.Flying && !FlyingPlayersCanPlay)
                CheckGameStart();
        }

        [EventListener]
        private void On(MoveEvent e)
        {
            CheckGameStart();
        }

        [EventListener]
        private void On(AfkEvent e)
        {
            if (!e.Afk)
                CheckGameStart();
        }

        [EventListener]
        private void On(AutoAfkEvent e)
        {
            if (!e.AutoAfk)
                CheckGameStart();
        }

        private async void CheckGameStart()
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
                    CheckGameStart();

                return;
            }

            Running = false;

            var leftPlayers = players.Playing;
            players.RemoveAllFromRound();
            new StopRoundEvent(leftPlayers).RaiseIn(BotBits);

            if (restart)
                CheckGameStart();
        }
    }
}