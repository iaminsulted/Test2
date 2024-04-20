using System;
using System.Collections;
using System.Windows.Media;

namespace System.Windows.Documents
{
	// Token: 0x02000625 RID: 1573
	internal sealed class HighlightVisual : Adorner
	{
		// Token: 0x06004D81 RID: 19841 RVA: 0x002400A4 File Offset: 0x0023F0A4
		internal HighlightVisual(FixedDocument panel, FixedPage page) : base(page)
		{
			this._panel = panel;
			this._page = page;
		}

		// Token: 0x06004D82 RID: 19842 RVA: 0x00109403 File Offset: 0x00108403
		protected override GeometryHitTestResult HitTestCore(GeometryHitTestParameters hitTestParameters)
		{
			return null;
		}

		// Token: 0x06004D83 RID: 19843 RVA: 0x00109403 File Offset: 0x00108403
		protected override HitTestResult HitTestCore(PointHitTestParameters hitTestParameters)
		{
			return null;
		}

		// Token: 0x06004D84 RID: 19844 RVA: 0x002400BC File Offset: 0x0023F0BC
		protected override void OnRender(DrawingContext dc)
		{
			if (this._panel.Highlights.ContainsKey(this._page))
			{
				ArrayList arrayList = this._panel.Highlights[this._page];
				Size size = this._panel.ComputePageSize(this._page);
				Rect rect = new Rect(new Point(0.0, 0.0), size);
				dc.PushClip(new RectangleGeometry(rect));
				if (arrayList != null)
				{
					this._UpdateHighlightBackground(dc, arrayList);
					this._UpdateHighlightForeground(dc, arrayList);
				}
				dc.Pop();
			}
			if (this._rubberbandSelector != null && this._rubberbandSelector.Page == this._page)
			{
				Rect selectionRect = this._rubberbandSelector.SelectionRect;
				if (!selectionRect.IsEmpty)
				{
					dc.DrawRectangle(SelectionHighlightInfo.ObjectMaskBrush, null, selectionRect);
				}
			}
		}

		// Token: 0x06004D85 RID: 19845 RVA: 0x0024018C File Offset: 0x0023F18C
		internal void InvalidateHighlights()
		{
			AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this._page);
			if (adornerLayer == null)
			{
				return;
			}
			adornerLayer.Update(this._page);
		}

		// Token: 0x06004D86 RID: 19846 RVA: 0x002401B5 File Offset: 0x0023F1B5
		internal void UpdateRubberbandSelection(RubberbandSelector selector)
		{
			this._rubberbandSelector = selector;
			this.InvalidateHighlights();
		}

		// Token: 0x06004D87 RID: 19847 RVA: 0x002401C4 File Offset: 0x0023F1C4
		internal static HighlightVisual GetHighlightVisual(FixedPage page)
		{
			AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(page);
			if (adornerLayer == null)
			{
				return null;
			}
			Adorner[] adorners = adornerLayer.GetAdorners(page);
			if (adorners != null)
			{
				Adorner[] array = adorners;
				for (int i = 0; i < array.Length; i++)
				{
					HighlightVisual highlightVisual = array[i] as HighlightVisual;
					if (highlightVisual != null)
					{
						return highlightVisual;
					}
				}
			}
			return null;
		}

