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
            UI.tb_ItemInfo.Text = item.info;
        }

        public void RemoveItem() => AddItem("000");

        public Item item { get; set; }
    }
}
