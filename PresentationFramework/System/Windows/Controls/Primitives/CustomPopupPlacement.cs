using System;

namespace System.Windows.Controls.Primitives
{
	// Token: 0x02000827 RID: 2087
	public struct CustomPopupPlacement
	{
		// Token: 0x060079F0 RID: 31216 RVA: 0x00305DAA File Offset: 0x00304DAA
		public CustomPopupPlacement(Point point, PopupPrimaryAxis primaryAxis)
		{
			this._point = point;
			this._primaryAxis = primaryAxis;
		}

		// Token: 0x17001C3D RID: 7229
		// (get) Token: 0x060079F1 RID: 31217 RVA: 0x00305DBA File Offset: 0x00304DBA
		// (set) Token: 0x060079F2 RID: 31218 RVA: 0x00305DC2 File Offset: 0x00304DC2
		public Point Point
		{
			get
			{
				return this._point;
			}
			set
			{
				this._point = value;
			}
		}

		// Token: 0x17001C3E RID: 7230
		// (get) Token: 0x060079F3 RID: 31219 RVA: 0x00305DCB File Offset: 0x00304DCB
		// (set) Token: 0x060079F4 RID: 31220 RVA: 0x00305DD3 File Offset: 0x00304DD3
		public PopupPrimaryAxis PrimaryAxis
		{
			get
			{
				return this._primaryAxis;
			}
			set
			{
				this._primaryAxis = value;
			}
		}

		// Token: 0x060079F5 RID: 31221 RVA: 0x00305DDC File Offset: 0x00304DDC
		public static bool operator ==(CustomPopupPlacement placement1, CustomPopupPlacement placement2)
		{
			return placement1.Equals(placement2);
		}

		// Token: 0x060079F6 RID: 31222 RVA: 0x00305DF1 File Offset: 0x00304DF1
		public static bool operator !=(CustomPopupPlacement placement1, CustomPopupPlacement placement2)
		{
			return !placement1.Equals(placement2);
		}

		// Token: 0x060079F7 RID: 31223 RVA: 0x00305E0C File Offset: 0x00304E0C
		public override bool Equals(object o)
		{
			if (o is CustomPopupPlacement)
			{
				CustomPopupPlacement customPopupPlacement = (CustomPopupPlacement)o;
				return customPopupPlacement._primaryAxis == this._primaryAxis && customPopupPlacement._point == this._point;
			}
			return false;
		}

		// Token: 0x060079F8 RID: 31224 RVA: 0x00305E4B File Offset: 0x00304E4B
		public override int GetHashCode()
		{
			return this._primaryAxis.GetHashCode() ^ this._point.GetHashCode();
		}

		// Token: 0x040039D9 RID: 14809
		private Point _point;

		// Token: 0x040039DA RID: 14810
		private PopupPrimaryAxis _primaryAxis;
	}
}
