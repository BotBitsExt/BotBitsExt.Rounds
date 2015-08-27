using System;
using BotBits;
using BotBits.Events;
using BotBitsExt.Rounds.Events;
using System.Threading.Tasks;
using System.Threading;

namespace BotBitsExt.Rounds
{
    public sealed class RoundsManager : EventListenerPackage<RoundsManager>
    {
        private RoundPlayers players;
        private CancellationTokenSource cts = new CancellationTokenSource();
        private bool starting;

        public RoundsManager()
        {
            InitializeFinish += (sender, e) =>
                players = RoundPlayers.Of(BotBits);
        }

        /// <summary>
        /// Gets or sets the amount of minimum players required to start new round.
        /// </summary>
        /// <value>The minimum amount of players required to start new round.</value>
        public int MinimumPlayers { get; internal set; }

        /// <summary>
        /// Gets or sets the wait time before starting each round. (in seconds)
        /// </summary>
        /// <value>The wait time before each round.</value>
        public int WaitTime { get; internal set; }

        /// <summary>
        /// Gets or sets a value indicating whether flying players can join game.
        /// also play.
        /// </summary>
        /// <value><c>true</c> if flying players can join game; otherwise, <c>false</c>.</value>
        public bool FlyingPlayersCanPlay { get; internal set; }

        private bool _enabled;
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="BotBitsExt.Rounds.RoundsManager"/> is enabled.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        public bool Enabled
        {
            get
            {
                return _enabled;
            }
            set
            {
                _enabled = value;

                if (_enabled)
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

                    if (starting)
                    {
                        cts.Cancel();
                    }
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether round is running.
        /// </summary>
        /// <value><c>true</c> if running; otherwise, <c>false</c>.</value>
        public bool Running { get; private set; }

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

        private async void CheckGameStart()
        {
            if (!Running && Enabled && players.Potential.Length >= MinimumPlayers)
            {
                if (WaitTime > 0)
                {
                    try
                    {
                        starting = true;
                        await Task.Delay(WaitTime * 1000, cts.Token);
                        starting = false;
                    }
                    catch (TaskCanceledException)
                    {
                        return;
                    }
                }

                ForceStart();
            }
        }

        /// <summary>
        /// Forces the start of new round.
        /// </summary>
        public void ForceStart()
        {
            // Stop already running round
            if (Running)
            {
                ForceStop();
            }

            Running = true;
            players.AddAllToRound();
            new StartRoundEvent().RaiseIn(BotBits);
        }

        /// <summary>
        /// Forces the stop of currently running round.
        /// </summary>
        public void ForceStop()
        {
            if (!Running)
                return;

            Running = false;
            players.RemoveAllFromRound();
            new StopRoundEvent().RaiseIn(BotBits);

            CheckGameStart();
        }
    }
}

