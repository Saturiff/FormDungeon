using System.Drawing;
using System.Windows.Forms;

namespace DungeonGame
{
    public class Pickable : Panel, IInteractable
    {
        public Pickable(string itemNum, (int x, int y) loc)
        {
            this.itemNum = itemNum;

            Location = new Point(loc.x, loc.y);
            Size = size;

            Bitmap bmp = new Bitmap(size.Width, size.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.DrawImage(ItemData.data[itemNum].icon,
                    new Rectangle(0, 0, bmp.Width, bmp.Height));
            }

            BackgroundImage = bmp;
        }

        public void Interact()
        {
            if (UI.s_Slot.item != ItemData.data["000"])
                Pickup();
        }

        private void Pickup()
        {
            UI.s_Slot.AddItem(itemNum);
        }

        public void Destory()
        {
            UI.p_Viewport.Controls.Remove(this);
            Dispose();
        }

        public string itemNum { get; set; }
        public static readonly Size size = new Size(20, 20);
    }
}
