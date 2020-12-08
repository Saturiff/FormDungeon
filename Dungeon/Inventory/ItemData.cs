using System.Collections.Generic;
using System.Drawing;

namespace DungeonGame
{
    /// <summary>
    /// 物品資料，由此提取Item類
    /// </summary>
    public static class ItemData
    {
        public static readonly Dictionary<string, Item> data = new Dictionary<string, Item>
        {
            { "000", new Item { Type = WeaponType.None,            Bullet = new Projectile{ type = AmmunitionType.None,   damage =   0, lifetime =    0, speed =  0 }, Name = "None",            Desc = "無武器", Icon = EmptyImage                   } },
            { "001", new Item { Type = WeaponType.Rifle,           Bullet = new Projectile{ type = AmmunitionType.Auto,   damage =  25, lifetime = 1500, speed =  4 }, Name = "Rifle",           Desc = "步槍",   Icon = Properties.Resources.item001 } },
            { "002", new Item { Type = WeaponType.Shotgun,         Bullet = new Projectile{ type = AmmunitionType.Mult,   damage =  50, lifetime = 1000, speed =  4 }, Name = "Shotgun",         Desc = "霰彈槍", Icon = Properties.Resources.item002 } },
            { "003", new Item { Type = WeaponType.Sniper,          Bullet = new Projectile{ type = AmmunitionType.Single, damage =  80, lifetime = 2000, speed =  7 }, Name = "Sniper",          Desc = "狙擊槍", Icon = Properties.Resources.item003 } },
            { "004", new Item { Type = WeaponType.HeavyMachineGun, Bullet = new Projectile{ type = AmmunitionType.Auto,   damage =  20, lifetime = 1500, speed =  6 }, Name = "HeavyMachineGun", Desc = "重機槍", Icon = Properties.Resources.item004 } },
            { "005", new Item { Type = WeaponType.Laser,           Bullet = new Projectile{ type = AmmunitionType.Dot,    damage =   5, lifetime =    0, speed = 10 }, Name = "Laser",           Desc = "雷射槍", Icon = Properties.Resources.item005 } },
            { "006", new Item { Type = WeaponType.Grenade,         Bullet = new Projectile{ type = AmmunitionType.Blast,  damage = 100, lifetime = 3000, speed =  3 }, Name = "Grenade",         Desc = "手榴彈", Icon = Properties.Resources.item006 } },
            { "007", new Item { Type = WeaponType.RPG,             Bullet = new Projectile{ type = AmmunitionType.Blast,  damage = 120, lifetime = 3000, speed = 10 }, Name = "RPG",             Desc = "火箭筒", Icon = Properties.Resources.item007 } }
        };

        private static Bitmap EmptyImage
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
        }
    }
}
