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

And then the last part required is to use `RoundsManager.Of(BotBits).ForceStop()` each time when round ends. If you don't do it rounds would only end after number of playing players decreased under required minimum.

# Additional events

In addition you can listen to `StartingRoundEvent` and `RoundStartFailedEvent` to preform custom actions.

`StartingRoundEvent` is sent each time when new round is starting. It provides time telling for how long bot will wait before starting new round. You can use it to inform players about new round or preform preparation tasks.

```csharp
[EventListener]
void OnStartingRound(StartingRoundEvent e)
{
    Chat.Of(BotBits).Say("New round will start in {0} seconds!", e.WaitTime);
}
```

`RoundStartFailedEvent` is sent when start fails during the preparation time (`waitTime` which you set when loading `RoundsExtension`). Here is an example of how to use it to inform about missing players:

```csharp
[EventListener]
void OnStartFailed(RoundStartFailedEvent e)
{
    Chat.Of(BotBits).Say("Not enough players to start new round. Waiting for more...");
}
```
