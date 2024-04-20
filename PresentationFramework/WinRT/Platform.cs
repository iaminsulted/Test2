using System;
using System.Runtime.InteropServices;

namespace WinRT
{
	// Token: 0x02000097 RID: 151
	internal class Platform
	{
		// Token: 0x0600021E RID: 542
		[DllImport("api-ms-win-core-com-l1-1-0.dll")]
		public static extern int CoDecrementMTAUsage(IntPtr cookie);

		// Token: 0x0600021F RID: 543
		[DllImport("api-ms-win-core-com-l1-1-0.dll")]
		public unsafe static extern int CoIncrementMTAUsage(IntPtr* cookie);

		// Token: 0x06000220 RID: 544
		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool FreeLibrary(IntPtr moduleHandle);

		// Token: 0x06000221 RID: 545
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern IntPtr GetProcAddress(IntPtr moduleHandle, [MarshalAs(UnmanagedType.LPStr)] string functionName);

		// Token: 0x06000222 RID: 546 RVA: 0x000F94B4 File Offset: 0x000F84B4
		public static T GetProcAddress<T>(IntPtr moduleHandle)
		{
			IntPtr procAddress = Platform.GetProcAddress(moduleHandle, typeof(T).Name);
			if (procAddress == IntPtr.Zero)
			{
				Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
			}
			return Marshal.GetDelegateForFunctionPointer<T>(procAddress);
		}

		// Token: 0x06000223 RID: 547
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern IntPtr LoadLibraryExW([MarshalAs(UnmanagedType.LPWStr)] string fileName, IntPtr fileHandle, uint flags);

		// Token: 0x06000224 RID: 548
		[DllImport("api-ms-win-core-winrt-l1-1-0.dll")]
		public unsafe static extern int RoGetActivationFactory(IntPtr runtimeClassId, ref Guid iid, IntPtr* factory);

		// Token: 0x06000225 RID: 549
		[DllImport("api-ms-win-core-winrt-string-l1-1-0.dll", CallingConvention = 3)]
		public unsafe static extern int WindowsCreateString([MarshalAs(UnmanagedType.LPWStr)] string sourceString, int length, IntPtr* hstring);

		// Token: 0x06000226 RID: 550
		[DllImport("api-ms-win-core-winrt-string-l1-1-0.dll", CallingConvention = 3)]
		public unsafe static extern int WindowsCreateStringReference(char* sourceString, int length, IntPtr* hstring_header, IntPtr* hstring);

		// Token: 0x06000227 RID: 551
		[DllImport("api-ms-win-core-winrt-string-l1-1-0.dll", CallingConvention = 3)]
		public static extern int WindowsDeleteString(IntPtr hstring);

		// Token: 0x06000228 RID: 552
		[DllImport("api-ms-win-core-winrt-string-l1-1-0.dll", CallingConvention = 3)]
		public unsafe static extern int WindowsDuplicateString(IntPtr sourceString, IntPtr* hstring);

		// Token: 0x06000229 RID: 553
		[DllImport("api-ms-win-core-winrt-string-l1-1-0.dll", CallingConvention = 3)]
		public unsafe static extern char* WindowsGetStringRawBuffer(IntPtr hstring, uint* length);
	}
}
