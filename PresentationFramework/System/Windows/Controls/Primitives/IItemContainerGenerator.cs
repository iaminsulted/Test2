using System;

namespace System.Windows.Controls.Primitives
{
	// Token: 0x0200083C RID: 2108
	public interface IItemContainerGenerator
	{
		// Token: 0x06007B98 RID: 31640
		ItemContainerGenerator GetItemContainerGeneratorForPanel(Panel panel);

		// Token: 0x06007B99 RID: 31641
		IDisposable StartAt(GeneratorPosition position, GeneratorDirection direction);

		// Token: 0x06007B9A RID: 31642
		IDisposable StartAt(GeneratorPosition position, GeneratorDirection direction, bool allowStartAtRealizedItem);

		// Token: 0x06007B9B RID: 31643
		DependencyObject GenerateNext();

		// Token: 0x06007B9C RID: 31644
		DependencyObject GenerateNext(out bool isNewlyRealized);

		// Token: 0x06007B9D RID: 31645
		void PrepareItemContainer(DependencyObject container);

		// Token: 0x06007B9E RID: 31646
		void RemoveAll();

		// Token: 0x06007B9F RID: 31647
		void Remove(GeneratorPosition position, int count);

		// Token: 0x06007BA0 RID: 31648
		GeneratorPosition GeneratorPositionFromIndex(int itemIndex);

		// Token: 0x06007BA1 RID: 31649
		int IndexFromGeneratorPosition(GeneratorPosition position);
	}
}
