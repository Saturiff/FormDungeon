namespace DungeonGame.Inventory
{
    /// <summary>
    /// 管理所有在Form中的InventoryGrid
    /// </summary>
    public class InventorySystem
    {
        public InventorySystem()
        {

        }

        // Resync all slot, call by ClientManager
        public void Update(string name, string itemPack)
        {
            if (name == UI.player.name)
                UI.inv_Player.itemPack = itemPack;
            else
                UI.inv_Their.itemPack = itemPack;
        }

        public void Use()
        {
            if (UseItem())
                UI.inv_Player.RemoveItem(UI.inv_Player.selectedIdx);
        }

        // todo: 是否成功使用物品
        private bool UseItem()
        {
            return true;
        }

        // player - player
        // Player selected (in player's inventory) item
        // To their inventory
        // ---
        // shop - player
        // Buy()
        // To player's inventory
        public void Transfer()
        {
            // ClientManager.
            // SendToServer();
            // while(not get message);
            // if(success) remove
            // else ui.log.add(full)

            // Buy();
            // UI.inv_Their.UpdateItem
        }

        // Player selected (in shop's inventory) item
        // Calc(price-coin) 
        // Update coin
        // Transfer to player's inventory
        public void Buy()
        {

        }

        // Player selected (in player's inventory) item
        // Calc(sell price+coin)
        // Update coin
        // Transfer to player's inventory
        public void Sell()
        {

        }

        // Remove item from player's inventory
        public void Drop()
        {
            UI.inv_Player.RemoveItem(UI.inv_Player.selectedIdx);
        }

        private void Clear(InventoryGrid target)
        {
            target.ClearInventory();
        }
    }
}
