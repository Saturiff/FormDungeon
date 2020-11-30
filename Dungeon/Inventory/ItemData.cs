using System.Collections.Generic;

namespace DungeonGame
{
    public static class ItemData
    {
        public static readonly Dictionary<string, Item> data = new Dictionary<string, Item>
        {
            { "000", new Item { name = "None", atk = 0,  def = 0,  buyPrice = 0, sellPrice = 0, desc = "" } },
            { "001", new Item { name = "Pendant", atk = 500,  def = 500,  buyPrice = 15000, sellPrice = 3000, desc = "墜飾" } },
            { "002", new Item { name = "Buff",    atk = 150,  def = 150,  buyPrice = 5000, sellPrice = 1000, desc = "增益" } },
            { "003", new Item { name = "Ring A",  atk = 5,  def = 5,   buyPrice = 75, sellPrice = 10, desc = "戒指 A" } },
            { "004", new Item { name = "Ring B",  atk = 25,  def = 25,   buyPrice = 800, sellPrice = 200, desc = "戒指 B" } },
            { "005", new Item { name = "Ring C",  atk = 75, def = 75,   buyPrice = 1500, sellPrice = 300, desc = "戒指 C"  } },
            { "006", new Item { name = "Atk++ A", atk = 10,  def = 5,   buyPrice = 100, sellPrice = 20 , desc = "攻擊 A" } },
            { "007", new Item { name = "Atk++ B", atk = 50,  def = 25,  buyPrice = 1000, sellPrice = 200, desc = "攻擊 B" } },
            { "008", new Item { name = "Atk++ C", atk = 100, def = 50,  buyPrice = 1500, sellPrice = 300, desc = "攻擊 C" } },
            { "009", new Item { name = "Def++ A", atk = 5,   def = 10,  buyPrice = 100, sellPrice = 20, desc = "防禦 A"} },
            { "010", new Item { name = "Def++ B", atk = 25,  def = 50,  buyPrice = 1000, sellPrice = 200, desc = "防禦 B"} },
            { "011", new Item { name = "Def++ C", atk = 50,  def = 100, buyPrice = 1500, sellPrice = 300, desc = "防禦 C" } }
        };
    }
}
