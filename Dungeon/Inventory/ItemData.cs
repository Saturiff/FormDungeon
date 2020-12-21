using DungeonGame.Weapons;
using System.Collections.Generic;
using System.Drawing;

namespace DungeonGame
{
    /// <summary>
    /// 物品資料，由此提取Item類
    /// </summary>
    public class ItemData
    {
        public static readonly Dictionary<string, Item> weaponData = new Dictionary<string, Item>
        {
            { "0", new Item { Type = WeaponType.None,    Bullet = new Projectile(),    Name = "None",    Desc = "無武器", Icon = EmptyImage                   } },
            { "1", new Item { Type = WeaponType.Rifle,   Bullet = new RifleBullet(),   Name = "Rifle",   Desc = "步槍",   Icon = Properties.Resources.item001 } },
            { "2", new Item { Type = WeaponType.Shotgun, Bullet = new ShotgunBullet(), Name = "Shotgun", Desc = "霰彈槍", Icon = Properties.Resources.item002 } },
            { "3", new Item { Type = WeaponType.Sniper,  Bullet = new SniperBullet(),  Name = "Sniper",  Desc = "狙擊槍", Icon = Properties.Resources.item003 } },
            { "4", new Item { Type = WeaponType.Grenade, Bullet = new Grenade(),       Name = "Grenade", Desc = "手榴彈", Icon = Properties.Resources.item004 } },
            { "5", new Item { Type = WeaponType.RPG,     Bullet = new Rpg(),           Name = "RPG",     Desc = "火箭筒", Icon = Properties.Resources.item005 } }
        };

        public static Projectile GetBullet(string weaponNum)
        {
            return new Projectile
            {
                type = weaponData[weaponNum].Bullet.type,
                damage = weaponData[weaponNum].Bullet.damage,
                lifetime = weaponData[weaponNum].Bullet.lifetime,
                speed = weaponData[weaponNum].Bullet.speed
            };
        }

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
