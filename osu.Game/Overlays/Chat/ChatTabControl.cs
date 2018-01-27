// Copyright (c) 2007-2018 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Graphics.UserInterface;
using OpenTK;
using OpenTK.Graphics;
using osu.Framework.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using osu.Game.Graphics.Containers;

namespace osu.Game.Overlays.Chat
{
    public abstract class ChatTabControl<T> : OsuTabControl<T>
    {
        protected const float SHEAR_WIDTH = 10;

        public Action<T> OnRequestLeave;

        public readonly Bindable<bool> SelectorActive = new Bindable<bool>();

        public IEnumerable<T> OpenTabs => TabContainer.Children.Select(c => c.Value);

        protected readonly ChatTabItem SelectorTab;

        public ChatTabControl()
        {
            TabContainer.Margin = new MarginPadding { Left = 50 };
            TabContainer.Spacing = new Vector2(-SHEAR_WIDTH, 0);
            TabContainer.Masking = false;

            // TODO: remove these virtual calls from constructor
            AddTabItem(SelectorTab = CreateSelectorTab());

            SelectorActive.BindTo(SelectorTab.Active);
        }

        public void Select(T value)
        {
            SelectTab(TabContainer.First(c => c.Value.Equals(value)));
        }

        public void DeselectAll() => SelectTab(null);

        protected override void AddTabItem(TabItem<T> item, bool addToDropdown = true)
        {
            if (SelectorTab.Depth != float.MaxValue)
                // performTabSort might've made SelectorTab's position wonky, fix it
                TabContainer.ChangeChildDepth(SelectorTab, float.MaxValue);

            base.AddTabItem(item, addToDropdown);

            if (SelectedTab == null)
                SelectTab(item);
        }

        protected override TabItem<T> CreateTabItem(T value) => new ChatTabItem(value) { OnRequestClose = TabCloseRequested };

        protected abstract ChatTabItem CreateSelectorTab();

        protected override void SelectTab(TabItem<T> tab)
        {
            if (tab == SelectorTab)
            {
                tab.Active.Toggle();
                return;
            }

            SelectorTab.Active.Value = false;

            base.SelectTab(tab);
        }

        protected void TabCloseRequested(TabItem<T> tab)
        {
            int totalTabs = TabContainer.Count - 1; // account for SelectorTab
            int currentIndex = MathHelper.Clamp(TabContainer.IndexOf(tab), 1, totalTabs);

            if (tab == SelectedTab && totalTabs > 1)
                // Select the tab after tab-to-be-removed's index, or the tab before if current == last
                SelectTab(TabContainer[currentIndex == totalTabs ? currentIndex - 1 : currentIndex + 1]);
            else if (totalTabs == 1 && !SelectorTab.Active)
                // Open channel selection overlay if all channel tabs will be closed after removing this tab
                SelectTab(SelectorTab);

            OnRequestLeave?.Invoke(tab.Value);
        }

        protected class ChatTabItem : TabItem<T>
        {
            protected Color4 BackgroundInactive;
            protected Color4 BackgroundHover;
            protected Color4 BackgroundActive;

            public override bool IsRemovable => !Pinned;

            private readonly ClickableContainer closeButton;

            protected readonly SpriteText Text;
            protected readonly SpriteText TextBold;
            protected readonly Box Box;
            protected readonly Box HighlightBox;
            protected readonly SpriteIcon Icon;
            protected readonly Container ContentContainer;

            protected virtual FontAwesome BackgroundIcon => FontAwesome.fa_hashtag;

            public Action<ChatTabItem> OnRequestClose;

            private void updateState()
            {
                if (Active)
                    FadeActive();
                else
                    FadeInactive();
            }

            protected const float TRANSITION_LENGTH = 400;

            protected virtual void FadeActive()
            {
                this.ResizeTo(new Vector2(Width, 1.1f), TRANSITION_LENGTH, Easing.OutQuint);

                Box.FadeColour(BackgroundActive, TRANSITION_LENGTH, Easing.OutQuint);
                HighlightBox.FadeIn(TRANSITION_LENGTH, Easing.OutQuint);

                Text.FadeOut(TRANSITION_LENGTH, Easing.OutQuint);
                TextBold.FadeIn(TRANSITION_LENGTH, Easing.OutQuint);
            }

            protected virtual void FadeInactive()
            {
                this.ResizeTo(new Vector2(Width, 1), TRANSITION_LENGTH, Easing.OutQuint);

                Box.FadeColour(BackgroundInactive, TRANSITION_LENGTH, Easing.OutQuint);
                HighlightBox.FadeOut(TRANSITION_LENGTH, Easing.OutQuint);

                Text.FadeIn(TRANSITION_LENGTH, Easing.OutQuint);
                TextBold.FadeOut(TRANSITION_LENGTH, Easing.OutQuint);
            }

            protected override bool OnHover(InputState state)
            {
                if (IsRemovable)
                    closeButton.FadeIn(200, Easing.OutQuint);

                if (!Active)
                    Box.FadeColour(BackgroundHover, TRANSITION_LENGTH, Easing.OutQuint);
                return true;
            }

