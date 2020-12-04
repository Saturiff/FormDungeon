using System.Drawing;

namespace DungeonGame
{
    public class Pickable : Actor
    {
        public Pickable((int x, int y) loc, string itemNum)
        {
            this.itemNum = itemNum;
            Location = new Point(loc.x, loc.y);
            BackgroundImage = ItemData.data[this.itemNum].icon;
            Size = new Size(size.width, size.height);
        }

        public new void Interact()
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
        public static (int width, int height) size = (20, 20);
    }
}
