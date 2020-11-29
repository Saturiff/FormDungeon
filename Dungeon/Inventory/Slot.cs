using System;
using System.Windows.Forms;

namespace DungeonGame
{
    /// <summary>
    /// 用於存放物品
    /// </summary>
    public partial class Slot : UserControl
    {
        public Slot() => InitializeComponent();

        public void AddItem(string itemNum)
        {
            item = ItemData.data[itemNum];
            BackgroundImage = item.icon;
        }

        public void RemoveItem() => AddItem("000");

        private void InventorySlot_Click(object sender, EventArgs e)
        {
            if (item != null) 
                UI.tb_ItemInfo.Text = item.info;

            // InventoryGrid <- FlowLayoutPanel <- Slot
            InventoryGrid parentGrid = (InventoryGrid)Parent.Parent;
            parentGrid.selected = (Slot)sender;
        }

        public Item item { get; set; }
    }
}
