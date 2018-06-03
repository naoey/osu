// Copyright (c) 2007-2018 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using System;
using System.Collections.Generic;
using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.MathUtils;
using osu.Game.Graphics;
using osu.Game.Overlays.Profile.Sections;
using osu.Game.Overlays.Profile.Sections.Historical;
using osu.Game.Users;

namespace osu.Game.Tests.Visual
{
    [TestFixture]
    public class TestCaseUserProfileHistoricalSection : OsuTestCase
    {
        public override IReadOnlyList<Type> RequiredTypes =>
            new[]
            {
                typeof(HistoricalSection),
                typeof(PaginatedMostPlayedBeatmapContainer),
                typeof(DrawableMostPlayedRow),
                typeof(DrawableProfileRow),
                typeof(PlayHistoryRow),
                typeof(ReplayWatchedHistoryRow),
                typeof(HistoricalGraph),
            };

        private HistoricalSection section;

        public TestCaseUserProfileHistoricalSection()
        {

            Add(new Box
            {
                RelativeSizeAxes = Axes.Both,
                Colour = OsuColour.Gray(0.2f)
            });

            Add(new ScrollContainer
            {
                RelativeSizeAxes = Axes.Both,
                Child = section = new HistoricalSection(),
            });

            AddStep("Show graphs", showUserWithGraphs);
            AddStep("Show peppy", () => section.User.Value = new User { Id = 2 });
            AddStep("Show WubWoofWolf", () => section.User.Value = new User { Id = 39828 });
        }

        private void showUserWithGraphs()
        {
            User userWithHistoricalCounts = new User { Id = 2, MonthlyPlayCounts = new User.HistoricalCount[8], ReplaysWatchedCounts = new User.HistoricalCount[8] };

            for (int i = 0; i < 8; i++)
            {
                userWithHistoricalCounts.MonthlyPlayCounts[i] = userWithHistoricalCounts.ReplaysWatchedCounts[i] = new User.HistoricalCount
                {
                    StartDate = new DateTime(2017, 12 - i, 1),
                    Count = RNG.Next(0, 3000),
                };
            }

            section.User.Value = userWithHistoricalCounts;
        }
    }
}
