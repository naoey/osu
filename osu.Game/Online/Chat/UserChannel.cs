// Copyright (c) 2007-2018 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using System;
using osu.Game.Users;

namespace osu.Game.Online.Chat
{
    public class UserChannel : Channel, IEquatable<UserChannel>
    {
        public User ToUser { get; }

        public User FromUser { get; }

        public UserChannel(User to, User from)
        {
            ToUser = to;
            FromUser = from;

            Id = ToUser.Id;
        }

        public override bool Equals(Channel other) => Equals(other as UserChannel);

        public bool Equals(UserChannel other) => ToUser?.Id == other?.ToUser?.Id;

        public override string ToString() => ToUser?.Username ?? string.Empty;
    }
}
