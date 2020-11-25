using DungeonGame.Hook;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DungeonGame
{
    public class KeyboardHook
    {
        public void Hook()
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                m_KbdHookProc = new HookProc(KeyboardHookProc);

                m_HookHandle = SetWindowsHookEx(WH_KEYBOARD_LL, m_KbdHookProc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }

            if (m_HookHandle == 0)
            {
                Console.WriteLine("呼叫 SetWindowsHookEx 失敗!");
                return;
            }

            Console.WriteLine("Hooked!");
        }

        public void Unhook()
        {
            bool ret = UnhookWindowsHookEx(m_HookHandle);
            if (ret == false)
            {
                Console.WriteLine("呼叫 UnhookWindowsHookEx 失敗!");
                return;
            }
            m_HookHandle = 0;

            Console.WriteLine("Unhooked!");
        }

        public static int KeyboardHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            // 當按鍵按下及鬆開時都會觸發此函式，這裡只處理鍵盤按下的情形。
            bool isPressed = (lParam.ToInt32() & 0x80000000) == 0;

            if (nCode < 0 || !isPressed)
            {
                return CallNextHookEx(m_HookHandle, nCode, wParam, lParam);
            }

            // 取得欲攔截之按鍵狀態
            KeyStateInfo keyW = KeyboardInfo.GetKeyState(Keys.W);
            KeyStateInfo keyA = KeyboardInfo.GetKeyState(Keys.A);
            KeyStateInfo keyS = KeyboardInfo.GetKeyState(Keys.S);
            KeyStateInfo keyD = KeyboardInfo.GetKeyState(Keys.D);

            if (keyW.IsPressed)
            {
                Debug.WriteLine("keyW Pressed!");
            }
            if (keyA.IsPressed)
            {
                Debug.WriteLine("keyA Pressed!");
            }
            if (keyS.IsPressed)
            {
                Debug.WriteLine("keyS Pressed!");
            }
            if (keyD.IsPressed)
            {
                Debug.WriteLine("keyD Pressed!");
            }

            return CallNextHookEx(m_HookHandle, nCode, wParam, lParam);
        }

        const int WH_KEYBOARD_LL = 13;

        public delegate int HookProc(int nCode, IntPtr wParam, IntPtr lParam);

        private static int m_HookHandle = 0; // Hook handle
        private HookProc m_KbdHookProc;      // 鍵盤掛鉤函式指標

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        // 設置掛鉤.
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);

        // 將之前設置的掛鉤移除。記得在應用程式結束前呼叫此函式.
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern bool UnhookWindowsHookEx(int idHook);

        // 呼叫下一個掛鉤處理常式（若不這麼做，會令其他掛鉤處理常式失效）.
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int CallNextHookEx(int idHook, int nCode, IntPtr wParam, IntPtr lParam);
    }
}
