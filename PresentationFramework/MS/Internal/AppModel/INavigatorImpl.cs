using System;
using System.Windows.Media;

namespace MS.Internal.AppModel
{
	// Token: 0x0200027D RID: 637
	internal interface INavigatorImpl
	{
		// Token: 0x06001859 RID: 6233
		void OnSourceUpdatedFromNavService(bool journalOrCancel);

		// Token: 0x0600185A RID: 6234
		Visual FindRootViewer();
	}
}
