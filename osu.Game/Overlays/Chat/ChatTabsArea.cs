// Copyright (c) 2007-2018 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using System;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Online.Chat;
using OpenTK;
using OpenTK.Graphics;

namespace osu.Game.Overlays.Chat
{
    public class ChatTabsArea : Container
    {
        public readonly ChannelTabControl ChannelTabs;
        public readonly UserTabControl UserTabs;
        public readonly Box Background;

        public BindableBool UserSelectorActive = new BindableBool();
        public BindableBool ChannelSelectorActive = new BindableBool();
        public Bindable<Channel> CurrentChannel = new Bindable<Channel>();

        public Action<Channel> OnRequestLeave;

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
                    Size = new Vector2(0.5f, 1f),
                    RelativeSizeAxes = Axes.Both,
                    OnRequestLeave = onRequestleave,
                },
                UserTabs = new UserTabControl
                {
                    Size = new Vector2(0.5f, 1f),
                    RelativeSizeAxes = Axes.Both,
                    Anchor = Anchor.TopRight,
                    Origin = Anchor.TopRight,
                    OnRequestLeave = onRequestleave,
                },
            };

            UserSelectorActive.BindTo(UserTabs.SelectorActive);
            ChannelSelectorActive.BindTo(ChannelTabs.SelectorActive);

            UserTabs.Current.ValueChanged += c => CurrentChannel.Value = c;
            ChannelTabs.Current.ValueChanged += c => CurrentChannel.Value = c;

            CurrentChannel.ValueChanged += channel =>
            {
                var newChannel = channel as UserChannel;

                if (newChannel != null)
                {
                    ChannelTabs.Current.Value = null;
                    UserTabs.Current.Value = newChannel;
                }
                else
                {
                    UserTabs.Current.Value = null;
                    ChannelTabs.Current.Value = channel;
                }
            };
        }

        private void onRequestleave(Channel c) => OnRequestLeave?.Invoke(c);

        public void AddChannel(Channel value)
        {
            var channel = value as UserChannel;

            if (channel != null)
                UserTabs.AddItem(channel);
            else
                ChannelTabs.AddItem(value);
        }

        public void RemoveChannel(Channel value)
        {
            var channel = value as UserChannel;

            if (channel != null)
                UserTabs.RemoveItem(channel);
            else
                ChannelTabs.RemoveItem(value);
        }
    }
}