            protected override void OnHoverLost(InputState state)
            {
                closeButton.FadeOut(200, Easing.OutQuint);
                updateState();
            }

            [BackgroundDependencyLoader]
            private void load(OsuColour colours)
            {
                BackgroundActive = colours.ChatBlue;
                BackgroundInactive = colours.Gray4;
                BackgroundHover = colours.Gray7;

                HighlightBox.Colour = colours.Yellow;
            }

            protected override void LoadComplete()
            {
                base.LoadComplete();

                updateState();
            }

            public ChatTabItem(T value) : base(value)
            {
                Width = 150;

                Anchor = Anchor.BottomRight;
                Origin = Anchor.BottomRight;

                RelativeSizeAxes = Axes.Y;

                Shear = new Vector2(SHEAR_WIDTH / ChatOverlay.TAB_AREA_HEIGHT, 0);

                Masking = true;
                EdgeEffect = new EdgeEffectParameters
                {
                    Type = EdgeEffectType.Shadow,
                    Radius = 10,
                    Colour = Color4.Black.Opacity(0.2f),
                };

                Children = new Drawable[]
                {
                    Box = new Box
                    {
                        EdgeSmoothness = new Vector2(1, 0),
                        RelativeSizeAxes = Axes.Both,
                    },
                    HighlightBox = new Box
                    {
                        Width = 5,
                        Alpha = 0,
                        Anchor = Anchor.BottomRight,
                        Origin = Anchor.BottomRight,
                        EdgeSmoothness = new Vector2(1, 0),
                        RelativeSizeAxes = Axes.Y,
                    },
                    ContentContainer = new Container
                    {
                        Shear = new Vector2(-SHEAR_WIDTH / ChatOverlay.TAB_AREA_HEIGHT, 0),
                        RelativeSizeAxes = Axes.Both,
                        Children = new Drawable[]
                        {
                            Icon = new SpriteIcon
                            {
                                Icon = BackgroundIcon,
                                Anchor = Anchor.CentreLeft,
                                Origin = Anchor.CentreLeft,
                                Colour = Color4.Black,
                                X = -10,
                                Alpha = 0.2f,
                                Size = new Vector2(ChatOverlay.TAB_AREA_HEIGHT),
                            },
                            Text = new OsuSpriteText
                            {
                                Margin = new MarginPadding(5),
                                Origin = Anchor.CentreLeft,
                                Anchor = Anchor.CentreLeft,
                                Text = value.ToString(),
                                TextSize = 18,
                            },
                            TextBold = new OsuSpriteText
                            {
                                Alpha = 0,
                                Margin = new MarginPadding(5),
                                Origin = Anchor.CentreLeft,
                                Anchor = Anchor.CentreLeft,
                                Text = value.ToString(),
                                Font = @"Exo2.0-Bold",
                                TextSize = 18,
                            },
                            closeButton = new CloseButton
                            {
                                Alpha = 0,
                                Margin = new MarginPadding { Right = 20 },
                                Origin = Anchor.CentreRight,
                                Anchor = Anchor.CentreRight,
                                Action = delegate
                                {
                                    if (IsRemovable) OnRequestClose?.Invoke(this);
                                },
                            },
                        },
                    },
                };
            }

            private class CloseButton : OsuClickableContainer
            {
                private readonly SpriteIcon icon;

                public CloseButton()
                {
                    Size = new Vector2(20);

                    Child = icon = new SpriteIcon
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Scale = new Vector2(0.75f),
                        Icon = FontAwesome.fa_close,
                        RelativeSizeAxes = Axes.Both,
                    };
                }

                protected override bool OnMouseDown(InputState state, MouseDownEventArgs args)
                {
                    icon.ScaleTo(0.5f, 1000, Easing.OutQuint);
                    return base.OnMouseDown(state, args);
                }

                protected override bool OnMouseUp(InputState state, MouseUpEventArgs args)
                {
                    icon.ScaleTo(0.75f, 1000, Easing.OutElastic);
                    return base.OnMouseUp(state, args);
                }

                protected override bool OnHover(InputState state)
                {
                    icon.FadeColour(Color4.Red, 200, Easing.OutQuint);
                    return base.OnHover(state);
                }

                protected override void OnHoverLost(InputState state)
                {
                    icon.FadeColour(Color4.White, 200, Easing.OutQuint);
                    base.OnHoverLost(state);
                }
            }

            protected override void OnActivated() => updateState();

            protected override void OnDeactivated() => updateState();
        }

        protected abstract class SelectorTabItem : ChatTabItem
        {
            public override bool IsRemovable => false;

            protected SelectorTabItem(T value)
                : base(value)
            {
                Depth = float.MaxValue;
                Width = 45;

                Icon.Alpha = Text.Alpha = TextBold.Alpha = 0;

                AddInternal(CreateSelectorIcon());
            }

            protected abstract SpriteIcon CreateSelectorIcon();
        }
    }
}
