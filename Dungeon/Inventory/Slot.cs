﻿using System.Drawing;
using System.Windows.Forms;

namespace DungeonGame
{
    /// <summary>
    /// 物品格子類，用於存放Item類
    /// </summary>
    public partial class Slot : UserControl
    {
        public Slot() => InitializeComponent();

        public void AddItem(string itemNum)
        {
            Item = ItemData.data[itemNum];
            Game.tb_ItemInfo.Text = Item.Info;

            Bitmap bmp = new Bitmap(ClientSize.Width, ClientSize.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.DrawImage(ItemData.data[itemNum].Icon,
                    new Rectangle(0, 0, bmp.Width, bmp.Height));
            }

            BackgroundImage = bmp;
        }

        public void RemoveItem() => AddItem("000");

        public Item Item { get; set; }
    }
}
