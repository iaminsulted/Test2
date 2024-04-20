using System;

namespace System.Windows.Markup
{
	// Token: 0x0200048A RID: 1162
	internal interface IOptimizedMarkupExtension
	{
		// Token: 0x17000D17 RID: 3351
		// (get) Token: 0x06003C54 RID: 15444
		short ExtensionTypeId { get; }

		// Token: 0x17000D18 RID: 3352
		// (get) Token: 0x06003C55 RID: 15445
		short ValueId { get; }

		// Token: 0x17000D19 RID: 3353
		// (get) Token: 0x06003C56 RID: 15446
		bool IsValueTypeExtension { get; }

		// Token: 0x17000D1A RID: 3354
		// (get) Token: 0x06003C57 RID: 15447
		bool IsValueStaticExtension { get; }
	}
}
