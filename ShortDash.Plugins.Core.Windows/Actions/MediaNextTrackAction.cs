﻿using ShortDash.Core.Plugins;

namespace ShortDash.Plugins.Core.Windows
{
    public class MediaNextTrackAction : KeyboardActionBase
    {
        private readonly IShortDashPluginLogger<MediaNextTrackAction> logger;

        public MediaNextTrackAction(IShortDashPluginLogger<MediaNextTrackAction> logger)
        {
            this.logger = logger;
        }

        public override string Description => "Goes to the next track for the current system media.";

        public override string Title => "Next Track";

        public override bool Execute(object parametersObject, ref bool toggleState)
        {
            logger.LogDebug("Sending next track keyboard events.");
            PressKey(0xB0 /* VK_MEDIA_NEXT_TRACK */);
            return true;
        }
    }
}