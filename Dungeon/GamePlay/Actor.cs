using System;
using System.Drawing;
using System.Windows.Forms;

namespace DungeonGame
{
    /// <summary>
    /// 任何產生在Viewport中的物件之基底，不應由此類直接創建遊戲物件，而是新增子類繼承
    /// </summary>
    public class Actor : Panel, IInteractable
    {
        public void Interact() { }

        public double DistanceOf(Actor target)
            => Math.Sqrt((Location.X - target.Location.X) * (Location.X - target.Location.X) +
                (Location.Y - target.Location.Y) * (Location.Y - target.Location.Y));

        public static double DistanceOf((int x, int y) pA, (int x, int y) pB)
            => Math.Sqrt((pA.x - pB.x) * (pA.x - pB.x) + (pA.y - pB.y) * (pA.y - pB.y));

        public bool IsOverlapped(Rectangle rect) => Rect.IntersectsWith(rect);

        public Rectangle Rect => DisplayRectangle;
    }
}
