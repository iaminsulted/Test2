using System;
using System.Windows.Markup;

namespace MS.Internal.Globalization
{
	// Token: 0x020001A9 RID: 425
	internal sealed class BamlTextNode : BamlTreeNode
	{
		// Token: 0x06000E02 RID: 3586 RVA: 0x00137163 File Offset: 0x00136163
		internal BamlTextNode(string text) : this(text, null, null)
		{
		}

		// Token: 0x06000E03 RID: 3587 RVA: 0x0013716E File Offset: 0x0013616E
		internal BamlTextNode(string text, string typeConverterAssemblyName, string typeConverterName) : base(BamlNodeType.Text)
		{
			this._content = text;
			this._typeConverterAssemblyName = typeConverterAssemblyName;
			this._typeConverterName = typeConverterName;
		}

		// Token: 0x06000E04 RID: 3588 RVA: 0x0013718D File Offset: 0x0013618D
		internal override void Serialize(BamlWriter writer)
		{
			writer.WriteText(this._content, this._typeConverterAssemblyName, this._typeConverterName);
		}

		// Token: 0x06000E05 RID: 3589 RVA: 0x001371A7 File Offset: 0x001361A7
		internal override BamlTreeNode Copy()
		{
			return new BamlTextNode(this._content, this._typeConverterAssemblyName, this._typeConverterName);
		}

		// Token: 0x17000258 RID: 600
		// (get) Token: 0x06000E06 RID: 3590 RVA: 0x001371C0 File Offset: 0x001361C0
		internal string Content
		{
			get
			{
				return this._content;
			}
		}

		// Token: 0x04000A1E RID: 2590
		private string _content;

		// Token: 0x04000A1F RID: 2591
		private string _typeConverterAssemblyName;

		// Token: 0x04000A20 RID: 2592
		private string _typeConverterName;
	}
}
