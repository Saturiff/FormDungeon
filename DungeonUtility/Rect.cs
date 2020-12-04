using System;

namespace DungeonUtility
{
    // 0,0====1,0
    //  | Form |
    //  | Axis |
    // 0,1====1,1
    public class Rect
    {
        public Rect() { }

        public Rect(int width, int height)
        {
            x0y0 = (0, 0);
            x0y1 = (0, width);
            x1y0 = (height, 0);
            x1y1 = (height, width);
        }

        public Rect(int x, int y, int width, int height)
        {
            x0y0 = (x, y);
            x0y1 = (x, y + width);
            x1y0 = (x + height, y);
            x1y1 = (x + height, y + width);
        }

        public (int x, int y) x0y0;
        public (int x, int y) x0y1;
        public (int x, int y) x1y0;
        public (int x, int y) x1y1;
        public int width => x1y0.x - x0y0.x;
        public int height => x0y1.y - x0y0.y;

        public Rect Offset((int x, int y) p)
        {
            (int dx, int dy) offset = (p.x - x0y0.x, p.y - x0y0.y);
            return new Rect()
            {
                x0y0 = (x0y0.x + offset.dx, x0y0.y + offset.dy),
                x0y1 = (x0y1.x + offset.dx, x0y1.y + offset.dy),
                x1y0 = (x1y0.x + offset.dx, x1y0.y + offset.dy),
                x1y1 = (x1y1.x + offset.dx, x1y1.y + offset.dy)
            };
        }
    }
}
