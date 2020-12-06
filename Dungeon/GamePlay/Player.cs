﻿
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
            // ClientManager.RequestFire();


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
        public static readonly int attackRange = 50;
        public static readonly int pickRange = 50;
    }
}
