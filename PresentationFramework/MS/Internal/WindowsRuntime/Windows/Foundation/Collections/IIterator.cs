using System;
using System.Runtime.InteropServices;

namespace MS.Internal.WindowsRuntime.Windows.Foundation.Collections
{
	// Token: 0x02000305 RID: 773
	[Guid("6A79E863-4300-459A-9966-CBB660963EE1")]
	internal interface IIterator<T>
	{
		// Token: 0x06001CF0 RID: 7408
		bool _MoveNext();

		// Token: 0x06001CF1 RID: 7409
		uint GetMany(ref T[] items);

		// Token: 0x17000548 RID: 1352
		// (get) Token: 0x06001CF2 RID: 7410
		T _Current { get; }

		// Token: 0x17000549 RID: 1353
		// (get) Token: 0x06001CF3 RID: 7411
		bool HasCurrent { get; }
	}
}
