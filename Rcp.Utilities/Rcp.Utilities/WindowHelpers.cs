// Copyright (c) 2020 Jeremy Oursler All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Rcp.Utilities
{
    public class WindowHelpers
    {
        private static class User32
        {
            [DllImport("User32.dll")]
            internal static extern IntPtr SetForegroundWindow(IntPtr hWnd);

            [DllImport("user32.dll")]
            internal static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

            internal static readonly IntPtr InvalidHandleValue = IntPtr.Zero;
            internal const int SW_SHOWNORMAL=1;
        }
        public static void Activate()
        {
            Process currentProcess = Process.GetCurrentProcess();
            IntPtr hWnd = currentProcess.MainWindowHandle;
            if (hWnd != User32.InvalidHandleValue)
            {
                User32.SetForegroundWindow(hWnd);
                User32.ShowWindow(hWnd, User32.SW_SHOWNORMAL);
            }
        }
    }
}
