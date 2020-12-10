using System.Drawing;

namespace DungeonUtility
{
    // 0,0====1,0
    //  | Form |
    //  | Axis |
    // 0,1====1,1
    public class Rect
    {
        public Rect() { }

        public Rect(int w, int h)
        {
            x0y0 = (0, 0);
            x0y1 = (0, h);
            x1y0 = (w, 0);
            x1y1 = (w, h);
        }

        public Rect(int x, int y, int w, int h)
        {
            x0y0 = (x, y);
            x0y1 = (x, y + h);
            x1y0 = (x + w, y);
            x1y1 = (x + w, y + h);
        }

        public Rect((int x, int y) _x0y0, (int x, int y) _x0y1, (int x, int y) _x1y0, (int x, int y) _x1y1)
        {
            x0y0 = _x0y0;
            x0y1 = _x0y1;
            x1y0 = _x1y0;
            x1y1 = _x1y1;
        }

        public (int x, int y) x0y0;
        public (int x, int y) x0y1;
        public (int x, int y) x1y0;
        public (int x, int y) x1y1;
        public int width => x1y0.x - x0y0.x;
        public int height => x0y1.y - x0y0.y;

        public Rect Offset((int x, int y) p)
        {
            (int dx, int dy) = (p.x - x0y0.x, p.y - x0y0.y);
            return new Rect((x0y0.x + dx, x0y0.y + dy),
                            (x0y1.x + dx, x0y1.y + dy),
                            (x1y0.x + dx, x1y0.y + dy),
                            (x1y1.x + dx, x1y1.y + dy));
        }

        public bool IsOverlapped(Rect rect)
            => ToRectangle(this).IntersectsWith(ToRectangle(rect));

        public bool IsOverlapped(Rectangle rect)
            => ToRectangle(this).IntersectsWith(rect);

        public static bool IsOverlapped(Rectangle rectA, Rectangle rectB)
            => rectA.IntersectsWith(rectB);

        public static Rect ToRect(Rectangle rect)
            => new Rect(rect.X, rect.Y, rect.Width, rect.Height);

        public static Rectangle ToRectangle(Rect rect)
            => new Rectangle(rect.x0y0.x, rect.x0y0.y, rect.width, rect.height);
    }
}
