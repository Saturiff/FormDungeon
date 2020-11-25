using System.Collections.Generic;
using System.Windows.Forms;

namespace DungeonGame
{
    public class InventorySystem
    {
        public InventorySystem(FlowLayoutPanel player, FlowLayoutPanel their)
        {
            FLP_Player = player;
            FLP_Their = their;

            for(uint i = 0; i < slotNumber; i++)
            {
                FLP_Player.Controls.Add(new Slot());
                FLP_Their.Controls.Add(new Slot());
                playerItems.Add(new Slot());
                theirItems.Add(new Slot());
            }

            playerItems.ForEach(x => FLP_Player.Controls.Add(x));
            theirItems.ForEach(x => FLP_Their.Controls.Add(x));
        }

        public void Transfer()
        {

        }

        public void Buy()
        {

        }

        public void Sell()
        {

        }

        public void Drop()
        {

        }

        public readonly uint slotNumber = 15;
        public FlowLayoutPanel FLP_Player;
        public FlowLayoutPanel FLP_Their;
        public List<Slot> playerItems = new List<Slot>();
        public List<Slot> theirItems = new List<Slot>();

    }
}
