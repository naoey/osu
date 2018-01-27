// Copyright (c) 2007-2018 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using osu.Framework.Allocation;
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
        private DependencyContainer dependencies;

        protected override IReadOnlyDependencyContainer CreateLocalDependencies(IReadOnlyDependencyContainer parent) => dependencies = new DependencyContainer(parent);

        public override IReadOnlyList<Type> RequiredTypes => new[]
        {
            typeof(ChatTabsArea),
            typeof(ChannelTabControl),
            typeof(UserTabControl),
            typeof(ChatOverlay),
        };

        private readonly ChatOverlay overlay;

        private ChatTabsArea area;

        public TestCaseChatDisplay()
        {
            overlay = new ChatOverlay
            {
                State = Visibility.Hidden,
            };
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            dependencies.Cache(overlay);

            Add(overlay);
            Add(area = new ChatTabsArea
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Height = 50f,
                Alpha = 0,
            });

            area.ChannelTabs.OnRequestLeave += removeChannelTabItem;
            area.UserTabs.OnRequestLeave += removeUserTabItem;

            AddStep("Toggle ChatTabsArea", () => area.Alpha = area.Alpha = 1 - area.Alpha);
            AddStep("Toggle ChatOverlay", () => overlay.ToggleVisibility());
            AddStep("Add channel tab item", addChannelTabItem);
            AddStep("Add user tab item", addUserTabItem);
        }

        private void addChannelTabItem() => area.ChannelTabs.AddItem(new Channel
        {
            Name = $"Channel {area.ChannelTabs.OpenTabs.Count()}",
        });

        private void removeChannelTabItem(Channel value) => area.ChannelTabs.RemoveItem(value);

        private void addUserTabItem()
        {
            var user = new User
            {
                Username = $"User {area.UserTabs.OpenTabs.Count()}",
                Colour = @"6644cc",
                Id = 2,
                Country = new Country { FlagName = @"AU" },
                CoverUrl = @"https://osu.ppy.sh/images/headers/profile-covers/c3.jpg",
                IsSupporter = true,
            };

            area.UserTabs.AddItem(new UserChannel(user, new User()));
        }

        private void removeUserTabItem(UserChannel value) => area.UserTabs.RemoveItem(value);
    }
}
