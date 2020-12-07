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
            BackgroundImage = Item.Icon;
            Game.tb_ItemInfo.Text = Item.Info;
        }

        public void RemoveItem() => AddItem("000");

        public Item Item { get; set; }
    }
}
