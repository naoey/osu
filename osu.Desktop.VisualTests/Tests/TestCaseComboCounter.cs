// Copyright (c) 2007-2016 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu-framework/master/LICENCE

using osu.Framework.GameModes.Testing;
using osu.Framework.Graphics;
using osu.Game.Graphics.UserInterface;

namespace osu.Desktop.Tests
{
    class TestCaseComboCounter  : TestCase
    {
        public override string Name => @"ComboCounter";

        public override string Description => @"Tests combo counter";

        public override void Reset()
        {
            base.Reset();
            ComboCounter counter;
            Add(counter = new ComboCounter
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                IsCounting = true
            });
        }
    }
}
