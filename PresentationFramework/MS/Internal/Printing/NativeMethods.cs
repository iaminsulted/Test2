using System;
using System.Runtime.InteropServices;

namespace MS.Internal.Printing
{
	// Token: 0x02000154 RID: 340
	internal static class NativeMethods
	{
		// Token: 0x040008A5 RID: 2213
		internal const uint PD_ALLPAGES = 0U;

		// Token: 0x040008A6 RID: 2214
		internal const uint PD_SELECTION = 1U;

		// Token: 0x040008A7 RID: 2215
		internal const uint PD_PAGENUMS = 2U;

		// Token: 0x040008A8 RID: 2216
		internal const uint PD_NOSELECTION = 4U;

		// Token: 0x040008A9 RID: 2217
		internal const uint PD_NOPAGENUMS = 8U;

		// Token: 0x040008AA RID: 2218
		internal const uint PD_USEDEVMODECOPIESANDCOLLATE = 262144U;

		// Token: 0x040008AB RID: 2219
		internal const uint PD_DISABLEPRINTTOFILE = 524288U;

		// Token: 0x040008AC RID: 2220
		internal const uint PD_HIDEPRINTTOFILE = 1048576U;

		// Token: 0x040008AD RID: 2221
		internal const uint PD_CURRENTPAGE = 4194304U;

		// Token: 0x040008AE RID: 2222
		internal const uint PD_NOCURRENTPAGE = 8388608U;

		// Token: 0x040008AF RID: 2223
		internal const uint PD_RESULT_CANCEL = 0U;

		// Token: 0x040008B0 RID: 2224
		internal const uint PD_RESULT_PRINT = 1U;

		// Token: 0x040008B1 RID: 2225
		internal const uint PD_RESULT_APPLY = 2U;

		// Token: 0x040008B2 RID: 2226
		internal const uint START_PAGE_GENERAL = 4294967295U;

