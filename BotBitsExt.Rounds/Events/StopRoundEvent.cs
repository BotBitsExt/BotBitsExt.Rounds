using BotBits;

namespace BotBitsExt.Rounds.Events
{
    /// <summary>
    ///     Event raised when currently running round stopped.
    /// </summary>
    public sealed class StopRoundEvent : Event<StopRoundEvent>
    {
        internal StopRoundEvent()
        {
        }
    }
}