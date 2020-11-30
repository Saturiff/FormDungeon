using System;
using System.Drawing;
using System.Windows.Forms;

namespace DungeonGame
{
    public class CharacterBase : Panel
    {
        protected CharacterBase()
        {
            Init();
        }

        protected CharacterBase(string dataPack)
        {
            UpdateByDataPack(dataPack);

            Init();
        }

        private void Init()
        {
            Size = characterSize;
            BorderStyle = BorderStyle.FixedSingle;
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

        protected void MoveTo(Point newLocation)
        {
            if (UI.map.IsWalkable(newLocation))
                Location = newLocation;

            ClientManager.UpdatePlayerLocation();
        }

        public void Attack(CharacterBase target) { }

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
            isIndestructible = default;
        }

        private Size characterSize = new Size(20, 20);

        public string name { get; set; }
        public uint currentHealth { get; set; }
        public const uint maxHealth = 200;
        public int atk { get; set; }
        public int def { get; set; }
        public uint coin { get; set; }
        public bool isIndestructible { get; set; }
        public bool isAlive => currentHealth <= 0;
        public const int attackRange = 100;
    }
}
