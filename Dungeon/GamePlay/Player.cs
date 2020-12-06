using System;
using System.Drawing;

namespace DungeonGame
{
    /// <summary>
    /// 玩家角色
    /// </summary>
    public class Player : CharacterBase
    {
        public Player() : base() { }

        public Player(string dataPack) : base(dataPack) { }

        public void Attack(CharacterBase target)
        {
            double dist = DistanceOf(target);
            if (dist > attackRange)
                return;

            int damage = 10 + (int)(atk * (attackRange - dist)) / 100;
            Console.WriteLine("damage = " + damage);
            // todo
            // ClientManager.Hit(target.name, damage);
        }

        public void CalcMove()
        {
            int mult = 15;
            int up = ((isMovingUp ? -1 : 0) + (isMovingDown ? 1 : 0)) * mult;
            int right = ((isMovingRight ? 1 : 0) + (isMovingLeft ? -1 : 0)) * mult;

            MoveTo(new Point(Location.X + right, Location.Y + up));
        }

        public bool isMovingUp;
        public bool isMovingDown;
        public bool isMovingLeft;
        public bool isMovingRight;
        public string status
        {
            get
            {
                return "Name:\t" + name
                + Environment.NewLine + "Health:\t" + currentHealth + " / " + maxHealth
                + Environment.NewLine + "Atk:\t" + atk
                + Environment.NewLine + "Def:\t" + def;
            }
        }
    }
}
