using System;
using System.Collections;

namespace System.Windows
{
	// Token: 0x020003E6 RID: 998
	internal class SingleChildEnumerator : IEnumerator
	{
		// Token: 0x06002AEE RID: 10990 RVA: 0x001A0A54 File Offset: 0x0019FA54
		internal SingleChildEnumerator(object Child)
		{
			this._child = Child;
			this._count = ((Child == null) ? 0 : 1);
		}

		// Token: 0x170009FC RID: 2556
		// (get) Token: 0x06002AEF RID: 10991 RVA: 0x001A0A77 File Offset: 0x0019FA77
		object IEnumerator.Current
		{
			get
			{
				if (this._index != 0)
				{
					return null;
				}
				return this._child;
			}
		}

		// Token: 0x06002AF0 RID: 10992 RVA: 0x001A0A89 File Offset: 0x0019FA89
		bool IEnumerator.MoveNext()
		{
			this._index++;
			return this._index < this._count;
		}

		// Token: 0x06002AF1 RID: 10993 RVA: 0x001A0AA7 File Offset: 0x0019FAA7
		void IEnumerator.Reset()
		{
			this._index = -1;
		}

		// Token: 0x04001575 RID: 5493
		private int _index = -1;

		// Token: 0x04001576 RID: 5494
		private int _count;

		// Token: 0x04001577 RID: 5495
		private object _child;
	}
}
