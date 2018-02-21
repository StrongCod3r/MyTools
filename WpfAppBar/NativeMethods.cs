﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfAppBar
{
    internal static class NativeMethods
    {
        private const string User32 = "user32.dll";

        public enum ABEdge
        {
            Left = 0,
            Top,
            Right,
            Bottom,
            None
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct APPBARDATA
        {
            public int cbSize;
            public IntPtr hWnd;
            public int uCallbackMessage;
            public int uEdge;
            public RECT rc;
            public IntPtr lParam;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;

            public RECT(int left, int top, int right, int bottom)
            {
                this.left = left;
                this.top = top;
                this.right = right;
                this.bottom = bottom;
            }

            public int Width
            {
                get { return right - left; }
            }

            public int Height
            {
                get { return bottom - top; }
            }

            public void Offset(int dx, int dy)
            {
                left += dx;
                top += dy;
                right += dx;
                bottom += dy;
            }

            public bool IsEmpty
            {
                get
                {
                    return left >= right || top >= bottom;
                }
            }

            public static explicit operator Int32Rect(RECT r)
            {
                return new Int32Rect(r.left, r.top, r.Width, r.Height);
            }

            public static explicit operator Rect(RECT r)
            {
                return new Rect(r.left, r.top, r.Width, r.Height);
            }

            public static explicit operator RECT(Rect r)
            {
                return new RECT((int)r.Left, (int)r.Top, (int)r.Right, (int)r.Bottom);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct WINDOWPOS
        {
            public IntPtr hwnd;
            public IntPtr hwndInsertAfter;
            public int x;
            public int y;
            public int cx;
            public int cy;
            public int flags;

            public Rect Bounds
            {
                get { return new Rect(x, y, cx, cy); }
                set
                {
                    x = (int)value.X;
                    y = (int)value.Y;
                    cx = (int)value.Width;
                    cy = (int)value.Height;
                }
            }
        }

        public const int
            SWP_NOMOVE = 0x0002,
            SWP_NOSIZE = 0x0001;

        public const int
            WM_ACTIVATE = 0x0006,
            WM_WINDOWPOSCHANGED = 0x0047,
            WM_SYSCOMMAND = 0x0112,
            WM_WINDOWPOSCHANGING = 0x0046;

        public const int
            SC_MOVE = 0xF010;

        public static int SC_FROM_WPARAM(IntPtr wparam)
        {
            // In WM_SYSCOMMAND messages, the four low-order bits of the wParam parameter are used internally by
            // the system. To obtain the correct result when testing the value of wParam, an application must
            // combine the value 0xFFF0 with the wParam value by using the bitwise AND operator.
            return ((int)wparam & 0xfff0);
        }

        public enum ABM
        {
            NEW = 0,
            REMOVE,
            QUERYPOS,
            SETPOS,
            GETSTATE,
            GETTASKBARPOS,
            ACTIVATE,
            GETAUTOHIDEBAR,
            SETAUTOHIDEBAR,
            WINDOWPOSCHANGED,
            SETSTATE
        }

        public enum ABN
        {
            STATECHANGE = 0,
            POSCHANGED,
            FULLSCREENAPP,
            WINDOWARRANGE
        }

        [DllImport("shell32.dll", ExactSpelling = true)]
        public static extern uint SHAppBarMessage(ABM dwMessage, ref APPBARDATA pData);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern int RegisterWindowMessage(string msg);

        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint flags);

        [Flags]
        public enum MONITORINFOF
        {
            PRIMARY = 0x1
        }

        private const int CCHDEVICENAME = 32;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct MONITORINFOEX
        {
            public int cbSize;
            public RECT rcMonitor;
            public RECT rcWork;
            public MONITORINFOF dwFlags;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCHDEVICENAME)]
            public string szDevice;
        }

        public delegate bool MonitorEnumDelegate(IntPtr hMonitor, IntPtr hdcMonitor, ref RECT lprcMonitor, IntPtr dwData);

        [DllImport(User32, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool GetMonitorInfo(IntPtr hMonitor, ref MONITORINFOEX lpmi);

        [DllImport(User32)]
        public static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip, MonitorEnumDelegate lpfnEnum, IntPtr dwData);
    }
}
