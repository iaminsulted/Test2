using System;

namespace System.Windows.Markup
{
	// Token: 0x0200047F RID: 1151
	internal class StaticResourceHolder : StaticResourceExtension
	{
		// Token: 0x06003BF3 RID: 15347 RVA: 0x001FAC57 File Offset: 0x001F9C57
		internal StaticResourceHolder(object resourceKey, DeferredResourceReference prefetchedValue) : base(resourceKey)
		{
			this._prefetchedValue = prefetchedValue;
		}

		// Token: 0x17000CF4 RID: 3316
		// (get) Token: 0x06003BF4 RID: 15348 RVA: 0x001FAC67 File Offset: 0x001F9C67
		internal override DeferredResourceReference PrefetchedValue
		{
			get
			{
				return this._prefetchedValue;
			}
		}

		// Token: 0x04001E45 RID: 7749
		private DeferredResourceReference _prefetchedValue;
	}
}
