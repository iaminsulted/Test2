using System;
using System.Collections;

namespace MS.Internal
{
	// Token: 0x020000F8 RID: 248
	internal interface IWeakHashtable
	{
		// Token: 0x170000E2 RID: 226
		object this[object key]
		{
			get;
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x060005BF RID: 1471
		ICollection Keys { get; }

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x060005C0 RID: 1472
		int Count { get; }

		// Token: 0x060005C1 RID: 1473
		bool ContainsKey(object key);

		// Token: 0x060005C2 RID: 1474
		void Remove(object key);

		// Token: 0x060005C3 RID: 1475
		void Clear();

		// Token: 0x060005C4 RID: 1476
		void SetWeak(object key, object value);

		// Token: 0x060005C5 RID: 1477
		object UnwrapKey(object key);
	}
}
