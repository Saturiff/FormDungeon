using System;
using System.Windows.Forms;

namespace DungeonGame
{
    /// <summary>
    /// 任何產生在Viewport中的物件之基底，不應由此類直接創建遊戲物件，而是新增子類繼承
    /// </summary>
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
