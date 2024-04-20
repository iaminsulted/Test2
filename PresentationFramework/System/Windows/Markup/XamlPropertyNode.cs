using System;

namespace System.Windows.Markup
{
	// Token: 0x020004EA RID: 1258
	internal class XamlPropertyNode : XamlPropertyBaseNode
	{
		// Token: 0x06003FA9 RID: 16297 RVA: 0x00212428 File Offset: 0x00211428
		internal XamlPropertyNode(int lineNumber, int linePosition, int depth, object propertyMember, string assemblyName, string typeFullName, string propertyName, string value, BamlAttributeUsage attributeUsage, bool complexAsSimple) : base(XamlNodeType.Property, lineNumber, linePosition, depth, propertyMember, assemblyName, typeFullName, propertyName)
		{
			this._value = value;
			this._attributeUsage = attributeUsage;
			this._complexAsSimple = complexAsSimple;
		}

		// Token: 0x06003FAA RID: 16298 RVA: 0x00212460 File Offset: 0x00211460
		internal XamlPropertyNode(int lineNumber, int linePosition, int depth, object propertyMember, string assemblyName, string typeFullName, string propertyName, string value, BamlAttributeUsage attributeUsage, bool complexAsSimple, bool isDefinitionName) : this(lineNumber, linePosition, depth, propertyMember, assemblyName, typeFullName, propertyName, value, attributeUsage, complexAsSimple)
		{
			this._isDefinitionName = isDefinitionName;
		}

		// Token: 0x17000E1C RID: 3612
		// (get) Token: 0x06003FAB RID: 16299 RVA: 0x0021248C File Offset: 0x0021148C
		internal string Value
		{
			get
			{
				return this._value;
			}
		}

		// Token: 0x06003FAC RID: 16300 RVA: 0x00212494 File Offset: 0x00211494
		internal void SetValue(string value)
		{
			this._value = value;
		}

		// Token: 0x17000E1D RID: 3613
		// (get) Token: 0x06003FAD RID: 16301 RVA: 0x0021249D File Offset: 0x0021149D
		// (set) Token: 0x06003FAE RID: 16302 RVA: 0x002124BA File Offset: 0x002114BA
		internal Type ValueDeclaringType
		{
			get
			{
				if (this._valueDeclaringType == null)
				{
					return base.PropDeclaringType;
				}
				return this._valueDeclaringType;
			}
			set
			{
				this._valueDeclaringType = value;
			}
		}

		// Token: 0x17000E1E RID: 3614
		// (get) Token: 0x06003FAF RID: 16303 RVA: 0x002124C3 File Offset: 0x002114C3
		// (set) Token: 0x06003FB0 RID: 16304 RVA: 0x002124DA File Offset: 0x002114DA
		internal string ValuePropertyName
		{
			get
			{
				if (this._valuePropertyName == null)
				{
					return base.PropName;
				}
				return this._valuePropertyName;
			}
			set
			{
				this._valuePropertyName = value;
			}
		}

		// Token: 0x17000E1F RID: 3615
		// (get) Token: 0x06003FB1 RID: 16305 RVA: 0x002124E3 File Offset: 0x002114E3
		// (set) Token: 0x06003FB2 RID: 16306 RVA: 0x00212500 File Offset: 0x00211500
		internal Type ValuePropertyType
		{
			get
			{
				if (this._valuePropertyType == null)
				{
					return base.PropValidType;
				}
				return this._valuePropertyType;
			}
			set
			{
				this._valuePropertyType = value;
			}
		}

		// Token: 0x17000E20 RID: 3616
		// (get) Token: 0x06003FB3 RID: 16307 RVA: 0x00212509 File Offset: 0x00211509
		// (set) Token: 0x06003FB4 RID: 16308 RVA: 0x00212520 File Offset: 0x00211520
		internal object ValuePropertyMember
		{
			get
			{
				if (this._valuePropertyMember == null)
				{
					return base.PropertyMember;
				}
				return this._valuePropertyMember;
			}
			set
			{
				this._valuePropertyMember = value;
			}
		}

		// Token: 0x17000E21 RID: 3617
		// (get) Token: 0x06003FB5 RID: 16309 RVA: 0x00212529 File Offset: 0x00211529
		internal bool HasValueId
		{
			get
			{
				return this._hasValueId;
			}
		}

		// Token: 0x17000E22 RID: 3618
		// (get) Token: 0x06003FB6 RID: 16310 RVA: 0x00212531 File Offset: 0x00211531
		// (set) Token: 0x06003FB7 RID: 16311 RVA: 0x00212539 File Offset: 0x00211539
		internal short ValueId
		{
			get
			{
				return this._valueId;
			}
			set
			{
				this._valueId = value;
				this._hasValueId = true;
			}
		}

		// Token: 0x17000E23 RID: 3619
		// (get) Token: 0x06003FB8 RID: 16312 RVA: 0x00212549 File Offset: 0x00211549
		// (set) Token: 0x06003FB9 RID: 16313 RVA: 0x00212551 File Offset: 0x00211551
		internal string MemberName
		{
			get
			{
				return this._memberName;
			}
			set
			{
				this._memberName = value;
			}
		}

		// Token: 0x17000E24 RID: 3620
		// (get) Token: 0x06003FBA RID: 16314 RVA: 0x0021255A File Offset: 0x0021155A
		// (set) Token: 0x06003FBB RID: 16315 RVA: 0x00212562 File Offset: 0x00211562
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

		// Token: 0x17000E25 RID: 3621
		// (get) Token: 0x06003FBC RID: 16316 RVA: 0x0021256B File Offset: 0x0021156B
		internal BamlAttributeUsage AttributeUsage
		{
			get
			{
				return this._attributeUsage;
			}
		}

		// Token: 0x17000E26 RID: 3622
		// (get) Token: 0x06003FBD RID: 16317 RVA: 0x00212573 File Offset: 0x00211573
		internal bool ComplexAsSimple
		{
			get
			{
				return this._complexAsSimple;
			}
		}

		// Token: 0x040023BD RID: 9149
		private string _value;

		// Token: 0x040023BE RID: 9150
		private BamlAttributeUsage _attributeUsage;

		// Token: 0x040023BF RID: 9151
		private bool _complexAsSimple;

		// Token: 0x040023C0 RID: 9152
		private bool _isDefinitionName;

		// Token: 0x040023C1 RID: 9153
		private Type _valueDeclaringType;

		// Token: 0x040023C2 RID: 9154
		private string _valuePropertyName;

		// Token: 0x040023C3 RID: 9155
		private Type _valuePropertyType;

		// Token: 0x040023C4 RID: 9156
		private object _valuePropertyMember;

		// Token: 0x040023C5 RID: 9157
		private bool _hasValueId;

		// Token: 0x040023C6 RID: 9158
		private short _valueId;

		// Token: 0x040023C7 RID: 9159
		private string _memberName;

		// Token: 0x040023C8 RID: 9160
		private Type _defaultTargetType;
	}
}
