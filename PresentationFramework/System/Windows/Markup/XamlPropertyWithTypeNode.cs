using System;

namespace System.Windows.Markup
{
	// Token: 0x020004EB RID: 1259
	internal class XamlPropertyWithTypeNode : XamlPropertyBaseNode
	{
		// Token: 0x06003FBE RID: 16318 RVA: 0x0021257C File Offset: 0x0021157C
		internal XamlPropertyWithTypeNode(int lineNumber, int linePosition, int depth, object propertyMember, string assemblyName, string typeFullName, string propertyName, string valueTypeFullName, string valueAssemblyName, Type valueElementType, string valueSerializerTypeFullName, string valueSerializerTypeAssemblyName) : base(XamlNodeType.PropertyWithType, lineNumber, linePosition, depth, propertyMember, assemblyName, typeFullName, propertyName)
		{
			this._valueTypeFullname = valueTypeFullName;
			this._valueTypeAssemblyName = valueAssemblyName;
			this._valueElementType = valueElementType;
			this._valueSerializerTypeFullName = valueSerializerTypeFullName;
			this._valueSerializerTypeAssemblyName = valueSerializerTypeAssemblyName;
		}

		// Token: 0x17000E27 RID: 3623
		// (get) Token: 0x06003FBF RID: 16319 RVA: 0x002125C4 File Offset: 0x002115C4
		internal string ValueTypeFullName
		{
			get
			{
				return this._valueTypeFullname;
			}
		}

		// Token: 0x17000E28 RID: 3624
		// (get) Token: 0x06003FC0 RID: 16320 RVA: 0x002125CC File Offset: 0x002115CC
		internal string ValueTypeAssemblyName
		{
			get
			{
				return this._valueTypeAssemblyName;
			}
		}

		// Token: 0x17000E29 RID: 3625
		// (get) Token: 0x06003FC1 RID: 16321 RVA: 0x002125D4 File Offset: 0x002115D4
		internal Type ValueElementType
		{
			get
			{
				return this._valueElementType;
			}
		}

		// Token: 0x17000E2A RID: 3626
		// (get) Token: 0x06003FC2 RID: 16322 RVA: 0x002125DC File Offset: 0x002115DC
		internal string ValueSerializerTypeFullName
		{
			get
			{
				return this._valueSerializerTypeFullName;
			}
		}

		// Token: 0x17000E2B RID: 3627
		// (get) Token: 0x06003FC3 RID: 16323 RVA: 0x002125E4 File Offset: 0x002115E4
		internal string ValueSerializerTypeAssemblyName
		{
			get
			{
				return this._valueSerializerTypeAssemblyName;
			}
		}

		// Token: 0x040023C9 RID: 9161
		private string _valueTypeFullname;

		// Token: 0x040023CA RID: 9162
		private string _valueTypeAssemblyName;

		// Token: 0x040023CB RID: 9163
		private Type _valueElementType;

		// Token: 0x040023CC RID: 9164
		private string _valueSerializerTypeFullName;

		// Token: 0x040023CD RID: 9165
		private string _valueSerializerTypeAssemblyName;
	}
}
