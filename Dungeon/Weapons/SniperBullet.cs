namespace DungeonGame.Weapons
{
    public class SniperBullet : Projectile
    {
        public SniperBullet()
        {
            type = AmmunitionType.Single;
            damage = 80;
            lifetime = 2000;
            speed = 7;
        }
    }
}
