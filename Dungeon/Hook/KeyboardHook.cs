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
                kbHookProc = new HookProc(KeyboardHookProc);

                hookHandle = SetWindowsHookEx(WH_KEYBOARD_LL, kbHookProc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }

            if (hookHandle == 0)
            {
                Console.WriteLine("SetWindowsHookEx Fail.");
                return;
            }
        }

        public void Unhook()
        {
            bool ret = UnhookWindowsHookEx(hookHandle);
            if (ret == false)
            {
                Console.WriteLine("UnhookWindowsHookEx Fail.");
                return;
            }
            hookHandle = 0;
        }

        private static int KeyboardHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (Game.isInViewport)
            {
                bool isPressed = (lParam.ToInt32() & 0x80000000) == 0;

                if (nCode < 0 || !isPressed)
                    return CallNextHookEx(hookHandle, nCode, wParam, lParam);

                KeyStateInfo keyW = KeyboardInfo.GetKeyState(Keys.W);
                KeyStateInfo keyA = KeyboardInfo.GetKeyState(Keys.A);
                KeyStateInfo keyS = KeyboardInfo.GetKeyState(Keys.S);
                KeyStateInfo keyD = KeyboardInfo.GetKeyState(Keys.D);

                Game.player.isMovingUp = keyW.IsPressed ? true : false;
                Game.player.isMovingDown = keyS.IsPressed ? true : false;
                Game.player.isMovingLeft = keyA.IsPressed ? true : false;
                Game.player.isMovingRight = keyD.IsPressed ? true : false;

                Game.player.CalcMove();
            }

            return CallNextHookEx(hookHandle, nCode, wParam, lParam);
        }

        public delegate int HookProc(int nCode, IntPtr wParam, IntPtr lParam);

        private HookProc kbHookProc;
        private const int WH_KEYBOARD_LL = 13;
        private static int hookHandle = 0;

        #region DLL
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern bool UnhookWindowsHookEx(int idHook);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int CallNextHookEx(int idHook, int nCode, IntPtr wParam, IntPtr lParam);
        #endregion
    }
}
