using System;

namespace System.Windows
{
	// Token: 0x02000355 RID: 853
	public struct ValueSource
	{
		// Token: 0x06002054 RID: 8276 RVA: 0x0017509F File Offset: 0x0017409F
		internal ValueSource(BaseValueSourceInternal source, bool isExpression, bool isAnimated, bool isCoerced, bool isCurrent)
		{
			this._baseValueSource = (BaseValueSource)source;
			this._isExpression = isExpression;
			this._isAnimated = isAnimated;
			this._isCoerced = isCoerced;
			this._isCurrent = isCurrent;
		}

		// Token: 0x1700062F RID: 1583
		// (get) Token: 0x06002055 RID: 8277 RVA: 0x001750C6 File Offset: 0x001740C6
		public BaseValueSource BaseValueSource
		{
			get
			{
				return this._baseValueSource;
			}
		}

		// Token: 0x17000630 RID: 1584
		// (get) Token: 0x06002056 RID: 8278 RVA: 0x001750CE File Offset: 0x001740CE
		public bool IsExpression
		{
			get
			{
				return this._isExpression;
			}
		}

		// Token: 0x17000631 RID: 1585
		// (get) Token: 0x06002057 RID: 8279 RVA: 0x001750D6 File Offset: 0x001740D6
		public bool IsAnimated
		{
			get
			{
				return this._isAnimated;
			}
		}

		// Token: 0x17000632 RID: 1586
		// (get) Token: 0x06002058 RID: 8280 RVA: 0x001750DE File Offset: 0x001740DE
		public bool IsCoerced
		{
			get
			{
				return this._isCoerced;
			}
		}

		// Token: 0x17000633 RID: 1587
		// (get) Token: 0x06002059 RID: 8281 RVA: 0x001750E6 File Offset: 0x001740E6
		public bool IsCurrent
		{
			get
			{
				return this._isCurrent;
			}
		}

		// Token: 0x0600205A RID: 8282 RVA: 0x001750EE File Offset: 0x001740EE
		public override int GetHashCode()
		{
			return this._baseValueSource.GetHashCode();
		}

		// Token: 0x0600205B RID: 8283 RVA: 0x00175104 File Offset: 0x00174104
		public override bool Equals(object o)
		{
			if (o is ValueSource)
			{
				ValueSource valueSource = (ValueSource)o;
				return this._baseValueSource == valueSource._baseValueSource && this._isExpression == valueSource._isExpression && this._isAnimated == valueSource._isAnimated && this._isCoerced == valueSource._isCoerced;
			}
			return false;
		}

		// Token: 0x0600205C RID: 8284 RVA: 0x0017515C File Offset: 0x0017415C
		public static bool operator ==(ValueSource vs1, ValueSource vs2)
		{
			return vs1.Equals(vs2);
		}

		// Token: 0x0600205D RID: 8285 RVA: 0x00175171 File Offset: 0x00174171
		public static bool operator !=(ValueSource vs1, ValueSource vs2)
		{
			return !vs1.Equals(vs2);
		}

		// Token: 0x04000FD6 RID: 4054
		private BaseValueSource _baseValueSource;

		// Token: 0x04000FD7 RID: 4055
		private bool _isExpression;

		// Token: 0x04000FD8 RID: 4056
		private bool _isAnimated;

		// Token: 0x04000FD9 RID: 4057
		private bool _isCoerced;

		// Token: 0x04000FDA RID: 4058
		private bool _isCurrent;
	}
}
