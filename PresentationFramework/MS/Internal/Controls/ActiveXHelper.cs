using System;
using System.Collections.Specialized;
using System.Runtime.InteropServices;
using MS.Win32;

namespace MS.Internal.Controls
{
	// Token: 0x02000250 RID: 592
	internal class ActiveXHelper
	{
		// Token: 0x060016DA RID: 5850 RVA: 0x000F7BD9 File Offset: 0x000F6BD9
		private ActiveXHelper()
		{
		}

		// Token: 0x060016DB RID: 5851 RVA: 0x0015C3E4 File Offset: 0x0015B3E4
		public static int Pix2HM(int pix, int logP)
		{
			return (2540 * pix + (logP >> 1)) / logP;
		}

		// Token: 0x060016DC RID: 5852 RVA: 0x0015C3F3 File Offset: 0x0015B3F3
		public static int HM2Pix(int hm, int logP)
		{
			return (logP * hm + 1270) / 2540;
		}

		// Token: 0x1700046C RID: 1132
		// (get) Token: 0x060016DD RID: 5853 RVA: 0x0015C404 File Offset: 0x0015B404
		public static int LogPixelsX
		{
			get
			{
				if (ActiveXHelper.logPixelsX == -1)
				{
					IntPtr dc = UnsafeNativeMethods.GetDC(NativeMethods.NullHandleRef);
					if (dc != IntPtr.Zero)
					{
						ActiveXHelper.logPixelsX = UnsafeNativeMethods.GetDeviceCaps(new HandleRef(null, dc), 88);
						UnsafeNativeMethods.ReleaseDC(NativeMethods.NullHandleRef, new HandleRef(null, dc));
					}
				}
				return ActiveXHelper.logPixelsX;
			}
		}

		// Token: 0x060016DE RID: 5854 RVA: 0x0015C45B File Offset: 0x0015B45B
		public static void ResetLogPixelsX()
		{
			ActiveXHelper.logPixelsX = -1;
		}

		// Token: 0x1700046D RID: 1133
		// (get) Token: 0x060016DF RID: 5855 RVA: 0x0015C464 File Offset: 0x0015B464
		public static int LogPixelsY
		{
			get
			{
				if (ActiveXHelper.logPixelsY == -1)
				{
					IntPtr dc = UnsafeNativeMethods.GetDC(NativeMethods.NullHandleRef);
					if (dc != IntPtr.Zero)
					{
						ActiveXHelper.logPixelsY = UnsafeNativeMethods.GetDeviceCaps(new HandleRef(null, dc), 90);
						UnsafeNativeMethods.ReleaseDC(NativeMethods.NullHandleRef, new HandleRef(null, dc));
					}
				}
				return ActiveXHelper.logPixelsY;
			}
		}

		// Token: 0x060016E0 RID: 5856 RVA: 0x0015C4BB File Offset: 0x0015B4BB
		public static void ResetLogPixelsY()
		{
			ActiveXHelper.logPixelsY = -1;
		}

		// Token: 0x060016E1 RID: 5857
		[DllImport("PresentationHost_cor3.dll", PreserveSig = false)]
		[return: MarshalAs(UnmanagedType.IDispatch)]
		internal static extern object CreateIDispatchSTAForwarder([MarshalAs(UnmanagedType.IDispatch)] object pDispatchDelegate);

		// Token: 0x04000C7D RID: 3197
		public static readonly int sinkAttached = BitVector32.CreateMask();

		// Token: 0x04000C7E RID: 3198
		public static readonly int inTransition = BitVector32.CreateMask(ActiveXHelper.sinkAttached);

		// Token: 0x04000C7F RID: 3199
		public static readonly int processingKeyUp = BitVector32.CreateMask(ActiveXHelper.inTransition);

		// Token: 0x04000C80 RID: 3200
		private static int logPixelsX = -1;

		// Token: 0x04000C81 RID: 3201
		private static int logPixelsY = -1;

		// Token: 0x04000C82 RID: 3202
		private const int HMperInch = 2540;

		// Token: 0x02000A0A RID: 2570
		public enum ActiveXState
		{
			// Token: 0x04004072 RID: 16498
			Passive,
			// Token: 0x04004073 RID: 16499
			Loaded,
			// Token: 0x04004074 RID: 16500
			Running,
			// Token: 0x04004075 RID: 16501
			InPlaceActive = 4,
			// Token: 0x04004076 RID: 16502
			UIActive = 8,
			// Token: 0x04004077 RID: 16503
			Open = 16
		}
	}
}
