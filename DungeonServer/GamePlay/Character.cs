using System;
using System.Drawing;
using System.IO;

namespace DungeonServer
{
    public class Character
    {
        public Character(string inName) => name = inName;

        public void UpdateLocation(Point newLocation) => loc = newLocation;

        // call when logout
        public void Save() => File.WriteAllText(dataPath, dataPack);

        // call when login
        public void Read()
        {
            if (!File.Exists(dataPath))
                using (StreamWriter sw = File.CreateText(dataPath))
                    sw.WriteLine("0|0|0|0");

            string rawData = "";
            using (StreamReader sr = File.OpenText(dataPath))
                rawData = sr.ReadLine();
            string[] datas = rawData.Split('|');

            health = Convert.ToUInt32(datas[0]);
            atk = Convert.ToInt32(datas[1]);
            def = Convert.ToInt32(datas[2]);
            coin = Convert.ToUInt32(datas[3]);
        }

        private string name { get; set; }
        private uint health { get; set; }
        private int atk { get; set; }
        private int def { get; set; }
        private uint coin { get; set; }
        private Point loc { get; set; }
        private string dataPath => @"./saves/" + name;
        
        public string dataPack => string.Format("{0}|{1}|{2}|{3}|{4}|{5}", 
            health.ToString(), atk.ToString(), def.ToString(), coin.ToString(), loc.X, loc.Y);
    }
}
