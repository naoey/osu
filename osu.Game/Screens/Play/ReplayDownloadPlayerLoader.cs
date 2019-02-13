// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.IO;
using System.Threading.Tasks;
using osu.Framework.Allocation;
using osu.Framework.Logging;
using osu.Framework.Screens;
using osu.Game.Beatmaps;
using osu.Game.Online.API;
using osu.Game.Online.API.Requests;
using osu.Game.Rulesets;
using osu.Game.Scoring;
using osu.Game.Scoring.Legacy;

namespace osu.Game.Screens.Play
{
    public class ReplayDownloadPlayerLoader : PlayerLoader
    {
        [Resolved]
        private APIAccess api { get; set; }

        [Resolved]
        private BeatmapManager beatmaps { get; set; }

        [Resolved]
        private RulesetStore rulesets { get; set; }

        private readonly ScoreInfo score;

        public ReplayDownloadPlayerLoader(ScoreInfo score)
            : base(null)
        {
            this.score = score;
        }

        protected override Task CreatePlayerLoadTask(int restartCount, Action<Player> onLoaded)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    var req = new DownloadReplayRequest(score);

                    req.Success += file =>
                    {
                        using (var stream = new FileStream(file, FileMode.Open))
                        {
                            var replayScore = new Score
                            {
                                ScoreInfo = score,
                                Replay = new DatabasedLegacyScoreParser(rulesets, beatmaps).Parse(stream).Replay,
                            };

                            var p = new ReplayPlayer(replayScore)
                            {
                                AllowResults = false,
                            };

                            Beatmap.Value.Mods.Value = score.Mods;

                            LoadComponentAsync(p, onLoaded);
                        }
                    };

                    req.Failure += e =>
                    {
                        this.Exit();
                        Logger.Error(e, @"Couldn't download the replay!");
                    };

                    req.Perform(api);
                }
                catch (Exception e)
                {
                    this.Exit();
                    Logger.Error(e, @"Couldn't download the replay!");
                }
            });
        }
    }
}
