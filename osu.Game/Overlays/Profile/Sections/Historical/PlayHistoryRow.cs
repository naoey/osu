using Humanizer;
using OpenTK.Graphics;
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
    public class PlayHistoryRow : Container
    {
        private readonly Bindable<User> user;

        private PlayHistoryGraph graph;

        public PlayHistoryRow(Bindable<User> user)
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
                    Text = @"Play History",
                    Font = "Exo2.0-RegularItalic",
                    Margin = new MarginPadding { Top = 10, Bottom = 10 },
                },
                graph = new PlayHistoryGraph
                {
                    RelativeSizeAxes = Axes.X,
                    Height = 180,
                    Margin = new MarginPadding { Top = 40 },
                },
            };
        }

        private void userChanged(User user)
        {
            graph.PlayCounts = user.MonthlyPlayCounts.ToList();
        }

        private class PlayHistoryGraph : LineGraph
        {
            private List<User.HistoricalCount> playCounts;
            public List<User.HistoricalCount> PlayCounts
            {
                private get => playCounts;
                set
                {
                    if (value == playCounts) return;

                    playCounts = value;

                    if (playCounts.Count == 1)
                    {
                        // Add another zero count for the month before so that we have a graph
                        DateTime date = playCounts[0].StartDate;

                        // TODO: clear this hax and use the right way to get -1 month from date in C#
                        playCounts.Insert(0, new User.HistoricalCount { Count = 0, StartDate = new DateTime(date.Year, date.Month - 1, date.Day) });
                    }

                    Values = playCounts.Select(p => (float)p.Count);
                    updateRulers();
                }
            }

            private void updateRulers()
            {
                int horizontalRulerCount = (int)Math.Round(Values.Max() / 500);
                float horizontalRulerSpacing = 180 / horizontalRulerCount;

                for (int i = 0; i <= horizontalRulerCount; i++)
                {
                    Add(new Container
                    {
                        RelativeSizeAxes = Axes.X,
                        AutoSizeAxes = Axes.Y,
                        Y = horizontalRulerSpacing * (i + 1),
                        RelativePositionAxes = Axes.Y,
                        Children = new Drawable[]
                        {
                            new OsuSpriteText
                            {
                                Text = $@"{500 * (i + 1)}",
                                Anchor = Anchor.CentreLeft,
                                Origin = Anchor.CentreLeft,
                            },
                            new Container
                            {
                                RelativeSizeAxes = Axes.X,
                                Height = 3,
                                BorderColour = Color4.Gray,
                                BorderThickness = 3,
                                Margin = new MarginPadding { Left = 10 },
                                Anchor = Anchor.CentreLeft,
                                Origin = Anchor.CentreLeft,
                            },
                        },
                    });
                }

                float verticalRulerSpacing = Width / playCounts.Count();

                for (int i = 0; i < playCounts.Count(); i++)
                {
                    Add(new Container
                    {
                        RelativeSizeAxes = Axes.Y,
                        AutoSizeAxes = Axes.X,
                        X = verticalRulerSpacing * (i + 1),
                        RelativePositionAxes = Axes.X,
                        Children = new Drawable[]
                        {
                            new OsuSpriteText
                            {
                                Text = playCounts[i].StartDate.Humanize(),
                                Rotation = 45,
                                Anchor = Anchor.BottomCentre,
                                Origin = Anchor.BottomCentre,
                            },
                            new Container
                            {
                                RelativeSizeAxes = Axes.Y,
                                Width = 3,
                                BorderColour = Color4.Gray,
                                BorderThickness = 3,
                                Margin = new MarginPadding { Bottom = 10 },
                                Anchor = Anchor.BottomCentre,
                                Origin = Anchor.BottomCentre,
                            },
                        }
                    });
                }
            }
        }
    }
}
