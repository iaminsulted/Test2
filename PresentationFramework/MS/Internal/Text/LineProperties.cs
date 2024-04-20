using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.TextFormatting;
using MS.Internal.PtsHost;

namespace MS.Internal.Text
{
	// Token: 0x02000323 RID: 803
	internal class LineProperties : TextParagraphProperties
	{
		// Token: 0x17000588 RID: 1416
		// (get) Token: 0x06001DE6 RID: 7654 RVA: 0x0016E95D File Offset: 0x0016D95D
		public override FlowDirection FlowDirection
		{
			get
			{
				return this._flowDirection;
			}
		}

		// Token: 0x17000589 RID: 1417
		// (get) Token: 0x06001DE7 RID: 7655 RVA: 0x0016E965 File Offset: 0x0016D965
		public override TextAlignment TextAlignment
		{
			get
			{
				if (!this.IgnoreTextAlignment)
				{
					return this._textAlignment;
				}
				return TextAlignment.Left;
			}
		}

		// Token: 0x1700058A RID: 1418
		// (get) Token: 0x06001DE8 RID: 7656 RVA: 0x0016E977 File Offset: 0x0016D977
		public override double LineHeight
		{
			get
			{
				if (this.LineStackingStrategy == LineStackingStrategy.BlockLineHeight && !double.IsNaN(this._lineHeight))
				{
					return this._lineHeight;
				}
				return 0.0;
			}
		}

		// Token: 0x1700058B RID: 1419
		// (get) Token: 0x06001DE9 RID: 7657 RVA: 0x00105F35 File Offset: 0x00104F35
		public override bool FirstLineInParagraph
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700058C RID: 1420
		// (get) Token: 0x06001DEA RID: 7658 RVA: 0x0016E99E File Offset: 0x0016D99E
		public override TextRunProperties DefaultTextRunProperties
		{
			get
			{
				return this._defaultTextProperties;
			}
		}

		// Token: 0x1700058D RID: 1421
		// (get) Token: 0x06001DEB RID: 7659 RVA: 0x0016E9A6 File Offset: 0x0016D9A6
		public override TextDecorationCollection TextDecorations
		{
			get
			{
				return this._defaultTextProperties.TextDecorations;
			}
		}

		// Token: 0x1700058E RID: 1422
		// (get) Token: 0x06001DEC RID: 7660 RVA: 0x0016E9B3 File Offset: 0x0016D9B3
		public override TextWrapping TextWrapping
		{
			get
			{
				return this._textWrapping;
			}
		}

		// Token: 0x1700058F RID: 1423
		// (get) Token: 0x06001DED RID: 7661 RVA: 0x0016E9BB File Offset: 0x0016D9BB
		public override TextMarkerProperties TextMarkerProperties
		{
			get
			{
				return this._markerProperties;
			}
		}

		// Token: 0x17000590 RID: 1424
		// (get) Token: 0x06001DEE RID: 7662 RVA: 0x0016E9C3 File Offset: 0x0016D9C3
		public override double Indent
		{
			get
			{
				return 0.0;
			}
		}

		// Token: 0x06001DEF RID: 7663 RVA: 0x0016E9CE File Offset: 0x0016D9CE
		internal LineProperties(DependencyObject element, DependencyObject contentHost, TextProperties defaultTextProperties, MarkerProperties markerProperties) : this(element, contentHost, defaultTextProperties, markerProperties, (TextAlignment)element.GetValue(Block.TextAlignmentProperty))
		{
		}

