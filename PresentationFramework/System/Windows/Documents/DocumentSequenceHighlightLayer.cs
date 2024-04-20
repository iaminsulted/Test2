using System;
using System.Collections;

namespace System.Windows.Documents
{
	// Token: 0x020005E8 RID: 1512
	internal class DocumentSequenceHighlightLayer : HighlightLayer
	{
		// Token: 0x06004912 RID: 18706 RVA: 0x0022F584 File Offset: 0x0022E584
		internal DocumentSequenceHighlightLayer(DocumentSequenceTextContainer docSeqContainer)
		{
			this._docSeqContainer = docSeqContainer;
		}

		// Token: 0x06004913 RID: 18707 RVA: 0x00109403 File Offset: 0x00108403
		internal override object GetHighlightValue(StaticTextPointer staticTextPointer, LogicalDirection direction)
		{
			return null;
		}

		// Token: 0x06004914 RID: 18708 RVA: 0x0022F593 File Offset: 0x0022E593
		internal override bool IsContentHighlighted(StaticTextPointer staticTextPointer, LogicalDirection direction)
		{
			return this._docSeqContainer.Highlights.IsContentHighlighted(staticTextPointer, direction);
		}

		// Token: 0x06004915 RID: 18709 RVA: 0x0022F5A7 File Offset: 0x0022E5A7
		internal override StaticTextPointer GetNextChangePosition(StaticTextPointer staticTextPointer, LogicalDirection direction)
		{
			return this._docSeqContainer.Highlights.GetNextHighlightChangePosition(staticTextPointer, direction);
		}

		// Token: 0x06004916 RID: 18710 RVA: 0x0022F5BC File Offset: 0x0022E5BC
		internal void RaiseHighlightChangedEvent(IList ranges)
		{
			if (this.Changed != null)
			{
				DocumentSequenceHighlightLayer.DocumentSequenceHighlightChangedEventArgs args = new DocumentSequenceHighlightLayer.DocumentSequenceHighlightChangedEventArgs(ranges);
				this.Changed(this, args);
			}
		}

		// Token: 0x1700106D RID: 4205
		// (get) Token: 0x06004917 RID: 18711 RVA: 0x0022F5E5 File Offset: 0x0022E5E5
		internal override Type OwnerType
		{
			get
			{
				return typeof(TextSelection);
			}
		}

		// Token: 0x140000AE RID: 174
		// (add) Token: 0x06004918 RID: 18712 RVA: 0x0022F5F4 File Offset: 0x0022E5F4
		// (remove) Token: 0x06004919 RID: 18713 RVA: 0x0022F62C File Offset: 0x0022E62C
		internal override event HighlightChangedEventHandler Changed;

		// Token: 0x04002662 RID: 9826
		private readonly DocumentSequenceTextContainer _docSeqContainer;

		// Token: 0x02000B30 RID: 2864
		private class DocumentSequenceHighlightChangedEventArgs : HighlightChangedEventArgs
		{
			// Token: 0x06008C95 RID: 35989 RVA: 0x0033D1B9 File Offset: 0x0033C1B9
			internal DocumentSequenceHighlightChangedEventArgs(IList ranges)
			{
				this._ranges = ranges;
			}

			// Token: 0x17001ECC RID: 7884
			// (get) Token: 0x06008C96 RID: 35990 RVA: 0x0033D1C8 File Offset: 0x0033C1C8
			internal override IList Ranges
			{
				get
				{
					return this._ranges;
				}
			}

			// Token: 0x17001ECD RID: 7885
			// (get) Token: 0x06008C97 RID: 35991 RVA: 0x0022F5E5 File Offset: 0x0022E5E5
			internal override Type OwnerType
			{
				get
				{
					return typeof(TextSelection);
				}
			}

			// Token: 0x040047FD RID: 18429
			private readonly IList _ranges;
		}
	}
}
