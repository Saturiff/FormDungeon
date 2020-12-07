using System.Drawing;

namespace DungeonGame
{
    public class Item
    {
        public string Name { get; set; }
        public int Atk { get; set; }
        public int Def { get; set; }
        public string Desc { get; set; }
        public Bitmap Icon { get; set; }
        public string Info
            => string.Format("name:\t\t{0}\r\natk\t\t{1}\r\ndef\t\t{2}\r\nDesc:\t\t{3}",
                    Name, Atk, Def, Desc);
    }
}
