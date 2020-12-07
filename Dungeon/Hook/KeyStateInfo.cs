using System.Windows.Forms;

namespace DungeonGame.Hook
{
    /// <summary>
    /// 鍵盤按鍵狀態結構
    /// </summary>
    public struct KeyStateInfo
    {
        public KeyStateInfo(Keys key, bool ispressed, bool istoggled)
        {
            MKey = key;
            MIsPressed = ispressed;
            MIsToggled = istoggled;
        }

        public static KeyStateInfo Default => new KeyStateInfo(Keys.None, false, false);
        public Keys Key => MKey;
        public bool IsPressed => MIsPressed;
        public bool IsToggled => MIsToggled;

        private Keys MKey { get; set; }
        private bool MIsPressed { get; set; }
        private bool MIsToggled { get; set; }
    }
}
