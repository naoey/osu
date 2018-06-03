using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics.Sprites;
using osu.Game.Users;
using System.Linq;

namespace osu.Game.Overlays.Profile.Sections.Historical
{
    class ReplayWatchedHistoryRow : Container
    {
        private readonly HistoricalGraph graph;

        public ReplayWatchedHistoryRow(Bindable<User> user)
        {
            RelativeSizeAxes = Axes.X;
            Height = 325;

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
                graph = new HistoricalGraph
                {
                    RelativeSizeAxes = Axes.Both,
                    Margin = new MarginPadding { Top = 40 },
                    Padding = new MarginPadding { Right = 60 },
                },
            };
        }

        private void userChanged(User user)
        {
            float yInterval = 0.1f;
            float maxValue = user.ReplaysWatchedCounts.Select(r => r.Count).Max();

            while ((maxValue /= 10) > 0.1f)
                yInterval *= 10;

            graph.YInterval = yInterval;
            graph.Counts = user.ReplaysWatchedCounts.ToList();
        }
    }
}
