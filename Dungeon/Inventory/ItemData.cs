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
            { "000", new Item { Type = WeaponType.None,            Name = "None", Atk = 0,  Def = 0, Desc = "", Icon = EmptyImage } },
            { "001", new Item { Type = WeaponType.Rifle,           Name = "Rifle", Atk = 500,  Def = 500, Desc = "墜飾", Icon = Properties.Resources.item001 } },
            { "002", new Item { Type = WeaponType.Shotgun,         Name = "Shotgun",    Atk = 150,  Def = 150, Desc = "增益", Icon = Properties.Resources.item002 } },
            { "003", new Item { Type = WeaponType.Sniper,          Name = "Sniper",  Atk = 5,  Def = 5, Desc = "戒指 A", Icon = Properties.Resources.item003 } },
            { "004", new Item { Type = WeaponType.HeavyMachineGun, Name = "HeavyMachineGun",  Atk = 25,  Def = 25, Desc = "戒指 B", Icon = Properties.Resources.item004 } },
            { "005", new Item { Type = WeaponType.Laser,           Name = "Laser",  Atk = 75, Def = 75, Desc = "戒指 C", Icon = Properties.Resources.item005  } },
            { "006", new Item { Type = WeaponType.Grenade,         Name = "Grenade", Atk = 10,  Def = 5, Desc = "攻擊 A", Icon = Properties.Resources.item006 } },
            { "007", new Item { Type = WeaponType.RPG,             Name = "RPG", Atk = 50,  Def = 25, Desc = "攻擊 B", Icon = Properties.Resources.item007 } }
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
