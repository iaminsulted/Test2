using System;

namespace System.Windows.Controls
{
	// Token: 0x020007C0 RID: 1984
	public struct HierarchicalVirtualizationHeaderDesiredSizes
	{
		// Token: 0x06007171 RID: 29041 RVA: 0x002DA7C4 File Offset: 0x002D97C4
		public HierarchicalVirtualizationHeaderDesiredSizes(Size logicalSize, Size pixelSize)
		{
			this._logicalSize = logicalSize;
			this._pixelSize = pixelSize;
		}

		// Token: 0x17001A3D RID: 6717
		// (get) Token: 0x06007172 RID: 29042 RVA: 0x002DA7D4 File Offset: 0x002D97D4
		public Size LogicalSize
		{
			get
			{
				return this._logicalSize;
			}
		}

		// Token: 0x17001A3E RID: 6718
		// (get) Token: 0x06007173 RID: 29043 RVA: 0x002DA7DC File Offset: 0x002D97DC
		public Size PixelSize
		{
			get
			{
				return this._pixelSize;
			}
		}

		// Token: 0x06007174 RID: 29044 RVA: 0x002DA7E4 File Offset: 0x002D97E4
		public static bool operator ==(HierarchicalVirtualizationHeaderDesiredSizes headerDesiredSizes1, HierarchicalVirtualizationHeaderDesiredSizes headerDesiredSizes2)
		{
			return headerDesiredSizes1.LogicalSize == headerDesiredSizes2.LogicalSize && headerDesiredSizes1.PixelSize == headerDesiredSizes2.PixelSize;
		}

		// Token: 0x06007175 RID: 29045 RVA: 0x002DA810 File Offset: 0x002D9810
		public static bool operator !=(HierarchicalVirtualizationHeaderDesiredSizes headerDesiredSizes1, HierarchicalVirtualizationHeaderDesiredSizes headerDesiredSizes2)
		{
			return headerDesiredSizes1.LogicalSize != headerDesiredSizes2.LogicalSize || headerDesiredSizes1.PixelSize != headerDesiredSizes2.PixelSize;
		}

		// Token: 0x06007176 RID: 29046 RVA: 0x002DA83C File Offset: 0x002D983C
		public override bool Equals(object oCompare)
		{
			if (oCompare is HierarchicalVirtualizationHeaderDesiredSizes)
			{
				HierarchicalVirtualizationHeaderDesiredSizes headerDesiredSizes = (HierarchicalVirtualizationHeaderDesiredSizes)oCompare;
				return this == headerDesiredSizes;
			}
			return false;
		}

		// Token: 0x06007177 RID: 29047 RVA: 0x002DA866 File Offset: 0x002D9866
		public bool Equals(HierarchicalVirtualizationHeaderDesiredSizes comparisonHeaderSizes)
		{
			return this == comparisonHeaderSizes;
		}

		// Token: 0x06007178 RID: 29048 RVA: 0x002DA874 File Offset: 0x002D9874
		public override int GetHashCode()
		{
			return this._logicalSize.GetHashCode() ^ this._pixelSize.GetHashCode();
		}

		// Token: 0x04003726 RID: 14118
		private Size _logicalSize;

		// Token: 0x04003727 RID: 14119
		private Size _pixelSize;
	}
}
