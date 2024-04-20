using System;
using System.Collections;

namespace System.Windows.Data
{
	// Token: 0x02000454 RID: 1108
	// (Invoke) Token: 0x060037E0 RID: 14304
	public delegate void CollectionSynchronizationCallback(IEnumerable collection, object context, Action accessMethod, bool writeAccess);
}
