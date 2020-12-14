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
            CurrentHealth = Convert.ToInt32(datas[1]);
            Location = new Point(Convert.ToInt32(datas[2]), Convert.ToInt32(datas[3]));
            BackColor = Color.FromArgb(Convert.ToUInt16(datas[4]), Convert.ToUInt16(datas[5]), Convert.ToUInt16(datas[6]));
            itemNum = datas[7];
        }

        public void UpdateHealth(int newHealth)
        {
            CurrentHealth = newHealth;
        }

        public void Respawn(int hp, int x, int y, string itemNum)
        {
            CurrentHealth = hp;
            Location = new Point(x, y);
            this.itemNum = itemNum;
        }

        protected void MoveTo(Point newLoc)
        {
            if (Game.map.IsWalkable(Rect.ToRect(ActorRect).Offset((newLoc.X, newLoc.Y))))
                Location = newLoc;

            Game.client.RequestUpdatePlayerLocation();
        }

        public new void Interact() { }

        protected void Destory()
        {
            Name = default;
            CurrentHealth = default;
        }

        private static readonly Size characterSize = new Size(20, 20);
        protected const int maxHealth = 200;
        protected int currentHealth;

        public int CurrentHealth
        {
            get => currentHealth;
            set
            {
                currentHealth = value;

                if (IsAlive)
                    Show();
                else
                    Hide();
            }
        }
        public static int MaxHealth => maxHealth;
        public bool IsAlive => currentHealth > 0;
        public string itemNum;
        public new string Name { get; set; }
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
