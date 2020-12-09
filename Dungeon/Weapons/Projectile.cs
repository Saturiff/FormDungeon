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
            renderTimer.Interval = 10;
            renderTimer.Tick += RenderTimer_Tick;
        }

        private void RenderTimer_Tick(object sender, EventArgs e)
        {
            // if (interTime >= 1)
            // {
            //     // interTime -= 1;
            //     Game.DestroyFromViewport(this);
            //     Dispose();
            // }

            (double x, double y) = (begin.x + speed * time++ * Math.Cos(angle), 
                                    begin.y + speed * time++ * Math.Sin(angle));

            Location = new Point((int)x, (int)y);
        }
        int time = 1;
        double angle = 0;
        private void LifetimeTimer_Tick(object sender, System.EventArgs e)
        {
            // Game.DestroyFromViewport(this);
            // Dispose();
        }

        public double CalcAngle((int x, int y) a, (int x, int y) b) // ?
        {
            double h = a.y - b.y;
            double w = a.x - b.x;
            if (w == 0)
                return a.y >= b.y ? 90 : 270;
            else
            {
                double atan = Math.Atan(h / w);
                double _angle = atan * 180.0 / Math.PI;

                if (atan > 0)
                {
                    if (a.x > b.x)
                        return _angle;
                    else
                        return _angle + 180;
                }
                else
                {
                    if (a.x > b.x)
                        return _angle + 360;
                    else

                        return _angle + 180;
                }
            }
        }

        public void Start()
        {
            angle = Math.PI / 180 * CalcAngle(dest, begin);
            renderTimer.Start();
            
            // while(true) // not arrived || not overlapped
            // for (int i = 0; i < 100; i++)
            // {
            // 
            //     await Task.Delay(11 - speed);
            // }
        }

        // lifetimeTimer.Interval = lifetime;
        // lifetimeTimer.Tick += LifetimeTimer_Tick;
        // lifetimeTimer.Start();
        // for (int i = 0; i< 100; i++)
        // {
        //     Top++;
        //     await Task.Delay(10);
        // }

        public AmmunitionType type;
        public int damage;
        public int lifetime; // 1/1000 s
        public int speed;
        public (int x, int y) dest;
        public (int x, int y) begin;
        public float interTime = 0;
        public Timer lifetimeTimer = new Timer();
        public Timer renderTimer = new Timer();
    }
}
