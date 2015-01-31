using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoubanFM.Desktop.Interop
{
    public static class WM
	{
		#region DWM Messages
		public const int DWMCOMPOSITIONCHANGED = 0x031E;
		public const int DWMNCRENDERINGCHANGED = 0x031F;
		#endregion
	}
}
