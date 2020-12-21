using System;
using System.Drawing;
using System.Linq;
using System.Timers;

namespace DungeonGame.Weapons
{
    /// <summary>
    /// 投射物類，所有子彈之基類，提供初始化、運算子彈路徑之計時器與銷毀機制
    /// </summary>
    public class Projectile : Actor
    {
        public Projectile()
        {
            Size = new Size(8, 8);

            BackColor = Color.Red;

            renderTimer = new Timer(50);
            renderTimer.Elapsed += RenderTimer_Tick;
        }

        protected void RenderTimer_Tick(object sender, ElapsedEventArgs e)
        {
            if ((!Game.client.IsOnline)
                || (time >= lifetime / renderTimer.Interval)
                || (!Game.map.IsWalkable(new DungeonUtility.Rect(ActorRect.X, ActorRect.Y, ActorRect.Width, ActorRect.Height))))
            {
                Destory();
                return;
            }

            if (DetectHit())
                return;

            foreach (PlayerCharacter pc in Game.client.players.Values.ToList())
            {
                if (pc.Name == senderName)
                    continue;

                if (IsOverlapped(pc.ActorRect))
                {
                    Destory();
                    return;
                }
            }

            (double x, double y) = (begin.x + speed * time++ * Math.Cos(radians),
                                    begin.y + speed * time++ * Math.Sin(radians));

            if (this != null)
                Location = new Point((int)x, (int)y);
        }

        public virtual bool DetectHit()
        {
            if (IsOverlapped(Game.player.ActorRect) && (senderName != Game.player.Name))
            {
                Game.client.RequestHit(damage);
                Destory();
                return true;
            }

            return false;
        }

        public virtual void Start(string name, (int x, int y) begin, double radians)
        {
            Init(name, begin, radians);
            renderTimer.Start();
        }

        private void Init(string name, (int x, int y) begin, double radians)
        {
            senderName = name;
            this.begin = begin;
            this.radians = radians;

            Location = new Point(begin.x, begin.y);

            Game.SpawnInViewport(this);
        }

        public virtual void Destory()
        {
            Game.DestroyFromViewport(this);
            renderTimer.Stop();
        }

        private readonly Timer renderTimer;
        private int time = 1;

        protected string senderName;
        protected (int x, int y) begin;
        protected double radians = 0;
        protected bool canFriendlyFire = false;

        public AmmunitionType type;
        public int damage;
        public int lifetime; // 1/1000 s
        public int speed;
    }
}
