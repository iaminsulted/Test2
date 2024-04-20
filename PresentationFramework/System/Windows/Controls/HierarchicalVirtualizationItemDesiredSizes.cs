using System;

namespace System.Windows.Controls
{
	// Token: 0x020007C1 RID: 1985
	public struct HierarchicalVirtualizationItemDesiredSizes
	{
		// Token: 0x06007179 RID: 29049 RVA: 0x002DA899 File Offset: 0x002D9899
		public HierarchicalVirtualizationItemDesiredSizes(Size logicalSize, Size logicalSizeInViewport, Size logicalSizeBeforeViewport, Size logicalSizeAfterViewport, Size pixelSize, Size pixelSizeInViewport, Size pixelSizeBeforeViewport, Size pixelSizeAfterViewport)
		{
			this._logicalSize = logicalSize;
			this._logicalSizeInViewport = logicalSizeInViewport;
			this._logicalSizeBeforeViewport = logicalSizeBeforeViewport;
			this._logicalSizeAfterViewport = logicalSizeAfterViewport;
			this._pixelSize = pixelSize;
			this._pixelSizeInViewport = pixelSizeInViewport;
			this._pixelSizeBeforeViewport = pixelSizeBeforeViewport;
			this._pixelSizeAfterViewport = pixelSizeAfterViewport;
		}

		// Token: 0x17001A3F RID: 6719
		// (get) Token: 0x0600717A RID: 29050 RVA: 0x002DA8D8 File Offset: 0x002D98D8
		public Size LogicalSize
		{
			get
			{
				return this._logicalSize;
			}
		}

		// Token: 0x17001A40 RID: 6720
		// (get) Token: 0x0600717B RID: 29051 RVA: 0x002DA8E0 File Offset: 0x002D98E0
		public Size LogicalSizeInViewport
		{
			get
			{
				return this._logicalSizeInViewport;
			}
		}

		// Token: 0x17001A41 RID: 6721
		// (get) Token: 0x0600717C RID: 29052 RVA: 0x002DA8E8 File Offset: 0x002D98E8
		public Size LogicalSizeBeforeViewport
		{
			get
			{
				return this._logicalSizeBeforeViewport;
			}
		}

		// Token: 0x17001A42 RID: 6722
		// (get) Token: 0x0600717D RID: 29053 RVA: 0x002DA8F0 File Offset: 0x002D98F0
		public Size LogicalSizeAfterViewport
		{
			get
			{
				return this._logicalSizeAfterViewport;
			}
		}

		// Token: 0x17001A43 RID: 6723
		// (get) Token: 0x0600717E RID: 29054 RVA: 0x002DA8F8 File Offset: 0x002D98F8
		public Size PixelSize
		{
			get
			{
				return this._pixelSize;
			}
		}

		// Token: 0x17001A44 RID: 6724
		// (get) Token: 0x0600717F RID: 29055 RVA: 0x002DA900 File Offset: 0x002D9900
		public Size PixelSizeInViewport
		{
			get
			{
				return this._pixelSizeInViewport;
			}
		}

		// Token: 0x17001A45 RID: 6725
		// (get) Token: 0x06007180 RID: 29056 RVA: 0x002DA908 File Offset: 0x002D9908
		public Size PixelSizeBeforeViewport
		{
			get
			{
				return this._pixelSizeBeforeViewport;
			}
		}

		// Token: 0x17001A46 RID: 6726
		// (get) Token: 0x06007181 RID: 29057 RVA: 0x002DA910 File Offset: 0x002D9910
		public Size PixelSizeAfterViewport
		{
			get
			{
				return this._pixelSizeAfterViewport;
			}
		}

