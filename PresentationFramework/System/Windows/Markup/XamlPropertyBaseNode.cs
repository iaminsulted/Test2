using System;
using System.Diagnostics;

namespace System.Windows.Markup
{
	// Token: 0x020004E6 RID: 1254
	[DebuggerDisplay("Prop:{_typeFullName}.{_propName}")]
	internal class XamlPropertyBaseNode : XamlNode
	{
		// Token: 0x06003F97 RID: 16279 RVA: 0x00212288 File Offset: 0x00211288
		internal XamlPropertyBaseNode(XamlNodeType token, int lineNumber, int linePosition, int depth, object propertyMember, string assemblyName, string typeFullName, string propertyName) : base(token, lineNumber, linePosition, depth)
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

		// Token: 0x17000E11 RID: 3601
		// (get) Token: 0x06003F98 RID: 16280 RVA: 0x002122DE File Offset: 0x002112DE
		internal string AssemblyName
		{
			get
			{
				return this._assemblyName;
			}
		}

		// Token: 0x17000E12 RID: 3602
		// (get) Token: 0x06003F99 RID: 16281 RVA: 0x002122E6 File Offset: 0x002112E6
		internal string TypeFullName
		{
			get
			{
				return this._typeFullName;
			}
		}

		// Token: 0x17000E13 RID: 3603
		// (get) Token: 0x06003F9A RID: 16282 RVA: 0x002122EE File Offset: 0x002112EE
		internal string PropName
		{
			get
			{
				return this._propName;
			}
		}

		// Token: 0x17000E14 RID: 3604
		// (get) Token: 0x06003F9B RID: 16283 RVA: 0x002122F6 File Offset: 0x002112F6
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

		// Token: 0x17000E15 RID: 3605
		// (get) Token: 0x06003F9C RID: 16284 RVA: 0x00212325 File Offset: 0x00211325
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

		// Token: 0x17000E16 RID: 3606
		// (get) Token: 0x06003F9D RID: 16285 RVA: 0x0021234C File Offset: 0x0021134C
		internal object PropertyMember
		{
			get
			{
				return this._propertyMember;
			}
		}

		// Token: 0x040023B2 RID: 9138
		private object _propertyMember;

		// Token: 0x040023B3 RID: 9139
		private string _assemblyName;

		// Token: 0x040023B4 RID: 9140
		private string _typeFullName;

		// Token: 0x040023B5 RID: 9141
		private string _propName;

		// Token: 0x040023B6 RID: 9142
		private Type _validType;

		// Token: 0x040023B7 RID: 9143
		private Type _declaringType;
	}
}
