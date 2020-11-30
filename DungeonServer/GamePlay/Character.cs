using System;
using System.Drawing;
using System.IO;

namespace DungeonServer
{
    public class Character
    {
        public Character(string inName)
        {
            name = inName;
            for (int i = 0; i < 15; i++)
                items[i] = "000";
        }

        public void UpdateLocation(Point newLocation) => loc = newLocation;

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
            loc.X = Convert.ToInt32(datas[2]);
            loc.Y = Convert.ToInt32(datas[3]);
            color = Color.FromArgb(Convert.ToUInt16(datas[4]), Convert.ToUInt16(datas[5]), Convert.ToUInt16(datas[6]));
            for (int i = 0; i < 15; i++)
                items[i] = datas[7 + i];
        }

        private static int GetNextRandomByte() => r.Next(255);

        private string name { get; set; }
        private uint health { get; set; }
        private uint coin { get; set; }
        private Point loc = new Point(400, 220);
        private Color color = Color.FromArgb(GetNextRandomByte(), GetNextRandomByte(), GetNextRandomByte());
        private string dataPath => @"./saves/" + name;
        private string[] items = new string[15];
        private static Random r = new Random();

        public string dataPack => string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}",
            health.ToString(), coin.ToString(), loc.X, loc.Y, color.R, color.G, color.B);
        public string itemPack => string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}|{13}|{14}",
            items);
    }
}