		// Token: 0x06001DF0 RID: 7664 RVA: 0x0016E9EC File Offset: 0x0016D9EC
		internal LineProperties(DependencyObject element, DependencyObject contentHost, TextProperties defaultTextProperties, MarkerProperties markerProperties, TextAlignment textAlignment)
		{
			this._defaultTextProperties = defaultTextProperties;
			this._markerProperties = ((markerProperties != null) ? markerProperties.GetTextMarkerProperties(this) : null);
			this._flowDirection = (FlowDirection)element.GetValue(Block.FlowDirectionProperty);
			this._textAlignment = textAlignment;
			this._lineHeight = (double)element.GetValue(Block.LineHeightProperty);
			this._textIndent = (double)element.GetValue(Paragraph.TextIndentProperty);
			this._lineStackingStrategy = (LineStackingStrategy)element.GetValue(Block.LineStackingStrategyProperty);
			this._textWrapping = TextWrapping.Wrap;
			this._textTrimming = TextTrimming.None;
			if (contentHost is TextBlock || contentHost is ITextBoxViewHost)
			{
				this._textWrapping = (TextWrapping)contentHost.GetValue(TextBlock.TextWrappingProperty);
				this._textTrimming = (TextTrimming)contentHost.GetValue(TextBlock.TextTrimmingProperty);
				return;
			}
			if (contentHost is FlowDocument)
			{
				this._textWrapping = ((FlowDocument)contentHost).TextWrapping;
			}
		}

		// Token: 0x06001DF1 RID: 7665 RVA: 0x0016EAE0 File Offset: 0x0016DAE0
		internal double CalcLineAdvanceForTextParagraph(TextParagraph textParagraph, int dcp, double lineAdvance)
		{
			if (!DoubleUtil.IsNaN(this._lineHeight))
			{
				LineStackingStrategy lineStackingStrategy = this.LineStackingStrategy;
				if (lineStackingStrategy != LineStackingStrategy.BlockLineHeight)
				{
					if (lineStackingStrategy != LineStackingStrategy.MaxHeight)
					{
					}
					if (dcp == 0 && textParagraph.HasFiguresOrFloaters() && textParagraph.GetLastDcpAttachedObjectBeforeLine(0) + textParagraph.ParagraphStartCharacterPosition == textParagraph.ParagraphEndCharacterPosition)
					{
						lineAdvance = this._lineHeight;
					}
					else
					{
						lineAdvance = Math.Max(lineAdvance, this._lineHeight);
					}
				}
				else
				{
					lineAdvance = this._lineHeight;
				}
			}
			return lineAdvance;
		}

		// Token: 0x06001DF2 RID: 7666 RVA: 0x0016EB50 File Offset: 0x0016DB50
		internal double CalcLineAdvance(double lineAdvance)
		{
			if (!DoubleUtil.IsNaN(this._lineHeight))
			{
				LineStackingStrategy lineStackingStrategy = this.LineStackingStrategy;
				if (lineStackingStrategy != LineStackingStrategy.BlockLineHeight)
				{
					if (lineStackingStrategy != LineStackingStrategy.MaxHeight)
					{
					}
					lineAdvance = Math.Max(lineAdvance, this._lineHeight);
				}
				else
				{
					lineAdvance = this._lineHeight;
				}
			}
			return lineAdvance;
		}

		// Token: 0x17000591 RID: 1425
		// (get) Token: 0x06001DF3 RID: 7667 RVA: 0x0016EB93 File Offset: 0x0016DB93
		internal TextAlignment TextAlignmentInternal
		{
			get
			{
				return this._textAlignment;
			}
		}

		// Token: 0x17000592 RID: 1426
		// (get) Token: 0x06001DF4 RID: 7668 RVA: 0x0016EB9B File Offset: 0x0016DB9B
		// (set) Token: 0x06001DF5 RID: 7669 RVA: 0x0016EBA3 File Offset: 0x0016DBA3
		internal bool IgnoreTextAlignment
		{
			get
			{
				return this._ignoreTextAlignment;
			}
			set
			{
				this._ignoreTextAlignment = value;
			}
		}

		// Token: 0x17000593 RID: 1427
		// (get) Token: 0x06001DF6 RID: 7670 RVA: 0x0016EBAC File Offset: 0x0016DBAC
		internal LineStackingStrategy LineStackingStrategy
		{
			get
			{
				return this._lineStackingStrategy;
			}
		}

		// Token: 0x17000594 RID: 1428
		// (get) Token: 0x06001DF7 RID: 7671 RVA: 0x0016EBB4 File Offset: 0x0016DBB4
		internal TextTrimming TextTrimming
		{
			get
			{
				return this._textTrimming;
			}
		}

