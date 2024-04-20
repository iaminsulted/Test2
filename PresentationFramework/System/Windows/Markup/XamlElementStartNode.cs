using System;
using System.Diagnostics;

namespace System.Windows.Markup
{
	// Token: 0x020004ED RID: 1261
	[DebuggerDisplay("Elem:{_typeFullName}")]
	internal class XamlElementStartNode : XamlNode
	{
		// Token: 0x06003FC7 RID: 16327 RVA: 0x00212624 File Offset: 0x00211624
		internal XamlElementStartNode(int lineNumber, int linePosition, int depth, string assemblyName, string typeFullName, Type elementType, Type serializerType) : this(XamlNodeType.ElementStart, lineNumber, linePosition, depth, assemblyName, typeFullName, elementType, serializerType, false, false, false)
		{
		}

		// Token: 0x06003FC8 RID: 16328 RVA: 0x00212648 File Offset: 0x00211648
		internal XamlElementStartNode(XamlNodeType tokenType, int lineNumber, int linePosition, int depth, string assemblyName, string typeFullName, Type elementType, Type serializerType, bool isEmptyElement, bool needsDictionaryKey, bool isInjected) : base(tokenType, lineNumber, linePosition, depth)
		{
			this._assemblyName = assemblyName;
			this._typeFullName = typeFullName;
			this._elementType = elementType;
			this._serializerType = serializerType;
			this._isEmptyElement = isEmptyElement;
			this._needsDictionaryKey = needsDictionaryKey;
			this._useTypeConverter = false;
			this.IsInjected = isInjected;
		}

		// Token: 0x17000E2E RID: 3630
		// (get) Token: 0x06003FC9 RID: 16329 RVA: 0x0021269F File Offset: 0x0021169F
		internal string AssemblyName
		{
			get
			{
				return this._assemblyName;
			}
		}

		// Token: 0x17000E2F RID: 3631
		// (get) Token: 0x06003FCA RID: 16330 RVA: 0x002126A7 File Offset: 0x002116A7
		internal string TypeFullName
		{
			get
			{
				return this._typeFullName;
			}
		}

		// Token: 0x17000E30 RID: 3632
		// (get) Token: 0x06003FCB RID: 16331 RVA: 0x002126AF File Offset: 0x002116AF
		internal Type ElementType
		{
			get
			{
				return this._elementType;
			}
		}

		// Token: 0x17000E31 RID: 3633
		// (get) Token: 0x06003FCC RID: 16332 RVA: 0x002126B7 File Offset: 0x002116B7
		internal Type SerializerType
		{
			get
			{
				return this._serializerType;
			}
		}

		// Token: 0x17000E32 RID: 3634
		// (get) Token: 0x06003FCD RID: 16333 RVA: 0x002126BF File Offset: 0x002116BF
		internal string SerializerTypeFullName
		{
			get
			{
				if (!(this._serializerType == null))
				{
					return this._serializerType.FullName;
				}
				return string.Empty;
			}
		}

		// Token: 0x17000E33 RID: 3635
		// (get) Token: 0x06003FCE RID: 16334 RVA: 0x002126E0 File Offset: 0x002116E0
		// (set) Token: 0x06003FCF RID: 16335 RVA: 0x002126E8 File Offset: 0x002116E8
		internal bool CreateUsingTypeConverter
		{
			get
			{
				return this._useTypeConverter;
			}
			set
			{
				this._useTypeConverter = value;
			}
		}

		// Token: 0x17000E34 RID: 3636
		// (get) Token: 0x06003FD0 RID: 16336 RVA: 0x002126F1 File Offset: 0x002116F1
		// (set) Token: 0x06003FD1 RID: 16337 RVA: 0x002126F9 File Offset: 0x002116F9
		internal bool IsInjected
		{
			get
			{
				return this._isInjected;
			}
			set
			{
				this._isInjected = value;
			}
		}

		// Token: 0x040023D1 RID: 9169
		private string _assemblyName;

		// Token: 0x040023D2 RID: 9170
		private string _typeFullName;

		// Token: 0x040023D3 RID: 9171
		private Type _elementType;

		// Token: 0x040023D4 RID: 9172
		private Type _serializerType;

		// Token: 0x040023D5 RID: 9173
		private bool _isEmptyElement;

		// Token: 0x040023D6 RID: 9174
		private bool _needsDictionaryKey;

		// Token: 0x040023D7 RID: 9175
		private bool _useTypeConverter;

		// Token: 0x040023D8 RID: 9176
		private bool _isInjected;
	}
}
