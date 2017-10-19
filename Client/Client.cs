using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    using ComStringGuidMarshallingDemo.Common;
    using System.Runtime.InteropServices;
    using System.Runtime.InteropServices.ComTypes;
    using static NativeMethods;

    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var rot = GetRunningObjectTable(0);
            var moniker = CreateItemMoniker("!", "TestMoniker");

            object utobj;
            if (rot.GetObject(moniker, out utobj) != 0 /* S_OK */)
            {
                throw new InvalidOperationException("Moniker not in table");
            }

            var target = (IComTestTarget)utobj;

            // The parameter is incorrect. (Exception from HRESULT: 0x80070057 (E_INVALIDARG))
            // from GetRecordInfoFromTypeInfo
            // at OleVariant::CreateSafeArrayDescriptorForArrayRef
            // at System.StubHelpers.MngdSafeArrayMarshaler.ConvertSpaceToNative
            var results = target.GetInterestingRecords();
            Console.WriteLine(results);
        }
    }

    static class NativeMethods
    {
        public const int ROTFLAGS_REGISTRATIONKEEPSALIVE = 1;
        public const int ROTFLAGS_ALLOWANYCLIENT = 2;

        [DllImport("ole32.dll", ExactSpelling = true, PreserveSig = false)]
        public static extern IRunningObjectTable GetRunningObjectTable(int reserved);

        [DllImport("ole32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = false)]
        public static extern IMoniker CreateItemMoniker([In] string lpszDelim, [In] string lpszItem);
    }
}
