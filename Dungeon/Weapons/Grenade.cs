namespace DungeonGame
{
    public class Grenade : Projectile
    {
        public Grenade()
        {
            type = AmmunitionType.Blast;
            damage = 100;
            lifetime = 3000;
            speed = 3;
        }
    }
}
