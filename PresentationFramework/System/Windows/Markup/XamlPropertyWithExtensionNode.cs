using System;

namespace System.Windows.Markup
{
	// Token: 0x020004E9 RID: 1257
	internal class XamlPropertyWithExtensionNode : XamlPropertyBaseNode
	{
		// Token: 0x06003FA2 RID: 16290 RVA: 0x002123B0 File Offset: 0x002113B0
		internal XamlPropertyWithExtensionNode(int lineNumber, int linePosition, int depth, object propertyMember, string assemblyName, string typeFullName, string propertyName, string value, short extensionTypeId, bool isValueNestedExtension, bool isValueTypeExtension) : base(XamlNodeType.PropertyWithExtension, lineNumber, linePosition, depth, propertyMember, assemblyName, typeFullName, propertyName)
		{
			this._value = value;
			this._extensionTypeId = extensionTypeId;
			this._isValueNestedExtension = isValueNestedExtension;
			this._isValueTypeExtension = isValueTypeExtension;
			this._defaultTargetType = null;
		}

		// Token: 0x17000E17 RID: 3607
		// (get) Token: 0x06003FA3 RID: 16291 RVA: 0x002123F7 File Offset: 0x002113F7
		internal short ExtensionTypeId
		{
			get
			{
				return this._extensionTypeId;
			}
		}

		// Token: 0x17000E18 RID: 3608
		// (get) Token: 0x06003FA4 RID: 16292 RVA: 0x002123FF File Offset: 0x002113FF
		internal string Value
		{
			get
			{
				return this._value;
			}
		}

		// Token: 0x17000E19 RID: 3609
		// (get) Token: 0x06003FA5 RID: 16293 RVA: 0x00212407 File Offset: 0x00211407
		internal bool IsValueNestedExtension
		{
			get
			{
				return this._isValueNestedExtension;
			}
		}

		// Token: 0x17000E1A RID: 3610
		// (get) Token: 0x06003FA6 RID: 16294 RVA: 0x0021240F File Offset: 0x0021140F
		internal bool IsValueTypeExtension
		{
			get
			{
				return this._isValueTypeExtension;
			}
		}

		// Token: 0x17000E1B RID: 3611
		// (get) Token: 0x06003FA7 RID: 16295 RVA: 0x00212417 File Offset: 0x00211417
		// (set) Token: 0x06003FA8 RID: 16296 RVA: 0x0021241F File Offset: 0x0021141F
		internal Type DefaultTargetType
		{
			get
			{
				return this._defaultTargetType;
			}
			set
			{
				this._defaultTargetType = value;
			}
		}

		// Token: 0x040023B8 RID: 9144
		private short _extensionTypeId;

		// Token: 0x040023B9 RID: 9145
		private string _value;

		// Token: 0x040023BA RID: 9146
		private bool _isValueNestedExtension;

		// Token: 0x040023BB RID: 9147
		private bool _isValueTypeExtension;

		// Token: 0x040023BC RID: 9148
		private Type _defaultTargetType;
	}
}
