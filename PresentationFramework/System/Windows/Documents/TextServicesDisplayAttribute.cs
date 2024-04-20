using System;
using System.Windows.Media;
using MS.Internal;
using MS.Win32;

namespace System.Windows.Documents
{
	// Token: 0x020006C2 RID: 1730
	internal class TextServicesDisplayAttribute
	{
		// Token: 0x060059DE RID: 23006 RVA: 0x0027E848 File Offset: 0x0027D848
		internal TextServicesDisplayAttribute(UnsafeNativeMethods.TF_DISPLAYATTRIBUTE attr)
		{
			this._attr = attr;
		}

		// Token: 0x060059DF RID: 23007 RVA: 0x0027E858 File Offset: 0x0027D858
		internal bool IsEmptyAttribute()
		{
			return this._attr.crText.type == UnsafeNativeMethods.TF_DA_COLORTYPE.TF_CT_NONE && this._attr.crBk.type == UnsafeNativeMethods.TF_DA_COLORTYPE.TF_CT_NONE && this._attr.crLine.type == UnsafeNativeMethods.TF_DA_COLORTYPE.TF_CT_NONE && this._attr.lsStyle == UnsafeNativeMethods.TF_DA_LINESTYLE.TF_LS_NONE;
		}

		// Token: 0x060059E0 RID: 23008 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		internal void Apply(ITextPointer start, ITextPointer end)
		{
		}

		// Token: 0x060059E1 RID: 23009 RVA: 0x0027E8AC File Offset: 0x0027D8AC
		internal static Color GetColor(UnsafeNativeMethods.TF_DA_COLOR dacolor, ITextPointer position)
		{
			if (dacolor.type == UnsafeNativeMethods.TF_DA_COLORTYPE.TF_CT_SYSCOLOR)
			{
				return TextServicesDisplayAttribute.GetSystemColor(dacolor.indexOrColorRef);
			}
			if (dacolor.type == UnsafeNativeMethods.TF_DA_COLORTYPE.TF_CT_COLORREF)
			{
				uint num = (uint)TextServicesDisplayAttribute.FromWin32Value(dacolor.indexOrColorRef);
				return Color.FromArgb((byte)((num & 4278190080U) >> 24), (byte)((num & 16711680U) >> 16), (byte)((num & 65280U) >> 8), (byte)(num & 255U));
			}
			Invariant.Assert(position != null, "position can't be null");
			return ((SolidColorBrush)position.GetValue(TextElement.ForegroundProperty)).Color;
		}

		// Token: 0x060059E2 RID: 23010 RVA: 0x0027E934 File Offset: 0x0027D934
		internal Color GetLineColor(ITextPointer position)
		{
			return TextServicesDisplayAttribute.GetColor(this._attr.crLine, position);
		}

		// Token: 0x170014D1 RID: 5329
		// (get) Token: 0x060059E3 RID: 23011 RVA: 0x0027E947 File Offset: 0x0027D947
		internal UnsafeNativeMethods.TF_DA_LINESTYLE LineStyle
		{
			get
			{
				return this._attr.lsStyle;
			}
		}

		// Token: 0x170014D2 RID: 5330
		// (get) Token: 0x060059E4 RID: 23012 RVA: 0x0027E954 File Offset: 0x0027D954
		internal bool IsBoldLine
		{
			get
			{
				return this._attr.fBoldLine;
			}
		}

		// Token: 0x170014D3 RID: 5331
		// (get) Token: 0x060059E5 RID: 23013 RVA: 0x0027E961 File Offset: 0x0027D961
		internal UnsafeNativeMethods.TF_DA_ATTR_INFO AttrInfo
		{
			get
			{
				return this._attr.bAttr;
			}
		}

		// Token: 0x060059E6 RID: 23014 RVA: 0x0018BD24 File Offset: 0x0018AD24
		private static int Encode(int alpha, int red, int green, int blue)
		{
			return red << 16 | green << 8 | blue | alpha << 24;
		}

		// Token: 0x060059E7 RID: 23015 RVA: 0x0027E96E File Offset: 0x0027D96E
		private static int FromWin32Value(int value)
		{
			return TextServicesDisplayAttribute.Encode(255, value & 255, value >> 8 & 255, value >> 16 & 255);
		}

		// Token: 0x060059E8 RID: 23016 RVA: 0x0027E994 File Offset: 0x0027D994
		private static Color GetSystemColor(int index)
		{
			uint num = (uint)TextServicesDisplayAttribute.FromWin32Value(SafeNativeMethods.GetSysColor(index));
			return Color.FromArgb((byte)((num & 4278190080U) >> 24), (byte)((num & 16711680U) >> 16), (byte)((num & 65280U) >> 8), (byte)(num & 255U));
		}

		// Token: 0x0400301B RID: 12315
		private const int AlphaShift = 24;

		// Token: 0x0400301C RID: 12316
		private const int RedShift = 16;

		// Token: 0x0400301D RID: 12317
		private const int GreenShift = 8;

		// Token: 0x0400301E RID: 12318
		private const int BlueShift = 0;

		// Token: 0x0400301F RID: 12319
		private const int Win32RedShift = 0;

		// Token: 0x04003020 RID: 12320
		private const int Win32GreenShift = 8;

		// Token: 0x04003021 RID: 12321
		private const int Win32BlueShift = 16;

		// Token: 0x04003022 RID: 12322
		private UnsafeNativeMethods.TF_DISPLAYATTRIBUTE _attr;
	}
}
