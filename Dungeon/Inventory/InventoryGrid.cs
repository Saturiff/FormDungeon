using System.Collections.Generic;
using System.Windows.Forms;

namespace DungeonGame
{
    /// <summary>
    /// 管理15格物品格子的調度
    /// </summary>
    public partial class InventoryGrid : UserControl
    {
        public InventoryGrid() => InitializeComponent();

        public void ClearInventory()
        {
            foreach (var slot in slots)
                ((Slot)slot).RemoveItem();
        }

        // 新增至空格
        public bool AddItem(string itemNum)
        {
            int slotIdx = FindEmptySlot();
            if (slotIdx == -1)
                return false;

            UpdateItem(itemNum, slotIdx);
            return true;
        }

        // 替換
        public void UpdateItem(string itemNum, int slotIdx)
            => ((Slot)slots[slotIdx]).AddItem(itemNum);

        // 移除
        public void RemoveItem(int slotIdx)
            => ((Slot)slots[slotIdx]).RemoveItem();

        private string GetItemIndexByItem(Item item)
        {
            foreach (var itemNum in ItemData.data.Keys)
                if (ItemData.data[itemNum] == item)
                    return itemNum;

            return "000";
        }

        // -1: full
        private int FindEmptySlot()
        {
            for (int i = 0; i < slots.Count; i++)
                if (GetItemIndexByItem(((Slot)slots[i]).item) == "000")
                    return i;

            return -1;
        }

        private ControlCollection slots => ((FlowLayoutPanel)Controls[0]).Controls;

        public Slot selected { get; set; }
        public int selectedIdx
        {
            get
            {
                if (selected != null)
                    for (int i = 0; i < slots.Count; i++)
                        if (slots[i] == selected)
                            return i;

                return -1;
            }
        }
        public string itemPack
        {
            get
            {
                List<string> itemNums = new List<string>();

                foreach (var slot in slots)
                    itemNums.Add(GetItemIndexByItem(((Slot)slot).item));

                return string.Join("|", itemNums);
            }
        }
    }
}
