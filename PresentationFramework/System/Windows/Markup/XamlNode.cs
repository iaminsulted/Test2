using System;

namespace System.Windows.Markup
{
	// Token: 0x020004E2 RID: 1250
	internal class XamlNode
	{
		// Token: 0x06003F8B RID: 16267 RVA: 0x002121C5 File Offset: 0x002111C5
		internal XamlNode(XamlNodeType tokenType, int lineNumber, int linePosition, int depth)
		{
			this._token = tokenType;
			this._lineNumber = lineNumber;
			this._linePosition = linePosition;
			this._depth = depth;
		}

		// Token: 0x17000E0B RID: 3595
		// (get) Token: 0x06003F8C RID: 16268 RVA: 0x002121EA File Offset: 0x002111EA
		internal XamlNodeType TokenType
		{
			get
			{
				return this._token;
			}
		}

		// Token: 0x17000E0C RID: 3596
		// (get) Token: 0x06003F8D RID: 16269 RVA: 0x002121F2 File Offset: 0x002111F2
		internal int LineNumber
		{
			get
			{
				return this._lineNumber;
			}
		}

		// Token: 0x17000E0D RID: 3597
		// (get) Token: 0x06003F8E RID: 16270 RVA: 0x002121FA File Offset: 0x002111FA
		internal int LinePosition
		{
			get
			{
				return this._linePosition;
			}
		}

		// Token: 0x17000E0E RID: 3598
		// (get) Token: 0x06003F8F RID: 16271 RVA: 0x00212202 File Offset: 0x00211202
		internal int Depth
		{
			get
			{
				return this._depth;
			}
		}

		// Token: 0x040023AA RID: 9130
		internal static XamlNodeType[] ScopeStartTokens = new XamlNodeType[]
		{
			XamlNodeType.DocumentStart,
			XamlNodeType.ElementStart,
			XamlNodeType.PropertyComplexStart,
			XamlNodeType.PropertyArrayStart,
			XamlNodeType.PropertyIListStart,
			XamlNodeType.PropertyIDictionaryStart
		};

		// Token: 0x040023AB RID: 9131
		internal static XamlNodeType[] ScopeEndTokens = new XamlNodeType[]
		{
			XamlNodeType.DocumentEnd,
			XamlNodeType.ElementEnd,
			XamlNodeType.PropertyComplexEnd,
			XamlNodeType.PropertyArrayEnd,
			XamlNodeType.PropertyIListEnd,
			XamlNodeType.PropertyIDictionaryEnd
		};

		// Token: 0x040023AC RID: 9132
		private XamlNodeType _token;

		// Token: 0x040023AD RID: 9133
		private int _lineNumber;

		// Token: 0x040023AE RID: 9134
		private int _linePosition;

		// Token: 0x040023AF RID: 9135
		private int _depth;
	}
}
