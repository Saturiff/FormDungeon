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
            this.health = health;
            this.atk = atk;
            this.def = def;
            this.coin = coin;

            Init();
        }

        public Character(string dataPack)
        {
            UpdateByDataPack(dataPack);

            Init();
        }

        private void Init()
        {
            Size = characterSize;
            
            DrawCharacter();
        }

        public void UpdateByDataPack(string dataPack)
        {
            string[] datas = dataPack.Split('|');
            name = datas[0];
            health = Convert.ToUInt32(datas[1]);
            atk = Convert.ToInt32(datas[2]);
            def = Convert.ToInt32(datas[3]);
            coin = Convert.ToUInt32(datas[4]);
            Location = new Point(Convert.ToInt32(datas[5]), Convert.ToInt32(datas[6]));
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

            // timer tick?
            ClientManager.UpdatePlayerLocation();
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
