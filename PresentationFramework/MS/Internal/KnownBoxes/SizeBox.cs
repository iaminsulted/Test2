using System;
using System.Windows;

namespace MS.Internal.KnownBoxes
{
	// Token: 0x02000159 RID: 345
	internal class SizeBox
	{
		// Token: 0x06000B77 RID: 2935 RVA: 0x0012C849 File Offset: 0x0012B849
		internal SizeBox(double width, double height)
		{
			if (width < 0.0 || height < 0.0)
			{
				throw new ArgumentException(SR.Get("Rect_WidthAndHeightCannotBeNegative"));
			}
			this._width = width;
			this._height = height;
		}

		// Token: 0x06000B78 RID: 2936 RVA: 0x0012C887 File Offset: 0x0012B887
		internal SizeBox(Size size) : this(size.Width, size.Height)
		{
		}

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x06000B79 RID: 2937 RVA: 0x0012C89D File Offset: 0x0012B89D
		// (set) Token: 0x06000B7A RID: 2938 RVA: 0x0012C8A5 File Offset: 0x0012B8A5
		internal double Width
		{
			get
			{
				return this._width;
			}
			set
			{
				if (value < 0.0)
				{
					throw new ArgumentException(SR.Get("Rect_WidthAndHeightCannotBeNegative"));
				}
				this._width = value;
			}
		}

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x06000B7B RID: 2939 RVA: 0x0012C8CA File Offset: 0x0012B8CA
		// (set) Token: 0x06000B7C RID: 2940 RVA: 0x0012C8D2 File Offset: 0x0012B8D2
		internal double Height
		{
			get
			{
				return this._height;
			}
			set
			{
				if (value < 0.0)
				{
					throw new ArgumentException(SR.Get("Rect_WidthAndHeightCannotBeNegative"));
				}
				this._height = value;
			}
		}

		// Token: 0x040008C5 RID: 2245
		private double _width;

		// Token: 0x040008C6 RID: 2246
		private double _height;
	}
}
