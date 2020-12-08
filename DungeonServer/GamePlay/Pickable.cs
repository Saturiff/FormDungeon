using DungeonUtility;
using System;

namespace DungeonServer
{
    /// <summary>
    /// 伺服器可撿起類，保存著一個Item類編號，隨機生成在地圖中
    /// </summary>
    public class Pickable
    {
        public Pickable(string itemNum, (int x, int y) loc)
        {
            ItemNum = itemNum;
            Location = loc;
        }

        public Pickable(string itemInfo)
        {
            string[] infos = itemInfo.Split('|');
            ItemNum = infos[0];
            Location = (Convert.ToInt32(infos[1]), Convert.ToInt32(infos[2]));
        }

        public static string GetRandomItemNum()
        {
            int num = Rand.GetRandNum(1, 7);

            return ((num < 10) ? "00" : "0") + num.ToString();
        }

        public static bool operator ==(Pickable a, Pickable b)
        {
            return (a.ItemNum == b.ItemNum)
                && (a.Location == b.Location);
        }

        public static bool operator !=(Pickable a, Pickable b)
        {
            return !((a.ItemNum == b.ItemNum)
                && (a.Location == b.Location));
        }

        public override bool Equals(object obj) => base.Equals(obj);

        public override int GetHashCode() => base.GetHashCode();

        public override string ToString() => string.Format("{0}|{1}|{2}", ItemNum, Location.x, Location.y);

        private (int x, int y) Location { get; set; }

        public string ItemNum { get; set; }
        public static readonly (int w, int h) size = (20, 20);
        public Rect Rect => new Rect(Location.x, Location.y, size.w, size.h);
    }
}
