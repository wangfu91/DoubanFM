using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace DoubanFM.Desktop.Infrastructure.Extension
{
	public static class SecureStringExtension
	{
		public static string ConvertToUnsecureString(this SecureString securePassword)
		{
			if (securePassword == null)
				throw new ArgumentNullException(nameof(securePassword));

			IntPtr unmanagedString = IntPtr.Zero;
			try
			{
				unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(securePassword);
				return Marshal.PtrToStringUni(unmanagedString);
			}
			finally
			{
				Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
			}
		}

		public static SecureString ConvertToSecureString(this string password)
		{
			if (password == null)
				throw new ArgumentNullException(nameof(password));

			unsafe
			{
				fixed (char* passwordChars = password)
				{
					var securePassword = new SecureString(passwordChars, password.Length);
					securePassword.MakeReadOnly();
					return securePassword;
				}
			}
		}

	}
}
