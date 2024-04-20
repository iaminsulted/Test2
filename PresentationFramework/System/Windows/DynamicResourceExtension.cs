using System;
using System.ComponentModel;
using System.Windows.Markup;
using MS.Internal;

namespace System.Windows
{
	// Token: 0x0200035C RID: 860
	[TypeConverter(typeof(DynamicResourceExtensionConverter))]
	[MarkupExtensionReturnType(typeof(object))]
	public class DynamicResourceExtension : MarkupExtension
	{
		// Token: 0x06002079 RID: 8313 RVA: 0x00173A19 File Offset: 0x00172A19
		public DynamicResourceExtension()
		{
		}

		// Token: 0x0600207A RID: 8314 RVA: 0x00175846 File Offset: 0x00174846
		public DynamicResourceExtension(object resourceKey)
		{
			if (resourceKey == null)
			{
				throw new ArgumentNullException("resourceKey");
			}
			this._resourceKey = resourceKey;
		}

		// Token: 0x0600207B RID: 8315 RVA: 0x00175864 File Offset: 0x00174864
		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			if (this.ResourceKey == null)
			{
				throw new InvalidOperationException(SR.Get("MarkupExtensionResourceKey"));
			}
			if (serviceProvider != null)
			{
				DependencyObject dependencyObject;
				DependencyProperty dependencyProperty;
				Helper.CheckCanReceiveMarkupExtension(this, serviceProvider, out dependencyObject, out dependencyProperty);
			}
			return new ResourceReferenceExpression(this.ResourceKey);
		}

		// Token: 0x17000635 RID: 1589
		// (get) Token: 0x0600207C RID: 8316 RVA: 0x001758A2 File Offset: 0x001748A2
		// (set) Token: 0x0600207D RID: 8317 RVA: 0x001758AA File Offset: 0x001748AA
		[ConstructorArgument("resourceKey")]
		public object ResourceKey
		{
			get
			{
				return this._resourceKey;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this._resourceKey = value;
			}
		}

		// Token: 0x04000FE4 RID: 4068
		private object _resourceKey;
	}
}
