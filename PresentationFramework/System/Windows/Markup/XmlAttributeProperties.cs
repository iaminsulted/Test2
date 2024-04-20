using System;
using System.Collections;
using System.ComponentModel;
using System.Reflection;

namespace System.Windows.Markup
{
	// Token: 0x02000520 RID: 1312
	public sealed class XmlAttributeProperties
	{
		// Token: 0x0600413A RID: 16698 RVA: 0x000F7BD9 File Offset: 0x000F6BD9
		private XmlAttributeProperties()
		{
		}

		// Token: 0x0600413B RID: 16699 RVA: 0x002175A4 File Offset: 0x002165A4
		static XmlAttributeProperties()
		{
			XmlAttributeProperties.XmlSpaceProperty = DependencyProperty.RegisterAttached("XmlSpace", typeof(string), typeof(XmlAttributeProperties), new FrameworkPropertyMetadata("default"));
			XmlAttributeProperties.XmlnsDictionaryProperty = DependencyProperty.RegisterAttached("XmlnsDictionary", typeof(XmlnsDictionary), typeof(XmlAttributeProperties), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));
			XmlAttributeProperties.XmlnsDefinitionProperty = DependencyProperty.RegisterAttached("XmlnsDefinition", typeof(string), typeof(XmlAttributeProperties), new FrameworkPropertyMetadata("http://schemas.microsoft.com/winfx/2006/xaml", FrameworkPropertyMetadataOptions.Inherits));
			XmlAttributeProperties.XmlNamespaceMapsProperty = DependencyProperty.RegisterAttached("XmlNamespaceMaps", typeof(Hashtable), typeof(XmlAttributeProperties), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));
		}

		// Token: 0x0600413C RID: 16700 RVA: 0x00217687 File Offset: 0x00216687
		[DesignerSerializationOptions(DesignerSerializationOptions.SerializeAsAttribute)]
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static string GetXmlSpace(DependencyObject dependencyObject)
		{
			if (dependencyObject == null)
			{
				throw new ArgumentNullException("dependencyObject");
			}
			return (string)dependencyObject.GetValue(XmlAttributeProperties.XmlSpaceProperty);
		}

		// Token: 0x0600413D RID: 16701 RVA: 0x002176A7 File Offset: 0x002166A7
		public static void SetXmlSpace(DependencyObject dependencyObject, string value)
		{
			if (dependencyObject == null)
			{
				throw new ArgumentNullException("dependencyObject");
			}
			dependencyObject.SetValue(XmlAttributeProperties.XmlSpaceProperty, value);
		}

		// Token: 0x0600413E RID: 16702 RVA: 0x002176C3 File Offset: 0x002166C3
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public static XmlnsDictionary GetXmlnsDictionary(DependencyObject dependencyObject)
		{
			if (dependencyObject == null)
			{
				throw new ArgumentNullException("dependencyObject");
			}
			return (XmlnsDictionary)dependencyObject.GetValue(XmlAttributeProperties.XmlnsDictionaryProperty);
		}

		// Token: 0x0600413F RID: 16703 RVA: 0x002176E3 File Offset: 0x002166E3
		public static void SetXmlnsDictionary(DependencyObject dependencyObject, XmlnsDictionary value)
		{
			if (dependencyObject == null)
			{
				throw new ArgumentNullException("dependencyObject");
			}
			if (!dependencyObject.IsSealed)
			{
				dependencyObject.SetValue(XmlAttributeProperties.XmlnsDictionaryProperty, value);
			}
		}

		// Token: 0x06004140 RID: 16704 RVA: 0x00217707 File Offset: 0x00216707
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DesignerSerializationOptions(DesignerSerializationOptions.SerializeAsAttribute)]
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static string GetXmlnsDefinition(DependencyObject dependencyObject)
		{
			if (dependencyObject == null)
			{
				throw new ArgumentNullException("dependencyObject");
			}
			return (string)dependencyObject.GetValue(XmlAttributeProperties.XmlnsDefinitionProperty);
		}

		// Token: 0x06004141 RID: 16705 RVA: 0x00217727 File Offset: 0x00216727
		public static void SetXmlnsDefinition(DependencyObject dependencyObject, string value)
		{
			if (dependencyObject == null)
			{
				throw new ArgumentNullException("dependencyObject");
			}
			dependencyObject.SetValue(XmlAttributeProperties.XmlnsDefinitionProperty, value);
		}

		// Token: 0x06004142 RID: 16706 RVA: 0x00217743 File Offset: 0x00216743
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static string GetXmlNamespaceMaps(DependencyObject dependencyObject)
		{
			if (dependencyObject == null)
			{
				throw new ArgumentNullException("dependencyObject");
			}
			return (string)dependencyObject.GetValue(XmlAttributeProperties.XmlNamespaceMapsProperty);
		}

		// Token: 0x06004143 RID: 16707 RVA: 0x00217763 File Offset: 0x00216763
		public static void SetXmlNamespaceMaps(DependencyObject dependencyObject, string value)
		{
			if (dependencyObject == null)
			{
				throw new ArgumentNullException("dependencyObject");
			}
			dependencyObject.SetValue(XmlAttributeProperties.XmlNamespaceMapsProperty, value);
		}

		// Token: 0x17000E7F RID: 3711
		// (get) Token: 0x06004144 RID: 16708 RVA: 0x0021777F File Offset: 0x0021677F
		internal static MethodInfo XmlSpaceSetter
		{
			get
			{
				if (XmlAttributeProperties._xmlSpaceSetter == null)
				{
					XmlAttributeProperties._xmlSpaceSetter = typeof(XmlAttributeProperties).GetMethod("SetXmlSpace", BindingFlags.Static | BindingFlags.Public);
				}
				return XmlAttributeProperties._xmlSpaceSetter;
			}
		}

		// Token: 0x040024B0 RID: 9392
		[Browsable(false)]
		[Localizability(LocalizationCategory.NeverLocalize)]
		public static readonly DependencyProperty XmlSpaceProperty;

		// Token: 0x040024B1 RID: 9393
		[Browsable(false)]
		public static readonly DependencyProperty XmlnsDictionaryProperty;

		// Token: 0x040024B2 RID: 9394
		[Browsable(false)]
		public static readonly DependencyProperty XmlnsDefinitionProperty;

		// Token: 0x040024B3 RID: 9395
		[Browsable(false)]
		public static readonly DependencyProperty XmlNamespaceMapsProperty;

		// Token: 0x040024B4 RID: 9396
		internal static readonly string XmlSpaceString = "xml:space";

		// Token: 0x040024B5 RID: 9397
		internal static readonly string XmlLangString = "xml:lang";

		// Token: 0x040024B6 RID: 9398
		internal static readonly string XmlnsDefinitionString = "xmlns";

		// Token: 0x040024B7 RID: 9399
		private static MethodInfo _xmlSpaceSetter = null;
	}
}
