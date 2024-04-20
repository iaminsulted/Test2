using System;

namespace System.Windows.Markup
{
	// Token: 0x02000502 RID: 1282
	internal class XamlDefAttributeKeyTypeNode : XamlAttributeNode
	{
		// Token: 0x06003FF7 RID: 16375 RVA: 0x00212968 File Offset: 0x00211968
		internal XamlDefAttributeKeyTypeNode(int lineNumber, int linePosition, int depth, string value, string assemblyName, Type valueType) : base(XamlNodeType.DefKeyTypeAttribute, lineNumber, linePosition, depth, value)
		{
			this._assemblyName = assemblyName;
			this._valueType = valueType;
		}

		// Token: 0x17000E44 RID: 3652
		// (get) Token: 0x06003FF8 RID: 16376 RVA: 0x00212987 File Offset: 0x00211987
		internal string AssemblyName
		{
			get
			{
				return this._assemblyName;
			}
		}

		// Token: 0x17000E45 RID: 3653
		// (get) Token: 0x06003FF9 RID: 16377 RVA: 0x0021298F File Offset: 0x0021198F
		internal Type ValueType
		{
			get
			{
				return this._valueType;
			}
		}

		// Token: 0x040023EA RID: 9194
		private string _assemblyName;

		// Token: 0x040023EB RID: 9195
		private Type _valueType;
	}
}
