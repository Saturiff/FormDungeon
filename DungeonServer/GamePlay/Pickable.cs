using DungeonUtility;
using System;

namespace DungeonServer
{
    public class Pickable
    {
        public Pickable(string itemNum, (int x, int y) loc)
        {
            this.itemNum = itemNum;
            location = loc;
        }

        public Pickable(string itemInfo)
        {
            string[] infos = itemInfo.Split('|');
            itemNum = infos[0];
            location = (Convert.ToInt32(infos[1]), Convert.ToInt32(infos[2]));
        }

        public static string GetRandomItemNum()
        {
            int num = Rand.GetRandNum(1, 11);

            return ((num < 10) ? "00" : "0") + num.ToString();
        }

        public static bool operator ==(Pickable a, Pickable b)
        {
            return (a.itemNum == b.itemNum)
                && (a.location == b.location);
        }

        public static bool operator !=(Pickable a, Pickable b)
        {
            return !((a.itemNum == b.itemNum)
                && (a.location == b.location));
        }

        public new string ToString() => string.Format("{0}|{1}|{2}", itemNum, location.x, location.y);

        public string itemNum { get; set; }
        public (int x, int y) location { get; set; }
        public static readonly (int w, int h) size = (20, 20);
        public Rect rect => new Rect(location.x, location.y, size.w, size.h);
    }
}
