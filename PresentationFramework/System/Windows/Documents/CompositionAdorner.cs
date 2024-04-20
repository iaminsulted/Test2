using System;
using System.Collections;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using MS.Internal;
using MS.Win32;

namespace System.Windows.Documents
{
	// Token: 0x020005E3 RID: 1507
	internal class CompositionAdorner : Adorner
	{
		// Token: 0x060048C2 RID: 18626 RVA: 0x0022DD76 File Offset: 0x0022CD76
		static CompositionAdorner()
		{
			UIElement.IsEnabledProperty.OverrideMetadata(typeof(CompositionAdorner), new FrameworkPropertyMetadata(false));
		}

		// Token: 0x060048C3 RID: 18627 RVA: 0x0022DD97 File Offset: 0x0022CD97
		internal CompositionAdorner(ITextView textView) : this(textView, new ArrayList())
		{
		}

		// Token: 0x060048C4 RID: 18628 RVA: 0x0022DDA5 File Offset: 0x0022CDA5
		internal CompositionAdorner(ITextView textView, ArrayList attributeRanges) : base(textView.RenderScope)
		{
			this._textView = textView;
			this._attributeRanges = attributeRanges;
		}

		// Token: 0x060048C5 RID: 18629 RVA: 0x0022DDC4 File Offset: 0x0022CDC4
		public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
		{
			GeneralTransformGroup generalTransformGroup = new GeneralTransformGroup();
			Transform transform2 = transform.AffineTransform;
			if (transform2 == null)
			{
				transform2 = Transform.Identity;
			}
			TranslateTransform value = new TranslateTransform(-transform2.Value.OffsetX, -transform2.Value.OffsetY);
			generalTransformGroup.Children.Add(value);
			if (transform != null)
			{
				generalTransformGroup.Children.Add(transform);
			}
			return generalTransformGroup;
		}

