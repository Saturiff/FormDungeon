using System;

namespace DungeonUtility
{
    public static class Rand
    {
        public static (int x, int y) GetRandPointInRect(Rect rect)
            => (rect.x0y0.x + r.Next(0, rect.width),
                rect.x0y0.y + r.Next(0, rect.height));

        public static string GetRandItemNum()
        {
            int randResult = r.Next(0, 11); // fix: magic number
            string itemStr = randResult.ToString();
            if (randResult < 10) itemStr = "00" + itemStr;
            else if (randResult < 10) itemStr = "0" + itemStr;

            return itemStr;
        }

        private static Random r = new Random();
    }
}
