// Copyright (c) 2007-2016 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu-framework/master/LICENCE

using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
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
                if (count != value)
                {
                    count = value;
                    shadowEffect();
                }
            }
        }

        public void ResetCount() => Count = 0;

        private bool isLit;
        public bool IsLit
        {
            get { return isLit; }
            protected set
            {
                if (isLit != value)
                {
                    isLit = value;
                    if (value && IsCounting)
                        Count++;
                }
            }
        }

        public Color4 textColour { get; set; } = Color4.White;

        public double ShadowFadeTime { get; set; } = 250.0f;

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
                    Text = Count.ToString(@"#,0"),
                    Colour = textColour,
                    Scale = new Vector2(3.5f)
                },
                shadowText = new SpriteText
                {
                    Anchor = Anchor.BottomLeft,
                    Origin = Anchor.BottomLeft,
                    Text = Count.ToString(@"#,0"),
                    Colour = textColour,
                    Alpha = 0,
                    Scale = new Vector2(4.5f),
                    Position = new Vector2(-0.5f, -1.5f)
                }
            };
        }
        
        private void shadowEffect()
        {
            countText.Scale = new Vector2(4.2f);
            shadowText.Scale = new Vector2(5.2f);
            shadowText.Alpha = 0.4f;
            shadowText.Text = Count.ToString();
            shadowText.FadeOut(ShadowFadeTime);
            countText.Text = Count.ToString();
            countText.ScaleTo(new Vector2(3.5f), 200f);
            shadowText.ScaleTo(new Vector2(4.5f), 200f);
        }

        public bool NoteHit()
        {
            Count++;
            return true;
        }

        protected override bool OnMouseDown(InputState state, MouseDownEventArgs args) => NoteHit();
    }
}