		// Token: 0x060048C6 RID: 18630 RVA: 0x0022DE28 File Offset: 0x0022CE28
		protected override void OnRender(DrawingContext drawingContext)
		{
			Visual visual = VisualTreeHelper.GetParent(base.AdornedElement) as Visual;
			if (visual == null)
			{
				return;
			}
			GeneralTransform generalTransform = base.AdornedElement.TransformToAncestor(visual);
			if (generalTransform == null)
			{
				return;
			}
			bool flag = "zh-CN".Equals(InputLanguageManager.Current.CurrentInputLanguage.IetfLanguageTag);
			for (int i = 0; i < this._attributeRanges.Count; i++)
			{
				CompositionAdorner.AttributeRange attributeRange = (CompositionAdorner.AttributeRange)this._attributeRanges[i];
				if (attributeRange.CompositionLines.Count != 0)
				{
					bool isBoldLine = attributeRange.TextServicesDisplayAttribute.IsBoldLine;
					bool flag2 = false;
					bool flag3 = (attributeRange.TextServicesDisplayAttribute.AttrInfo & UnsafeNativeMethods.TF_DA_ATTR_INFO.TF_ATTR_TARGET_CONVERTED) > UnsafeNativeMethods.TF_DA_ATTR_INFO.TF_ATTR_INPUT;
					Brush brush = null;
					double opacity = -1.0;
					Pen pen = null;
					if (flag && flag3)
					{
						DependencyObject parent = this._textView.TextContainer.Parent;
						brush = (Brush)parent.GetValue(TextBoxBase.SelectionBrushProperty);
						opacity = (double)parent.GetValue(TextBoxBase.SelectionOpacityProperty);
					}
					double height = attributeRange.Height;
					double num = height * (isBoldLine ? 0.08 : 0.06);
					double num2 = height * 0.09;
					Pen pen2 = new Pen(new SolidColorBrush(Colors.Black), num);
					switch (attributeRange.TextServicesDisplayAttribute.LineStyle)
					{
					case UnsafeNativeMethods.TF_DA_LINESTYLE.TF_LS_SOLID:
						pen2.StartLineCap = PenLineCap.Round;
						pen2.EndLineCap = PenLineCap.Round;
						break;
					case UnsafeNativeMethods.TF_DA_LINESTYLE.TF_LS_DOT:
						pen2.DashStyle = new DashStyle(new DoubleCollection
						{
							1.2,
							1.2
						}, 0.0);
						pen2.DashCap = PenLineCap.Round;
						pen2.StartLineCap = PenLineCap.Round;
						pen2.EndLineCap = PenLineCap.Round;
						num = height * (isBoldLine ? 0.1 : 0.08);
						break;
					case UnsafeNativeMethods.TF_DA_LINESTYLE.TF_LS_DASH:
					{
						double value = height * (isBoldLine ? 0.39 : 0.27);
						double value2 = height * (isBoldLine ? 0.06 : 0.04);
						pen2.DashStyle = new DashStyle(new DoubleCollection
						{
							value,
							value2
						}, 0.0);
						pen2.DashCap = PenLineCap.Round;
						pen2.StartLineCap = PenLineCap.Round;
						pen2.EndLineCap = PenLineCap.Round;
						break;
					}
					case UnsafeNativeMethods.TF_DA_LINESTYLE.TF_LS_SQUIGGLE:
						flag2 = true;
						break;
					}
					double num3 = num / 2.0;
					for (int j = 0; j < attributeRange.CompositionLines.Count; j++)
					{
						CompositionAdorner.CompositionLine compositionLine = (CompositionAdorner.CompositionLine)attributeRange.CompositionLines[j];
						Point point = new Point(compositionLine.StartPoint.X + num2, compositionLine.StartPoint.Y - num3);
						Point point2 = new Point(compositionLine.EndPoint.X - num2, compositionLine.EndPoint.Y - num3);
						pen2.Brush = new SolidColorBrush(compositionLine.LineColor);
						generalTransform.TryTransform(point, out point);
						generalTransform.TryTransform(point2, out point2);
						if (flag && flag3)
						{
							Rect rect = Rect.Union(compositionLine.StartRect, compositionLine.EndRect);
							rect = generalTransform.TransformBounds(rect);
							drawingContext.PushOpacity(opacity);
							drawingContext.DrawRectangle(brush, pen, rect);
							drawingContext.Pop();
						}
						if (flag2)
						{
							Point point3 = new Point(point.X, point.Y - num3);
							double num4 = num3;
							PathFigure pathFigure = new PathFigure();
							pathFigure.StartPoint = point3;
							int num5 = 0;
							while ((double)num5 < (point2.X - point.X) / num4)
							{
								if (num5 % 4 == 0 || num5 % 4 == 3)
								{
									point3 = new Point(point3.X + num4, point3.Y + num3);
									pathFigure.Segments.Add(new LineSegment(point3, true));
								}
								else if (num5 % 4 == 1 || num5 % 4 == 2)
								{
									point3 = new Point(point3.X + num4, point3.Y - num3);
									pathFigure.Segments.Add(new LineSegment(point3, true));
								}
								num5++;
							}
							drawingContext.DrawGeometry(null, pen2, new PathGeometry
							{
								Figures = 
								{
									pathFigure
								}
							});
						}
						else
						{
							drawingContext.DrawLine(pen2, point, point2);
						}
					}
				}
			}
		}

		// Token: 0x060048C7 RID: 18631 RVA: 0x0022E2B8 File Offset: 0x0022D2B8
		internal void AddAttributeRange(ITextPointer start, ITextPointer end, TextServicesDisplayAttribute textServiceDisplayAttribute)
		{
			ITextPointer start2 = start.CreatePointer(LogicalDirection.Forward);
			ITextPointer end2 = end.CreatePointer(LogicalDirection.Backward);
			this._attributeRanges.Add(new CompositionAdorner.AttributeRange(this._textView, start2, end2, textServiceDisplayAttribute));
		}

		// Token: 0x060048C8 RID: 18632 RVA: 0x0022E2F0 File Offset: 0x0022D2F0
		internal void InvalidateAdorner()
		{
			for (int i = 0; i < this._attributeRanges.Count; i++)
			{
				((CompositionAdorner.AttributeRange)this._attributeRanges[i]).AddCompositionLines();
			}
			AdornerLayer adornerLayer = VisualTreeHelper.GetParent(this) as AdornerLayer;
			if (adornerLayer != null)
			{
				adornerLayer.Update(base.AdornedElement);
				adornerLayer.InvalidateArrange();
			}
		}

		// Token: 0x060048C9 RID: 18633 RVA: 0x0022E34A File Offset: 0x0022D34A
		internal void Initialize(ITextView textView)
		{
			this._adornerLayer = AdornerLayer.GetAdornerLayer(textView.RenderScope);
			if (this._adornerLayer != null)
			{
				this._adornerLayer.Add(this);
			}
		}

