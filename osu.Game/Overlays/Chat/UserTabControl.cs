// Copyright (c) 2007-2018 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using osu.Game.Online.Chat;

namespace osu.Game.Overlays.Chat
{
    public class UserTabControl : ChatTabControl<UserChannel>
    {
        protected override ChatTabItem CreateSelectorTab() => null;
    }
}
