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
            if (time >= lifetime / renderTimer.Interval)
            {
                Game.DestroyFromViewport(this);
                Dispose();
            }

            (double x, double y) = (begin.x + speed * time++ * Math.Cos(radians),
                                    begin.y + speed * time++ * Math.Sin(radians));

            Location = new Point((int)x, (int)y);
        }

        public void StartNormal((int x, int y) begin, (int x, int y) dest, double radians)
        {
            Init(begin, dest, radians);

            renderTimer.Start();
        }

        private void Init((int x, int y) begin, (int x, int y) dest, double radians)
        {
            this.begin = begin;
            this.dest = dest;
            this.radians = radians;

            Game.SpawnInViewport(this);

        }

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