		// Token: 0x060048CA RID: 18634 RVA: 0x0022E371 File Offset: 0x0022D371
		internal void Uninitialize()
		{
			if (this._adornerLayer != null)
			{
				this._adornerLayer.Remove(this);
				this._adornerLayer = null;
			}
		}

		// Token: 0x0400263E RID: 9790
		private AdornerLayer _adornerLayer;

		// Token: 0x0400263F RID: 9791
		private ITextView _textView;

		// Token: 0x04002640 RID: 9792
		private readonly ArrayList _attributeRanges;

		// Token: 0x04002641 RID: 9793
		private const double DotLength = 1.2;

		// Token: 0x04002642 RID: 9794
		private const double NormalLineHeightRatio = 0.06;

		// Token: 0x04002643 RID: 9795
		private const double BoldLineHeightRatio = 0.08;

		// Token: 0x04002644 RID: 9796
		private const double NormalDotLineHeightRatio = 0.08;

		// Token: 0x04002645 RID: 9797
		private const double BoldDotLineHeightRatio = 0.1;

		// Token: 0x04002646 RID: 9798
		private const double NormalDashRatio = 0.27;

		// Token: 0x04002647 RID: 9799
		private const double BoldDashRatio = 0.39;

		// Token: 0x04002648 RID: 9800
		private const double ClauseGapRatio = 0.09;

		// Token: 0x04002649 RID: 9801
		private const double NormalDashGapRatio = 0.04;

		// Token: 0x0400264A RID: 9802
		private const double BoldDashGapRatio = 0.06;

		// Token: 0x0400264B RID: 9803
		private const string chinesePinyin = "zh-CN";

		// Token: 0x02000B2C RID: 2860
		private class AttributeRange
		{
			// Token: 0x06008C82 RID: 35970 RVA: 0x0033CE9B File Offset: 0x0033BE9B
			internal AttributeRange(ITextView textView, ITextPointer start, ITextPointer end, TextServicesDisplayAttribute textServicesDisplayAttribute)
			{
				this._textView = textView;
				this._startOffset = start.Offset;
				this._endOffset = end.Offset;
				this._textServicesDisplayAttribute = textServicesDisplayAttribute;
				this._compositionLines = new ArrayList(1);
			}

			// Token: 0x06008C83 RID: 35971 RVA: 0x0033CED8 File Offset: 0x0033BED8
			internal void AddCompositionLines()
			{
				this._compositionLines.Clear();
				ITextPointer textPointer = this._textView.TextContainer.Start.CreatePointer(this._startOffset, LogicalDirection.Forward);
				ITextPointer textPointer2 = this._textView.TextContainer.Start.CreatePointer(this._endOffset, LogicalDirection.Backward);
				while (textPointer.CompareTo(textPointer2) < 0 && textPointer.GetPointerContext(LogicalDirection.Forward) != TextPointerContext.Text)
				{
					textPointer.MoveToNextContextPosition(LogicalDirection.Forward);
				}
				Invariant.Assert(textPointer.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text);
				if (textPointer2.HasValidLayout)
				{
					this._startRect = this._textView.GetRectangleFromTextPosition(textPointer);
					this._endRect = this._textView.GetRectangleFromTextPosition(textPointer2);
					if (this._startRect.Top != this._endRect.Top)
					{
						this.AddMultipleCompositionLines(textPointer, textPointer2);
						return;
					}
					Color lineColor = this._textServicesDisplayAttribute.GetLineColor(textPointer);
					this._compositionLines.Add(new CompositionAdorner.CompositionLine(this._startRect, this._endRect, lineColor));
				}
			}

			// Token: 0x17001EC4 RID: 7876
			// (get) Token: 0x06008C84 RID: 35972 RVA: 0x0033CFCF File Offset: 0x0033BFCF
			internal double Height
			{
				get
				{
					return this._startRect.Bottom - this._startRect.Top;
				}
			}

			// Token: 0x17001EC5 RID: 7877
			// (get) Token: 0x06008C85 RID: 35973 RVA: 0x0033CFE8 File Offset: 0x0033BFE8
			internal ArrayList CompositionLines
			{
				get
				{
					return this._compositionLines;
				}
			}

			// Token: 0x17001EC6 RID: 7878
			// (get) Token: 0x06008C86 RID: 35974 RVA: 0x0033CFF0 File Offset: 0x0033BFF0
			internal TextServicesDisplayAttribute TextServicesDisplayAttribute
			{
				get
				{
					return this._textServicesDisplayAttribute;
				}
			}

