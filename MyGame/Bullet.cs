using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MyGame
{
    class Bullet : BaseObject,ICollision
    {
        public Bullet(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            gunImg = Image.FromFile(@"images\gun.png");
        }
        Image gunImg;
        public override void Draw()
        {
            Game.Buffer.Graphics.DrawImage(gunImg, Pos.X, Pos.Y, Size.Width, Size.Height);
        }
        public override void Update()
        {
            Pos.X = Pos.X + Dir.X;
            Pos.X = Pos.X + 3;
        }
    }
}
