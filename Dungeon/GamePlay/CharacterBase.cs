using DungeonUtility;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace DungeonGame
{
    public class CharacterBase : Actor
    {
        protected CharacterBase() => Init();

        protected CharacterBase(string dataPack)
        {
            UpdateByDataPack(dataPack);

            Init();
        }

        private void Init()
        {
            BorderStyle = BorderStyle.FixedSingle;
            Size = characterSize;
        }

        public void UpdateByDataPack(string dataPack)
        {
            string[] datas = dataPack.Split('|');
            name = datas[0];
            currentHealth = Convert.ToUInt32(datas[1]);
            coin = Convert.ToUInt32(datas[2]);
            Location = new Point(Convert.ToInt32(datas[3]), Convert.ToInt32(datas[4]));
            BackColor = Color.FromArgb(Convert.ToUInt16(datas[5]), Convert.ToUInt16(datas[6]), Convert.ToUInt16(datas[7]));
        }

        protected void MoveTo(Point newLoc)
        {
            if (UI.map.IsWalkable(rect.Offset((newLoc.X, newLoc.Y))))
                Location = newLoc;

            ClientManager.UpdatePlayerLocation();
        }

        protected double DistanceOf(CharacterBase target)
        {
            return Math.Sqrt((Location.X - target.Location.X) * (Location.X - target.Location.X) +
                (Location.Y - target.Location.Y) * (Location.Y - target.Location.Y));
        }

        protected void Destory()
        {
            name = default;
            currentHealth = default;
            coin = default;
        }

        private Size characterSize = new Size(20, 20);
        private Rect rect => new Rect(Location.X, Location.Y, Size.Width, Size.Height);
        
        public string name { get; set; }
        public uint currentHealth { get; set; }
        public const uint maxHealth = 200;
        public int atk { get; set; }
        public int def { get; set; }
        public uint coin { get; set; }
        public bool isAlive => currentHealth <= 0;
        public const int attackRange = 100;
        public const int pickRange = 100;
    }
}
