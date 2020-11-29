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
            // 0: health
            // 1: atk
            // 2: def
            // 3: coin
            // 4: Location.X
            // 5: Location.Y
            // 6: Color.R
            // 7: Color.G
            // 8: Color.B
            // 9~24: Inventory
            if (!File.Exists(dataPath))
                using (StreamWriter sw = File.CreateText(dataPath))
                    sw.WriteLine(dataPack + "|" + itemPack);

            string rawData = "";
            using (StreamReader sr = File.OpenText(dataPath))
                rawData = sr.ReadLine();

            string[] datas = rawData.Split('|');

            health = Convert.ToUInt32(datas[0]);
            atk = Convert.ToInt32(datas[1]);
            def = Convert.ToInt32(datas[2]);
            coin = Convert.ToUInt32(datas[3]);
            loc.X = Convert.ToInt32(datas[4]);
            loc.Y = Convert.ToInt32(datas[5]);
            color = Color.FromArgb(Convert.ToUInt16(datas[6]), Convert.ToUInt16(datas[7]), Convert.ToUInt16(datas[8]));
            for (int i = 0; i < 15; i++)
                items[i] = datas[9 + i];
        }

        private static int GetNextRandomByte() => r.Next(255);

        private string name { get; set; }
        private uint health { get; set; }
        private int atk { get; set; }
        private int def { get; set; }
        private uint coin { get; set; }
        private Point loc = new Point(400, 220); // todo: spwan point
        private Color color = Color.FromArgb(GetNextRandomByte(), GetNextRandomByte(), GetNextRandomByte());
        private string dataPath => @"./saves/" + name;
        private string[] items = new string[15];
        private static Random r = new Random();

        public string dataPack => string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}",
            health.ToString(), atk.ToString(), def.ToString(), coin.ToString(), loc.X, loc.Y, color.R, color.G, color.B);
        public string itemPack => string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}|{13}|{14}",
            items);
    }
}
