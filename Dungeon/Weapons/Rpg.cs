namespace DungeonGame.Weapons
{
    public class Rpg : Projectile
    {
        public Rpg()
        {
            type = AmmunitionType.Blast;
            damage = 120;
            lifetime = 3000;
            speed = 10;
        }
    }
}
