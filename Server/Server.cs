using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using ComStringGuidMarshallingDemo.Common;

namespace Server
{
    using static NativeMethods;

    class Program
    {
        class ComTestTarget : StandardOleMarshalObject, IComTestTarget
        {
            public InterestingRecord[] GetInterestingRecords()
            {
                return new InterestingRecord[0];
            }
        }

        [STAThread]
        static void Main(string[] args)
        {
            var rot = GetRunningObjectTable(0);
            IMoniker moniker = CreateItemMoniker("!", "TestMoniker");

            var ct = new ComTestTarget();
            var hRotEntry = rot.Register(ROTFLAGS_REGISTRATIONKEEPSALIVE, (IComTestTarget)ct, moniker);

            MSG msg;
            while (GetMessage(out msg, IntPtr.Zero, 0, 0))
            {
                TranslateMessage(ref msg);
                DispatchMessage(ref msg);
            }

            rot.Revoke(hRotEntry);
            GC.KeepAlive(ct);
        }
    }

    static class NativeMethods
    {
        public const int ROTFLAGS_REGISTRATIONKEEPSALIVE = 1;

        [DllImport("ole32.dll", ExactSpelling = true, PreserveSig = false)]
        public static extern IRunningObjectTable GetRunningObjectTable(int reserved);

        [DllImport("ole32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = false)]
        public static extern IMoniker CreateItemMoniker([In] string lpszDelim, [In] string lpszItem);

        [DllImport("user32.dll")]
        internal static extern bool GetMessage(out MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax);

        [DllImport("user32.dll")]
        internal static extern bool TranslateMessage([In] ref MSG lpMsg);

        [DllImport("user32.dll")]
        internal static extern IntPtr DispatchMessage([In] ref MSG lpMsg);
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct MSG
    {
        public IntPtr hWnd;
        public uint message;
        public IntPtr wParam;
        public IntPtr lParam;
        public uint time;
        public POINT pt;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct POINT
    {
        public int X;
        public int Y;
    }
}
