using System;
using System.ComponentModel;
using System.Globalization;
using MS.Internal;

namespace System.Windows.Controls
{
	// Token: 0x02000804 RID: 2052
	[TypeConverter(typeof(VirtualizationCacheLengthConverter))]
	public struct VirtualizationCacheLength : IEquatable<VirtualizationCacheLength>
	{
		// Token: 0x06007727 RID: 30503 RVA: 0x002F15E6 File Offset: 0x002F05E6
		public VirtualizationCacheLength(double cacheBeforeAndAfterViewport)
		{
			this = new VirtualizationCacheLength(cacheBeforeAndAfterViewport, cacheBeforeAndAfterViewport);
		}

		// Token: 0x06007728 RID: 30504 RVA: 0x002F15F0 File Offset: 0x002F05F0
		public VirtualizationCacheLength(double cacheBeforeViewport, double cacheAfterViewport)
		{
			if (DoubleUtil.IsNaN(cacheBeforeViewport))
			{
				throw new ArgumentException(SR.Get("InvalidCtorParameterNoNaN", new object[]
				{
					"cacheBeforeViewport"
				}));
			}
			if (DoubleUtil.IsNaN(cacheAfterViewport))
			{
				throw new ArgumentException(SR.Get("InvalidCtorParameterNoNaN", new object[]
				{
					"cacheAfterViewport"
				}));
			}
			this._cacheBeforeViewport = cacheBeforeViewport;
			this._cacheAfterViewport = cacheAfterViewport;
		}

		// Token: 0x06007729 RID: 30505 RVA: 0x002F1657 File Offset: 0x002F0657
		public static bool operator ==(VirtualizationCacheLength cl1, VirtualizationCacheLength cl2)
		{
			return cl1.CacheBeforeViewport == cl2.CacheBeforeViewport && cl1.CacheAfterViewport == cl2.CacheAfterViewport;
		}

		// Token: 0x0600772A RID: 30506 RVA: 0x002F167B File Offset: 0x002F067B
		public static bool operator !=(VirtualizationCacheLength cl1, VirtualizationCacheLength cl2)
		{
			return cl1.CacheBeforeViewport != cl2.CacheBeforeViewport || cl1.CacheAfterViewport != cl2.CacheAfterViewport;
		}

		// Token: 0x0600772B RID: 30507 RVA: 0x002F16A4 File Offset: 0x002F06A4
		public override bool Equals(object oCompare)
		{
			if (oCompare is VirtualizationCacheLength)
			{
				VirtualizationCacheLength cl = (VirtualizationCacheLength)oCompare;
				return this == cl;
			}
			return false;
		}

		// Token: 0x0600772C RID: 30508 RVA: 0x002F16CE File Offset: 0x002F06CE
		public bool Equals(VirtualizationCacheLength cacheLength)
		{
			return this == cacheLength;
		}

		// Token: 0x0600772D RID: 30509 RVA: 0x002F16DC File Offset: 0x002F06DC
		public override int GetHashCode()
		{
			return (int)this._cacheBeforeViewport + (int)this._cacheAfterViewport;
		}

		// Token: 0x17001BAE RID: 7086
		// (get) Token: 0x0600772E RID: 30510 RVA: 0x002F16ED File Offset: 0x002F06ED
		public double CacheBeforeViewport
		{
			get
			{
				return this._cacheBeforeViewport;
			}
		}

		// Token: 0x17001BAF RID: 7087
		// (get) Token: 0x0600772F RID: 30511 RVA: 0x002F16F5 File Offset: 0x002F06F5
		public double CacheAfterViewport
		{
			get
			{
				return this._cacheAfterViewport;
			}
		}

		// Token: 0x06007730 RID: 30512 RVA: 0x002F16FD File Offset: 0x002F06FD
		public override string ToString()
		{
			return VirtualizationCacheLengthConverter.ToString(this, CultureInfo.InvariantCulture);
		}

		// Token: 0x040038CF RID: 14543
		private double _cacheBeforeViewport;

		// Token: 0x040038D0 RID: 14544
		private double _cacheAfterViewport;
	}
}
