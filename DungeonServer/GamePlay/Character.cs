using System;
using System.IO;

namespace DungeonServer
{
    public class Character
    {
        public Character(string inName) => name = inName;

        public void UpdateLocation(int newX, int newY) => loc = (newX, newY);

        // call when logout
        public void Save() => File.WriteAllText(dataPath, dataPack);

        // call when login
        public void Read()
        {
            // 0: health(current)
            // 1: Location.X
            // 2: Location.Y
            // 3: Color.R
            // 4: Color.G
            // 5: Color.B
            if (!File.Exists(dataPath))
                using (StreamWriter sw = File.CreateText(dataPath))
                    sw.WriteLine(dataPack);

            string rawData = "";
            using (StreamReader sr = File.OpenText(dataPath))
                rawData = sr.ReadLine();

            string[] datas = rawData.Split('|');
            health = Convert.ToUInt32(datas[0]);
            loc.x = Convert.ToInt32(datas[1]);
            loc.y = Convert.ToInt32(datas[2]);
            color = (Convert.ToUInt16(datas[3]), Convert.ToUInt16(datas[4]), Convert.ToUInt16(datas[5]));
        }

        private static int GetNextRandomByte() => r.Next(255);

        private string name { get; set; }
        private uint health { get => 200; set { } }
        private (int x, int y) loc = (400, 220);
        private (int r, int g, int b) color = (GetNextRandomByte(), GetNextRandomByte(), GetNextRandomByte());
        private string dataPath => @"./saves/" + name;
        private static Random r = new Random();

        // 玩家現有的物品，不予保存，但會在遊戲時同步給所有玩家
        public string item = "000";
        private string dataPack => string.Format("{0}|{1}|{2}|{3}|{4}|{5}",
            health.ToString(), loc.x, loc.y, color.r, color.g, color.b);
        public string dataPackWithItem => string.Format("{0}|{1}",
            dataPack, item);
    }
}
