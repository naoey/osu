// Copyright (c) 2007-2018 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using osu.Game.Users;

namespace osu.Game.Online.Chat
{
    public class UserChannel : Channel
    {
        public User User { get; set; }

        public override string ToString() => User?.Username ?? string.Empty;
    }
}
