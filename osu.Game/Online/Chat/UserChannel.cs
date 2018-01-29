// Copyright (c) 2007-2018 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using System;
using osu.Game.Users;

namespace osu.Game.Online.Chat
{
    public class UserChannel : Channel, IEquatable<UserChannel>
    {
        public User User { get; set; }

        public override bool Equals(Channel other) => Equals(other as UserChannel);

        public bool Equals(UserChannel other) => User?.Id == other?.User?.Id;

        public override string ToString() => User?.Username ?? string.Empty;
    }
}
