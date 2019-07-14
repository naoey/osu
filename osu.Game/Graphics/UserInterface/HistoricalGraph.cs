using System;
using System.Collections.Generic;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics;
using System.Linq;
using osuTK;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Allocation;

namespace osu.Game.Graphics.UserInterface
{
    public class HistoricalGraph : Container
    {
        private IEnumerable<KeyValuePair<DateTimeOffset, float>> values;

        public IEnumerable<KeyValuePair<DateTimeOffset, float>> Values
        {
            get => values ?? Array.Empty<KeyValuePair<DateTimeOffset, float>>();
            set
            {
                if (value.Equals(values))
                    return;

                values = value;

                line.Values = values.Select(v => v.Value);

                updateRulers();
            }
        }

        private LineGraph line;
        private FillFlowContainer verticalRulersContainer;
        private FillFlowContainer horitontalRulersContainer;

        public HistoricalGraph()
        {
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            Children = new Drawable[]
            {
                verticalRulersContainer = new FillFlowContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    Direction = FillDirection.Horizontal,
                },
                horitontalRulersContainer = new FillFlowContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    Direction = FillDirection.Vertical,
                },
                line = new LineGraph
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = colours.YellowDark,
                },
            };
        }

        private void updateRulers()
        {
            verticalRulersContainer.Clear(true);
            horitontalRulersContainer.Clear(true);

            var dates = Values.Select(v => v.Key);
            var counts = Values.Select(v => v.Value);

            verticalRulersContainer.Spacing = new Vector2(DrawWidth / dates.Count());
            horitontalRulersContainer.Spacing = new Vector2(DrawHeight / counts.Count());

            foreach (var date in dates)
                verticalRulersContainer.Add(new Box
                {
                    RelativeSizeAxes = Axes.Y,
                    Width = 1,
                    Colour = OsuColour.FromHex("#FFF"),
                    Alpha = 0.4f,
                });

            foreach (var count in counts)
                horitontalRulersContainer.Add(new Box
                {
                    RelativeSizeAxes = Axes.X,
                    Height = 1,
                    Colour = OsuColour.FromHex("#FFF"),
                    Alpha = 0.4f,
                });
        }
    }
}
