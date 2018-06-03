using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics.Sprites;
using osu.Game.Users;
using System.Linq;

namespace osu.Game.Overlays.Profile.Sections.Historical
{
    public class PlayHistoryRow : Container
    {
        private readonly HistoricalGraph graph;

        public PlayHistoryRow(Bindable<User> user)
        {
            RelativeSizeAxes = Axes.X;
            Height = 325;

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
            graph.YInterval = 1000;
            graph.Counts = user.MonthlyPlayCounts.ToList();
        }
    }
}
