using System;
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

            renderTimer = new Timer();
            renderTimer.Interval = 50;
            renderTimer.Tick += RenderTimer_Tick;
        }

        private void RenderTimer_Tick(object sender, EventArgs e)
        {
            if ((time >= lifetime / renderTimer.Interval)
                && (!Game.map.IsWalkable(new DungeonUtility.Rect(Rect.X, Rect.Y, Rect.Width, Rect.Height))))
                Destory();

            foreach (PlayerCharacter p in Game.client.players.Values)
            {
                if (p != Game.player && IsOverlapped(p.Rect))
                {
                    Game.client.Hit(p.Name, damage);
                    Destory();
                }
            }

            (double x, double y) = (begin.x + speed * time++ * Math.Cos(radians),
                                    begin.y + speed * time++ * Math.Sin(radians));

            Location = new Point((int)x, (int)y);
        }

        public void StartNormal(string name, (int x, int y) begin, (int x, int y) dest, double radians)
        {
            Init(name, begin, dest, radians);

            renderTimer.Start();
        }

        public void StartDot((int x, int y) begin, (int x, int y) dest, double radians) { }

        public void StartAuto((int x, int y) begin, (int x, int y) dest, double radians) { }

        public void StopAuto((int x, int y) begin, (int x, int y) dest, double radians) { }

        public void StartBlast((int x, int y) begin, (int x, int y) dest, double radians) { }

        private void Init(string name, (int x, int y) begin, (int x, int y) dest, double radians)
        {
            this.name = name;
            this.begin = begin;
            this.dest = dest;
            this.radians = radians;

            Location = new Point(begin.x, begin.y);

            Game.SpawnInViewport(this);
        }

        private void Destory()
        {
            Game.DestroyFromViewport(this);
            Dispose();
        }

        private string name;
        private (int x, int y) begin;
        private (int x, int y) dest;
        private int time = 1;
        private double radians = 0;
        private Timer renderTimer = new Timer();

        public AmmunitionType type;
        public int damage;
        public int lifetime; // 1/1000 s
        public int speed;
    }
}
