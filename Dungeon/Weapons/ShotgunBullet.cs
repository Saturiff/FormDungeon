namespace DungeonGame
{
    public class ShotgunBullet : Projectile
    {
        public ShotgunBullet()
        {
            type = AmmunitionType.Mult;
            damage = 50;
            lifetime = 1000;
            speed = 4;
        }
    }
}
