using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace MyGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Form form = new Form();
            Rectangle bounds = Screen.PrimaryScreen.Bounds;
            form.Width = bounds.Width;
            bounds = Screen.PrimaryScreen.Bounds;
            form.Height = bounds.Height;
            Game.Init(form);
            form.Show();
            Game.Draw();
            Application.Run(form);
        }
    }
}
