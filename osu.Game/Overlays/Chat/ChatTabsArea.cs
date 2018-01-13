// Copyright (c) 2007-2018 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using OpenTK.Graphics;

namespace osu.Game.Overlays.Chat
{
    public class ChatTabsArea : Container
    {
        public readonly ChannelTabControl ChannelTabs;
        public readonly UserTabControl UserTabs;

        public ChatTabsArea()
        {
            Name = @"chat tabs";
            RelativeSizeAxes = Axes.X;
            Children = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.Black,
                },
                ChannelTabs = new ChannelTabControl
                {
                    RelativeSizeAxes = Axes.Both,
                },
                UserTabs = new UserTabControl
                {
                    RelativeSizeAxes = Axes.Both,
                }
            };
        }
    }
}
