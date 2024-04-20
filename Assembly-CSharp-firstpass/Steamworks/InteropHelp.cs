using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace Steamworks
{
	// Token: 0x020001B3 RID: 435
	public class InteropHelp
	{
		// Token: 0x06000D34 RID: 3380 RVA: 0x0002A09F File Offset: 0x0002829F
		public static void TestIfPlatformSupported()
		{
		}

		// Token: 0x06000D35 RID: 3381 RVA: 0x0002A0A1 File Offset: 0x000282A1
		public static void TestIfAvailableClient()
		{
			InteropHelp.TestIfPlatformSupported();
			if (NativeMethods.SteamClient() == IntPtr.Zero)
			{
				throw new InvalidOperationException("Steamworks is not initialized.");
			}
		}

		// Token: 0x06000D36 RID: 3382 RVA: 0x0002A0C4 File Offset: 0x000282C4
		public static void TestIfAvailableGameServer()
		{
			InteropHelp.TestIfPlatformSupported();
			if (NativeMethods.SteamGameServerClient() == IntPtr.Zero)
			{
				throw new InvalidOperationException("Steamworks is not initialized.");
			}
		}

		// Token: 0x06000D37 RID: 3383 RVA: 0x0002A0E8 File Offset: 0x000282E8
		public static string PtrToStringUTF8(IntPtr nativeUtf8)
		{
			if (nativeUtf8 == IntPtr.Zero)
			{
				return null;
			}
			int num = 0;
			while (Marshal.ReadByte(nativeUtf8, num) != 0)
			{
				num++;
			}
			if (num == 0)
			{
				return string.Empty;
			}
			byte[] array = new byte[num];
			Marshal.Copy(nativeUtf8, array, 0, array.Length);
			return Encoding.UTF8.GetString(array);
		}

		// Token: 0x020002BD RID: 701
		public class UTF8StringHandle : SafeHandleZeroOrMinusOneIsInvalid
		{
			// Token: 0x0600129C RID: 4764 RVA: 0x00038394 File Offset: 0x00036594
			public UTF8StringHandle(string str) : base(true)
			{
				if (str == null)
				{
					base.SetHandle(IntPtr.Zero);
					return;
				}
				byte[] array = new byte[Encoding.UTF8.GetByteCount(str) + 1];
				Encoding.UTF8.GetBytes(str, 0, str.Length, array, 0);
				IntPtr intPtr = Marshal.AllocHGlobal(array.Length);
				Marshal.Copy(array, 0, intPtr, array.Length);
				base.SetHandle(intPtr);
			}

			// Token: 0x0600129D RID: 4765 RVA: 0x000383FA File Offset: 0x000365FA
			protected override bool ReleaseHandle()
			{
				if (!this.IsInvalid)
				{
					Marshal.FreeHGlobal(this.handle);
				}
				return true;
			}
		}

		// Token: 0x020002BE RID: 702
		public class SteamParamStringArray
		{
			// Token: 0x0600129E RID: 4766 RVA: 0x00038410 File Offset: 0x00036610
			public SteamParamStringArray(IList<string> strings)
			{
				if (strings == null)
				{
					this.m_pSteamParamStringArray = IntPtr.Zero;
					return;
				}
				this.m_Strings = new IntPtr[strings.Count];
				for (int i = 0; i < strings.Count; i++)
				{
					byte[] array = new byte[Encoding.UTF8.GetByteCount(strings[i]) + 1];
					Encoding.UTF8.GetBytes(strings[i], 0, strings[i].Length, array, 0);
					this.m_Strings[i] = Marshal.AllocHGlobal(array.Length);
					Marshal.Copy(array, 0, this.m_Strings[i], array.Length);
				}
				this.m_ptrStrings = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(IntPtr)) * this.m_Strings.Length);
				SteamParamStringArray_t steamParamStringArray_t = new SteamParamStringArray_t
				{
					m_ppStrings = this.m_ptrStrings,
					m_nNumStrings = this.m_Strings.Length
				};
				Marshal.Copy(this.m_Strings, 0, steamParamStringArray_t.m_ppStrings, this.m_Strings.Length);
				this.m_pSteamParamStringArray = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(SteamParamStringArray_t)));
				Marshal.StructureToPtr<SteamParamStringArray_t>(steamParamStringArray_t, this.m_pSteamParamStringArray, false);
			}

			// Token: 0x0600129F RID: 4767 RVA: 0x0003853C File Offset: 0x0003673C
			protected override void Finalize()
			{
				try
				{
					IntPtr[] strings = this.m_Strings;
					for (int i = 0; i < strings.Length; i++)
					{
						Marshal.FreeHGlobal(strings[i]);
					}
					if (this.m_ptrStrings != IntPtr.Zero)
					{
						Marshal.FreeHGlobal(this.m_ptrStrings);
					}
					if (this.m_pSteamParamStringArray != IntPtr.Zero)
					{
						Marshal.FreeHGlobal(this.m_pSteamParamStringArray);
					}
				}
				finally
				{
					base.Finalize();
				}
			}

			// Token: 0x060012A0 RID: 4768 RVA: 0x000385BC File Offset: 0x000367BC
			public static implicit operator IntPtr(InteropHelp.SteamParamStringArray that)
			{
				return that.m_pSteamParamStringArray;
			}

			// Token: 0x04000DC1 RID: 3521
			private IntPtr[] m_Strings;

			// Token: 0x04000DC2 RID: 3522
			private IntPtr m_ptrStrings;

			// Token: 0x04000DC3 RID: 3523
			private IntPtr m_pSteamParamStringArray;
		}
	}
}
