using osu.Framework.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Sprites;
using osu.Game.Graphics;
using osu.Game.Graphics.UserInterface;
using osu.Game.Online;
using osu.Game.Scoring;

namespace osu.Game.Screens.Play
{
    public class ReplayDownloadButton : DownloadTrackingComposite<ScoreInfo, ScoreManager>
    {
        [Resolved]
        private OsuGame game { get; set; }

        [Resolved]
        private ScoreManager scores { get; set; }

        public ReplayDownloadButton(ScoreInfo score)
            : base(score)
        {
            AutoSizeAxes = Axes.Both;
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            AddInternal(new TwoLayerButton
            {
                BackgroundColour = colours.Yellow,
                Icon = FontAwesome.Solid.PlayCircle,
                Text = @"Replay",
                HoverColour = colours.YellowDark,
                Action = onReplay,
            });
        }

        private void onReplay()
        {
            if (scores.IsAvailableLocally(ModelInfo.Value))
            {
                game.PresentScore(ModelInfo.Value);
                return;
            }

            scores.Download(ModelInfo.Value);

            scores.ItemAdded += (score, _) =>
            {
                if (score.Equals(ModelInfo.Value))
                    // use the newly added score instead of ModelInfo.Score because that won't have the Files property populated
                    game.PresentScore(score);
            };
        }
    }
}
