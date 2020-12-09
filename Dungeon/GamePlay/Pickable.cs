using System;
using System.Drawing;

namespace DungeonGame
{
    /// <summary>
    /// 在Viewport地面生成的可撿起物件
    /// </summary>
    public class Pickable : Actor
    {
        public Pickable(string itemNum, (int x, int y) loc)
        {
            ItemNum = itemNum;

            Location = new Point(loc.x, loc.y);
            Size = new Size(50, 50);

            Bitmap bmp = new Bitmap(Size.Width, Size.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.DrawImage(ItemData.weaponData[itemNum].Icon,
                    new Rectangle(0, 0, bmp.Width, bmp.Height));
            }

            BackgroundImage = bmp;
            BackColor = Color.Transparent;
        }

        public Pickable(string itemInfo)
        {
            string[] infos = itemInfo.Split('|');
            ItemNum = infos[0];
            Location = new Point(Convert.ToInt32(infos[1]), Convert.ToInt32(infos[2]));
        }

        public new void Interact()
            => Game.client.RequestPickup(this);

        public void Destory()
        {
            Game.DestroyFromViewport(this);
            Dispose();
        }

        public static bool operator ==(Pickable a, Pickable b)
        {
            return (a.ItemNum == b.ItemNum)
                && (a.Location == b.Location);
        }

        public static bool operator !=(Pickable a, Pickable b)
            => !((a.ItemNum == b.ItemNum) && (a.Location == b.Location));

        public override bool Equals(object obj)
            => (obj is Pickable p) && this == p;

        public override int GetHashCode()
            => base.GetHashCode();

        public override string ToString()
            => string.Format("{0}|{1}|{2}", ItemNum, Location.X, Location.Y);

        public string ItemNum { get; set; }
    }
}
