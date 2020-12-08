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
        }

        private void LifetimeTimer_Tick(object sender, System.EventArgs e)
        {
            Game.DestroyFromViewport(this);
            Dispose();
        }

        public void Start()
        {
            lifetimeTimer.Interval = lifetime;
            lifetimeTimer.Tick += LifetimeTimer_Tick;
            lifetimeTimer.Start();
        }

        public bool IsOverlapped(Rectangle rect) => Rect.IntersectsWith(rect);

        public AmmunitionType type;
        public int damage;
        public int lifetime; // 1/1000 s
        public int speed;
        public Point target;

        public Rectangle Rect => DisplayRectangle;
        public Timer lifetimeTimer = new Timer();
        public Timer flyTimer = new Timer();
    }
}