		// Token: 0x17000595 RID: 1429
		// (get) Token: 0x06001DF8 RID: 7672 RVA: 0x0016EBBC File Offset: 0x0016DBBC
		internal bool HasFirstLineProperties
		{
			get
			{
				return this._markerProperties != null || !DoubleUtil.IsZero(this._textIndent);
			}
		}

		// Token: 0x17000596 RID: 1430
		// (get) Token: 0x06001DF9 RID: 7673 RVA: 0x0016EBD6 File Offset: 0x0016DBD6
		internal TextParagraphProperties FirstLineProps
		{
			get
			{
				if (this._firstLineProperties == null)
				{
					this._firstLineProperties = new LineProperties.FirstLineProperties(this);
				}
				return this._firstLineProperties;
			}
		}

		// Token: 0x06001DFA RID: 7674 RVA: 0x0016EBF2 File Offset: 0x0016DBF2
		internal TextParagraphProperties GetParaEllipsisLineProps(bool firstLine)
		{
			return new LineProperties.ParaEllipsisLineProperties(firstLine ? this.FirstLineProps : this);
		}

		// Token: 0x04000ED4 RID: 3796
		private TextRunProperties _defaultTextProperties;

		// Token: 0x04000ED5 RID: 3797
		private TextMarkerProperties _markerProperties;

		// Token: 0x04000ED6 RID: 3798
		private LineProperties.FirstLineProperties _firstLineProperties;

		// Token: 0x04000ED7 RID: 3799
		private bool _ignoreTextAlignment;

		// Token: 0x04000ED8 RID: 3800
		private FlowDirection _flowDirection;

		// Token: 0x04000ED9 RID: 3801
		private TextAlignment _textAlignment;

		// Token: 0x04000EDA RID: 3802
		private TextWrapping _textWrapping;

		// Token: 0x04000EDB RID: 3803
		private TextTrimming _textTrimming;

		// Token: 0x04000EDC RID: 3804
		private double _lineHeight;

		// Token: 0x04000EDD RID: 3805
		private double _textIndent;

		// Token: 0x04000EDE RID: 3806
		private LineStackingStrategy _lineStackingStrategy;

		// Token: 0x02000A6D RID: 2669
		private sealed class FirstLineProperties : TextParagraphProperties
		{
			// Token: 0x17001DF8 RID: 7672
			// (get) Token: 0x06008622 RID: 34338 RVA: 0x0032A207 File Offset: 0x00329207
			public override FlowDirection FlowDirection
			{
				get
				{
					return this._lp.FlowDirection;
				}
			}

			// Token: 0x17001DF9 RID: 7673
			// (get) Token: 0x06008623 RID: 34339 RVA: 0x0032A214 File Offset: 0x00329214
			public override TextAlignment TextAlignment
			{
				get
				{
					return this._lp.TextAlignment;
				}
			}

			// Token: 0x17001DFA RID: 7674
			// (get) Token: 0x06008624 RID: 34340 RVA: 0x0032A221 File Offset: 0x00329221
			public override double LineHeight
			{
				get
				{
					return this._lp.LineHeight;
				}
			}

			// Token: 0x17001DFB RID: 7675
			// (get) Token: 0x06008625 RID: 34341 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
			public override bool FirstLineInParagraph
			{
				get
				{
					return true;
				}
			}

			// Token: 0x17001DFC RID: 7676
			// (get) Token: 0x06008626 RID: 34342 RVA: 0x0032A22E File Offset: 0x0032922E
			public override TextRunProperties DefaultTextRunProperties
			{
				get
				{
					return this._lp.DefaultTextRunProperties;
				}
			}

			// Token: 0x17001DFD RID: 7677
			// (get) Token: 0x06008627 RID: 34343 RVA: 0x0032A23B File Offset: 0x0032923B
			public override TextDecorationCollection TextDecorations
			{
				get
				{
					return this._lp.TextDecorations;
				}
			}

			// Token: 0x17001DFE RID: 7678
			// (get) Token: 0x06008628 RID: 34344 RVA: 0x0032A248 File Offset: 0x00329248
			public override TextWrapping TextWrapping
			{
				get
				{
					return this._lp.TextWrapping;
				}
			}