		// Token: 0x06007182 RID: 29058 RVA: 0x002DA918 File Offset: 0x002D9918
		public static bool operator ==(HierarchicalVirtualizationItemDesiredSizes itemDesiredSizes1, HierarchicalVirtualizationItemDesiredSizes itemDesiredSizes2)
		{
			return itemDesiredSizes1.LogicalSize == itemDesiredSizes2.LogicalSize && itemDesiredSizes1.LogicalSizeInViewport == itemDesiredSizes2.LogicalSizeInViewport && itemDesiredSizes1.LogicalSizeBeforeViewport == itemDesiredSizes2.LogicalSizeBeforeViewport && itemDesiredSizes1.LogicalSizeAfterViewport == itemDesiredSizes2.LogicalSizeAfterViewport && itemDesiredSizes1.PixelSize == itemDesiredSizes2.PixelSize && itemDesiredSizes1.PixelSizeInViewport == itemDesiredSizes2.PixelSizeInViewport && itemDesiredSizes1.PixelSizeBeforeViewport == itemDesiredSizes2.PixelSizeBeforeViewport && itemDesiredSizes1.PixelSizeAfterViewport == itemDesiredSizes2.PixelSizeAfterViewport;
		}

		// Token: 0x06007183 RID: 29059 RVA: 0x002DA9D0 File Offset: 0x002D99D0
		public static bool operator !=(HierarchicalVirtualizationItemDesiredSizes itemDesiredSizes1, HierarchicalVirtualizationItemDesiredSizes itemDesiredSizes2)
		{
			return itemDesiredSizes1.LogicalSize != itemDesiredSizes2.LogicalSize || itemDesiredSizes1.LogicalSizeInViewport != itemDesiredSizes2.LogicalSizeInViewport || itemDesiredSizes1.LogicalSizeBeforeViewport != itemDesiredSizes2.LogicalSizeBeforeViewport || itemDesiredSizes1.LogicalSizeAfterViewport != itemDesiredSizes2.LogicalSizeAfterViewport || itemDesiredSizes1.PixelSize != itemDesiredSizes2.PixelSize || itemDesiredSizes1.PixelSizeInViewport != itemDesiredSizes2.PixelSizeInViewport || itemDesiredSizes1.PixelSizeBeforeViewport != itemDesiredSizes2.PixelSizeBeforeViewport || itemDesiredSizes1.PixelSizeAfterViewport != itemDesiredSizes2.PixelSizeAfterViewport;
		}

		// Token: 0x06007184 RID: 29060 RVA: 0x002DAA88 File Offset: 0x002D9A88
		public override bool Equals(object oCompare)
		{
			if (oCompare is HierarchicalVirtualizationItemDesiredSizes)
			{
				HierarchicalVirtualizationItemDesiredSizes itemDesiredSizes = (HierarchicalVirtualizationItemDesiredSizes)oCompare;
				return this == itemDesiredSizes;
			}
			return false;
		}

		// Token: 0x06007185 RID: 29061 RVA: 0x002DAAB2 File Offset: 0x002D9AB2
		public bool Equals(HierarchicalVirtualizationItemDesiredSizes comparisonItemSizes)
		{
			return this == comparisonItemSizes;
		}

		// Token: 0x06007186 RID: 29062 RVA: 0x002DAAC0 File Offset: 0x002D9AC0
		public override int GetHashCode()
		{
			return this._logicalSize.GetHashCode() ^ this._logicalSizeInViewport.GetHashCode() ^ this._logicalSizeBeforeViewport.GetHashCode() ^ this._logicalSizeAfterViewport.GetHashCode() ^ this._pixelSize.GetHashCode() ^ this._pixelSizeInViewport.GetHashCode() ^ this._pixelSizeBeforeViewport.GetHashCode() ^ this._pixelSizeAfterViewport.GetHashCode();
		}

		// Token: 0x04003728 RID: 14120
		private Size _logicalSize;

		// Token: 0x04003729 RID: 14121
		private Size _logicalSizeInViewport;

		// Token: 0x0400372A RID: 14122
		private Size _logicalSizeBeforeViewport;

		// Token: 0x0400372B RID: 14123
		private Size _logicalSizeAfterViewport;

		// Token: 0x0400372C RID: 14124
		private Size _pixelSize;

		// Token: 0x0400372D RID: 14125
		private Size _pixelSizeInViewport;

		// Token: 0x0400372E RID: 14126
		private Size _pixelSizeBeforeViewport;

		// Token: 0x0400372F RID: 14127
		private Size _pixelSizeAfterViewport;
	}
}
