using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoubanFM.Desktop.Interop
{
    public struct KBDLLHOOKSTRUCT
	{
		public UInt32 vkCode;
		public UInt32 scanCode;
		public UInt32 flags;
		public UInt32 time;
		public IntPtr extraInfo;
	}
}
