namespace DungeonGame.Weapons
{
    public class Explode : Projectile
    {
        public Explode()
        {
            type = AmmunitionType.Blast;
            damage = 150;
            lifetime = 1000;
            speed = 0;
            canFriendlyFire = true;
        }

        public override bool DetectHit()
        {
            if (IsOverlapped(Game.player.ActorRect))
            {
                Game.client.RequestHit(damage);
                Destory();
                return true;
            }

            return false;
        }
    }
}
