using System;

namespace MS.Internal.Data
{
	// Token: 0x02000245 RID: 581
	internal struct WeakRefKey
	{
		// Token: 0x06001670 RID: 5744 RVA: 0x0015AAAC File Offset: 0x00159AAC
		internal WeakRefKey(object target)
		{
			this._weakRef = new WeakReference(target);
			this._hashCode = ((target != null) ? target.GetHashCode() : 314159);
		}

		// Token: 0x17000455 RID: 1109
		// (get) Token: 0x06001671 RID: 5745 RVA: 0x0015AAD0 File Offset: 0x00159AD0
		internal object Target
		{
			get
			{
				return this._weakRef.Target;
			}
		}

		// Token: 0x06001672 RID: 5746 RVA: 0x0015AADD File Offset: 0x00159ADD
		public override int GetHashCode()
		{
			return this._hashCode;
		}

		// Token: 0x06001673 RID: 5747 RVA: 0x0015AAE8 File Offset: 0x00159AE8
		public override bool Equals(object o)
		{
			if (!(o is WeakRefKey))
			{
				return false;
			}
			WeakRefKey weakRefKey = (WeakRefKey)o;
			object target = this.Target;
			object target2 = weakRefKey.Target;
			if (target != null && target2 != null)
			{
				return target == target2;
			}
			return this._weakRef == weakRefKey._weakRef;
		}

		// Token: 0x06001674 RID: 5748 RVA: 0x0015AB2E File Offset: 0x00159B2E
		public static bool operator ==(WeakRefKey left, WeakRefKey right)
		{
			if (left == null)
			{
				return right == null;
			}
			return left.Equals(right);
		}

		// Token: 0x06001675 RID: 5749 RVA: 0x0015AB55 File Offset: 0x00159B55
		public static bool operator !=(WeakRefKey left, WeakRefKey right)
		{
			return !(left == right);
		}

		// Token: 0x04000C5D RID: 3165
		private WeakReference _weakRef;

		// Token: 0x04000C5E RID: 3166
		private int _hashCode;
	}
}
