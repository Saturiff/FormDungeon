using System.Drawing;
using System.Windows.Forms;

namespace DungeonGame
{
    public class Pickable : Panel, IInteractable
    {
        public Pickable((int x, int y) loc)
        {
            Location = new Point(loc.x, loc.y);

            Bitmap bmp = new Bitmap(ClientSize.Width, ClientSize.Height);
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
    }
}
