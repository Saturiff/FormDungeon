using System;
using System.Windows.Forms;

namespace DungeonGame
{
    public partial class Slot : UserControl
    {
        public Slot()
        {
            InitializeComponent();
        }

        public void AddItem(Item inItem)
        {
            item = inItem;
            BackgroundImage = item.icon;
        }

        public void RemoveItem()
        {
            item = default;
            BackgroundImage = default;
        }

        private void InventorySlot_Click(object sender, EventArgs e)
        {
            if (item != null) UI.tb_ItemInfo.Text = item.info;
        }

        public Item item;
    }
}
