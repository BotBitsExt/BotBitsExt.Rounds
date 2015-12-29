using BotBits;
using BotBits.Commands;
using BotBits.Permissions;

namespace BotBitsExt.Rounds
{
    internal sealed class Commands : Package<Commands>
    {
        private bool requireModerator;
        private RoundsManager roundsManager;

        public Commands()
        {
            InitializeFinish += (sender, e) =>
            {
                roundsManager = RoundsManager.Of(BotBits);

                if (!CommandsExtension.IsLoadedInto(BotBits))
                    return;

                CommandLoader.Of(BotBits).Load(this);

                requireModerator = PermissionsExtension.IsLoadedInto(BotBits);
            };
        }

        private void RequireModerator(IInvokeSource source)
        {
            if (requireModerator)
                Group.Moderator.RequireFor(source);
        }

        [Command(0, "start")]
        private void StartCommand(IInvokeSource source, ParsedRequest request)
        {
            RequireModerator(source);

            if (!roundsManager.Enabled)
            {
                source.Reply("Bot is not enabled!");
            }
            else
            {
                source.Reply("Starting new round...");
                roundsManager.ForceStart();
            }
        }

        [Command(0, "stop")]
        private void StopCommand(IInvokeSource source, ParsedRequest request)
        {
            RequireModerator(source);

            if (!roundsManager.Enabled)
            {
                source.Reply("Bot is not enabled!");
            }
            else
            {
                source.Reply("Stopping round...");
                roundsManager.ForceStop();
            }
        }

        [Command(0, "on")]
        private void OnCommand(IInvokeSource source, ParsedRequest request)
        {
            RequireModerator(source);

            if (roundsManager.Enabled)
            {
                source.Reply("Bot is already enabled.");
            }
            else
            {
                source.Reply("Enabling bot...");
                roundsManager.Enabled = true;
            }
        }

        [Command(0, "off")]
        private void OffCommand(IInvokeSource source, ParsedRequest request)
        {
            RequireModerator(source);

            if (!roundsManager.Enabled)
            {
                source.Reply("Bot is already disabled.");
            }
            else
            {
                source.Reply("Disabling bot...");
                roundsManager.Enabled = false;
            }
        }
    }
}