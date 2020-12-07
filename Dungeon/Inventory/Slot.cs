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
            item = ItemData.data[itemNum];
            BackgroundImage = item.icon;
            Game.tb_ItemInfo.Text = item.info;
        }

        public void RemoveItem() => AddItem("000");

        public Item item { get; set; }
    }
}
