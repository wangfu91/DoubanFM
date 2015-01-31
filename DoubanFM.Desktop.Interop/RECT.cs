using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace DoubanFM.Desktop.Interop
{
	[StructLayout(LayoutKind.Sequential)]
    public struct RECT
	{
		public int left, top, right, bottom;

		public RECT(int left, int top, int right, int bottom)
		{
			this.left = left;
			this.top = top;
			this.right = right;
			this.bottom = bottom;
		}
	}
}
