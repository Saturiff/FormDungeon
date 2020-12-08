using DungeonUtility;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace DungeonGame
{
    /// <summary>
    /// 玩家角色之基底類
    /// </summary>
    public class CharacterBase : Actor
    {
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
            Name = datas[0];
            CurrentHealth = Convert.ToUInt32(datas[1]);
            Location = new Point(Convert.ToInt32(datas[2]), Convert.ToInt32(datas[3]));
            BackColor = Color.FromArgb(Convert.ToUInt16(datas[4]), Convert.ToUInt16(datas[5]), Convert.ToUInt16(datas[6]));
            item = ItemData.data[datas[7]];
        }

        protected void MoveTo(Point newLoc)
        {
            if (Game.map.IsWalkable(Rect.Offset((newLoc.X, newLoc.Y))))
                Location = newLoc;

            Game.client.UpdatePlayerLocation();
        }

        public new void Interact() { }

        protected void Destory()
        {
            Name = default;
            CurrentHealth = default;
        }

        private static readonly Size characterSize = new Size(20, 20);
        private const uint maxHealth = 200;
        private uint CurrentHealth { get; set; }
        private Rect Rect => new Rect(Location.X, Location.Y, Size.Width, Size.Height);

        public Item item;
        public new string Name { get; set; }
        public bool IsAlive => CurrentHealth <= 0;
        public string Status
        {
            get
            {
                return "Name:\t" + Name + Environment.NewLine 
                + "Health:\t" + CurrentHealth + " / " + maxHealth;
            }
        }
    }
}
