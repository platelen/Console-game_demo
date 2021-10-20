using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MyGame
{
    class Energy : BaseObject
    {
        protected Image healImg = Image.FromFile(@"images\heal.png");
        public Energy(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            
        }

        public override void Draw()
        {
            Game.Buffer.Graphics.DrawImage(healImg, Pos.X, Pos.Y, Size.Width, Size.Height);
        }

        public override void Update()
        {
            Pos.X -= Dir.X;
            if (Pos.X < -100)
            {
                Pos.X = Game.Width + Size.Width;
            }

        }
    }
}
