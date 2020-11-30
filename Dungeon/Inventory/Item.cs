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
        public Bitmap icon { get; set; }
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
