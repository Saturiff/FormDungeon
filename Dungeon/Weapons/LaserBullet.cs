namespace DungeonGame.Weapons
{
    public class Laser : Projectile
    {
        public Laser()
        {
            type = AmmunitionType.Dot;
            damage = 5;
            lifetime = 0;
            speed = 10;
        }
    }
}
