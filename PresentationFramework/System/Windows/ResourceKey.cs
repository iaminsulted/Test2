using System;
using System.Reflection;
using System.Windows.Markup;

namespace System.Windows
{
	// Token: 0x02000390 RID: 912
	[MarkupExtensionReturnType(typeof(ResourceKey))]
	public abstract class ResourceKey : MarkupExtension
	{
		// Token: 0x1700075B RID: 1883
		// (get) Token: 0x060024F9 RID: 9465
		public abstract Assembly Assembly { get; }

		// Token: 0x060024FA RID: 9466 RVA: 0x000F93D3 File Offset: 0x000F83D3
		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			return this;
		}
	}
}
