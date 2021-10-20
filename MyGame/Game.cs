using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using WMPLib;
using System.Media;


namespace MyGame
{
    static class Game
    {
        /// <summary>
        /// Ширина и высота игррового поля
        /// </summary>
        public static int Width
        {
            get; private set;
        }
        public static int Height
        {
            get; private set;
        }

        private static BufferedGraphicsContext _context;

        public static BufferedGraphics Buffer;

        public static Random rnd;

        private static Timer _timer;

        public static int Lvl;

        public static Image Spase;

        private static Energy[] energy;

        private static List<Bullet> _bullet;

        private static Ship _ship;

        private static List<Asteroid> _asteroids;

        public static int AsteroidsNum;

        public static int AsteroidsDestroyed;

        public static BaseObject[] _stars;

        public static int Score;

        public static SoundPlayer bulletSound;
        public static SoundPlayer failSound;
        public static SoundPlayer collisionSound;
        public static SoundPlayer blowSound;
        public static SoundPlayer asterDestroySound;

        /// <summary>
        /// Таймер.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Timer_Tick(object sender, EventArgs e)
        {
            Draw();
            Update();
            NextLevel();
        }

        /// <summary>
        /// Инициализация объектов.
        /// </summary>
        public static void Load()
        {
             WindowsMediaPlayer wmp = new WindowsMediaPlayer();
             AsteroidsNum = 15;
            _stars = new BaseObject[40];
             energy = new Energy[5];
            _bullet = new List<Bullet>();
             LoadAsteroids();

            for (var i = 0; i < _stars.Length; i++)
            {
                _stars[i] = new Star(new Point(rnd.Next(0, Width), rnd.Next(0, Height)), new Point(rnd.Next(-4, -1), 0), new Size(30, 30));
            }

            for (var i = 0; i < energy.Length; i++)
            {
                energy[i] = new Energy(
                    new Point(rnd.Next(Game.Height - 100, Game.Width - 100), rnd.Next(1, Game.Height - 100)),
                    new Point(15, 15),
                    new Size(50, 50));
            }
        }

        /// <summary>
        /// Фоновая музыка.
        /// </summary>
        public static void SoundFon()
        {
            WindowsMediaPlayer wmp = new WindowsMediaPlayer();
            wmp.URL = "media\\8 Bit Fantasy - Space Battle.mp3";
        }

        /// <summary>
        /// Проигрывание звуков.
        /// </summary>
        /// <param name="choiceMelody">выбор звука.</param>
        public static void playSimpleSound(int choiceMelody)
        {
            switch (choiceMelody)
            {
                case 1:
                    asterDestroySound.Play();
                    break;
                case 2:
                    bulletSound.Play();
                    break;
                case 3:
                    collisionSound.Play();
                    break;
                case 4:
                    failSound.Play();
                    break;
                case 5:
                    blowSound.Play();
                    break;
            }
        }

        /// <summary>
        /// Конструктор класса Game.
        /// </summary>
        static Game()
        {
            Lvl = 1;
            Score = 0;
            AsteroidsDestroyed = 0;
            _timer = new Timer { Interval = 50 };
            _ship = new Ship(new Point(10, 400), new Point(25, 25), new Size(80, 80));
            Spase = Image.FromFile(@"Images\Space2.jpg");
            bulletSound = new SoundPlayer("media\\Bullet.wav");
            failSound = new SoundPlayer("media\\Fail.wav");
            collisionSound = new SoundPlayer("media\\Collision.wav");
            blowSound = new SoundPlayer("media\\Blow.wav");
            asterDestroySound = new SoundPlayer("media\\AsterDestroy.wav");
            SoundFon();
        }

        /// <summary>
        /// Задаём управление кораблём
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
            {
                _bullet.Add(new Bullet(new Point(_ship.Rect.X + 10, _ship.Rect.Y + 4), new Point(85, 0), new Size(40, 40)));
                playSimpleSound(2);
            }
            if (e.KeyCode == Keys.Up)
            {
                _ship.Up();
            }
            if (e.KeyCode == Keys.Down)
            {
                _ship.Down();
            }
            if (e.KeyCode == Keys.Left)
            {
                _ship.Left();
            }
            if (e.KeyCode == Keys.Right)
            {
                _ship.Right();
            }

        }
        /// <summary>
        /// Инициализация графики.
        /// </summary>
        /// <param name="form">Форма для вывода графики</param>
        public static void Init(Form form)
        {
            rnd = new Random();
            _timer.Start();
            _timer.Tick += Timer_Tick;
            LoadAsteroids();
            
            #region Графическое устройства для вывода графики

            Graphics g;

            #endregion
            #region Предоставляет доступ к главному буферу графического контекста для текущего приложения.

            _context = BufferedGraphicsManager.Current;
            g = form.CreateGraphics();

            #endregion
            #region Обработчик событий

            form.KeyDown += Form_KeyDown;

            #endregion
            #region Создаём объект (поверхность рисования) и связывем его с формой.Запоминаем размеры формы.

            Width = form.ClientSize.Width;
            Height = form.ClientSize.Height;

            #endregion
            #region Сязываем буфер в памяти с графическим объекотм, что бы рисовать в буфере.

            Buffer = _context.Allocate(g, new Rectangle(0, 0, Width, Height));

            #endregion

            Ship.MessageDie += Finish;
            Load();
        }