			// Token: 0x17001DFF RID: 7679
			// (get) Token: 0x06008629 RID: 34345 RVA: 0x0032A255 File Offset: 0x00329255
			public override TextMarkerProperties TextMarkerProperties
			{
				get
				{
					return this._lp.TextMarkerProperties;
				}
			}

			// Token: 0x17001E00 RID: 7680
			// (get) Token: 0x0600862A RID: 34346 RVA: 0x0032A262 File Offset: 0x00329262
			public override double Indent
			{
				get
				{
					return this._lp._textIndent;
				}
			}

			// Token: 0x0600862B RID: 34347 RVA: 0x0032A26F File Offset: 0x0032926F
			internal FirstLineProperties(LineProperties lp)
			{
				this._lp = lp;
				this.Hyphenator = lp.Hyphenator;
			}

			// Token: 0x0400413C RID: 16700
			private LineProperties _lp;
		}

		// Token: 0x02000A6E RID: 2670
		private sealed class ParaEllipsisLineProperties : TextParagraphProperties
		{
			// Token: 0x17001E01 RID: 7681
			// (get) Token: 0x0600862C RID: 34348 RVA: 0x0032A28A File Offset: 0x0032928A
			public override FlowDirection FlowDirection
			{
				get
				{
					return this._lp.FlowDirection;
				}
			}

			// Token: 0x17001E02 RID: 7682
			// (get) Token: 0x0600862D RID: 34349 RVA: 0x0032A297 File Offset: 0x00329297
			public override TextAlignment TextAlignment
			{
				get
				{
					return this._lp.TextAlignment;
				}
			}

			// Token: 0x17001E03 RID: 7683
			// (get) Token: 0x0600862E RID: 34350 RVA: 0x0032A2A4 File Offset: 0x003292A4
			public override double LineHeight
			{
				get
				{
					return this._lp.LineHeight;
				}
			}

			// Token: 0x17001E04 RID: 7684
			// (get) Token: 0x0600862F RID: 34351 RVA: 0x0032A2B1 File Offset: 0x003292B1
			public override bool FirstLineInParagraph
			{
				get
				{
					return this._lp.FirstLineInParagraph;
				}
			}

			// Token: 0x17001E05 RID: 7685
			// (get) Token: 0x06008630 RID: 34352 RVA: 0x0032A2BE File Offset: 0x003292BE
			public override bool AlwaysCollapsible
			{
				get
				{
					return this._lp.AlwaysCollapsible;
				}
			}

			// Token: 0x17001E06 RID: 7686
			// (get) Token: 0x06008631 RID: 34353 RVA: 0x0032A2CB File Offset: 0x003292CB
			public override TextRunProperties DefaultTextRunProperties
			{
				get
				{
					return this._lp.DefaultTextRunProperties;
				}
			}

			// Token: 0x17001E07 RID: 7687
			// (get) Token: 0x06008632 RID: 34354 RVA: 0x0032A2D8 File Offset: 0x003292D8
			public override TextDecorationCollection TextDecorations
			{
				get
				{
					return this._lp.TextDecorations;
				}
			}

			// Token: 0x17001E08 RID: 7688
			// (get) Token: 0x06008633 RID: 34355 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
			public override TextWrapping TextWrapping
			{
				get
				{
					return TextWrapping.NoWrap;
				}
			}

			// Token: 0x17001E09 RID: 7689
			// (get) Token: 0x06008634 RID: 34356 RVA: 0x0032A2E5 File Offset: 0x003292E5
			public override TextMarkerProperties TextMarkerProperties
			{
				get
				{
					return this._lp.TextMarkerProperties;
				}
			}

			// Token: 0x17001E0A RID: 7690
			// (get) Token: 0x06008635 RID: 34357 RVA: 0x0032A2F2 File Offset: 0x003292F2
			public override double Indent
			{
				get
				{
					return this._lp.Indent;
				}
			}

			// Token: 0x06008636 RID: 34358 RVA: 0x0032A2FF File Offset: 0x003292FF
			internal ParaEllipsisLineProperties(TextParagraphProperties lp)
			{
				this._lp = lp;
			}

			// Token: 0x0400413D RID: 16701
			private TextParagraphProperties _lp;
		}
	}
}
