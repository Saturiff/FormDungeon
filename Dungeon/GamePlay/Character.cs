using System;
using System.Drawing;
using System.Windows.Forms;

namespace DungeonGame
{
    public class Character : Panel
    {
        public Character()
        {
            Init();
        }

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

                GraphicsExtensions.FillCircle(g, Brushes.Azure, tileSize.Width / 2, tileSize.Height / 2, 5);
            }

            BackgroundImage = bg;
        }

        public void MoveTo(Point newLocation)
        {
            // if (newLocation is ok)
            Location = newLocation;
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
