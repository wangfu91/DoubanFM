using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoubanFM.Desktop.Interop
{
	// hook method called by system
	public delegate IntPtr HOOKPROC(int code, IntPtr wParam, ref KBDLLHOOKSTRUCT lParam);
}
