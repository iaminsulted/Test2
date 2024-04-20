using System;

namespace System.Windows.Markup
{
	// Token: 0x0200051B RID: 1307
	internal static class XmlParserDefaults
	{
		// Token: 0x17000E7E RID: 3710
		// (get) Token: 0x0600412D RID: 16685 RVA: 0x00217402 File Offset: 0x00216402
		internal static XamlTypeMapper DefaultMapper
		{
			get
			{
				return new XamlTypeMapper(XmlParserDefaults.GetDefaultAssemblyNames(), XmlParserDefaults.GetDefaultNamespaceMaps());
			}
		}

		// Token: 0x0600412E RID: 16686 RVA: 0x00217413 File Offset: 0x00216413
		internal static string[] GetDefaultAssemblyNames()
		{
			return (string[])XmlParserDefaults._defaultAssemblies.Clone();
		}

		// Token: 0x0600412F RID: 16687 RVA: 0x00217424 File Offset: 0x00216424
		internal static NamespaceMapEntry[] GetDefaultNamespaceMaps()
		{
			return (NamespaceMapEntry[])XmlParserDefaults._defaultNamespaceMapTable.Clone();
		}

		// Token: 0x040024A8 RID: 9384
		private static readonly string[] _defaultAssemblies = new string[]
		{
			"WindowsBase",
			"PresentationCore",
			"PresentationFramework"
		};

		// Token: 0x040024A9 RID: 9385
		private static readonly NamespaceMapEntry[] _defaultNamespaceMapTable = new NamespaceMapEntry[0];
	}
}
