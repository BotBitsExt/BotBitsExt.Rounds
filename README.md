# BotBitsExt.Rounds [![Build status](https://ci.appveyor.com/api/projects/status/6343pau314ku0xy1?svg=true)](https://ci.appveyor.com/project/Tunous/botbitsext-rounds)

A BotBits extension that allows you to integrate automatic rounds management into your bots.

### Table of contents
 - [Installation](#installation)
 - [Player management](#player-management)
   - [Playing players](#playing)
   - [Potential players](#potential)
 - [Player extension methods](#player-extensions)
   - [Determing if player is playing](#is-playing)
   - [Locking player out of game](#can-play)
 - [Events](#events)
 - [Commands](#commands)

# <a id="installation">Installation</a>

To activate the extension you have to load it into your bot with `LoadInto` method.
It accepts a few settings that you can change to the modify behaviour of the round manager.

```csharp
int minimumPlayers = 2;                 // Number of players required to start new round
int waitTime = 15;                      // For how long to wait before starting new round (in seconds)
bool flyingPlayersCanPlay = false;      // Are flying players also set as playing? (false by default)

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

And then the last part required is to use `RoundsManager.Of(BotBits).ForceStop()` every time when round ends.
If you don't do it rounds would only end after number of playing players decreased under required minimum.

# <a id="player-management">Player management</a>

BotBitsExt.Rounds provides you with a set of properties that can make it easy to manage and list specific players.

All of the player management properties are contained in `RoundPlayers` package.
You can access it exactly how you would do it for builtin BotBits packages.

```csharp
RoundPlayers.Of(BotBits)
```

## <a id="playing">Playing players</a>

```csharp
Player[] playingPlayers = RoundPlayers.Of(BotBits).Playing
```

This property list all players that are currently playing running round.
It is exactly the same as getting all the players which have `Player.IsPlaying` metadata set to true.

## <a id="potential">Potential players</a>

```csharp
Player[] potentialPlayers = RoundPlayers.Of(BotBits).Potential
```

This property lists all players that would be added to the round if it was about to start when called.

Only players which met these requirements are added:

- Player is not the bot,
- Player is not afk,
  - Only determined if you enable the `AfkExtension`.
- Player can play,
  - Controled by bot creator with `Player.SetCanPlay()` extension method.
- Player is not flying, unless flying players were also allowed to play rounds.

# <a id="player-extensions">Player extensions methods</a>

In addition to Player management package BotBitsExt.Rounds adds few extension methods to the `Player` class which you can use.

## <a id="is-playing">Determining if player is playing</a>

`bool Player.IsPlaying()`

This extension method specifies if player is playing the game.
You can manually modify it by adding or removing player from rounds by using `Player.AddToRound()` and `Player.RemoveFromRound()` extension methods.

## <a id="can-play">Locking player out of game</a>

If you don't want some players to be able to play the game you can use the `Player.SetCanPlay()` extension method and lock them out from being able to join rounds.
To check if player is allowed to join new rounds use `Player.CanPlay()` extension method.

# <a id="events">Events</a>

- `CanPlayEvent`

Raised when player is allowed or disallowed to join rounds. It only happens when you use the `Player.SetCanPlay()` extension method.

- `JoinRoundEvent`

Raised when player is added to round. Happens automatically on round start or when you use `Player.AddToRound()` extension method.

- `LeaveRoundEvent`

Similar to `JoinRoundEvent` but raised when player is removed from round.

- `RoundStartFailedEvent`

Raised when start fails during the preparation time (`waitTime` which you set when loading `RoundsExtension`).
It contains `EnoughPlayers` property which is set to false when there weren't enough players to continue starting new round. Here is an example of how to use it to inform about missing players:

```csharp
[EventListener]
void OnStartFailed(RoundStartFailedEvent e)
{
    if (!e.EnoughPlayers)
        Chat.Of(BotBits).Say("Not enough players to start new round. Waiting for more...");
}
```

The `EnoughPlayers` property is set to true when round start was stopped by !stop command or other player action. Otherwise the event is sent because of missing players.

- `StartingRoundEvent`

Raised each time when new round is starting. It provides time telling for how long bot will wait before starting new round.
You can use it to inform players about new round or preform preparation tasks.

```csharp
[EventListener]
void OnStartingRound(StartingRoundEvent e)
{
    Chat.Of(BotBits).Say("New round will start in {0} seconds!", e.WaitTime);
}
```

- `StartRoundEvent`
- `StopRoundEvent`

Raised shortly before round starts or after it finished.
They contain `Players` property listing all the players that were playing the round.

# <a id="commands">Commands</a>

- `!start`

Stops running round and starts new without delay before start.

- `!stop`

Stops currently running round. After it's used new rounds can start automatically as always.
 
- `!on`

Enables automatic starting of rounds.

- `!off`

Disables automatic starting of rounds.
It has the same effect as setting `RoundsManager.Enabled` property to false.