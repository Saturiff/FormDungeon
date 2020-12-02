using System.Drawing;
using System.Windows.Forms;

namespace DungeonGame
{
    public class Pickable : Panel, IInteractable
    {
        public Pickable((int x, int y) loc)
        {
            Location = new Point(loc.x, loc.y);
            BackgroundImage = ItemData.data[itemNum].icon;
        }

        public void Interact()
        {
            var result = UI.inv_Player.FindEmptySlot();
            if (result.isSuccess)
                Pickup();
        }

        private void Pickup()
        {
            UI.inv_Player.AddItem(itemNum);
        }

        public void Destory()
        {
            UI.p_Viewport.Controls.Remove(this);
            Dispose();
        }

        public string itemNum { get; set; }
    }
}
