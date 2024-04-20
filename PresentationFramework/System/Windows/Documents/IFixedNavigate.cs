using System;

namespace System.Windows.Documents
{
	// Token: 0x020005F9 RID: 1529
	internal interface IFixedNavigate
	{
		// Token: 0x06004A99 RID: 19097
		UIElement FindElementByID(string elementID, out FixedPage rootFixedPage);

		// Token: 0x06004A9A RID: 19098
		void NavigateAsync(string elementID);
	}
}
