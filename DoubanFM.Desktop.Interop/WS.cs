using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoubanFM.Desktop.Interop
{
    public static class WS
	{
		public const int OVERLAPPED = 0x00000000;
		public const int POPUP = (int)-0x80000000;
		public const int CHILD = 0x40000000;
		public const int MINIMIZE = 0x20000000;
		public const int VISIBLE = 0x10000000;
		public const int DISABLED = 0x08000000;
		public const int CLIPSIBLINGS = 0x04000000;
		public const int CLIPCHILDREN = 0x02000000;
		public const int MAXIMIZE = 0x01000000;
		public const int CAPTION = BORDER | DLGFRAME;
		public const int BORDER = 0x00800000;
		public const int DLGFRAME = 0x00400000;
		public const int VSCROLL = 0x00200000;
		public const int HSCROLL = 0x00100000;
		public const int SYSMENU = 0x00080000;
		public const int THICKFRAME = 0x00040000;
		public const int GROUP = 0x00020000;
		public const int TABSTOP = 0x00010000;

		public const int MINIMIZEBOX = 0x00020000;
		public const int MAXIMIZEBOX = 0x00010000;

		public const int TILED = OVERLAPPED;
		public const int ICONIC = MINIMIZE;
		public const int SIZEBOX = THICKFRAME;
		public const int TILEDWINDOW = OVERLAPPEDWINDOW;

		// Common Window Styles
		public const int OVERLAPPEDWINDOW = OVERLAPPED | CAPTION | SYSMENU | THICKFRAME | MINIMIZEBOX | MAXIMIZEBOX;
		public const int POPUPWINDOW = POPUP | BORDER | SYSMENU;
		public const int CHILDWINDOW = CHILD;

		#region WS_EX
		public static class EX
		{
			public const int DLGMODALFRAME = 0x00000001;
			public const int NOPARENTNOTIFY = 0x00000004;
			public const int TOPMOST = 0x00000008;
			public const int ACCEPTFILES = 0x00000010;
			public const int TRANSPARENT = 0x00000020;

			//#if(WINVER >= 0x0400)
			public const int MDICHILD = 0x00000040;
			public const int TOOLWINDOW = 0x00000080;
			public const int WINDOWEDGE = 0x00000100;
			public const int CLIENTEDGE = 0x00000200;
			public const int CONTEXTHELP = 0x00000400;

			public const int RIGHT = 0x00001000;
			public const int LEFT = 0x00000000;
			public const int RTLREADING = 0x00002000;
			public const int LTRREADING = 0x00000000;
			public const int LEFTSCROLLBAR = 0x00004000;
			public const int RIGHTSCROLLBAR = 0x00000000;

			public const int CONTROLPARENT = 0x00010000;
			public const int STATICEDGE = 0x00020000;
			public const int APPWINDOW = 0x00040000;

			public const int OVERLAPPEDWINDOW = (WINDOWEDGE | CLIENTEDGE);
			public const int PALETTEWINDOW = (WINDOWEDGE | TOOLWINDOW | TOPMOST);
			//#endif /* WINVER >= 0x0400 */

			//#if(_WIN32_WINNT >= 0x0500)
			public const int LAYERED = 0x00080000;
			//#endif /* _WIN32_WINNT >= 0x0500 */

			//#if(WINVER >= 0x0500)
			public const int NOINHERITLAYOUT = 0x00100000; // Disable inheritence of mirroring by children
			public const int LAYOUTRTL = 0x00400000; // Right to left mirroring
			//#endif /* WINVER >= 0x0500 */

			//#if(_WIN32_WINNT >= 0x0500)
			public const int COMPOSITED = 0x02000000;
			public const int NOACTIVATE = 0x08000000;
			//#endif /* _WIN32_WINNT >= 0x0500 */
		}
		#endregion
	}
}