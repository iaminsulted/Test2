using System;

namespace System.Windows.Controls.Primitives
{
	// Token: 0x0200083A RID: 2106
	public interface IContainItemStorage
	{
		// Token: 0x06007B89 RID: 31625
		void StoreItemValue(object item, DependencyProperty dp, object value);

		// Token: 0x06007B8A RID: 31626
		object ReadItemValue(object item, DependencyProperty dp);

		// Token: 0x06007B8B RID: 31627
		void ClearItemValue(object item, DependencyProperty dp);

		// Token: 0x06007B8C RID: 31628
		void ClearValue(DependencyProperty dp);

		// Token: 0x06007B8D RID: 31629
		void Clear();
	}
}
