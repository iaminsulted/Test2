using System;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Markup;

namespace System.Windows
{
	// Token: 0x020003C3 RID: 963
	[MarkupExtensionReturnType(typeof(object))]
	[TypeConverter(typeof(TemplateBindingExtensionConverter))]
	public class TemplateBindingExtension : MarkupExtension
	{
		// Token: 0x06002882 RID: 10370 RVA: 0x00173A19 File Offset: 0x00172A19
		public TemplateBindingExtension()
		{
		}

		// Token: 0x06002883 RID: 10371 RVA: 0x001955F2 File Offset: 0x001945F2
		public TemplateBindingExtension(DependencyProperty property)
		{
			if (property != null)
			{
				this._property = property;
				return;
			}
			throw new ArgumentNullException("property");
		}

		// Token: 0x06002884 RID: 10372 RVA: 0x0019560F File Offset: 0x0019460F
		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			if (this.Property == null)
			{
				throw new InvalidOperationException(SR.Get("MarkupExtensionProperty"));
			}
			return new TemplateBindingExpression(this);
		}

		// Token: 0x1700097A RID: 2426
		// (get) Token: 0x06002885 RID: 10373 RVA: 0x0019562F File Offset: 0x0019462F
		// (set) Token: 0x06002886 RID: 10374 RVA: 0x00195637 File Offset: 0x00194637
		[ConstructorArgument("property")]
		public DependencyProperty Property
		{
			get
			{
				return this._property;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this._property = value;
			}
		}

		// Token: 0x1700097B RID: 2427
		// (get) Token: 0x06002887 RID: 10375 RVA: 0x0019564E File Offset: 0x0019464E
		// (set) Token: 0x06002888 RID: 10376 RVA: 0x00195656 File Offset: 0x00194656
		[DefaultValue(null)]
		public IValueConverter Converter
		{
			get
			{
				return this._converter;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this._converter = value;
			}
		}

		// Token: 0x1700097C RID: 2428
		// (get) Token: 0x06002889 RID: 10377 RVA: 0x0019566D File Offset: 0x0019466D
		// (set) Token: 0x0600288A RID: 10378 RVA: 0x00195675 File Offset: 0x00194675
		[DefaultValue(null)]
		public object ConverterParameter
		{
			get
			{
				return this._parameter;
			}
			set
			{
				this._parameter = value;
			}
		}

		// Token: 0x0400148D RID: 5261
		private DependencyProperty _property;

		// Token: 0x0400148E RID: 5262
		private IValueConverter _converter;

		// Token: 0x0400148F RID: 5263
		private object _parameter;
	}
}
