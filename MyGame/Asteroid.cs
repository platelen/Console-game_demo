using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MyGame
{
    class Asteroid : BaseObject,ICollision
    {
        public int Energy { get; set; }
        protected Image stoneImg = Image.FromFile(@"images\stoneOld.png");
        public Asteroid(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            
            Energy = 1;
        }
        public Asteroid()
        {

        }
        public override void Draw()
        {
            Game.Buffer.Graphics.DrawImage(stoneImg, Pos.X, Pos.Y, Size.Width, Size.Height);
        }
        public override void Update()
        {
            Pos.X = Pos.X + Dir.X;
            Pos.Y = Pos.Y - Dir.Y;
            if (Pos.Y < 0)
            {
                Dir.Y = +Dir.Y;
            }
            if (Pos.Y > Game.Height)
            {
                Dir.Y = +Dir.Y;
            }
            if (Pos.X < -100) Pos.X = Game.Width + Size.Width;
            if (Pos.Y < -100) Pos.Y = Game.Height + Size.Height;
            if (Pos.X < -48)
            {
                Pos.X = Game.Width + Size.Width;
                Pos.Y = Game.rnd.Next(0, Game.Height - 10);
            }
        }
    }
}
