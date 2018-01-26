// Copyright (c) 2007-2018 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input;
using osu.Game.Graphics;
using osu.Game.Online.Chat;
using osu.Game.Users;
using OpenTK;

namespace osu.Game.Overlays.Chat
{
    public class UserTabControl : ChatTabControl<UserChannel>
    {
        public UserTabControl()
        {
            HeaderBehaviour = DropdownHeaderBehaviour.Flowing;
            TabContainer.Anchor = Anchor.TopRight;
            TabContainer.Origin = Anchor.TopRight;
            Margin = new MarginPadding { Right = SHEAR_WIDTH };
        }

        protected override ChatTabItem CreateSelectorTab() => new UserSelectorTabItem(new UserChannel());

        protected override TabItem<UserChannel> CreateTabItem(UserChannel value) => new UserTabItem(value) { OnRequestClose = TabCloseRequested };

        protected class UserTabItem : ChatTabItem
        {
            private Avatar avatar;

            public UserTabItem(UserChannel value)
                : base(value)
            {
                Anchor = Anchor.BottomRight;
                Origin = Anchor.BottomRight;

                ContentContainer.Add(new Avatar(Value.User)
                {
                    Width = 40,
                    Height = 40,
                    CornerRadius = 20,
                    Masking = true,
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    Margin = new MarginPadding { Left = 5 },
                });

                Text.Margin = TextBold.Margin = new MarginPadding { Left = 50 };
            }

            [BackgroundDependencyLoader]
            private new void load(OsuColour colour)
            {
                if (Value.User.Colour != null)
                    BackgroundActive = BackgroundInactive = BackgroundHover = OsuColour.FromHex(Value.User.Colour);
                else
                    BackgroundActive = BackgroundInactive = BackgroundHover = colour.BlueDarker;
            }

            protected override void FadeActive()
            {
                base.FadeActive();
                this.ResizeWidthTo(150, TRANSITION_LENGTH, Easing.OutQuint);
            }

            protected override void FadeInactive()
            {
                base.FadeInactive();

                this.ResizeWidthTo(60, TRANSITION_LENGTH, Easing.OutQuint);
                Text.Hide();
                TextBold.Hide();
            }

            protected override bool OnHover(InputState state)
            {
                if (!Active)
                {
                    this.ResizeTo(new Vector2(150, Height), TRANSITION_LENGTH, Easing.OutQuint);
                    Text.FadeIn(150, Easing.InQuint);
                }

                return base.OnHover(state);
            }
        }

        protected class UserSelectorTabItem: SelectorTabItem
        {
            public UserSelectorTabItem(UserChannel value)
                : base(value)
            {
                Anchor = Anchor.BottomRight;
                Origin = Anchor.BottomRight;
            }

            [BackgroundDependencyLoader]
            private new void load(OsuColour colour)
            {
                BackgroundActive = colour.BlueDark;
                BackgroundInactive = colour.BlueDarker;
                BackgroundHover = colour.BlueLight;
            }

            protected override SpriteIcon CreateSelectorIcon() => new SpriteIcon
            {
                Icon = FontAwesome.fa_group,
                Size = new Vector2(20),
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Shear = -Shear,
            };
        }
    }
}
