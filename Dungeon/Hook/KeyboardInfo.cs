using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DungeonGame.Hook
{
    /// <summary>
    /// 鍵盤資訊，取得基礎按鍵狀態，回傳鍵盤按鍵狀態結構
    /// </summary>
    public class KeyboardInfo
    {
        public static KeyStateInfo GetKeyState(Keys key)
        {
            int vkey = (key == Keys.Alt) ? 0x12 : (int)key; // VK_ALT : Keys

            short keyState = GetKeyState(vkey);
            int low = Low(keyState);
            int high = High(keyState);
            bool toggled = (low == 1);
            bool pressed = (high == 1);

            return new KeyStateInfo(key, pressed, toggled);
        }

        private static int High(int keyState)
        {
            if (keyState > 0)
            {
                return keyState >> 0x10;
            }
            else
            {
                return (keyState >> 0x10) & 0x1;
            }
        }

        private static int Low(int keyState) => keyState & 0xffff;

        #region DLL
        [DllImport("user32")]
        private static extern short GetKeyState(int vKey);
        #endregion
    }
}
