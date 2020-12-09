using System;
using System.Drawing;
using System.Windows.Forms;

namespace DungeonGame
{
    /// <summary>
    /// 玩家角色類
    /// </summary>
    public class PlayerCharacter : CharacterBase
    {
        public PlayerCharacter(string dataPack) : base(dataPack) { }

        public void InitTick()
        {
            movementTick = new Timer();
            movementTick.Interval = 1;
            movementTick.Tick += MovementTick_Tick;
            movementTick.Start();
        }

        private void MovementTick_Tick(object sender, EventArgs e)
        {
            CalcMove();
        }

        public void AttackTo(Point loc)
        {
            if (item != null || item.Name != "000")
            {
                Projectile p = new Projectile();
                p = new RifleBullet();
                p.begin = (Location.X, Location.Y);
                p.dest = (loc.X, loc.Y);
                Game.SpawnInViewport(p);
                p.Start();
            }
        }

        // bind to tick
        public void CalcMove()
        {
            if (isMovingUp || isMovingDown || isMovingLeft || isMovingRight)
            {
                int mult = 1;
                int up = ((isMovingUp ? -1 : 0) + (isMovingDown ? 1 : 0)) * mult;
                int right = ((isMovingRight ? 1 : 0) + (isMovingLeft ? -1 : 0)) * mult;

                MoveTo(new Point(Location.X + right, Location.Y + up));
            }
        }

        private Timer movementTick;

        public bool isMovingUp;
        public bool isMovingDown;
        public bool isMovingLeft;
        public bool isMovingRight;
        public static readonly int pickRange = 50;
    }
}
