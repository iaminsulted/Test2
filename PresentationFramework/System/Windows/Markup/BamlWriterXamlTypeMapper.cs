using System;

namespace System.Windows.Markup
{
	// Token: 0x020004BF RID: 1215
	internal class BamlWriterXamlTypeMapper : XamlTypeMapper
	{
		// Token: 0x06003E7A RID: 15994 RVA: 0x00200F13 File Offset: 0x001FFF13
		internal BamlWriterXamlTypeMapper(string[] assemblyNames, NamespaceMapEntry[] namespaceMaps) : base(assemblyNames, namespaceMaps)
		{
		}

		// Token: 0x06003E7B RID: 15995 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		protected sealed override bool AllowInternalType(Type type)
		{
			return true;
		}
	}
}
