// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Logging;
using osu.Framework.Screens;
using osu.Game.Beatmaps;
using osu.Game.Scoring;
using osu.Game.Screens.Ranking;
using osu.Game.Screens.Ranking.Types;

namespace osu.Game.Screens.Play
{
    public class SoloResults : Results
    {
        [Resolved]
        private BeatmapManager beatmaps { get; set; }

        [Resolved]
        private ScoreManager scores { get; set; }

        private readonly ScoreInfo score;
        
        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent)
        {
            var dependencies = new DependencyContainer(base.CreateChildDependencies(parent));

            dependencies.Cache(this);

            return dependencies;
        }

        public SoloResults(ScoreInfo score)
            : base(score)
        {
            this.score = score;
        }

        /// <summary>
        /// Begins playback of the <see cref="ScoreInfo"/> diplayed by this results screen.
        /// </summary>
        public void PresentReplay()
        {
            // The given ScoreInfo may have missing properties if it was retrieved from online data. Re-retrieve it from the database
            // to ensure all the required data for presenting a replay are present.
            var databasedScoreInfo = scores.Query(s => s.OnlineScoreID == score.OnlineScoreID);
            var databasedScore = scores.GetScore(databasedScoreInfo);

            if (databasedScore.Replay == null)
            {
                Logger.Log("The loaded score has no replay data.", LoggingTarget.Information);
                return;
            }

            // If we came to results from song select or player, the beatmap will already be the correct selection. If we came here from
            // elsewhere by clicking on a score in notifications etc., then the current beatmap may need to be updated before playback.
            if (Beatmap.Value.BeatmapInfo.ID != score.Beatmap.ID)
            {
                var databasedBeatmap = beatmaps.QueryBeatmap(b => b.ID == score.Beatmap.ID);

                if (databasedBeatmap == null)
                {
                    Logger.Log("Tried to load a replay for a beatmap we don't have!", LoggingTarget.Information);
                    return;
                }

                Beatmap.Value = beatmaps.GetWorkingBeatmap(databasedBeatmap);
            }

            this.Push(new ReplayPlayerLoader(databasedScore));
        }

        protected override IEnumerable<IResultPageInfo> CreateResultPages() => new IResultPageInfo[]
        {
            new ScoreOverviewPageInfo(Score, Beatmap.Value),
            new LocalLeaderboardPageInfo(Score, Beatmap.Value)
        };
    }
}
