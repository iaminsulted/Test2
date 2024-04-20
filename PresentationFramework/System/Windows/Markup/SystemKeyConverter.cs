using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace System.Windows.Markup
{
	// Token: 0x02000525 RID: 1317
	internal class SystemKeyConverter : TypeConverter
	{
		// Token: 0x0600417F RID: 16767 RVA: 0x0021853C File Offset: 0x0021753C
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType == null)
			{
				throw new ArgumentNullException("sourceType");
			}
			return base.CanConvertFrom(context, sourceType);
		}

		// Token: 0x06004180 RID: 16768 RVA: 0x0021855A File Offset: 0x0021755A
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			return (destinationType == typeof(MarkupExtension) && context is IValueSerializerContext) || base.CanConvertTo(context, destinationType);
		}

		// Token: 0x06004181 RID: 16769 RVA: 0x00218594 File Offset: 0x00217594
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			return base.ConvertFrom(context, culture, value);
		}

		// Token: 0x06004182 RID: 16770 RVA: 0x002185A0 File Offset: 0x002175A0
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (destinationType == typeof(MarkupExtension) && this.CanConvertTo(context, destinationType))
			{
				SystemResourceKeyID internalKey;
				if (value is SystemResourceKey)
				{
					internalKey = (value as SystemResourceKey).InternalKey;
				}
				else
				{
					if (!(value is SystemThemeKey))
					{
						throw new ArgumentException(SR.Get("MustBeOfType", new object[]
						{
							"value",
							"SystemResourceKey or SystemThemeKey"
						}));
					}
					internalKey = (value as SystemThemeKey).InternalKey;
				}
				Type systemClassType = SystemKeyConverter.GetSystemClassType(internalKey);
				IValueSerializerContext valueSerializerContext = context as IValueSerializerContext;
				if (valueSerializerContext != null)
				{
					ValueSerializer valueSerializerFor = valueSerializerContext.GetValueSerializerFor(typeof(Type));
					if (valueSerializerFor != null)
					{
						return new StaticExtension(valueSerializerFor.ConvertToString(systemClassType, valueSerializerContext) + "." + SystemKeyConverter.GetSystemKeyName(internalKey));
					}
				}
			}
			return base.CanConvertTo(context, destinationType);
		}

		// Token: 0x06004183 RID: 16771 RVA: 0x0021868C File Offset: 0x0021768C
		internal static Type GetSystemClassType(SystemResourceKeyID id)
		{
			if ((SystemResourceKeyID.InternalSystemColorsStart < id && id < SystemResourceKeyID.InternalSystemColorsEnd) || (SystemResourceKeyID.InternalSystemColorsExtendedStart < id && id < SystemResourceKeyID.InternalSystemColorsExtendedEnd))
			{
				return typeof(SystemColors);
			}
			if (SystemResourceKeyID.InternalSystemFontsStart < id && id < SystemResourceKeyID.InternalSystemFontsEnd)
			{
				return typeof(SystemFonts);
			}
			if (SystemResourceKeyID.InternalSystemParametersStart < id && id < SystemResourceKeyID.InternalSystemParametersEnd)
			{
				return typeof(SystemParameters);
			}
			if (SystemResourceKeyID.MenuItemSeparatorStyle == id)
			{
				return typeof(MenuItem);
			}
			if (SystemResourceKeyID.ToolBarButtonStyle <= id && id <= SystemResourceKeyID.ToolBarMenuStyle)
			{
				return typeof(ToolBar);
			}
			if (SystemResourceKeyID.StatusBarSeparatorStyle == id)
			{
				return typeof(StatusBar);
			}
			if (SystemResourceKeyID.GridViewScrollViewerStyle <= id && id <= SystemResourceKeyID.GridViewItemContainerStyle)
			{
				return typeof(GridView);
			}
			return null;
		}

		// Token: 0x06004184 RID: 16772 RVA: 0x00218748 File Offset: 0x00217748
		internal static string GetSystemClassName(SystemResourceKeyID id)
		{
			if ((SystemResourceKeyID.InternalSystemColorsStart < id && id < SystemResourceKeyID.InternalSystemColorsEnd) || (SystemResourceKeyID.InternalSystemColorsExtendedStart < id && id < SystemResourceKeyID.InternalSystemColorsExtendedEnd))
			{
				return "SystemColors";
			}
			if (SystemResourceKeyID.InternalSystemFontsStart < id && id < SystemResourceKeyID.InternalSystemFontsEnd)
			{
				return "SystemFonts";
			}
			if (SystemResourceKeyID.InternalSystemParametersStart < id && id < SystemResourceKeyID.InternalSystemParametersEnd)
			{
				return "SystemParameters";
			}
			if (SystemResourceKeyID.MenuItemSeparatorStyle == id)
			{
				return "MenuItem";
			}
			if (SystemResourceKeyID.ToolBarButtonStyle <= id && id <= SystemResourceKeyID.ToolBarMenuStyle)
			{
				return "ToolBar";
			}
			if (SystemResourceKeyID.StatusBarSeparatorStyle == id)
			{
				return "StatusBar";
			}
			if (SystemResourceKeyID.GridViewScrollViewerStyle <= id && id <= SystemResourceKeyID.GridViewItemContainerStyle)
			{
				return "GridView";
			}
			return string.Empty;
		}

		// Token: 0x06004185 RID: 16773 RVA: 0x002187E4 File Offset: 0x002177E4
		internal static string GetSystemKeyName(SystemResourceKeyID id)
		{
			if ((SystemResourceKeyID.InternalSystemColorsStart < id && id < SystemResourceKeyID.InternalSystemParametersEnd) || (SystemResourceKeyID.InternalSystemColorsExtendedStart < id && id < SystemResourceKeyID.InternalSystemColorsExtendedEnd) || (SystemResourceKeyID.GridViewScrollViewerStyle <= id && id <= SystemResourceKeyID.GridViewItemContainerStyle))
			{
				return Enum.GetName(typeof(SystemResourceKeyID), id) + "Key";
			}
			if (SystemResourceKeyID.MenuItemSeparatorStyle == id || SystemResourceKeyID.StatusBarSeparatorStyle == id)
			{
				return "SeparatorStyleKey";
			}
			if (SystemResourceKeyID.ToolBarButtonStyle <= id && id <= SystemResourceKeyID.ToolBarMenuStyle)
			{
				return (Enum.GetName(typeof(SystemResourceKeyID), id) + "Key").Remove(0, 7);
			}
			return string.Empty;
		}

		// Token: 0x06004186 RID: 16774 RVA: 0x0021888F File Offset: 0x0021788F
		internal static string GetSystemPropertyName(SystemResourceKeyID id)
		{
			if (SystemResourceKeyID.InternalSystemColorsStart < id && id < SystemResourceKeyID.InternalSystemColorsExtendedEnd)
			{
				return Enum.GetName(typeof(SystemResourceKeyID), id);
			}
			return string.Empty;
		}
	}
}
