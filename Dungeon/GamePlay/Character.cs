using DungeonUtility;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace DungeonGame
{
    public class Character : Panel
    {
        public Character() => Init();

        public Character(uint health, int atk, int def, uint coin)
        {
            Init();
            this.health = health;
            this.atk = atk;
            this.def = def;
            this.coin = coin;
        }

        private void Init()
        {
            Size = characterSize;

            DrawCharacter();
        }

        /// <summary>
        /// 繪製角色
        /// </summary>
        private void DrawCharacter()
        {
            Bitmap bg = new Bitmap(tileSize.Width, tileSize.Height);
            using (Graphics g = Graphics.FromImage(bg))
            {
                g.Clear(Color.Transparent);

                GraphicsEx.FillCircle(g, Brushes.Azure, tileSize.Width / 2, tileSize.Height / 2, 5);
            }

            BackgroundImage = bg;
        }

        public void CalcMove()
        {
            int mult = 10;
            int up = ((isMovingUp ? -1 : 0) + (isMovingDown ? 1 : 0)) * mult;
            int right = ((isMovingRight ? 1 : 0) + (isMovingLeft ? -1 : 0)) * mult;

            MoveTo(new Point(Location.X + right, Location.Y + up));
        }

        private void MoveTo(Point newLocation)
        {
            if (UI.map.IsWalkable(newLocation))
                Location = newLocation;

            // todo:send location data to server
            // timer tick?
            
        }

        // enable when offline
        public void Destory()
        {
            name = default;
            health = default;
            atk = default;
            def = default;
            coin = default;
        }

        private Size tileSize = new Size(40, 40);
        private Size characterSize = new Size(20, 20);

        public bool isMovingUp;
        public bool isMovingDown;
        public bool isMovingLeft;
        public bool isMovingRight;

        public string name { get; set; }
        public uint health { get; set; }
        public int atk { get; set; }
        public int def { get; set; }
        public uint coin { get; set; }
        public string status
        {
            get
            {
                return "Name:\t" + name
                + Environment.NewLine + "Health:\t" + health
                + Environment.NewLine + "Atk:\t" + atk
                + Environment.NewLine + "Def:\t" + def
                + Environment.NewLine + "Coin:\t" + coin;
            }
        }

    }
}