        /// <summary>
        /// Изменение состояния объектов.Проверка столкновений.
        /// </summary>
        public static void Update()
        {
            foreach (BaseObject obj in _stars)
            {
                obj.Update();
            }
            foreach (Bullet b in _bullet)
            {
                b.Update();
            }
            _ship?.Draw();

            for (var i = 0; i < energy.Length; i++)
            {
                if (energy[i] == null) continue;
                energy[i].Update();

                if (_ship != null && _ship.Collision(energy[i]))
                {
                    energy[i] = null;
                    _ship?.EnergyAdd(1);
                }
            }

            for (var i = 0; i < _asteroids.Count; i++)
            {
                if (_asteroids[i] == null) continue;
                _asteroids[i].Update();
                for (int j = 0; j < _bullet.Count; j++)
                    if (_bullet[j] != null && _bullet[j].Collision(_asteroids[i]))
                    {
                        playSimpleSound(1);
                        _asteroids[i] = new Asteroid();
                        _bullet[j] = null;
                        AsteroidsDestroyed++;
                        Score++;
                        continue;
                    }

                if (_ship != null && _ship.Collision(_asteroids[i]))
                {
                    playSimpleSound(3);
                    _asteroids[i] = new Asteroid();
                    _ship?.EnergyLow(1);
                    AsteroidsDestroyed++;
                    if (_ship.Energy <= 0) _ship?.Die();
                }
            }
            _bullet.RemoveAll(item => item == null);
        }

        /// <summary>
        /// Отрисовка объектов.
        /// </summary>
        public static void Draw()
        {
            Buffer.Graphics.Clear(Color.Black);
            Buffer.Graphics.DrawImage(Spase, new Rectangle(0, 0, Width, Height));
            foreach (BaseObject obj in _stars)
                obj.Draw();
            foreach (Asteroid a in _asteroids)
            {
                a?.Draw();
            }
            foreach (Energy a in energy)
            {
                a?.Draw();
            }
            foreach (Bullet b in _bullet)
            {
                b.Draw();
            }

            _ship?.Draw();
            if (_ship != null)
            {
                Buffer.Graphics.DrawString("Score:" + Score /*+ "/" + AsteroidsNum*/, new Font(FontFamily.GenericSansSerif, 10, FontStyle.Underline), Brushes.Cyan, 0, 15);
                Buffer.Graphics.DrawString("Energy:" + _ship.Energy, new Font(FontFamily.GenericSansSerif, 10, FontStyle.Underline), Brushes.Cyan, 0, 0);
                Buffer.Graphics.DrawString("Level: " + Lvl, new Font(FontFamily.GenericSansSerif, 10, FontStyle.Underline), Brushes.Cyan, 0, 30);
                Buffer.Graphics.DrawString("Asteroid Destroid: " + AsteroidsDestroyed + "/" +AsteroidsNum, new Font(FontFamily.GenericSansSerif, 10, FontStyle.Underline), Brushes.Cyan, 0, 45);
            }
            Buffer.Render();
        }

        /// <summary>
        /// Отработка проигрыша.
        /// </summary>
        public static void Finish()
        {
            playSimpleSound(4);
            _timer.Stop();
            Buffer.Graphics.DrawString("Game Ower", new Font(FontFamily.GenericSansSerif, 60, FontStyle.Underline), Brushes.Coral, 200, 100);
            Buffer.Render();
        }

        /// <summary>
        /// Загрузка астероидов.
        /// </summary>
        public static void LoadAsteroids()
        {
            _asteroids = new List<Asteroid>();
            for (int i = 0; i < AsteroidsNum; i++)
            {
                _asteroids.Add(new Asteroid(new Point(rnd.Next(Game.Height - 100, Game.Width - 100), rnd.Next(1, Game.Height - 100)),
                    new Point(rnd.Next(-30, -5), rnd.Next(5, 20)),
                    new Size(80, 80)));
            }
        }

        /// <summary>
        /// Отработка перехода на следующий уровень или конца игры.
        /// </summary>
        public static void NextLevel()
        {
            if (AsteroidsDestroyed == AsteroidsNum)
            {
                if (Lvl < 10)
                {
                    AsteroidsDestroyed = 0;
                    Buffer.Graphics.DrawString("Next Level!", new Font(FontFamily.GenericSansSerif, 60f, FontStyle.Underline), Brushes.Coral, 200, 100);
                    Buffer.Render();
                    System.Threading.Thread.Sleep(3000);
                    AsteroidsNum += 5;
                    LoadAsteroids();
                    Lvl++;
                }
                else
                {
                    _timer.Stop();
                    Buffer.Graphics.DrawString("Finish!", new Font(FontFamily.GenericSansSerif, 60f, FontStyle.Underline), Brushes.Coral, 200, 100);
                    Buffer.Render();
                }
            }
        }
    }
}
