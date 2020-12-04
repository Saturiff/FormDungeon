using System.Windows.Forms;

namespace DungeonGame
{
    /// <summary>
    /// 可互動控件的基底類
    /// </summary>
    public class Actor : Panel, IInteractable
    {
        public void Interact() { }
    }
}
