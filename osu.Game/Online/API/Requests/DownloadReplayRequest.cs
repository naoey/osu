﻿using osu.Game.Scoring;
namespace osu.Game.Online.API.Requests
{
    public class DownloadReplayRequest : ArchiveDownloadRequest<ScoreInfo>
    {
        public DownloadReplayRequest(ScoreInfo score)
            : base(score)
        {
        }

        protected override string FileExtension => @".osr";

        protected override string Target => $@"scores/{Info.Ruleset.ShortName}/{Info.OnlineScoreID}/download";
    }
}
