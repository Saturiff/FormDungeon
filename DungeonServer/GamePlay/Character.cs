using System;
using System.IO;

namespace DungeonServer
{
    public class Character
    {
        public Character(string inName)
        {
            name = inName;
            for (int i = 0; i < slotNum; i++)
                items[i] = "000";
        }

        public void UpdateLocation(int newX, int newY) => loc = (newX, newY);

        // call when logout
        public void Save() => File.WriteAllText(dataPath, dataPack + "|" + itemPack);

        // call when login
        public void Read()
        {
            // 0: health(current)
            // 1: coin
            // 2: Location.X
            // 3: Location.Y
            // 4: Color.R
            // 5: Color.G
            // 6: Color.B
            // 7~22: Inventory
            if (!File.Exists(dataPath))
                using (StreamWriter sw = File.CreateText(dataPath))
                    sw.WriteLine(dataPack + "|" + itemPack);

            string rawData = "";
            using (StreamReader sr = File.OpenText(dataPath))
                rawData = sr.ReadLine();

            string[] datas = rawData.Split('|');
            health = Convert.ToUInt32(datas[0]);
            coin = Convert.ToUInt32(datas[1]);
            loc.x = Convert.ToInt32(datas[2]);
            loc.y = Convert.ToInt32(datas[3]);
            color = (Convert.ToUInt16(datas[4]), Convert.ToUInt16(datas[5]), Convert.ToUInt16(datas[6]));
            for (int i = 0; i < slotNum; i++)
                items[i] = datas[7 + i];
        }

        private static int GetNextRandomByte() => r.Next(255);

        private string name { get; set; }
        private uint health { get => 200; set { } }
        private uint coin { get; set; }
        private (int x, int y) loc = (400, 220);
        private (int r, int g, int b) color = (GetNextRandomByte(), GetNextRandomByte(), GetNextRandomByte());
        private string dataPath => @"./saves/" + name;
        private const int slotNum = 15;
        private string[] items = new string[slotNum];
        private static Random r = new Random();

        public string dataPack => string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}",
            health.ToString(), coin.ToString(), loc.x, loc.y, color.r, color.g, color.b);
        public string itemPack => string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}|{13}|{14}",
            items);
    }
}
