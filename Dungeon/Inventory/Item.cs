using System.Drawing;

namespace DungeonGame
{
    public class Item
    {
        public WeaponType Type { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public Bitmap Icon { get; set; }
        public Projectile Bullet { get; set; }
        public string Info
            => string.Format("name:\t\t{0}\r\nDesc:\t\t{1}", Name, Desc);
    }
}
