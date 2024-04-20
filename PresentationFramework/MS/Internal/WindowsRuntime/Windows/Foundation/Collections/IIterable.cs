using System;
using System.Runtime.InteropServices;

namespace MS.Internal.WindowsRuntime.Windows.Foundation.Collections
{
	// Token: 0x02000304 RID: 772
	[Guid("FAA585EA-6214-4217-AFDA-7F46DE5869B3")]
	internal interface IIterable<T>
	{
		// Token: 0x06001CEF RID: 7407
		IIterator<T> First();
	}
}
