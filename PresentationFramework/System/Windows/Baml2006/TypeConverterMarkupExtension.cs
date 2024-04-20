using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Markup;

namespace System.Windows.Baml2006
{
	// Token: 0x02000413 RID: 1043
	internal class TypeConverterMarkupExtension : MarkupExtension
	{
		// Token: 0x06002D4C RID: 11596 RVA: 0x001ABA20 File Offset: 0x001AAA20
		public TypeConverterMarkupExtension(TypeConverter converter, object value)
		{
			this._converter = converter;
			this._value = value;
		}

		// Token: 0x06002D4D RID: 11597 RVA: 0x001ABA36 File Offset: 0x001AAA36
		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			return this._converter.ConvertFrom(new TypeConverterMarkupExtension.TypeConverterContext(serviceProvider), CultureInfo.InvariantCulture, this._value);
		}

		// Token: 0x04001BB4 RID: 7092
		private TypeConverter _converter;

		// Token: 0x04001BB5 RID: 7093
		private object _value;

		// Token: 0x02000AB2 RID: 2738
		private class TypeConverterContext : ITypeDescriptorContext, IServiceProvider
		{
			// Token: 0x06008757 RID: 34647 RVA: 0x00336127 File Offset: 0x00335127
			public TypeConverterContext(IServiceProvider serviceProvider)
			{
				this._serviceProvider = serviceProvider;
			}

			// Token: 0x06008758 RID: 34648 RVA: 0x00336136 File Offset: 0x00335136
			object IServiceProvider.GetService(Type serviceType)
			{
				return this._serviceProvider.GetService(serviceType);
			}

			// Token: 0x06008759 RID: 34649 RVA: 0x000F6B2C File Offset: 0x000F5B2C
			void ITypeDescriptorContext.OnComponentChanged()
			{
			}

			// Token: 0x0600875A RID: 34650 RVA: 0x00105F35 File Offset: 0x00104F35
			bool ITypeDescriptorContext.OnComponentChanging()
			{
				return false;
			}

			// Token: 0x17001E5C RID: 7772
			// (get) Token: 0x0600875B RID: 34651 RVA: 0x00109403 File Offset: 0x00108403
			IContainer ITypeDescriptorContext.Container
			{
				get
				{
					return null;
				}
			}

			// Token: 0x17001E5D RID: 7773
			// (get) Token: 0x0600875C RID: 34652 RVA: 0x00109403 File Offset: 0x00108403
			object ITypeDescriptorContext.Instance
			{
				get
				{
					return null;
				}
			}

			// Token: 0x17001E5E RID: 7774
			// (get) Token: 0x0600875D RID: 34653 RVA: 0x00109403 File Offset: 0x00108403
			PropertyDescriptor ITypeDescriptorContext.PropertyDescriptor
			{
				get
				{
					return null;
				}
			}

			// Token: 0x040042CC RID: 17100
			private IServiceProvider _serviceProvider;
		}
	}
}
