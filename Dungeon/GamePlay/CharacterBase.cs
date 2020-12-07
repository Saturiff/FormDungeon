using DungeonUtility;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace DungeonGame
{
    public class CharacterBase : Actor
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
            BorderStyle = BorderStyle.FixedSingle;
            Size = characterSize;
        }

        public void UpdateByDataPack(string dataPack)
        {
            string[] datas = dataPack.Split('|');
            name = datas[0];
            currentHealth = Convert.ToUInt32(datas[1]);
            Location = new Point(Convert.ToInt32(datas[2]), Convert.ToInt32(datas[3]));
            BackColor = Color.FromArgb(Convert.ToUInt16(datas[4]), Convert.ToUInt16(datas[5]), Convert.ToUInt16(datas[6]));
            item = ItemData.data[datas[7]];
        }

        protected void MoveTo(Point newLoc)
        {
            if (Game.map.IsWalkable(rect.Offset((newLoc.X, newLoc.Y))))
                Location = newLoc;

            Game.client.UpdatePlayerLocation();
        }

        public new void Interact() { }

        protected void Destory()
        {
            name = default;
            currentHealth = default;
        }

        private Size characterSize = new Size(20, 20);
        private Rect rect => new Rect(Location.X, Location.Y, Size.Width, Size.Height);

        public string name { get; set; }
        public uint currentHealth { get; set; }
        public const uint maxHealth = 200;
        public int atk => 20 + item.atk;
        public int def => item.def;
        public bool isAlive => currentHealth <= 0;
        public Item item;
        public string status
        {
            get
            {
                return "Name:\t" + name
                + Environment.NewLine + "Health:\t" + currentHealth + " / " + maxHealth
                + Environment.NewLine + "Atk:\t" + string.Format("{0}\r\n      (base:{1}, item:{2})", atk, 20, item.atk)
                + Environment.NewLine + "Def:\t" + string.Format("{0}\r\n      (base:{1}, item:{2})", def, 0, item.def);
            }
        }
    }
}
