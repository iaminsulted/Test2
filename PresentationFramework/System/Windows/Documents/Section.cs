using System;
using System.ComponentModel;
using System.Windows.Markup;

namespace System.Windows.Documents
{
	// Token: 0x02000684 RID: 1668
	[ContentProperty("Blocks")]
	public class Section : Block
	{
		// Token: 0x060052AF RID: 21167 RVA: 0x0022C811 File Offset: 0x0022B811
		public Section()
		{
		}

		// Token: 0x060052B0 RID: 21168 RVA: 0x00258E77 File Offset: 0x00257E77
		public Section(Block block)
		{
			if (block == null)
			{
				throw new ArgumentNullException("block");
			}
			this.Blocks.Add(block);
		}

		// Token: 0x17001383 RID: 4995
		// (get) Token: 0x060052B1 RID: 21169 RVA: 0x00258E99 File Offset: 0x00257E99
		// (set) Token: 0x060052B2 RID: 21170 RVA: 0x00258EA4 File Offset: 0x00257EA4
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DefaultValue(true)]
		public bool HasTrailingParagraphBreakOnPaste
		{
			get
			{
				return !this._ignoreTrailingParagraphBreakOnPaste;
			}
			set
			{
				this._ignoreTrailingParagraphBreakOnPaste = !value;
			}
		}

		// Token: 0x17001384 RID: 4996
		// (get) Token: 0x060052B3 RID: 21171 RVA: 0x0022BE7C File Offset: 0x0022AE7C
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public BlockCollection Blocks
		{
			get
			{
				return new BlockCollection(this, true);
			}
		}

		// Token: 0x060052B4 RID: 21172 RVA: 0x0022BF83 File Offset: 0x0022AF83
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeBlocks(XamlDesignerSerializationManager manager)
		{
			return manager != null && manager.XmlWriter == null;
		}

		// Token: 0x04002EB4 RID: 11956
		internal const string HasTrailingParagraphBreakOnPastePropertyName = "HasTrailingParagraphBreakOnPaste";

		// Token: 0x04002EB5 RID: 11957
		private bool _ignoreTrailingParagraphBreakOnPaste;
	}
}
