using System;

namespace DungeonUtility
{
    public static class Rand
    {
        public static (int x, int y) GetRandPointInRect(Rect rect)
            => (rect.x0y0.x + r.Next(0, rect.Width),
                rect.x0y0.y + r.Next(0, rect.Height));

        public static int GetRandNum(int min, int max) => r.Next(min, max);

        private static readonly Random r = new Random();
    }
}
