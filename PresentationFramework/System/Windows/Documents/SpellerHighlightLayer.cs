using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x02000689 RID: 1673
	internal class SpellerHighlightLayer : HighlightLayer
	{
		// Token: 0x060052FD RID: 21245 RVA: 0x0025AD0C File Offset: 0x00259D0C
		internal SpellerHighlightLayer(Speller speller)
		{
			this._speller = speller;
		}

		// Token: 0x060052FE RID: 21246 RVA: 0x0025AD1C File Offset: 0x00259D1C
		internal override object GetHighlightValue(StaticTextPointer textPosition, LogicalDirection direction)
		{
			object result;
			if (this.IsContentHighlighted(textPosition, direction))
			{
				result = SpellerHighlightLayer._errorTextDecorations;
			}
			else
			{
				result = DependencyProperty.UnsetValue;
			}
			return result;
		}

		// Token: 0x060052FF RID: 21247 RVA: 0x0025AD42 File Offset: 0x00259D42
		internal override bool IsContentHighlighted(StaticTextPointer textPosition, LogicalDirection direction)
		{
			return this._speller.StatusTable.IsRunType(textPosition, direction, SpellerStatusTable.RunType.Error);
		}

		// Token: 0x06005300 RID: 21248 RVA: 0x0025AD57 File Offset: 0x00259D57
		internal override StaticTextPointer GetNextChangePosition(StaticTextPointer textPosition, LogicalDirection direction)
		{
			return this._speller.StatusTable.GetNextErrorTransition(textPosition, direction);
		}

		// Token: 0x06005301 RID: 21249 RVA: 0x0025AD6B File Offset: 0x00259D6B
		internal void FireChangedEvent(ITextPointer start, ITextPointer end)
		{
			if (this.Changed != null)
			{
				this.Changed(this, new SpellerHighlightLayer.SpellerHighlightChangedEventArgs(start, end));
			}
		}

		// Token: 0x1700138C RID: 5004
		// (get) Token: 0x06005302 RID: 21250 RVA: 0x0025AD88 File Offset: 0x00259D88
		internal override Type OwnerType
		{
			get
			{
				return typeof(SpellerHighlightLayer);
			}
		}

		// Token: 0x140000C7 RID: 199
		// (add) Token: 0x06005303 RID: 21251 RVA: 0x0025AD94 File Offset: 0x00259D94
		// (remove) Token: 0x06005304 RID: 21252 RVA: 0x0025ADCC File Offset: 0x00259DCC
		internal override event HighlightChangedEventHandler Changed;

		// Token: 0x06005305 RID: 21253 RVA: 0x0025AE04 File Offset: 0x00259E04
		private static TextDecorationCollection GetErrorTextDecorations()
		{
			DrawingGroup drawingGroup = new DrawingGroup();
			DrawingContext drawingContext = drawingGroup.Open();
			Pen pen = new Pen(Brushes.Red, 0.33);
			drawingContext.DrawLine(pen, new Point(0.0, 0.0), new Point(0.5, 1.0));
			drawingContext.DrawLine(pen, new Point(0.5, 1.0), new Point(1.0, 0.0));
			drawingContext.Close();
			TextDecoration value = new TextDecoration(TextDecorationLocation.Underline, new Pen(new DrawingBrush(drawingGroup)
			{
				TileMode = TileMode.Tile,
				Viewport = new Rect(0.0, 0.0, 3.0, 3.0),
				ViewportUnits = BrushMappingMode.Absolute
			}, 3.0), 0.0, TextDecorationUnit.FontRecommended, TextDecorationUnit.Pixel);
			TextDecorationCollection textDecorationCollection = new TextDecorationCollection();
			textDecorationCollection.Add(value);
			textDecorationCollection.Freeze();
			return textDecorationCollection;
		}

		// Token: 0x04002ED0 RID: 11984
		private readonly Speller _speller;

		// Token: 0x04002ED1 RID: 11985
		private static readonly TextDecorationCollection _errorTextDecorations = SpellerHighlightLayer.GetErrorTextDecorations();

		// Token: 0x02000B53 RID: 2899
		private class SpellerHighlightChangedEventArgs : HighlightChangedEventArgs
		{
			// Token: 0x06008D9F RID: 36255 RVA: 0x0033EC70 File Offset: 0x0033DC70
			internal SpellerHighlightChangedEventArgs(ITextPointer start, ITextPointer end)
			{
				Invariant.Assert(start.CompareTo(end) < 0, "Bogus start/end combination!");
				this._ranges = new ReadOnlyCollection<TextSegment>(new List<TextSegment>(1)
				{
					new TextSegment(start, end)
				});
			}

			// Token: 0x17001EEB RID: 7915
			// (get) Token: 0x06008DA0 RID: 36256 RVA: 0x0033ECB7 File Offset: 0x0033DCB7
			internal override IList Ranges
			{
				get
				{
					return this._ranges;
				}
			}

			// Token: 0x17001EEC RID: 7916
			// (get) Token: 0x06008DA1 RID: 36257 RVA: 0x0025AD88 File Offset: 0x00259D88
			internal override Type OwnerType
			{
				get
				{
					return typeof(SpellerHighlightLayer);
				}
			}

			// Token: 0x0400489A RID: 18586
			private readonly ReadOnlyCollection<TextSegment> _ranges;
		}
	}
}
