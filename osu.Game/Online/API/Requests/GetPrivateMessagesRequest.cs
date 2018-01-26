// Copyright (c) 2007-2018 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using System.Collections.Generic;
using osu.Framework.IO.Network;
using osu.Game.Online.Chat;

namespace osu.Game.Online.API.Requests
{
    public class GetPrivateMessagesRequest : APIRequest<List<Message>>
    {
        private long? since;

        public GetPrivateMessagesRequest(long? since)
        {
            this.since = since;
        }

        protected override WebRequest CreateWebRequest()
        {
            var req = base.CreateWebRequest();
            if (since.HasValue) req.AddParameter(@"since", since.Value.ToString());

            return req;
        }

        protected override string Target => @"chat/messages/private";
    }
}
