using System;

namespace System.Windows.Controls
{
	// Token: 0x020007BF RID: 1983
	public struct HierarchicalVirtualizationConstraints
	{
		// Token: 0x06007166 RID: 29030 RVA: 0x002DA693 File Offset: 0x002D9693
		public HierarchicalVirtualizationConstraints(VirtualizationCacheLength cacheLength, VirtualizationCacheLengthUnit cacheLengthUnit, Rect viewport)
		{
			this._cacheLength = cacheLength;
			this._cacheLengthUnit = cacheLengthUnit;
			this._viewport = viewport;
			this._scrollGeneration = 0L;
		}

		// Token: 0x17001A39 RID: 6713
		// (get) Token: 0x06007167 RID: 29031 RVA: 0x002DA6B2 File Offset: 0x002D96B2
		public VirtualizationCacheLength CacheLength
		{
			get
			{
				return this._cacheLength;
			}
		}

		// Token: 0x17001A3A RID: 6714
		// (get) Token: 0x06007168 RID: 29032 RVA: 0x002DA6BA File Offset: 0x002D96BA
		public VirtualizationCacheLengthUnit CacheLengthUnit
		{
			get
			{
				return this._cacheLengthUnit;
			}
		}

		// Token: 0x17001A3B RID: 6715
		// (get) Token: 0x06007169 RID: 29033 RVA: 0x002DA6C2 File Offset: 0x002D96C2
		public Rect Viewport
		{
			get
			{
				return this._viewport;
			}
		}

		// Token: 0x0600716A RID: 29034 RVA: 0x002DA6CA File Offset: 0x002D96CA
		public static bool operator ==(HierarchicalVirtualizationConstraints constraints1, HierarchicalVirtualizationConstraints constraints2)
		{
			return constraints1.CacheLength == constraints2.CacheLength && constraints1.CacheLengthUnit == constraints2.CacheLengthUnit && constraints2.Viewport == constraints2.Viewport;
		}

		// Token: 0x0600716B RID: 29035 RVA: 0x002DA706 File Offset: 0x002D9706
		public static bool operator !=(HierarchicalVirtualizationConstraints constraints1, HierarchicalVirtualizationConstraints constraints2)
		{
			return constraints1.CacheLength != constraints2.CacheLength || constraints1.CacheLengthUnit != constraints2.CacheLengthUnit || constraints1.Viewport != constraints2.Viewport;
		}

		// Token: 0x0600716C RID: 29036 RVA: 0x002DA744 File Offset: 0x002D9744
		public override bool Equals(object oCompare)
		{
			if (oCompare is HierarchicalVirtualizationConstraints)
			{
				HierarchicalVirtualizationConstraints constraints = (HierarchicalVirtualizationConstraints)oCompare;
				return this == constraints;
			}
			return false;
		}

		// Token: 0x0600716D RID: 29037 RVA: 0x002DA76E File Offset: 0x002D976E
		public bool Equals(HierarchicalVirtualizationConstraints comparisonConstraints)
		{
			return this == comparisonConstraints;
		}

		// Token: 0x0600716E RID: 29038 RVA: 0x002DA77C File Offset: 0x002D977C
		public override int GetHashCode()
		{
			return this._cacheLength.GetHashCode() ^ this._cacheLengthUnit.GetHashCode() ^ this._viewport.GetHashCode();
		}

		// Token: 0x17001A3C RID: 6716
		// (get) Token: 0x0600716F RID: 29039 RVA: 0x002DA7B3 File Offset: 0x002D97B3
		// (set) Token: 0x06007170 RID: 29040 RVA: 0x002DA7BB File Offset: 0x002D97BB
		internal long ScrollGeneration
		{
			get
			{
				return this._scrollGeneration;
			}
			set
			{
				this._scrollGeneration = value;
			}
		}

		// Token: 0x04003722 RID: 14114
		private VirtualizationCacheLength _cacheLength;

		// Token: 0x04003723 RID: 14115
		private VirtualizationCacheLengthUnit _cacheLengthUnit;

		// Token: 0x04003724 RID: 14116
		private Rect _viewport;

		// Token: 0x04003725 RID: 14117
		private long _scrollGeneration;
	}
}
