using System;

namespace System.Windows.Controls.Primitives
{
	// Token: 0x02000840 RID: 2112
	public interface IRecyclingItemContainerGenerator : IItemContainerGenerator
	{
		// Token: 0x06007BAC RID: 31660
		void Recycle(GeneratorPosition position, int count);
	}
}
