using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace MS.Internal.Controls
{
	// Token: 0x02000256 RID: 598
	internal interface IGeneratorHost
	{
		// Token: 0x17000474 RID: 1140
		// (get) Token: 0x06001718 RID: 5912
		ItemCollection View { get; }

		// Token: 0x06001719 RID: 5913
		bool IsItemItsOwnContainer(object item);

		// Token: 0x0600171A RID: 5914
		DependencyObject GetContainerForItem(object item);

		// Token: 0x0600171B RID: 5915
		void PrepareItemContainer(DependencyObject container, object item);

		// Token: 0x0600171C RID: 5916
		void ClearContainerForItem(DependencyObject container, object item);

		// Token: 0x0600171D RID: 5917
		bool IsHostForItemContainer(DependencyObject container);

		// Token: 0x0600171E RID: 5918
		GroupStyle GetGroupStyle(CollectionViewGroup group, int level);

		// Token: 0x0600171F RID: 5919
		void SetIsGrouping(bool isGrouping);

		// Token: 0x17000475 RID: 1141
		// (get) Token: 0x06001720 RID: 5920
		int AlternationCount { get; }
	}
}
