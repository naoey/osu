using OpenTK.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Graphics.UserInterface;
using osu.Game.Users;
using System;
using System.Collections.Generic;
using System.Linq;

namespace osu.Game.Overlays.Profile.Sections.Historical
{
    public class HistoricalGraph : Container
    {
        private List<User.HistoricalCount> counts;
        public List<User.HistoricalCount> Counts
        {
            private get => counts;
            set
            {
                if (value == counts) return;

                counts = value;

                if (counts.Count == 1)
                {
                    // Add another zero count for the month before so that we have a line to show
                    DateTime date = counts[0].StartDate;

                    // TODO: clear this hax and use the right way to get -1 month from date in C#
                    counts.Insert(0, new User.HistoricalCount { Count = 0, StartDate = new DateTime(date.Year, date.Month - 1, date.Day) });
                }

                graph.Values = counts.Select(p => (float)p.Count);
                updateRulers();
            }
        }

        private readonly LineGraph graph;
        private readonly float yInterval;
        private readonly int xInterval;

        /// <summary>
        /// Create a historical graph of time (in months) vs. value.
        /// </summary>
        /// <param name="yInterval">The interval of the Y-axis guides (value).</param>
        /// <param name="xInterval">The interval of the X-axis guides (months).</param>
        public HistoricalGraph(float yInterval = 100, int xInterval = 3)
        {
            this.yInterval = yInterval;
            this.xInterval = xInterval;

            Child = graph = new LineGraph
            {
                Margin = new MarginPadding { Left = 50 },
                RelativeSizeAxes = Axes.X,
                Height = 180,
            };
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            graph.Colour = colours.PinkDarker;
        }

        private void updateRulers()
        {
            int horizontalRulerCount = (int)Math.Round(graph.Values.Max() / 500);
            float horizontalRulerSpacing = horizontalRulerCount <= 0 ? 0 : 180 / horizontalRulerCount;

            for (int i = horizontalRulerCount - 1; i >= 0; i--)
            {
                Add(new Container
                {
                    Anchor = Anchor.TopLeft,
                    Origin = Anchor.TopLeft,
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    Y = horizontalRulerSpacing * i,
                    Children = new Drawable[]
                    {
                        new OsuSpriteText
                        {
                            Text = (yInterval * (i + 1)).ToString(),
                            Anchor = Anchor.CentreLeft,
                            Origin = Anchor.CentreLeft,
                        },
                        new Box
                        {
                            RelativeSizeAxes = Axes.X,
                            Height = 1,
                            Colour = Color4.White.Opacity(0.6f),
                            Margin = new MarginPadding { Left = 50 },
                            Anchor = Anchor.CentreLeft,
                            Origin = Anchor.CentreLeft,
                        },
                    },
                });
            }

            float verticalRulerSpacing = Width / counts.Count;

            for (int i = 0; i < counts.Count; i += xInterval)
            {
                Add(new Container
                {
                    Anchor = Anchor.BottomLeft,
                    Origin = Anchor.BottomLeft,
                    RelativeSizeAxes = Axes.Y,
                    AutoSizeAxes = Axes.X,
                    X = verticalRulerSpacing * (i + 1),
                    RelativePositionAxes = Axes.X,
                    Margin = new MarginPadding { Bottom = 80 },
                    Children = new Drawable[]
                    {
                        new OsuSpriteText
                        {
                            Text = String.Format("{0:MMM yyyy}", counts[i].StartDate),
                            Rotation = 45,
                            Margin = new MarginPadding { Bottom = 20 },
                            Anchor = Anchor.BottomCentre,
                            Origin = Anchor.BottomCentre,
                        },
                        new Box
                        {
                            Height = 195,
                            Width = 1,
                            Colour = ColourInfo.GradientVertical(Color4.White.Opacity(0.1f), Color4.White.Opacity(0.8f)),
                            Margin = new MarginPadding { Bottom = 50 },
                            Anchor = Anchor.BottomCentre,
                            Origin = Anchor.BottomCentre,
                        },
                    },
                });
            }
        }
    }
}
