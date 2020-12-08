using System.Drawing;
using System.Windows.Forms;

namespace DungeonGame
{
    public class Projectile : Actor
    {
        public Projectile()
        {
            Size = new Size(8, 8);

            BackColor = Color.Red;

            lifetimeTimer.Interval = lifetime;
            lifetimeTimer.Tick += LifetimeTimer_Tick;
            lifetimeTimer.Start();
        }

        private void LifetimeTimer_Tick(object sender, System.EventArgs e)
        {
            Game.DestroyFromViewport(this);
            Dispose();
        }

        public bool IsOverlapped(Rectangle rect) => Rect.IntersectsWith(rect);

        public int lifetime; // 1/1000 s
        public int speed;
        public Point target;

        public Rectangle Rect => DisplayRectangle;
        public Timer lifetimeTimer = new Timer();
        public Timer flyTimer = new Timer();
    }
}
