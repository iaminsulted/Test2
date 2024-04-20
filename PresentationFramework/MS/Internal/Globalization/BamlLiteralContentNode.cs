using System;
using System.Windows.Markup;

namespace MS.Internal.Globalization
{
	// Token: 0x020001A8 RID: 424
	internal sealed class BamlLiteralContentNode : BamlTreeNode
	{
		// Token: 0x06000DFD RID: 3581 RVA: 0x00137126 File Offset: 0x00136126
		internal BamlLiteralContentNode(string literalContent) : base(BamlNodeType.LiteralContent)
		{
			this._literalContent = literalContent;
		}

		// Token: 0x06000DFE RID: 3582 RVA: 0x00137137 File Offset: 0x00136137
		internal override void Serialize(BamlWriter writer)
		{
			writer.WriteLiteralContent(this._literalContent);
		}

		// Token: 0x06000DFF RID: 3583 RVA: 0x00137145 File Offset: 0x00136145
		internal override BamlTreeNode Copy()
		{
			return new BamlLiteralContentNode(this._literalContent);
		}

		// Token: 0x17000257 RID: 599
		// (get) Token: 0x06000E00 RID: 3584 RVA: 0x00137152 File Offset: 0x00136152
		// (set) Token: 0x06000E01 RID: 3585 RVA: 0x0013715A File Offset: 0x0013615A
		internal string Content
		{
			get
			{
				return this._literalContent;
			}
			set
			{
				this._literalContent = value;
			}
		}

		// Token: 0x04000A1D RID: 2589
		private string _literalContent;
	}
}
