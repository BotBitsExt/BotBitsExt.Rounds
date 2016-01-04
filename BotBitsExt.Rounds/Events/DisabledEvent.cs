using BotBits;

namespace BotBitsExt.Rounds.Events
{
    /// <summary>
    ///     Event raised when <see cref="RoundsManager" /> is disabled.
    /// </summary>
    public sealed class DisabledEvent : Event<DisabledEvent>
    {
        internal DisabledEvent()
        {
        }
    }
}