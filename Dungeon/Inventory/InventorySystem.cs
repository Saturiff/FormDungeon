namespace DungeonGame.Inventory
{
    /// <summary>
    /// 管理所有在Form中的InventoryGrid
    /// </summary>
    public class InventorySystem
    {
        // Resync all slot, call by ClientManager
        public void Update(string itemPack)
        {
            UI.inv_Player.itemPack = itemPack;
        }

        // other player item -> current player item
        // Player selected (in player's inventory) item
        // To their inventory
        public void Transfer()
        {
            // ClientManager.
            // SendToServer();
            // while(not get message);
            // if(success) remove
            // else ui.log.add(full)
            //if(UI.inv_Their.selectedIdx != -1)
            //     ClientManager.RequestCharacterItem()
            //     UI.inv_Their.selected
            // UI.inv_Player.selectedIdx
        }

        // Remove item from player's inventory
        public void Drop()
        {
            int idx = UI.inv_Player.selectedIdx;
            ClientManager.RequestDropItem(idx);
            UI.inv_Player.RemoveItem(idx);
        }

        public void Clear(InventoryGrid target)
        {
            target.ClearInventory();
        }
    }
}