		// Token: 0x06004D88 RID: 19848 RVA: 0x0024020C File Offset: 0x0023F20C
		private void _UpdateHighlightBackground(DrawingContext dc, ArrayList highlights)
		{
			PathGeometry pathGeometry = null;
			Brush brush = null;
			Rect rect = Rect.Empty;
			foreach (object obj in highlights)
			{
				FixedHighlight fixedHighlight = (FixedHighlight)obj;
				Brush brush2 = null;
				if (fixedHighlight.HighlightType != FixedHighlightType.None)
				{
					Rect rect2 = fixedHighlight.ComputeDesignRect();
					if (!(rect2 == Rect.Empty))
					{
						GeneralTransform generalTransform = fixedHighlight.Element.TransformToAncestor(this._page);
						Transform transform = generalTransform.AffineTransform;
						if (transform == null)
						{
							transform = Transform.Identity;
						}
						Glyphs glyphs = fixedHighlight.Glyphs;
						if (fixedHighlight.HighlightType == FixedHighlightType.TextSelection)
						{
							brush2 = ((glyphs == null) ? SelectionHighlightInfo.ObjectMaskBrush : SelectionHighlightInfo.BackgroundBrush);
						}
						else if (fixedHighlight.HighlightType == FixedHighlightType.AnnotationHighlight)
						{
							brush2 = fixedHighlight.BackgroundBrush;
						}
						if (fixedHighlight.Element.Clip != null)
						{
							Rect bounds = fixedHighlight.Element.Clip.Bounds;
							rect2.Intersect(bounds);
						}
						Geometry geometry = new RectangleGeometry(rect2);
						geometry.Transform = transform;
						rect2 = generalTransform.TransformBounds(rect2);
						if (brush2 != brush || rect2.Top > rect.Bottom + 0.1 || rect2.Bottom + 0.1 < rect.Top || rect2.Left > rect.Right + 0.1 || rect2.Right + 0.1 < rect.Left)
						{
							if (brush != null)
							{
								pathGeometry.FillRule = FillRule.Nonzero;
								dc.DrawGeometry(brush, null, pathGeometry);
							}
							brush = brush2;
							pathGeometry = new PathGeometry();
							pathGeometry.AddGeometry(geometry);
							rect = rect2;
						}
						else
						{
							pathGeometry.AddGeometry(geometry);
							rect.Union(rect2);
						}
					}
				}
			}
			if (brush != null)
			{
				pathGeometry.FillRule = FillRule.Nonzero;
				dc.DrawGeometry(brush, null, pathGeometry);
			}
		}

		// Token: 0x06004D89 RID: 19849 RVA: 0x00240400 File Offset: 0x0023F400
		private void _UpdateHighlightForeground(DrawingContext dc, ArrayList highlights)
		{
			foreach (object obj in highlights)
			{
				FixedHighlight fixedHighlight = (FixedHighlight)obj;
				Brush brush = null;
				if (fixedHighlight.HighlightType != FixedHighlightType.None)
				{
					Glyphs glyphs = fixedHighlight.Glyphs;
					if (glyphs != null)
					{
						Rect rect = fixedHighlight.ComputeDesignRect();
						if (!(rect == Rect.Empty))
						{
							Transform affineTransform = fixedHighlight.Element.TransformToAncestor(this._page).AffineTransform;
							if (affineTransform != null)
							{
								dc.PushTransform(affineTransform);
							}
							else
							{
								dc.PushTransform(Transform.Identity);
							}
							dc.PushClip(new RectangleGeometry(rect));
							if (fixedHighlight.HighlightType == FixedHighlightType.TextSelection)
							{
								brush = SelectionHighlightInfo.ForegroundBrush;
							}
							else if (fixedHighlight.HighlightType == FixedHighlightType.AnnotationHighlight)
							{
								brush = fixedHighlight.ForegroundBrush;
							}
							GlyphRun glyphRun = glyphs.ToGlyphRun();
							if (brush == null)
							{
								brush = glyphs.Fill;
							}
							dc.PushGuidelineY1(glyphRun.BaselineOrigin.Y);
							dc.PushClip(glyphs.Clip);
							dc.DrawGlyphRun(brush, glyphRun);
							dc.Pop();
							dc.Pop();
							dc.Pop();
							dc.Pop();
						}
					}
				}
			}
		}

		// Token: 0x0400280F RID: 10255
		private FixedDocument _panel;

		// Token: 0x04002810 RID: 10256
		private RubberbandSelector _rubberbandSelector;

		// Token: 0x04002811 RID: 10257
		private FixedPage _page;
	}
}
