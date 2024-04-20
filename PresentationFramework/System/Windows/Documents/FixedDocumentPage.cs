using System;
using System.Windows.Media;

namespace System.Windows.Documents
{
	// Token: 0x020005F1 RID: 1521
	internal sealed class FixedDocumentPage : DocumentPage, IServiceProvider
	{
		// Token: 0x06004A5A RID: 19034 RVA: 0x00232C81 File Offset: 0x00231C81
		internal FixedDocumentPage(FixedDocument panel, FixedPage page, Size fixedSize, int index) : base(page, fixedSize, new Rect(fixedSize), new Rect(fixedSize))
		{
			this._panel = panel;
			this._page = page;
			this._index = index;
		}

		// Token: 0x06004A5B RID: 19035 RVA: 0x00232CAD File Offset: 0x00231CAD
		object IServiceProvider.GetService(Type serviceType)
		{
			if (serviceType == null)
			{
				throw new ArgumentNullException("serviceType");
			}
			if (serviceType == typeof(ITextView))
			{
				return this.TextView;
			}
			return null;
		}

		// Token: 0x170010FE RID: 4350
		// (get) Token: 0x06004A5C RID: 19036 RVA: 0x00232CE0 File Offset: 0x00231CE0
		public override Visual Visual
		{
			get
			{
				if (!this._layedOut)
				{
					this._layedOut = true;
					UIElement uielement;
					if ((uielement = (base.Visual as UIElement)) != null)
					{
						uielement.Measure(base.Size);
						uielement.Arrange(new Rect(base.Size));
					}
				}
				return base.Visual;
			}
		}

		// Token: 0x170010FF RID: 4351
		// (get) Token: 0x06004A5D RID: 19037 RVA: 0x00232D30 File Offset: 0x00231D30
		internal ContentPosition ContentPosition
		{
			get
			{
				FlowPosition pageStartFlowPosition = this._panel.FixedContainer.FixedTextBuilder.GetPageStartFlowPosition(this._index);
				return new FixedTextPointer(true, LogicalDirection.Forward, pageStartFlowPosition);
			}
		}

		// Token: 0x17001100 RID: 4352
		// (get) Token: 0x06004A5E RID: 19038 RVA: 0x00232D61 File Offset: 0x00231D61
		internal FixedPage FixedPage
		{
			get
			{
				return this._page;
			}
		}

		// Token: 0x17001101 RID: 4353
		// (get) Token: 0x06004A5F RID: 19039 RVA: 0x00232D69 File Offset: 0x00231D69
		internal int PageIndex
		{
			get
			{
				return this._panel.GetIndexOfPage(this._page);
			}
		}

		// Token: 0x17001102 RID: 4354
		// (get) Token: 0x06004A60 RID: 19040 RVA: 0x00232D7C File Offset: 0x00231D7C
		internal FixedTextView TextView
		{
			get
			{
				if (this._textView == null)
				{
					this._textView = new FixedTextView(this);
				}
				return this._textView;
			}
		}

		// Token: 0x17001103 RID: 4355
		// (get) Token: 0x06004A61 RID: 19041 RVA: 0x00232D98 File Offset: 0x00231D98
		internal FixedDocument Owner
		{
			get
			{
				return this._panel;
			}
		}

		// Token: 0x17001104 RID: 4356
		// (get) Token: 0x06004A62 RID: 19042 RVA: 0x00232DA0 File Offset: 0x00231DA0
		internal FixedTextContainer TextContainer
		{
			get
			{
				return this._panel.FixedContainer;
			}
		}

		// Token: 0x040026F3 RID: 9971
		private readonly FixedDocument _panel;

		// Token: 0x040026F4 RID: 9972
		private readonly FixedPage _page;

		// Token: 0x040026F5 RID: 9973
		private readonly int _index;

		// Token: 0x040026F6 RID: 9974
		private bool _layedOut;

		// Token: 0x040026F7 RID: 9975
		private FixedTextView _textView;
	}
}
