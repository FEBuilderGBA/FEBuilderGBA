using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace FEBuilderGBA
{
    public class SappyPlaying
    {
        public delegate bool EnumChildWindowsDelegate(IntPtr hWnd, IntPtr lparam);

        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        private static extern long GetWindowLong(IntPtr hWnd, int nIndex);
        const int GWL_STYLE = (-16);
        const long DS_MODALFRAME = 0x80;


        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public extern static bool EnumChildWindows(IntPtr handle, EnumChildWindowsDelegate lpEnumFunc,
            IntPtr lparam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowText(IntPtr hWnd,
            StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool SetWindowText(IntPtr hWnd, string text);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetClassName(IntPtr hWnd,
            StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

        //see
        //https://stackoverflow.com/questions/14634120/sendmessage-wm-settext-to-textbox-doesnt-trigger-textchanged-event
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, string lParam);

        [DllImport("user32.dll")]
        public static extern Int32 PostMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }
        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr HWND, out RECT rect);

        [DllImport("USER32.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern void SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(ref  System.Drawing.Point lpPoint);


        [DllImport("USER32.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        [DllImport("user32.dll")]
        public static extern bool ClientToScreen(IntPtr hwnd, ref  System.Drawing.Point lpPoint);

        [DllImport("user32.dll")]
        public static extern int ShowWindow(IntPtr handle, int command);
        [DllImport("user32.dll")]
        public static extern int SetForegroundWindow(IntPtr handle);

        public class ChildWindow
        {
            public string titleName;
            public string captionName;
            public string className;
            public IntPtr windowHandle;
            public uint processId;
            public List<ChildWindow> childs = new List<ChildWindow>();
        };
        static uint SappyPid = 0;
        static List<ChildWindow> currentChilds; //C#で  EnumChildWindows(win ,xx, (IntPtr)list) したいんだけど、やり方がわからん.
        public static List<ChildWindow> EnumChildWindows(IntPtr hWnd)
        {
            List<ChildWindow> list = new List<ChildWindow>();
            currentChilds = list;

            EnumChildWindows(hWnd, new EnumChildWindowsDelegate(EnumWindowCallBack), (IntPtr)0);
            for (int i = 0; i < list.Count; i++)
            {
                list[i].childs = EnumChildWindows(list[i].windowHandle);
            }
            return list;
        }
        private static bool EnumWindowCallBack(IntPtr hWnd, IntPtr lparam)
        {
            if (hWnd == IntPtr.Zero)
            {
                return true;
            }

            uint pid;
            GetWindowThreadProcessId(hWnd, out pid);

            if (pid != SappyPid)
            {//Sappyのプロセスではないので無視.
                return true;
            }

            ChildWindow win = new ChildWindow();
            win.windowHandle = hWnd;
            win.processId = pid;

            //ウィンドウのタイトルの長さを取得する
            int textLen = GetWindowTextLength(hWnd);
            if (0 < textLen)
            {
                //ウィンドウのタイトルを取得する
                StringBuilder tsb = new StringBuilder(textLen + 1);
                GetWindowText(hWnd, tsb, tsb.Capacity);

                win.titleName = tsb.ToString();
            }
            else
            {
                win.titleName = "";
            }

            //ウィンドウのクラス名を取得する
            StringBuilder csb = new StringBuilder(256);
            GetClassName(hWnd, csb, csb.Capacity);

            win.className = csb.ToString();
            currentChilds.Add(win);

            //すべてのウィンドウを列挙する
            return true;
        }
        public static ChildWindow FindChildWindows(List<ChildWindow> list, Func<ChildWindow, bool> condCallback)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (condCallback(list[i]))
                {
                    return list[i];
                }
            }
            for (int i = 0; i < list.Count; i++)
            {
                ChildWindow found = FindChildWindows(list[i].childs, condCallback);
                if (found != null)
                {
                    return found;
                }
            }
            return null;
        }
        
        
        public void StartPlay(Process p, uint songid)
        {
            if (p == null || p.MainWindowHandle == null || p.HasExited)
            {
                return;
            }

            long gwlstyle = GetWindowLong(p.MainWindowHandle, GWL_STYLE);
            if ((gwlstyle & DS_MODALFRAME) == DS_MODALFRAME)
            {//何かsappy側でエラーがモーダル表示されている.
                return;
            }

            SappyPid = (uint)p.Id;

            //VB6のプログラムの MainWindowはダミーらしい？ 
            //別にWindowをもっているらしいので検出する.
            List<ChildWindow> systemAllWindows
                = EnumChildWindows(IntPtr.Zero);
            ChildWindow soungnumber = FindChildWindows(systemAllWindows, (ChildWindow win) =>
            {
                if (p.Id != win.processId)
                {
                    return false;
                }
                if (win.titleName == "1" && win.className == "ThunderRT6TextBox")
                {
                    return true;
                }
                if (win.childs.Count != 0)
                {
                    return false;
                }

                return false;
            });
            if (soungnumber == null)
            {
                return;
            }
            //0x000C = WM_SETTEXT
            SendMessage(soungnumber.windowHandle, 0x000C, IntPtr.Zero, songid.ToString() );


            ChildWindow stopbutton = FindChildWindows(systemAllWindows, (ChildWindow win) =>
            {
                if (p.Id != win.processId)
                {
                    return false;
                }
                if (win.childs.Count != 1)
                {
                    return false;
                }
                if (win.className != "ThunderRT6UserControlDC")
                {
                    return false;
                }

                RECT rc;
                GetWindowRect(win.windowHandle, out rc);

                int width = rc.right - rc.left;
                if (width != 33)
                {
                    return false;
                }

                //最初にあるのが停止ボタン
                return true;
            });
            if (stopbutton == null)
            {
                return;
            }

            int hitCount = 0;
            ChildWindow playbutton = FindChildWindows(systemAllWindows, (ChildWindow win) =>
            {
                if (p.Id != win.processId)
                {
                    return false;
                }
                if (win.childs.Count != 1)
                {
                    return false;
                }
                if (win.className != "ThunderRT6UserControlDC")
                {
                    return false;
                }

                RECT rc;
                GetWindowRect(win.windowHandle, out rc);

                int width = rc.right - rc.left;
                if (width != 33)
                {
                    return false;
                }

                hitCount++;
                if (hitCount >= 2)
                {//N個目に見つけたのを返す.
                    return true;
                }

                return false;
            });
            if (playbutton == null)
            {
                return;
            }
            ShowWindow(p.MainWindowHandle, 0);
            SetForegroundWindow(p.MainWindowHandle);

            System.Drawing.Point oript = new System.Drawing.Point();
            GetCursorPos(ref oript);

            //停止ボタンを押す(曲再生中だと再度再生を押してもだめな時がある)
            System.Drawing.Point pt = new System.Drawing.Point();
            ClientToScreen(stopbutton.windowHandle, ref pt);
            SetCursorPos(pt.X, pt.Y);
            mouse_event(0x0002, 0, 0, 0, 0); // MOUSEEVENTF_LEFTDOWN
            mouse_event(0x0004, 0, 0, 0, 0); // MOUSEEVENTF_LEFTUP

            //再生ボタンを押す
            pt = new System.Drawing.Point();
            ClientToScreen(playbutton.windowHandle, ref pt);
            SetCursorPos(pt.X, pt.Y);
            mouse_event(0x0002, 0, 0, 0, 0); // MOUSEEVENTF_LEFTDOWN
            mouse_event(0x0004, 0, 0, 0, 0); // MOUSEEVENTF_LEFTUP
            SetCursorPos(oript.X, oript.Y);
        }

    }
}
