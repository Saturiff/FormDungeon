using System;
using System.Drawing;
using System.Linq;
using System.Timers;

namespace DungeonGame.Weapons
{
    public class Projectile : Actor
    {
        public Projectile()
        {
            Size = new Size(8, 8);

            BackColor = Color.Red;

            renderTimer = new Timer(50);
            renderTimer.Elapsed += RenderTimer_Tick;
        }

        private void RenderTimer_Tick(object sender, ElapsedEventArgs e)
        {
            if (!Game.client.IsOnline)
            {
                Destory();
                return;
            }

            if ((time >= lifetime / renderTimer.Interval)
                && (!Game.map.IsWalkable(new DungeonUtility.Rect(ActorRect.X, ActorRect.Y, ActorRect.Width, ActorRect.Height))))
            {
                Destory();
                return;
            }

            foreach (PlayerCharacter hittedPlayer in Game.client.players.Values.ToList())
            {
                if (hittedPlayer.Name != name && IsOverlapped(hittedPlayer.ActorRect))
                {
                    Game.client.RequestHit(hittedPlayer.Name, damage);
                    Destory();
                    return;
                }
            }

            (double x, double y) = (begin.x + speed * time++ * Math.Cos(radians),
                                    begin.y + speed * time++ * Math.Sin(radians));

            if (this != null)
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
            renderTimer.Stop();
            Dispose();
        }

        private string name;
        private (int x, int y) begin;
        private (int x, int y) dest;
        private int time = 1;
        private double radians = 0;
        private Timer renderTimer;

        public AmmunitionType type;
        public int damage;
        public int lifetime; // 1/1000 s
        public int speed;
    }
}
