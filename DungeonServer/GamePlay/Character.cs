using System;
using System.IO;
using System.Threading.Tasks;

namespace DungeonServer
{
    /// <summary>
    /// 伺服器端的玩家類，提供新建、儲存、載入存檔之功能，與保存玩家在遊戲中的狀態
    /// </summary>
    public class Character
    {
        public Character(string inName) => name = inName;

        public void UpdateLocation(int newX, int newY) => loc = (newX, newY);

        // call when logout
        public void Save() => File.WriteAllText(DataPath, DataPack);

        // call when login
        public void Read()
        {
            // 0: health(current)
            // 1: Location.X
            // 2: Location.Y
            // 3: Color.R
            // 4: Color.G
            // 5: Color.B
            if (!File.Exists(DataPath))
                using (StreamWriter sw = File.CreateText(DataPath))
                    sw.WriteLine(DataPack);

            string rawData = "";
            using (StreamReader sr = File.OpenText(DataPath))
                rawData = sr.ReadLine();

            string[] datas = rawData.Split('|');
            health = Convert.ToInt32(datas[0]);
            loc.x = Convert.ToInt32(datas[1]);
            loc.y = Convert.ToInt32(datas[2]);
            color = (Convert.ToUInt16(datas[3]), Convert.ToUInt16(datas[4]), Convert.ToUInt16(datas[5]));
        }

        public async void Respawn(Map map)
        {
            isRespawning = true;

            await Task.Delay(3000);

            health = 200;
            item = "000";

            (int x, int y) = map.GetRandomFitPointInPlayGround(size.w, size.h);

            UpdateLocation(x, y);

            UI.server.Respawn(name);

            isRespawning = false;
            UI.AddLog(name + " respawned.");
        }

        private static int GetNextRandomByte() => r.Next(255);

        private string name;

        private static readonly (int w, int h) size = (20, 20);
        private (int r, int g, int b) color = (GetNextRandomByte(), GetNextRandomByte(), GetNextRandomByte());
        private string DataPath => @"./saves/" + name;
        private static readonly Random r = new Random();

        private string DataPack => string.Format("{0}|{1}|{2}|{3}|{4}|{5}",
            health.ToString(), loc.x, loc.y, color.r, color.g, color.b);
        public string DataPackWithItem => string.Format("{0}|{1}",
            DataPack, item);
        public string RespawnDataPack => string.Format("{0}|{1}|{2}|{3}",
            health.ToString(), loc.x, loc.y, item);

        public int health = 200;
        public bool isRespawning = false;
        public (int x, int y) loc = (400, 220);
        // 玩家現有的物品，不予保存，但會在遊戲時同步給所有玩家
        public string item = "000";
    }
}
