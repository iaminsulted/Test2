using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x020006C1 RID: 1729
	internal class TextSelectionHighlightLayer : HighlightLayer
	{
		// Token: 0x060059D4 RID: 22996 RVA: 0x0027E434 File Offset: 0x0027D434
		internal TextSelectionHighlightLayer(ITextSelection selection)
		{
			this._selection = selection;
			this._selection.Changed += this.OnSelectionChanged;
			this._oldStart = this._selection.Start;
			this._oldEnd = this._selection.End;
		}

		// Token: 0x060059D5 RID: 22997 RVA: 0x0027E488 File Offset: 0x0027D488
		internal override object GetHighlightValue(StaticTextPointer textPosition, LogicalDirection direction)
		{
			object result;
			if (this.IsContentHighlighted(textPosition, direction))
			{
				result = TextSelectionHighlightLayer._selectedValue;
			}
			else
			{
				result = DependencyProperty.UnsetValue;
			}
			return result;
		}

		// Token: 0x060059D6 RID: 22998 RVA: 0x0027E4B0 File Offset: 0x0027D4B0
		internal override bool IsContentHighlighted(StaticTextPointer textPosition, LogicalDirection direction)
		{
			if (this._selection.IsInterimSelection)
			{
				return false;
			}
			List<TextSegment> textSegments = this._selection.TextSegments;
			int count = textSegments.Count;
			for (int i = 0; i < count; i++)
			{
				TextSegment textSegment = textSegments[i];
				if ((direction == LogicalDirection.Forward && textSegment.Start.CompareTo(textPosition) <= 0 && textPosition.CompareTo(textSegment.End) < 0) || (direction == LogicalDirection.Backward && textSegment.Start.CompareTo(textPosition) < 0 && textPosition.CompareTo(textSegment.End) <= 0))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060059D7 RID: 22999 RVA: 0x0027E540 File Offset: 0x0027D540
		internal override StaticTextPointer GetNextChangePosition(StaticTextPointer textPosition, LogicalDirection direction)
		{
			StaticTextPointer result = StaticTextPointer.Null;
			if (!this.IsTextRangeEmpty(this._selection) && !this._selection.IsInterimSelection)
			{
				List<TextSegment> textSegments = this._selection.TextSegments;
				int count = textSegments.Count;
				if (direction == LogicalDirection.Forward)
				{
					for (int i = 0; i < count; i++)
					{
						TextSegment textSegment = textSegments[i];
						if (textSegment.Start.CompareTo(textSegment.End) != 0)
						{
							if (textPosition.CompareTo(textSegment.Start) < 0)
							{
								result = textSegment.Start.CreateStaticPointer();
								break;
							}
							if (textPosition.CompareTo(textSegment.End) < 0)
							{
								result = textSegment.End.CreateStaticPointer();
								break;
							}
						}
					}
				}
				else
				{
					for (int j = count - 1; j >= 0; j--)
					{
						TextSegment textSegment = textSegments[j];
						if (textSegment.Start.CompareTo(textSegment.End) != 0)
						{
							if (textPosition.CompareTo(textSegment.End) > 0)
							{
								result = textSegment.End.CreateStaticPointer();
								break;
							}
							if (textPosition.CompareTo(textSegment.Start) > 0)
							{
								result = textSegment.Start.CreateStaticPointer();
								break;
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x060059D8 RID: 23000 RVA: 0x0027E670 File Offset: 0x0027D670
		internal void InternalOnSelectionChanged()
		{
			ITextPointer textPointer;
			if (!this._selection.IsInterimSelection)
			{
				textPointer = this._selection.Start;
			}
			else
			{
				textPointer = this._selection.End;
			}
			ITextPointer end = this._selection.End;
			ITextPointer textPointer2;
			ITextPointer textPointer3;
			if (this._oldStart.CompareTo(textPointer) < 0)
			{
				textPointer2 = this._oldStart;
				textPointer3 = TextPointerBase.Min(textPointer, this._oldEnd);
			}
			else
			{
				textPointer2 = textPointer;
				textPointer3 = TextPointerBase.Min(end, this._oldStart);
			}
			ITextPointer textPointer4;
			ITextPointer textPointer5;
			if (this._oldEnd.CompareTo(end) < 0)
			{
				textPointer4 = TextPointerBase.Max(textPointer, this._oldEnd);
				textPointer5 = end;
			}
			else
			{
				textPointer4 = TextPointerBase.Max(end, this._oldStart);
				textPointer5 = this._oldEnd;
			}
			this._oldStart = textPointer;
			this._oldEnd = end;
			if (this.Changed != null && (textPointer2.CompareTo(textPointer3) != 0 || textPointer4.CompareTo(textPointer5) != 0))
			{
				TextSelectionHighlightLayer.TextSelectionHighlightChangedEventArgs args = new TextSelectionHighlightLayer.TextSelectionHighlightChangedEventArgs(textPointer2, textPointer3, textPointer4, textPointer5);
				this.Changed(this, args);
			}
		}

		// Token: 0x170014D0 RID: 5328
		// (get) Token: 0x060059D9 RID: 23001 RVA: 0x0022F5E5 File Offset: 0x0022E5E5
		internal override Type OwnerType
		{
			get
			{
				return typeof(TextSelection);
			}
		}

		// Token: 0x140000D1 RID: 209
		// (add) Token: 0x060059DA RID: 23002 RVA: 0x0027E764 File Offset: 0x0027D764
		// (remove) Token: 0x060059DB RID: 23003 RVA: 0x0027E79C File Offset: 0x0027D79C
		internal override event HighlightChangedEventHandler Changed;

		// Token: 0x060059DC RID: 23004 RVA: 0x0027E7D1 File Offset: 0x0027D7D1
		private void OnSelectionChanged(object sender, EventArgs e)
		{
			Invariant.Assert(this._selection == (ITextSelection)sender);
			this.InternalOnSelectionChanged();
		}

		// Token: 0x060059DD RID: 23005 RVA: 0x0027E7EC File Offset: 0x0027D7EC
		private bool IsTextRangeEmpty(ITextRange textRange)
		{
			Invariant.Assert(textRange._TextSegments.Count > 0);
			return textRange._TextSegments[0].Start.CompareTo(textRange._TextSegments[textRange._TextSegments.Count - 1].End) == 0;
		}

		// Token: 0x04003017 RID: 12311
		private readonly ITextSelection _selection;

		// Token: 0x04003018 RID: 12312
		private ITextPointer _oldStart;

		// Token: 0x04003019 RID: 12313
		private ITextPointer _oldEnd;

		// Token: 0x0400301A RID: 12314
		private static readonly object _selectedValue = new object();

		// Token: 0x02000B72 RID: 2930
		private class TextSelectionHighlightChangedEventArgs : HighlightChangedEventArgs
		{
			// Token: 0x06008E01 RID: 36353 RVA: 0x0034027C File Offset: 0x0033F27C
			internal TextSelectionHighlightChangedEventArgs(ITextPointer invalidRangeLeftStart, ITextPointer invalidRangeLeftEnd, ITextPointer invalidRangeRightStart, ITextPointer invalidRangeRightEnd)
			{
				Invariant.Assert(invalidRangeLeftStart != invalidRangeLeftEnd || invalidRangeRightStart != invalidRangeRightEnd, "Unexpected empty range!");
				List<TextSegment> list;
				if (invalidRangeLeftStart.CompareTo(invalidRangeLeftEnd) == 0)
				{
					list = new List<TextSegment>(1);
					list.Add(new TextSegment(invalidRangeRightStart, invalidRangeRightEnd));
				}
				else if (invalidRangeRightStart.CompareTo(invalidRangeRightEnd) == 0)
				{
					list = new List<TextSegment>(1);
					list.Add(new TextSegment(invalidRangeLeftStart, invalidRangeLeftEnd));
				}
				else
				{
					list = new List<TextSegment>(2);
					list.Add(new TextSegment(invalidRangeLeftStart, invalidRangeLeftEnd));
					list.Add(new TextSegment(invalidRangeRightStart, invalidRangeRightEnd));
				}
				this._ranges = new ReadOnlyCollection<TextSegment>(list);
			}

			// Token: 0x17001F07 RID: 7943
			// (get) Token: 0x06008E02 RID: 36354 RVA: 0x00340316 File Offset: 0x0033F316
			internal override IList Ranges
			{
				get
				{
					return this._ranges;
				}
			}

			// Token: 0x17001F08 RID: 7944
			// (get) Token: 0x06008E03 RID: 36355 RVA: 0x0022F5E5 File Offset: 0x0022E5E5
			internal override Type OwnerType
			{
				get
				{
					return typeof(TextSelection);
				}
			}

			// Token: 0x040048EE RID: 18670
			private readonly ReadOnlyCollection<TextSegment> _ranges;
		}
	}
}
