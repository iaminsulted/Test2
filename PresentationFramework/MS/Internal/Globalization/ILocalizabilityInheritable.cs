using System;
using System.Windows;

namespace MS.Internal.Globalization
{
	// Token: 0x0200019D RID: 413
	internal interface ILocalizabilityInheritable
	{
		// Token: 0x1700023C RID: 572
		// (get) Token: 0x06000DAF RID: 3503
		ILocalizabilityInheritable LocalizabilityAncestor { get; }

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x06000DB0 RID: 3504
		// (set) Token: 0x06000DB1 RID: 3505
		LocalizabilityAttribute InheritableAttribute { get; set; }

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x06000DB2 RID: 3506
		// (set) Token: 0x06000DB3 RID: 3507
		bool IsIgnored { get; set; }
	}
}
