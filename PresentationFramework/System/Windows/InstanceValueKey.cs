using System;

namespace System.Windows
{
	// Token: 0x020003B1 RID: 945
	internal class InstanceValueKey
	{
		// Token: 0x0600261E RID: 9758 RVA: 0x0018B3BD File Offset: 0x0018A3BD
		internal InstanceValueKey(int childIndex, int dpIndex, int index)
		{
			this._childIndex = childIndex;
			this._dpIndex = dpIndex;
			this._index = index;
		}

		// Token: 0x0600261F RID: 9759 RVA: 0x0018B3DC File Offset: 0x0018A3DC
		public override bool Equals(object o)
		{
			InstanceValueKey instanceValueKey = o as InstanceValueKey;
			return instanceValueKey != null && (this._childIndex == instanceValueKey._childIndex && this._dpIndex == instanceValueKey._dpIndex) && this._index == instanceValueKey._index;
		}

		// Token: 0x06002620 RID: 9760 RVA: 0x0018B421 File Offset: 0x0018A421
		public override int GetHashCode()
		{
			return 20000 * this._childIndex + 20 * this._dpIndex + this._index;
		}

		// Token: 0x040011EE RID: 4590
		private int _childIndex;

		// Token: 0x040011EF RID: 4591
		private int _dpIndex;

		// Token: 0x040011F0 RID: 4592
		private int _index;
	}
}
