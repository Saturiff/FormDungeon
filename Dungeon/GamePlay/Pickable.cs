using System;
using System.Drawing;

namespace DungeonGame
{
    public class Pickable : Actor
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

        public Pickable(string itemInfo)
        {
            string[] infos = itemInfo.Split('|');
            itemNum = infos[0];
            Location = new Point(Convert.ToInt32(infos[1]), Convert.ToInt32(infos[2]));
        }

        public new void Interact()
        {
            if (DistanceOf(Game.player) < Player.pickRange)
                Game.client.RequestPickup(this);
        }

        public void Destory()
        {
            Game.p_Viewport.Controls.Remove(this);
            Dispose();
        }

        public static bool operator ==(Pickable a, Pickable b)
        {
            return (a.itemNum == b.itemNum)
                && (a.Location == b.Location);
        }

        public static bool operator !=(Pickable a, Pickable b)
        {
            return !((a.itemNum == b.itemNum)
                && (a.Location == b.Location));
        }

        public new string ToString() => string.Format("{0}|{1}|{2}", itemNum, Location.X, Location.Y);

        public string itemNum { get; set; }
        public static readonly Size size = new Size(20, 20);
    }
}
