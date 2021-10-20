using System;

using System.Drawing;

namespace MyGame
{
    class Star : BaseObject
    {
        protected Image starImg = Image.FromFile(@"Images\star.png");
        public Star(Point pos, Point dir, Size size) : base(pos, dir, size)
        {

        }

        public override void Draw()
        {
            Game.Buffer.Graphics.DrawImage(starImg, Pos.X, Pos.Y, Size.Width, Size.Height);
        }
        public override void Update()
        {
            Pos.X = Pos.X + Dir.X;
            if (Pos.X < -16)
            {
                Pos.X = Game.Width + Size.Width;
                Pos.Y = Game.rnd.Next(0, Game.Height);
            }
        }
    }
}
