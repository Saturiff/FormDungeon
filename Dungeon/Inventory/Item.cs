using System.Drawing;

namespace DungeonGame
{
    public class Item
    {
        public string name { get; set; }
        public int atk { get; set; }
        public int def { get; set; }
        public string desc { get; set; }
        public Bitmap icon { get; set; }
        public string info
        {
            get
            {
                return string.Format("name:\t\t{0}\r\natk\t\t{1}\r\ndef\t\t{2}\r\nDesc:\t\t{3}",
                    name, atk, def, desc);
            }
        }
    }
}
