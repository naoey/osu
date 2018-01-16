// Copyright (c) 2007-2018 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics;
using osu.Game.Online.Chat;
using osu.Game.Overlays;
using osu.Game.Overlays.Chat;
using osu.Game.Users;

namespace osu.Game.Tests.Visual
{
    [Description("Testing chat api and overlay")]
    public class TestCaseChatDisplay : OsuTestCase
    {
        public override IReadOnlyList<Type> RequiredTypes => new[]
        {
            typeof(ChatTabsArea),
            typeof(ChannelTabControl),
            typeof(UserTabControl),
            typeof(ChatOverlay),
        };

        private readonly ChatOverlay overlay;
        private readonly ChatTabsArea area;

        public TestCaseChatDisplay()
        {
            Add(overlay = new ChatOverlay
            {
                State = Visibility.Hidden,
            });

            Add(area = new ChatTabsArea
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Height = 50f,
            });

            area.ChannelTabs.OnRequestLeave += removeChannelTabItem;
            area.UserTabs.OnRequestLeave += removeUserTabItem;

            AddStep("Add channel tab item", addChannelTabItem);
            AddStep("Add user tab item", addUserTabItem);
            AddStep("Toggle ChatTabsArea", () => area.Alpha = area.Alpha = 1 - area.Alpha);
            AddStep("Toggle ChatOverlay", () => overlay.ToggleVisibility());
        }

        private void addChannelTabItem() => area.ChannelTabs.AddItem(new Channel
        {
            Name = $"Channel {area.ChannelTabs.OpenTabs.Count()}",
        });

        private void removeChannelTabItem(Channel value) => area.ChannelTabs.RemoveItem(value);

        private void addUserTabItem() => area.UserTabs.AddItem(new UserChannel
        {
            User = new User
            {
                Username = $"User {area.UserTabs.OpenTabs.Count()}",
                Colour = @"6644cc",
                Id = 2,
                Country = new Country { FlagName = @"AU" },
                CoverUrl = @"https://osu.ppy.sh/images/headers/profile-covers/c3.jpg",
                IsSupporter = true,
            },
        });

        private void removeUserTabItem(UserChannel value) => area.UserTabs.RemoveItem(value);
    }
}
