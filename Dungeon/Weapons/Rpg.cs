using System.Threading;
using System.Threading.Tasks;

namespace DungeonGame.Weapons
{
    public class Rpg : Projectile
    {
        public Rpg()
        {
            type = AmmunitionType.Blast;
            damage = 50;
            lifetime = 3000;
            speed = 10;
        }

        public override void Start(string name, (int x, int y) begin, double radians)
        {
            Game.client.RequestClearItem();
            Game.player.itemNum = "0";
            Game.s_Slot.RemoveItem();

            base.Start(name, begin, radians);
        }

        public override void Destory()
        {
            Hide();

            Thread th = new Thread(Exp);
            th.Start();
            
            base.Destory();
        }

        public void Exp()
        {
            Explode explode = new Explode();
            explode.Location = Location;
            explode.Size = new System.Drawing.Size(50, 50);

            Game.SpawnInViewport(explode);

            explode.Start(senderName, (Location.X - explode.Size.Width / 2, Location.Y - explode.Size.Height / 2), radians);
        }
    }
}
