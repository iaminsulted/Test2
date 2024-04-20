using System;
using System.ComponentModel;
using System.Globalization;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x020005EC RID: 1516
	internal class DPTypeDescriptorContext : ITypeDescriptorContext, IServiceProvider
	{
		// Token: 0x060049AB RID: 18859 RVA: 0x00230FB8 File Offset: 0x0022FFB8
		private DPTypeDescriptorContext(DependencyProperty property, object propertyValue)
		{
			Invariant.Assert(property != null, "property == null");
			Invariant.Assert(propertyValue != null, "propertyValue == null");
			Invariant.Assert(property.IsValidValue(propertyValue), "propertyValue must be of suitable type for the given dependency property");
			this._property = property;
			this._propertyValue = propertyValue;
		}

		// Token: 0x060049AC RID: 18860 RVA: 0x00231008 File Offset: 0x00230008
		internal static string GetStringValue(DependencyProperty property, object propertyValue)
		{
			string text = null;
			if (property == UIElement.BitmapEffectProperty)
			{
				return null;
			}
			if (property == Inline.TextDecorationsProperty)
			{
				text = DPTypeDescriptorContext.TextDecorationsFixup((TextDecorationCollection)propertyValue);
			}
			else if (typeof(CultureInfo).IsAssignableFrom(property.PropertyType))
			{
				text = DPTypeDescriptorContext.CultureInfoFixup(property, (CultureInfo)propertyValue);
			}
			if (text == null)
			{
				DPTypeDescriptorContext context = new DPTypeDescriptorContext(property, propertyValue);
				TypeConverter converter = TypeDescriptor.GetConverter(property.PropertyType);
				Invariant.Assert(converter != null);
				if (converter.CanConvertTo(context, typeof(string)))
				{
					text = (string)converter.ConvertTo(context, CultureInfo.InvariantCulture, propertyValue, typeof(string));
				}
			}
			return text;
		}

		// Token: 0x060049AD RID: 18861 RVA: 0x002310AC File Offset: 0x002300AC
		private static string TextDecorationsFixup(TextDecorationCollection textDecorations)
		{
			string result = null;
			if (TextDecorations.Underline.ValueEquals(textDecorations))
			{
				result = "Underline";
			}
			else if (TextDecorations.Strikethrough.ValueEquals(textDecorations))
			{
				result = "Strikethrough";
			}
			else if (TextDecorations.OverLine.ValueEquals(textDecorations))
			{
				result = "OverLine";
			}
			else if (TextDecorations.Baseline.ValueEquals(textDecorations))
			{
				result = "Baseline";
			}
			else if (textDecorations.Count == 0)
			{
				result = string.Empty;
			}
			return result;
		}

		// Token: 0x060049AE RID: 18862 RVA: 0x00231120 File Offset: 0x00230120
		private static string CultureInfoFixup(DependencyProperty property, CultureInfo cultureInfo)
		{
			string result = null;
			DPTypeDescriptorContext context = new DPTypeDescriptorContext(property, cultureInfo);
			TypeConverter typeConverter = new CultureInfoIetfLanguageTagConverter();
			if (typeConverter.CanConvertTo(context, typeof(string)))
			{
				result = (string)typeConverter.ConvertTo(context, CultureInfo.InvariantCulture, cultureInfo, typeof(string));
			}
			return result;
		}

		// Token: 0x1700108F RID: 4239
		// (get) Token: 0x060049AF RID: 18863 RVA: 0x00109403 File Offset: 0x00108403
		IContainer ITypeDescriptorContext.Container
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001090 RID: 4240
		// (get) Token: 0x060049B0 RID: 18864 RVA: 0x0023116E File Offset: 0x0023016E
		object ITypeDescriptorContext.Instance
		{
			get
			{
				return this._propertyValue;
			}
		}

		// Token: 0x060049B1 RID: 18865 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		void ITypeDescriptorContext.OnComponentChanged()
		{
		}

		// Token: 0x060049B2 RID: 18866 RVA: 0x00105F35 File Offset: 0x00104F35
		bool ITypeDescriptorContext.OnComponentChanging()
		{
			return false;
		}

		// Token: 0x17001091 RID: 4241
		// (get) Token: 0x060049B3 RID: 18867 RVA: 0x00109403 File Offset: 0x00108403
		PropertyDescriptor ITypeDescriptorContext.PropertyDescriptor
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060049B4 RID: 18868 RVA: 0x00109403 File Offset: 0x00108403
		object IServiceProvider.GetService(Type serviceType)
		{
			return null;
		}

		// Token: 0x04002678 RID: 9848
		private DependencyProperty _property;

		// Token: 0x04002679 RID: 9849
		private object _propertyValue;
	}
}
