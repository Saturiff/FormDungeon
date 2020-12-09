namespace DungeonGame
{
    public class HmgBullet : Projectile
    {
        public HmgBullet()
        {
            type = AmmunitionType.Auto;
            damage = 20;
            lifetime = 1500;
            speed = 6;
        }
    }
}
