using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ComStringGuidMarshallingDemo.Common
{
    [ComVisible(true)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IComTestTarget
    {
        InterestingRecord[] GetInterestingRecords();
    }

    [ComVisible(true)]
    public struct InterestingRecord
    {
        public Guid guid;

        [MarshalAs(UnmanagedType.BStr)]
        public string ChatText;
    }
}
