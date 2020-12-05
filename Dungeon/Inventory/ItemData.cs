using System.Collections.Generic;
using System.Drawing;

namespace DungeonGame
{
    public static class ItemData
    {
        public static readonly Dictionary<string, Item> data = new Dictionary<string, Item>
        {
            { "000", new Item { name = "None", atk = 0,  def = 0, desc = "", icon = emptyImage } },
            { "001", new Item { name = "Pendant", atk = 500,  def = 500, desc = "墜飾", icon = Properties.Resources.item001 } },
            { "002", new Item { name = "Buff",    atk = 150,  def = 150, desc = "增益", icon = Properties.Resources.item002 } },
            { "003", new Item { name = "Ring A",  atk = 5,  def = 5, desc = "戒指 A", icon = Properties.Resources.item003 } },
            { "004", new Item { name = "Ring B",  atk = 25,  def = 25, desc = "戒指 B", icon = Properties.Resources.item004 } },
            { "005", new Item { name = "Ring C",  atk = 75, def = 75, desc = "戒指 C", icon = Properties.Resources.item005  } },
            { "006", new Item { name = "Atk++ A", atk = 10,  def = 5, desc = "攻擊 A", icon = Properties.Resources.item006 } },
            { "007", new Item { name = "Atk++ B", atk = 50,  def = 25, desc = "攻擊 B", icon = Properties.Resources.item007 } },
            { "008", new Item { name = "Atk++ C", atk = 100, def = 50, desc = "攻擊 C", icon = Properties.Resources.item008 } },
            { "009", new Item { name = "Def++ A", atk = 5,   def = 10, desc = "防禦 A", icon = Properties.Resources.item009 } },
            { "010", new Item { name = "Def++ B", atk = 25,  def = 50, desc = "防禦 B", icon = Properties.Resources.item010 } },
            { "011", new Item { name = "Def++ C", atk = 50,  def = 100, desc = "防禦 C", icon = Properties.Resources.item011 } }
        };

        private static Bitmap emptyImage
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
