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
            m_Key = key;
            m_IsPressed = ispressed;
            m_IsToggled = istoggled;
        }

        public static KeyStateInfo Default => new KeyStateInfo(Keys.None, false, false);
        public Keys Key => m_Key;
        public bool IsPressed => m_IsPressed;
        public bool IsToggled => m_IsToggled;

        private Keys m_Key { get; set; }
        private bool m_IsPressed { get; set; }
        private bool m_IsToggled { get; set; }
    }
}
