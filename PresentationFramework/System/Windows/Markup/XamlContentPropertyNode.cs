using System;

namespace System.Windows.Markup
{
	// Token: 0x02000506 RID: 1286
	internal class XamlContentPropertyNode : XamlNode
	{
		// Token: 0x06003FFE RID: 16382 RVA: 0x002129E8 File Offset: 0x002119E8
		internal XamlContentPropertyNode(int lineNumber, int linePosition, int depth, object propertyMember, string assemblyName, string typeFullName, string propertyName) : base(XamlNodeType.ContentProperty, lineNumber, linePosition, depth)
		{
			if (typeFullName == null)
			{
				throw new ArgumentNullException("typeFullName");
			}
			if (propertyName == null)
			{
				throw new ArgumentNullException("propertyName");
			}
			this._propertyMember = propertyMember;
			this._assemblyName = assemblyName;
			this._typeFullName = typeFullName;
			this._propName = propertyName;
		}

		// Token: 0x17000E47 RID: 3655
		// (get) Token: 0x06003FFF RID: 16383 RVA: 0x00212A3E File Offset: 0x00211A3E
		internal string AssemblyName
		{
			get
			{
				return this._assemblyName;
			}
		}

		// Token: 0x17000E48 RID: 3656
		// (get) Token: 0x06004000 RID: 16384 RVA: 0x00212A46 File Offset: 0x00211A46
		internal string TypeFullName
		{
			get
			{
				return this._typeFullName;
			}
		}

		// Token: 0x17000E49 RID: 3657
		// (get) Token: 0x06004001 RID: 16385 RVA: 0x00212A4E File Offset: 0x00211A4E
		internal string PropName
		{
			get
			{
				return this._propName;
			}
		}

		// Token: 0x17000E4A RID: 3658
		// (get) Token: 0x06004002 RID: 16386 RVA: 0x00212A56 File Offset: 0x00211A56
		internal Type PropDeclaringType
		{
			get
			{
				if (this._declaringType == null && this._propertyMember != null)
				{
					this._declaringType = XamlTypeMapper.GetDeclaringType(this._propertyMember);
				}
				return this._declaringType;
			}
		}

		// Token: 0x17000E4B RID: 3659
		// (get) Token: 0x06004003 RID: 16387 RVA: 0x00212A85 File Offset: 0x00211A85
		internal Type PropValidType
		{
			get
			{
				if (this._validType == null)
				{
					this._validType = XamlTypeMapper.GetPropertyType(this._propertyMember);
				}
				return this._validType;
			}
		}

		// Token: 0x040023ED RID: 9197
		private Type _declaringType;

		// Token: 0x040023EE RID: 9198
		private Type _validType;

		// Token: 0x040023EF RID: 9199
		private object _propertyMember;

		// Token: 0x040023F0 RID: 9200
		private string _assemblyName;

		// Token: 0x040023F1 RID: 9201
		private string _typeFullName;

		// Token: 0x040023F2 RID: 9202
		private string _propName;
	}
}
