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
                    g.Clear(Color.Transparent);
                    Point pos = new Point(10, 10);
                    Size slotSize = new Size(30, 30);
                    Rectangle rect = new Rectangle(pos, slotSize);

                    g.FillRectangle(new SolidBrush(Color.Transparent), rect);
                }

                return bg;
            }
            set 
            { 
                icon = value; 
            }
        }
        public string info
        {
            get
            {
                return string.Format("name:\t\t{0}\r\natk\t\t{1}\r\ndef\t\t{2}\r\nSell Price:\t{3}\r\nBuy Price:\t{4}\r\nDesc:\t\t{5}",
                    name, atk, def, sellPrice, buyPrice, desc);
            }
        }
    }
}
