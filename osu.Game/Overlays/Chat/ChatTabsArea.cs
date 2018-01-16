// Copyright (c) 2007-2018 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using OpenTK;
using OpenTK.Graphics;

namespace osu.Game.Overlays.Chat
{
    public class ChatTabsArea : Container
    {
        public readonly ChannelTabControl ChannelTabs;
        public readonly UserTabControl UserTabs;
        public readonly Box Background;

        public ChatTabsArea()
        {
            Name = @"chat tabs";
            RelativeSizeAxes = Axes.X;
            Size = new Vector2(1, 50);
            Children = new Drawable[]
            {
                Background = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.Black,
                },
                ChannelTabs = new ChannelTabControl
                {
                    Width = 0.5f,
                    RelativeSizeAxes = Axes.Both,
                },
                UserTabs = new UserTabControl
                {
                    Width = 0.5f,
                    RelativeSizeAxes = Axes.Both,
                },
            };
        }
    }
}
