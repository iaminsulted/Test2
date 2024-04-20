using System;
using System.Runtime.InteropServices;

namespace MS.Internal.WindowsRuntime.Windows.Foundation.Collections
{
	// Token: 0x02000306 RID: 774
	[Guid("BBE1FA4C-B0E3-4583-BAEF-1F1B2E483E56")]
	internal interface IVectorView<T> : IIterable<T>
	{
		// Token: 0x06001CF4 RID: 7412
		T GetAt(uint index);

		// Token: 0x06001CF5 RID: 7413
		bool IndexOf(T value, out uint index);

		// Token: 0x06001CF6 RID: 7414
		uint GetMany(uint startIndex, ref T[] items);

		// Token: 0x1700054A RID: 1354
		// (get) Token: 0x06001CF7 RID: 7415
		uint Size { get; }
	}
}
