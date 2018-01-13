// Copyright (c) 2007-2018 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Graphics;
using osu.Game.Online.Chat;
using OpenTK;

namespace osu.Game.Overlays.Chat
{
    public class ChannelTabControl : ChatTabControl<Channel>
    {
        public ChannelTabControl()
        {
            AddInternal(new SpriteIcon
            {
                Icon = FontAwesome.fa_comments,
                Anchor = Anchor.CentreLeft,
                Origin = Anchor.CentreLeft,
                Size = new Vector2(20),
                Margin = new MarginPadding(10),
            });
        }

        protected override ChatTabItem CreateSelectorTab() => new ChannelSelectorTabItem(new Channel { Name = "+" });

        protected class ChannelSelectorTabItem : ChatTabItem
        {
            public override bool IsRemovable => false;

            public ChannelSelectorTabItem(Channel value) : base(value)
            {
                Depth = float.MaxValue;
                Width = 45;

                Icon.Alpha = 0;

                Text.TextSize = 45;
                TextBold.TextSize = 45;
            }

            [BackgroundDependencyLoader]
            private new void load(OsuColour colour)
            {
                BackgroundInactive = colour.Gray2;
                BackgroundActive = colour.Gray3;
            }
        }
    }
}
