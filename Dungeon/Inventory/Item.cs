using System;
using System.Drawing;

namespace DungeonGame
{
    public class Item
    {
        public string name { get; set; }
        public int atk { get; set; }
        public int def { get; set; }
        public int sellPrice { get; set; }
        public int buyPrice { get; set; }
        public string desc { get; set; }
        public Bitmap icon
        {
            get
            {
                Bitmap bg = new Bitmap(50, 50);
                using (Graphics g = Graphics.FromImage(bg))
                {
                    g.Clear(Color.Black);
                    Point pos = new Point(10, 10);
                    Size slotSize = new Size(30, 30);
                    Rectangle rect = new Rectangle(pos, slotSize);

                    g.FillRectangle(new SolidBrush(Color.Black), rect);
                }

                return bg;
            }
            set { icon = value; }
        }
        public string info
        {
            get
            {
                return "name:\t\t" + name
                    + Environment.NewLine + "atk:\t\t" + atk
                    + Environment.NewLine + "def:\t\t" + def
                    + Environment.NewLine + "sellPrice:\t" + sellPrice
                    + Environment.NewLine + "buyPrice:\t" + buyPrice
                    + Environment.NewLine + "desc:\t\t" + desc;
            }
        }
    }
}