			// Token: 0x06008C87 RID: 35975 RVA: 0x0033CFF8 File Offset: 0x0033BFF8
			private void AddMultipleCompositionLines(ITextPointer start, ITextPointer end)
			{
				ITextPointer textPointer = start;
				ITextPointer textPointer2 = textPointer;
				while (textPointer2.CompareTo(end) < 0)
				{
					TextSegment lineRange = this._textView.GetLineRange(textPointer2);
					if (lineRange.IsNull)
					{
						textPointer = textPointer2;
					}
					else
					{
						if (textPointer.CompareTo(lineRange.Start) < 0)
						{
							textPointer = lineRange.Start;
						}
						if (textPointer2.CompareTo(lineRange.End) < 0)
						{
							if (end.CompareTo(lineRange.End) < 0)
							{
								textPointer2 = end.CreatePointer();
							}
							else
							{
								textPointer2 = lineRange.End.CreatePointer(LogicalDirection.Backward);
							}
						}
						Rect rectangleFromTextPosition = this._textView.GetRectangleFromTextPosition(textPointer);
						Rect rectangleFromTextPosition2 = this._textView.GetRectangleFromTextPosition(textPointer2);
						this._compositionLines.Add(new CompositionAdorner.CompositionLine(rectangleFromTextPosition, rectangleFromTextPosition2, this._textServicesDisplayAttribute.GetLineColor(textPointer)));
						textPointer = lineRange.End.CreatePointer(LogicalDirection.Forward);
					}
					while (textPointer.GetPointerContext(LogicalDirection.Forward) != TextPointerContext.None && textPointer.GetPointerContext(LogicalDirection.Forward) != TextPointerContext.Text)
					{
						textPointer.MoveToNextContextPosition(LogicalDirection.Forward);
					}
					textPointer2 = textPointer;
				}
			}

			// Token: 0x040047ED RID: 18413
			private ITextView _textView;

			// Token: 0x040047EE RID: 18414
			private Rect _startRect;

			// Token: 0x040047EF RID: 18415
			private Rect _endRect;

			// Token: 0x040047F0 RID: 18416
			private readonly int _startOffset;

			// Token: 0x040047F1 RID: 18417
			private readonly int _endOffset;

			// Token: 0x040047F2 RID: 18418
			private readonly TextServicesDisplayAttribute _textServicesDisplayAttribute;

			// Token: 0x040047F3 RID: 18419
			private readonly ArrayList _compositionLines;
		}

		// Token: 0x02000B2D RID: 2861
		private class CompositionLine
		{
			// Token: 0x06008C88 RID: 35976 RVA: 0x0033D0EF File Offset: 0x0033C0EF
			internal CompositionLine(Rect startRect, Rect endRect, Color lineColor)
			{
				this._startRect = startRect;
				this._endRect = endRect;
				this._color = lineColor;
			}

			// Token: 0x17001EC7 RID: 7879
			// (get) Token: 0x06008C89 RID: 35977 RVA: 0x0033D10C File Offset: 0x0033C10C
			internal Point StartPoint
			{
				get
				{
					return this._startRect.BottomLeft;
				}
			}

			// Token: 0x17001EC8 RID: 7880
			// (get) Token: 0x06008C8A RID: 35978 RVA: 0x0033D119 File Offset: 0x0033C119
			internal Point EndPoint
			{
				get
				{
					return this._endRect.BottomRight;
				}
			}

			// Token: 0x17001EC9 RID: 7881
			// (get) Token: 0x06008C8B RID: 35979 RVA: 0x0033D126 File Offset: 0x0033C126
			internal Rect StartRect
			{
				get
				{
					return this._startRect;
				}
			}

			// Token: 0x17001ECA RID: 7882
			// (get) Token: 0x06008C8C RID: 35980 RVA: 0x0033D12E File Offset: 0x0033C12E
			internal Rect EndRect
			{
				get
				{
					return this._endRect;
				}
			}

			// Token: 0x17001ECB RID: 7883
			// (get) Token: 0x06008C8D RID: 35981 RVA: 0x0033D136 File Offset: 0x0033C136
			internal Color LineColor
			{
				get
				{
					return this._color;
				}
			}

			// Token: 0x040047F4 RID: 18420
			private Rect _startRect;

			// Token: 0x040047F5 RID: 18421
			private Rect _endRect;

			// Token: 0x040047F6 RID: 18422
			private Color _color;
		}
	}
}
