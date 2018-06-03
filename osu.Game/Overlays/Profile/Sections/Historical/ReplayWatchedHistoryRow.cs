using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics.Sprites;
using osu.Game.Graphics.UserInterface;
using osu.Game.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace osu.Game.Overlays.Profile.Sections.Historical
{
    class ReplayWatchedHistoryRow : Container
    {
        private Bindable<User> user;

        private ReplaysWatchedGraph graph;

        public ReplayWatchedHistoryRow(Bindable<User> user)
        {
            this.user = user;

            RelativeSizeAxes = Axes.X;
            Height = 285;

            user.ValueChanged += userChanged;

            Children = new Drawable[]
            {
                new OsuSpriteText
                {
                    TextSize = 15,
                    Text = @"Replays Watched History",
                    Font = "Exo2.0-RegularItalic",
                    Margin = new MarginPadding { Top = 10, Bottom = 10 },
                },
                graph = new ReplaysWatchedGraph
                {
                    RelativeSizeAxes = Axes.X,
                    Height = 180,
                    Margin = new MarginPadding { Top = 40 },
                },
            };
        }

        private void userChanged(User user)
        {
            graph.ReplaysWatchedCounts = user.ReplaysWatchedCounts.ToList();
        }

        private class ReplaysWatchedGraph : LineGraph
        {
            private List<User.HistoricalCount> replaysWatchedCounts;
            public List<User.HistoricalCount> ReplaysWatchedCounts
            {
                private get => replaysWatchedCounts;
                set
                {
                    if (value == replaysWatchedCounts) return;

                    replaysWatchedCounts = value;

                    if (replaysWatchedCounts.Count == 1)
                    {
                        // Add another zero count for the month before so that we have a graph
                        DateTime date = replaysWatchedCounts[0].StartDate;

                        // TODO: clear this hax and use the right way to get -1 month from date in C#
                        replaysWatchedCounts.Insert(0, new User.HistoricalCount { Count = 0, StartDate = new DateTime(date.Year, date.Month - 1, date.Day) });
                    }

                    Values = replaysWatchedCounts.Select(p => (float)p.Count);
                    updateRulers();
                }
            }

            private void updateRulers() { }
        }
    }
}
