using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MyGame
{
    class Ship : BaseObject, ICollision
    {
        private int _energy = 5;

        public int Energy => _energy;
        public static event Message MessageDie;
        protected Image shipImg = Image.FromFile(@"images\ship.png");
        public void EnergyLow(int n)
        {
            _energy -= n;
        }

        public void EnergyAdd(int n)
        {
            _energy += n;
        }

        public Ship(Point pos, Point dir, Size size) : base(pos, dir, size)
        {

        }

        public override void Draw()
        {
            Game.Buffer.Graphics.DrawImage(shipImg, Pos.X, Pos.Y, Size.Width, Size.Height);
        }
        public override void Update()
        {
        }

        public void Up()
        {
            if (Pos.Y > 0) Pos.Y = Pos.Y - Dir.Y;
        }
        public void Down()
        {
            if (Pos.Y < Game.Height) Pos.Y = Pos.Y + Dir.Y;
        }

        public void Left()
        {
            if (Pos.X > 0) Pos.X = Pos.X - Dir.X;
        }
        public void Right()
        {
            if (Pos.X < Game.Width) Pos.X = Pos.X + Dir.X;
        }

        public void Die()
        {
            MessageDie?.Invoke();
        }
    }
}
