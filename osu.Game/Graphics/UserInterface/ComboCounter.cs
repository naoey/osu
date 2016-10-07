// Copyright (c) 2007-2016 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu-framework/master/LICENCE

using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Transformations;
using OpenTK;
using OpenTK.Graphics;
using osu.Framework.Input;

namespace osu.Game.Graphics.UserInterface
{
    public class ComboCounter : Container
    {
        private SpriteText countText;
        private SpriteText shadowText;

        public bool IsCounting { get; set; }
        private int count;
        public int Count
        {
            get { return count; }
            private set
            {
                if (count != value && IsCounting)
                {
                    count = value;
                }
            }
        }

        public Color4 textColour { get; set; } = Color4.White;
        public double ShadowFadeTime { get; set; } = 250;
        public float BaseScale { get; set; } = 3.0f;

        public override bool Contains(Vector2 position) => true;

        public override void Load()
        {
            base.Load();

            Children = new Drawable[]
            {
                countText = new SpriteText
                {
                    Anchor = Anchor.BottomLeft,
                    Origin = Anchor.BottomLeft,
                    Text = Count.ToString(),
                    Colour = textColour,
                    Scale = new Vector2(BaseScale)
                },
                shadowText = new SpriteText
                {
                    Anchor = Anchor.BottomLeft,
                    Origin = Anchor.BottomLeft,
                    Text = Count.ToString(),
                    Colour = textColour,
                    Alpha = 0,
                    Scale = new Vector2(BaseScale),
                    Position = new Vector2(-1.5f, -5.5f)
                }
            };
        }

        public void NoteHit()
        {
            countText.ClearTransformations();
            shadowText.ClearTransformations();

            if (isResetting) isResetting = false;

            countText.Text = Count.ToString();
            Count++;
            shadowText.Text = Count.ToString();

            countText.Scale = new Vector2(BaseScale * 1.1f);
            shadowText.Scale = new Vector2(BaseScale * 1.7f);

            shadowText.Alpha = 0.4f;

            shadowText.FadeOut(ShadowFadeTime);
            countText.ScaleTo(new Vector2(BaseScale), 350);
            shadowText.ScaleTo(new Vector2(BaseScale * 1.3f), 200, EasingTypes.InBounce);
        }

        private bool isResetting = false;

        public void NoteMiss()
        {
            countText.ClearTransformations();
            shadowText.ClearTransformations();

            int rollingCount = Count;
            Count = 0;

            isResetting = true;

            countText.Scale = new Vector2(BaseScale * 1.1f);
            shadowText.Scale = new Vector2(BaseScale * 1.3f);

            for (int i = rollingCount; i > 0; i--)
            {
                if (!isResetting) break;

                rollingCount--;
                shadowText.Text = rollingCount.ToString();
                countText.Text = rollingCount.ToString();

                if (i == 1) isResetting = false;
            }

            countText.ScaleTo(new Vector2(BaseScale), 200);
            shadowText.ScaleTo(new Vector2(BaseScale), 350, EasingTypes.InBounce);
        }
    }
}
