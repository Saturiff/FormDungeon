namespace DungeonGame.Weapons
{
    public class RifleBullet : Projectile
    {
        public RifleBullet()
        {
            type = AmmunitionType.Single;
            damage = 25;
            lifetime = 1500;
            speed = 6;
        }
    }
}
