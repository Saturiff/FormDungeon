using DungeonUtility;

namespace DungeonServer
{
    public class Pickable
    {
        public string itemNum { get; set; }
        public (int x, int y) location { get; set; }
        public static readonly (int w, int h) size = (20, 20);
        public Rect rect => new Rect(location.x, location.y, size.w, size.h);
        public static string GetRandomItemNum()
        {
            int num = Rand.GetRandNum(1, 11);

            return ((num < 10) ? "00" : "0") + num.ToString();
        }

        public new string ToString() => string.Format("{0}|{1}|{2}", itemNum, location.x, location.y);
    }
}
