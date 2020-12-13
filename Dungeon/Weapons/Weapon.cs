using DungeonGame.Weapons;
using System;

namespace DungeonGame
{
    /// <summary>
    /// 武器類，根據武器做相應射擊
    /// </summary>
    public class Weapon
    {
        public void Fire(string fromPlayer, string weaponNum, (int x, int y) startPoint, (int x, int y) endPoint)
        {
            double angle = CalcAngle(endPoint, startPoint);

            Type T = ItemData.weaponData[weaponNum].Bullet.GetType();
            switch (ItemData.weaponData[weaponNum].Bullet.type)
            {
                case AmmunitionType.Mult:
                    ((Projectile)Activator.CreateInstance(T)).Start(fromPlayer, startPoint, GetRadians(angle + 30.0));
                    ((Projectile)Activator.CreateInstance(T)).Start(fromPlayer, startPoint, GetRadians(angle - 30.0));
                    goto case AmmunitionType.Single;

                case AmmunitionType.Single:
                case AmmunitionType.Blast:
                    ((Projectile)Activator.CreateInstance(T)).Start(fromPlayer, startPoint, GetRadians(angle));
                    break;

                default:
                    break;
            }
        }

        public double CalcAngle((int x, int y) a, (int x, int y) b)
        {
            double h = a.y - b.y;
            double w = a.x - b.x;
            if (w == 0)
                return a.y >= b.y ? 90 : 270;
            else
            {
                double atan = Math.Atan(h / w);
                double _angle = atan * 180.0 / Math.PI;

                return _angle + ((atan > 0) ? (a.x > b.x) ? 0 : 180 : (a.x > b.x) ? 360 : 180);
            }
        }

        public double GetRadians(double angle)
            => Math.PI / 180 * angle;
    }
}
