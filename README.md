# BotBitsExt.Rounds

A BotBits extension that allows you to integrate automatic rounds management with your bots.

# Installation

To activate the extension you have to load it into your bot with `LoadInto` method.
It accepts few settings that you can change to modify behaviour of the round manager.

```csharp
int minimumPlayers = 2;                 // Number of players required to start new round
int waitTime = 15;                      // For how long to wait before starting new round (in seconds)
bool flyingPlayersCanPlay = false;      // Are flying players also set as playing?

RoundsExtension.LoadInto(bot, minimumPlayers, waitTime, flyingPlayersCanPlay);
```

Once it's activated you should enable it when you are ready.
For example after receiving `JoinComplete` event:

```csharp
[EventListener]
void OnJoinComplete(JoinCompleteEvent e)
{
    RoundsManager.Of(BotBits).Enabled = true;
}
```

And then the last part required is to send `FinishRoundEvent` each time when round ends. If you didn't do it rounds would only end after number of playing players decreased under required minimum.

```csharp
new FinishRoundEvent().RaiseIn(BotBits);
```
