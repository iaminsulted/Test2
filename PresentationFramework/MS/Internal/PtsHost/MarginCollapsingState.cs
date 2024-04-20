using System;

namespace MS.Internal.PtsHost
{
	// Token: 0x0200012D RID: 301
	internal sealed class MarginCollapsingState : UnmanagedHandle
	{
		// Token: 0x06000827 RID: 2087 RVA: 0x00114178 File Offset: 0x00113178
		internal static void CollapseTopMargin(PtsContext ptsContext, MbpInfo mbp, MarginCollapsingState mcsCurrent, out MarginCollapsingState mcsNew, out int margin)
		{
			margin = 0;
			mcsNew = null;
			mcsNew = new MarginCollapsingState(ptsContext, mbp.MarginTop);
			if (mcsCurrent != null)
			{
				mcsNew.Collapse(mcsCurrent);
			}
			if (mbp.BPTop != 0)
			{
				margin = mcsNew.Margin;
				mcsNew.Dispose();
				mcsNew = null;
				return;
			}
			if (mcsCurrent == null && DoubleUtil.IsZero(mbp.Margin.Top))
			{
				mcsNew.Dispose();
				mcsNew = null;
			}
		}

		// Token: 0x06000828 RID: 2088 RVA: 0x001141E4 File Offset: 0x001131E4
		internal static void CollapseBottomMargin(PtsContext ptsContext, MbpInfo mbp, MarginCollapsingState mcsCurrent, out MarginCollapsingState mcsNew, out int margin)
		{
			margin = 0;
			mcsNew = null;
			if (!DoubleUtil.IsZero(mbp.Margin.Bottom))
			{
				mcsNew = new MarginCollapsingState(ptsContext, mbp.MarginBottom);
			}
			if (mcsCurrent != null)
			{
				if (mbp.BPBottom != 0)
				{
					margin = mcsCurrent.Margin;
					return;
				}
				if (mcsNew == null)
				{
					mcsNew = new MarginCollapsingState(ptsContext, 0);
				}
				mcsNew.Collapse(mcsCurrent);
			}
		}

		// Token: 0x06000829 RID: 2089 RVA: 0x00114245 File Offset: 0x00113245
		internal MarginCollapsingState(PtsContext ptsContext, int margin) : base(ptsContext)
		{
			this._maxPositive = ((margin >= 0) ? margin : 0);
			this._minNegative = ((margin < 0) ? margin : 0);
		}

		// Token: 0x0600082A RID: 2090 RVA: 0x0011426A File Offset: 0x0011326A
		private MarginCollapsingState(MarginCollapsingState mcs) : base(mcs.PtsContext)
		{
			this._maxPositive = mcs._maxPositive;
			this._minNegative = mcs._minNegative;
		}

		// Token: 0x0600082B RID: 2091 RVA: 0x00114290 File Offset: 0x00113290
		internal MarginCollapsingState Clone()
		{
			return new MarginCollapsingState(this);
		}

		// Token: 0x0600082C RID: 2092 RVA: 0x00114298 File Offset: 0x00113298
		internal bool IsEqual(MarginCollapsingState mcs)
		{
			return this._maxPositive == mcs._maxPositive && this._minNegative == mcs._minNegative;
		}

		// Token: 0x0600082D RID: 2093 RVA: 0x001142B8 File Offset: 0x001132B8
		internal void Collapse(MarginCollapsingState mcs)
		{
			this._maxPositive = Math.Max(this._maxPositive, mcs._maxPositive);
			this._minNegative = Math.Min(this._minNegative, mcs._minNegative);
		}

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x0600082E RID: 2094 RVA: 0x001142E8 File Offset: 0x001132E8
		internal int Margin
		{
			get
			{
				return this._maxPositive + this._minNegative;
			}
		}

		// Token: 0x040007AC RID: 1964
		private int _maxPositive;

		// Token: 0x040007AD RID: 1965
		private int _minNegative;
	}
}
