using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoubanFM.Desktop.Interop
{
	public static class HWND
	{
		public static readonly IntPtr NOTOPMOST = new IntPtr(-2);
		public static readonly IntPtr TOPMOST = new IntPtr(-1);
		public static readonly IntPtr TOP = new IntPtr(0);
		public static readonly IntPtr BOTTOM = new IntPtr(1);
	}
}