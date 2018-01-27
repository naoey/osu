// Copyright (c) 2007-2018 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using osu.Framework.Allocation;
using osu.Game.Overlays.SearchableList;
using osu.Game.Overlays.Social;
using osu.Game.Users;

namespace osu.Game.Overlays.Chat
{
    public class UserSelectionOverlay : SocialOverlay
    {
        private ChatOverlay chat;

        [BackgroundDependencyLoader]
        private void load(ChatOverlay chat)
        {
            this.chat = chat;
        }

        protected override SocialPanel CreatePanelForStyle(PanelDisplayStyle style, User user)
        {
            var panel = base.CreatePanelForStyle(style, user);

            panel.Action = () => chat.StartUserChat(user);

            return panel;
        }
    }
}
