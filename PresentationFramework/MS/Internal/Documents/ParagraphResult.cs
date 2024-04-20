using System;
using System.Windows;
using System.Windows.Documents;
using MS.Internal.PtsHost;

namespace MS.Internal.Documents
{
	// Token: 0x020001DC RID: 476
	internal abstract class ParagraphResult
	{
		// Token: 0x060010DB RID: 4315 RVA: 0x00141FFC File Offset: 0x00140FFC
		internal ParagraphResult(BaseParaClient paraClient)
		{
			this._paraClient = paraClient;
			this._layoutBox = this._paraClient.Rect.FromTextDpi();
			this._element = paraClient.Paragraph.Element;
		}

		// Token: 0x060010DC RID: 4316 RVA: 0x00142040 File Offset: 0x00141040
		internal ParagraphResult(BaseParaClient paraClient, Rect layoutBox, DependencyObject element) : this(paraClient)
		{
			this._layoutBox = layoutBox;
			this._element = element;
		}

		// Token: 0x060010DD RID: 4317 RVA: 0x00142057 File Offset: 0x00141057
		internal virtual bool Contains(ITextPointer position, bool strict)
		{
			this.EnsureTextContentRange();
			return this._contentRange.Contains(position, strict);
		}

		// Token: 0x170002FC RID: 764
		// (get) Token: 0x060010DE RID: 4318 RVA: 0x0014206C File Offset: 0x0014106C
		internal ITextPointer StartPosition
		{
			get
			{
				this.EnsureTextContentRange();
				return this._contentRange.StartPosition;
			}
		}

		// Token: 0x170002FD RID: 765
		// (get) Token: 0x060010DF RID: 4319 RVA: 0x0014207F File Offset: 0x0014107F
		internal ITextPointer EndPosition
		{
			get
			{
				this.EnsureTextContentRange();
				return this._contentRange.EndPosition;
			}
		}

		// Token: 0x170002FE RID: 766
		// (get) Token: 0x060010E0 RID: 4320 RVA: 0x00142092 File Offset: 0x00141092
		internal Rect LayoutBox
		{
			get
			{
				return this._layoutBox;
			}
		}

		// Token: 0x170002FF RID: 767
		// (get) Token: 0x060010E1 RID: 4321 RVA: 0x0014209A File Offset: 0x0014109A
		internal DependencyObject Element
		{
			get
			{
				return this._element;
			}
		}

		// Token: 0x17000300 RID: 768
		// (get) Token: 0x060010E2 RID: 4322 RVA: 0x00105F35 File Offset: 0x00104F35
		internal virtual bool HasTextContent
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060010E3 RID: 4323 RVA: 0x001420A2 File Offset: 0x001410A2
		private void EnsureTextContentRange()
		{
			if (this._contentRange == null)
			{
				this._contentRange = this._paraClient.GetTextContentRange();
				Invariant.Assert(this._contentRange != null);
			}
		}

		// Token: 0x04000AE0 RID: 2784
		protected readonly BaseParaClient _paraClient;

		// Token: 0x04000AE1 RID: 2785
		protected readonly Rect _layoutBox;

		// Token: 0x04000AE2 RID: 2786
		protected readonly DependencyObject _element;

		// Token: 0x04000AE3 RID: 2787
		private TextContentRange _contentRange;

		// Token: 0x04000AE4 RID: 2788
		protected bool _hasTextContent;
	}
}
