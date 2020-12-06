using System;
using System.Windows.Forms;

namespace DungeonGame
{
    public class Actor : Panel, IInteractable
    {
        public void Interact() { }

        public double DistanceOf(Actor target)
        {
            return Math.Sqrt((Location.X - target.Location.X) * (Location.X - target.Location.X) +
                (Location.Y - target.Location.Y) * (Location.Y - target.Location.Y));
        }
    }
}
