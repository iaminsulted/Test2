using System;

namespace System.Windows.Markup
{
	// Token: 0x020004C9 RID: 1225
	internal interface IParserHelper
	{
		// Token: 0x06003EAA RID: 16042
		string LookupNamespace(string prefix);

		// Token: 0x06003EAB RID: 16043
		bool GetElementType(bool extensionFirst, string localName, string namespaceURI, ref string assemblyName, ref string typeFullName, ref Type baseType, ref Type serializerType);

		// Token: 0x06003EAC RID: 16044
		bool CanResolveLocalAssemblies();
	}
}
