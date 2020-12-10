namespace DungeonGame.Weapons
{
    public class RifleBullet : Projectile
    {
        public RifleBullet()
        {
            type = AmmunitionType.Auto;
            damage = 25;
            lifetime = 1500;
            speed = 4;
        }
    }
}
