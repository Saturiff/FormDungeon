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
            movementTick.Interval = 10;
            movementTick.Tick += MovementTick_Tick;
            movementTick.Start();
        }

        private void MovementTick_Tick(object sender, EventArgs e)
        {
            if (CurrentHealth > 0)
                CalcMove();
        }

        public void AttackTo(Point loc)
        {
            if ((CurrentHealth > 0) && (itemNum != null) && (itemNum != "000"))
            {
                Game.client.RequestFireSingle(Name, itemNum, (Location.X, Location.Y), (loc.X, loc.Y));
            }
        }

        public void CalcMove()
        {
            if (isMovingUp || isMovingDown || isMovingLeft || isMovingRight)
            {
                int mult = 2;
                int up = ((isMovingUp ? -1 : 0) + (isMovingDown ? 1 : 0)) * mult;
                int right = ((isMovingRight ? 1 : 0) + (isMovingLeft ? -1 : 0)) * mult;

                MoveTo(new Point(Location.X + right, Location.Y + up));
            }
        }

        public override bool Equals(object obj)
        {
            return (obj is PlayerCharacter ch) && (ch.Name == Name);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(PlayerCharacter a, PlayerCharacter b)
        {
            return a.Name == b.Name;
        }

        public static bool operator !=(PlayerCharacter a, PlayerCharacter b)
        {
            return a.Name != b.Name;
        }

        private Timer movementTick;

        public bool isMovingUp;
        public bool isMovingDown;
        public bool isMovingLeft;
        public bool isMovingRight;
        public static readonly int pickRange = 50;
    }
}