		// Token: 0x020009B7 RID: 2487
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 1)]
		internal class PRINTDLGEX32
		{
			// Token: 0x04003F24 RID: 16164
			public int lStructSize = Marshal.SizeOf(typeof(NativeMethods.PRINTDLGEX32));

			// Token: 0x04003F25 RID: 16165
			public IntPtr hwndOwner = IntPtr.Zero;

			// Token: 0x04003F26 RID: 16166
			public IntPtr hDevMode = IntPtr.Zero;

			// Token: 0x04003F27 RID: 16167
			public IntPtr hDevNames = IntPtr.Zero;

			// Token: 0x04003F28 RID: 16168
			public IntPtr hDC = IntPtr.Zero;

			// Token: 0x04003F29 RID: 16169
			public uint Flags;

			// Token: 0x04003F2A RID: 16170
			public uint Flags2;

			// Token: 0x04003F2B RID: 16171
			public uint ExclusionFlags;

			// Token: 0x04003F2C RID: 16172
			public uint nPageRanges;

			// Token: 0x04003F2D RID: 16173
			public uint nMaxPageRanges;

			// Token: 0x04003F2E RID: 16174
			public IntPtr lpPageRanges = IntPtr.Zero;

			// Token: 0x04003F2F RID: 16175
			public uint nMinPage;

			// Token: 0x04003F30 RID: 16176
			public uint nMaxPage;

			// Token: 0x04003F31 RID: 16177
			public uint nCopies;

			// Token: 0x04003F32 RID: 16178
			public IntPtr hInstance = IntPtr.Zero;

			// Token: 0x04003F33 RID: 16179
			public IntPtr lpPrintTemplateName = IntPtr.Zero;

			// Token: 0x04003F34 RID: 16180
			public IntPtr lpCallback = IntPtr.Zero;

			// Token: 0x04003F35 RID: 16181
			public uint nPropertyPages;

			// Token: 0x04003F36 RID: 16182
			public IntPtr lphPropertyPages = IntPtr.Zero;

			// Token: 0x04003F37 RID: 16183
			public uint nStartPage = uint.MaxValue;

			// Token: 0x04003F38 RID: 16184
			public uint dwResultAction;
		}

		// Token: 0x020009B8 RID: 2488
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 8)]
		internal class PRINTDLGEX64
		{
			// Token: 0x04003F39 RID: 16185
			public int lStructSize = Marshal.SizeOf(typeof(NativeMethods.PRINTDLGEX64));

			// Token: 0x04003F3A RID: 16186
			public IntPtr hwndOwner = IntPtr.Zero;

			// Token: 0x04003F3B RID: 16187
			public IntPtr hDevMode = IntPtr.Zero;

			// Token: 0x04003F3C RID: 16188
			public IntPtr hDevNames = IntPtr.Zero;

			// Token: 0x04003F3D RID: 16189
			public IntPtr hDC = IntPtr.Zero;

			// Token: 0x04003F3E RID: 16190
			public uint Flags;

			// Token: 0x04003F3F RID: 16191
			public uint Flags2;

			// Token: 0x04003F40 RID: 16192
			public uint ExclusionFlags;

			// Token: 0x04003F41 RID: 16193
			public uint nPageRanges;

			// Token: 0x04003F42 RID: 16194
			public uint nMaxPageRanges;

			// Token: 0x04003F43 RID: 16195
			public IntPtr lpPageRanges = IntPtr.Zero;

			// Token: 0x04003F44 RID: 16196
			public uint nMinPage;

			// Token: 0x04003F45 RID: 16197
			public uint nMaxPage;

			// Token: 0x04003F46 RID: 16198
			public uint nCopies;

			// Token: 0x04003F47 RID: 16199
			public IntPtr hInstance = IntPtr.Zero;

			// Token: 0x04003F48 RID: 16200
			public IntPtr lpPrintTemplateName = IntPtr.Zero;

			// Token: 0x04003F49 RID: 16201
			public IntPtr lpCallback = IntPtr.Zero;

			// Token: 0x04003F4A RID: 16202
			public uint nPropertyPages;

			// Token: 0x04003F4B RID: 16203
			public IntPtr lphPropertyPages = IntPtr.Zero;

			// Token: 0x04003F4C RID: 16204
			public uint nStartPage = uint.MaxValue;

			// Token: 0x04003F4D RID: 16205
			public uint dwResultAction;
		}

		// Token: 0x020009B9 RID: 2489
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 1)]
		internal struct DEVMODE
		{
			// Token: 0x04003F4E RID: 16206
			private const int CCHDEVICENAME = 32;

			// Token: 0x04003F4F RID: 16207
			private const int CCHFORMNAME = 32;

			// Token: 0x04003F50 RID: 16208
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
			public string dmDeviceName;

			// Token: 0x04003F51 RID: 16209
			public ushort dmSpecVersion;

			// Token: 0x04003F52 RID: 16210
			public ushort dmDriverVersion;

			// Token: 0x04003F53 RID: 16211
			public ushort dmSize;

			// Token: 0x04003F54 RID: 16212
			public ushort dmDriverExtra;

			// Token: 0x04003F55 RID: 16213
			public uint dmFields;

			// Token: 0x04003F56 RID: 16214
			public int dmPositionX;

			// Token: 0x04003F57 RID: 16215
			public int dmPositionY;

			// Token: 0x04003F58 RID: 16216
			public uint dmDisplayOrientation;

			// Token: 0x04003F59 RID: 16217
			public uint dmDisplayFixedOutput;

			// Token: 0x04003F5A RID: 16218
			public short dmColor;

			// Token: 0x04003F5B RID: 16219
			public short dmDuplex;

			// Token: 0x04003F5C RID: 16220
			public short dmYResolution;

			// Token: 0x04003F5D RID: 16221
			public short dmTTOption;

			// Token: 0x04003F5E RID: 16222
			public short dmCollate;

			// Token: 0x04003F5F RID: 16223
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
			public string dmFormName;

			// Token: 0x04003F60 RID: 16224
			public ushort dmLogPixels;

			// Token: 0x04003F61 RID: 16225
			public uint dmBitsPerPel;

			// Token: 0x04003F62 RID: 16226
			public uint dmPelsWidth;

			// Token: 0x04003F63 RID: 16227
			public uint dmPelsHeight;

			// Token: 0x04003F64 RID: 16228
			public uint dmDisplayFlags;

			// Token: 0x04003F65 RID: 16229
			public uint dmDisplayFrequency;

			// Token: 0x04003F66 RID: 16230
			public uint dmICMMethod;

			// Token: 0x04003F67 RID: 16231
			public uint dmICMIntent;

			// Token: 0x04003F68 RID: 16232
			public uint dmMediaType;

			// Token: 0x04003F69 RID: 16233
			public uint dmDitherType;

			// Token: 0x04003F6A RID: 16234
			public uint dmReserved1;

			// Token: 0x04003F6B RID: 16235
			public uint dmReserved2;

			// Token: 0x04003F6C RID: 16236
			public uint dmPanningWidth;

			// Token: 0x04003F6D RID: 16237
			public uint dmPanningHeight;
		}

		// Token: 0x020009BA RID: 2490
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 1)]
		internal struct DEVNAMES
		{
			// Token: 0x04003F6E RID: 16238
			public ushort wDriverOffset;

			// Token: 0x04003F6F RID: 16239
			public ushort wDeviceOffset;

			// Token: 0x04003F70 RID: 16240
			public ushort wOutputOffset;

			// Token: 0x04003F71 RID: 16241
			public ushort wDefault;
		}

		// Token: 0x020009BB RID: 2491
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 1)]
		internal struct PRINTPAGERANGE
		{
			// Token: 0x04003F72 RID: 16242
			public uint nFromPage;

			// Token: 0x04003F73 RID: 16243
			public uint nToPage;
		}
	}
}
