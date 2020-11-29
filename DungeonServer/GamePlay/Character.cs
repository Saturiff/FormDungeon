using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace DungeonServer
{
    public class Character
    {
        public Character(string inName)
        {
            name = inName;
            for(int i = 0; i < 15; i++)
                items[i] = "000";
        }

        public void UpdateLocation(Point newLocation) => loc = newLocation;

        // call when logout
        public void Save() => File.WriteAllText(dataPath, dataPack + "|" + itemPack);

        // call when login
        public void Read()
        {
            if (!File.Exists(dataPath))
                using (StreamWriter sw = File.CreateText(dataPath))
                    sw.WriteLine("0|0|0|0|0|0|000|000|000|000|000|000|000|000|000|000|000|000|000|000|000");
            
            string rawData = "";
            using (StreamReader sr = File.OpenText(dataPath))
                rawData = sr.ReadLine();

            string[] datas = rawData.Split('|');

            health = Convert.ToUInt32(datas[0]);
            atk = Convert.ToInt32(datas[1]);
            def = Convert.ToInt32(datas[2]);
            coin = Convert.ToUInt32(datas[3]);

            for (int i = 0; i < 15; i++)
                items[i] = datas[4 + i];
        }

        private string name { get; set; }
        private uint health { get; set; }
        private int atk { get; set; }
        private int def { get; set; }
        private uint coin { get; set; }
        private Point loc = new Point(400, 220); // todo: spwan point
        private string dataPath => @"./saves/" + name;
        private string[] items = new string[15];
        public string dataPack => string.Format("{0}|{1}|{2}|{3}|{4}|{5}", 
            health.ToString(), atk.ToString(), def.ToString(), coin.ToString(), loc.X, loc.Y);
        public string itemPack => string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}|{13}|{14}",
            items);
    }
}
