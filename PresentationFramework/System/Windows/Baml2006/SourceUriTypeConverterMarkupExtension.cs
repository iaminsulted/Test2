using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Navigation;

namespace System.Windows.Baml2006
{
	// Token: 0x0200041E RID: 1054
	internal class SourceUriTypeConverterMarkupExtension : TypeConverterMarkupExtension
	{
		// Token: 0x060032A4 RID: 12964 RVA: 0x001D26E0 File Offset: 0x001D16E0
		public SourceUriTypeConverterMarkupExtension(TypeConverter converter, object value, Assembly assemblyInfo) : base(converter, value)
		{
			this._assemblyInfo = assemblyInfo;
		}

		// Token: 0x060032A5 RID: 12965 RVA: 0x001D26F4 File Offset: 0x001D16F4
		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			object obj = base.ProvideValue(serviceProvider);
			Uri uri = obj as Uri;
			if (uri != null)
			{
				Uri uri2 = BaseUriHelper.AppendAssemblyVersion(uri, this._assemblyInfo);
				if (uri2 != null)
				{
					return new ResourceDictionary.ResourceDictionarySourceUriWrapper(uri, uri2);
				}
			}
			return obj;
		}

		// Token: 0x04001BFA RID: 7162
		private Assembly _assemblyInfo;
	}
}
