﻿using ShortDash.Core.Plugins;

namespace ShortDash.Plugins.Core.Windows
{
    [ShortDashAction(
        Title = "Play/Pause Media [Windows]",
        Description = "Toggles the currently playing system media.")]
    [ShortDashActionDefaultSettings(
        Label = "Play/Pause",
        Icon = "fas fa-play")]
    public class MediaPlayPauseAction : KeyboardActionBase
    {
        private readonly IShortDashPluginLogger<MediaPlayPauseAction> logger;

        public MediaPlayPauseAction(IShortDashPluginLogger<MediaPlayPauseAction> logger)
        {
            this.logger = logger;
        }

        public override void ExecuteKeyboardAction()
        {
            logger.LogDebug("Sending play/pause keyboard events.");
            PressKey(0xB3 /* VK_MEDIA_PLAY_PAUSE */);
        }
    }
}