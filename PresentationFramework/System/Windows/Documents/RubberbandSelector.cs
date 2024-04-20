using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.TextFormatting;
using MS.Internal;
using MS.Internal.Documents;

namespace System.Windows.Documents
{
	// Token: 0x02000682 RID: 1666
	internal sealed class RubberbandSelector
	{
		// Token: 0x0600528B RID: 21131 RVA: 0x00257F48 File Offset: 0x00256F48
		internal void ClearSelection()
		{
			if (this.HasSelection)
			{
				FixedPage page = this._page;
				this._page = null;
				this.UpdateHighlightVisual(page);
			}
			this._selectionRect = Rect.Empty;
		}

		// Token: 0x0600528C RID: 21132 RVA: 0x00257F80 File Offset: 0x00256F80
		internal void AttachRubberbandSelector(FrameworkElement scope)
		{
			if (scope == null)
			{
				throw new ArgumentNullException("scope");
			}
			this.ClearSelection();
			scope.MouseLeftButtonDown += this.OnLeftMouseDown;
			scope.MouseLeftButtonUp += this.OnLeftMouseUp;
			scope.MouseMove += this.OnMouseMove;
			scope.QueryCursor += this.OnQueryCursor;
			scope.Cursor = null;
			if (scope is DocumentGrid)
			{
				this._uiScope = ((DocumentGrid)scope).DocumentViewerOwner;
				Invariant.Assert(this._uiScope != null, "DocumentGrid's DocumentViewerOwner cannot be null.");
			}
			else
			{
				this._uiScope = scope;
			}
			CommandBinding commandBinding = new CommandBinding(ApplicationCommands.Copy);
			commandBinding.Executed += this.OnCopy;
			commandBinding.CanExecute += this.QueryCopy;
			this._uiScope.CommandBindings.Add(commandBinding);
			this._scope = scope;
		}

		// Token: 0x0600528D RID: 21133 RVA: 0x00258070 File Offset: 0x00257070
		internal void DetachRubberbandSelector()
		{
			this.ClearSelection();
			if (this._scope != null)
			{
				this._scope.MouseLeftButtonDown -= this.OnLeftMouseDown;
				this._scope.MouseLeftButtonUp -= this.OnLeftMouseUp;
				this._scope.MouseMove -= this.OnMouseMove;
				this._scope.QueryCursor -= this.OnQueryCursor;
				this._scope = null;
			}
			if (this._uiScope != null)
			{
				foreach (object obj in this._uiScope.CommandBindings)
				{
					CommandBinding commandBinding = (CommandBinding)obj;
					if (commandBinding.Command == ApplicationCommands.Copy)
					{
						commandBinding.Executed -= this.OnCopy;
						commandBinding.CanExecute -= this.QueryCopy;
					}
				}
				this._uiScope = null;
			}
		}

		// Token: 0x0600528E RID: 21134 RVA: 0x00258178 File Offset: 0x00257178
		private void ExtendSelection(Point pt)
		{
			Size size = this._panel.ComputePageSize(this._page);
			if (pt.X < 0.0)
			{
				pt.X = 0.0;
			}
			else if (pt.X > size.Width)
			{
				pt.X = size.Width;
			}
			if (pt.Y < 0.0)
			{
				pt.Y = 0.0;
			}
			else if (pt.Y > size.Height)
			{
				pt.Y = size.Height;
			}
			this._selectionRect = new Rect(this._origin, pt);
			this.UpdateHighlightVisual(this._page);
		}

		// Token: 0x0600528F RID: 21135 RVA: 0x0025823C File Offset: 0x0025723C
		private void UpdateHighlightVisual(FixedPage page)
		{
			if (page != null)
			{
				HighlightVisual highlightVisual = HighlightVisual.GetHighlightVisual(page);
				if (highlightVisual != null)
				{
					highlightVisual.UpdateRubberbandSelection(this);
				}
			}
		}

		// Token: 0x06005290 RID: 21136 RVA: 0x00258260 File Offset: 0x00257260
		private void OnCopy(object sender, ExecutedRoutedEventArgs e)
		{
			if (this.HasSelection && this._selectionRect.Width > 0.0 && this._selectionRect.Height > 0.0)
			{
				string text = this.GetText();
				object bitmapFromBitmapSource = SystemDrawingHelper.GetBitmapFromBitmapSource(this.GetImage());
				IDataObject dataObject = new DataObject();
				dataObject.SetData(DataFormats.Text, text, true);
				dataObject.SetData(DataFormats.UnicodeText, text, true);
				if (bitmapFromBitmapSource != null)
				{
					dataObject.SetData(DataFormats.Bitmap, bitmapFromBitmapSource, true);
				}
				try
				{
					Clipboard.SetDataObject(dataObject, true);
				}
				catch (ExternalException)
				{
				}
			}
		}

		// Token: 0x06005291 RID: 21137 RVA: 0x00258304 File Offset: 0x00257304
		private BitmapSource GetImage()
		{
			Visual visual = this.GetVisual(-this._selectionRect.Left, -this._selectionRect.Top);
			double num = 96.0;
			double num2 = num / 96.0;
			RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int)(num2 * this._selectionRect.Width), (int)(num2 * this._selectionRect.Height), num, num, PixelFormats.Pbgra32);
			renderTargetBitmap.Render(visual);
			return renderTargetBitmap;
		}

		// Token: 0x06005292 RID: 21138 RVA: 0x00258378 File Offset: 0x00257378
		private Visual GetVisual(double offsetX, double offsetY)
		{
			ContainerVisual containerVisual = new ContainerVisual();
			DrawingVisual drawingVisual = new DrawingVisual();
			containerVisual.Children.Add(drawingVisual);
			drawingVisual.Offset = new Vector(offsetX, offsetY);
			DrawingContext drawingContext = drawingVisual.RenderOpen();
			drawingContext.DrawDrawing(this._page.GetDrawing());
			drawingContext.Close();
			foreach (object obj in this._page.Children)
			{
				UIElement old = (UIElement)obj;
				this.CloneVisualTree(drawingVisual, old);
			}
			return containerVisual;
		}

		// Token: 0x06005293 RID: 21139 RVA: 0x00258420 File Offset: 0x00257420
		private void CloneVisualTree(ContainerVisual parent, Visual old)
		{
			DrawingVisual drawingVisual = new DrawingVisual();
			parent.Children.Add(drawingVisual);
			drawingVisual.Clip = VisualTreeHelper.GetClip(old);
			drawingVisual.Offset = VisualTreeHelper.GetOffset(old);
			drawingVisual.Transform = VisualTreeHelper.GetTransform(old);
			drawingVisual.Opacity = VisualTreeHelper.GetOpacity(old);
			drawingVisual.OpacityMask = VisualTreeHelper.GetOpacityMask(old);
			drawingVisual.BitmapEffectInput = VisualTreeHelper.GetBitmapEffectInput(old);
			drawingVisual.BitmapEffect = VisualTreeHelper.GetBitmapEffect(old);
			DrawingContext drawingContext = drawingVisual.RenderOpen();
			drawingContext.DrawDrawing(old.GetDrawing());
			drawingContext.Close();
			int childrenCount = VisualTreeHelper.GetChildrenCount(old);
			for (int i = 0; i < childrenCount; i++)
			{
				Visual old2 = old.InternalGetVisualChild(i);
				this.CloneVisualTree(drawingVisual, old2);
			}
		}

		// Token: 0x06005294 RID: 21140 RVA: 0x002584D0 File Offset: 0x002574D0
		private string GetText()
		{
			double top = this._selectionRect.Top;
			double bottom = this._selectionRect.Bottom;
			double left = this._selectionRect.Left;
			double right = this._selectionRect.Right;
			double num = 0.0;
			double num2 = 0.0;
			int count = this._page.Children.Count;
			ArrayList arrayList = new ArrayList();
			FixedNode[] array = this._panel.FixedContainer.FixedTextBuilder.GetFirstLine(this._pageIndex);
			while (array != null && array.Length != 0)
			{
				RubberbandSelector.TextPositionPair textPositionPair = null;
				foreach (FixedNode node in array)
				{
					Glyphs glyphsElement = this._page.GetGlyphsElement(node);
					if (glyphsElement != null)
					{
						int num3;
						int charIndex;
						bool flag;
						if (this.IntersectGlyphs(glyphsElement, top, left, bottom, right, out num3, out charIndex, out flag, out num, out num2))
						{
							if (textPositionPair == null || num3 > 0)
							{
								textPositionPair = new RubberbandSelector.TextPositionPair();
								textPositionPair.first = this._GetTextPosition(node, num3);
								arrayList.Add(textPositionPair);
							}
							textPositionPair.second = this._GetTextPosition(node, charIndex);
							if (!flag)
							{
								textPositionPair = null;
							}
						}
						else
						{
							textPositionPair = null;
						}
					}
				}
				int num4 = 1;
				array = this._panel.FixedContainer.FixedTextBuilder.GetNextLine(array[0], true, ref num4);
			}
			string text = "";
			foreach (object obj in arrayList)
			{
				RubberbandSelector.TextPositionPair textPositionPair2 = (RubberbandSelector.TextPositionPair)obj;
				text = text + TextRangeBase.GetTextInternal(textPositionPair2.first, textPositionPair2.second) + "\r\n";
			}
			return text;
		}

		// Token: 0x06005295 RID: 21141 RVA: 0x002586A0 File Offset: 0x002576A0
		private ITextPointer _GetTextPosition(FixedNode node, int charIndex)
		{
			FixedPosition fixedPosition = new FixedPosition(node, charIndex);
			FlowPosition flowPosition = this._panel.FixedContainer.FixedTextBuilder.CreateFlowPosition(fixedPosition);
			if (flowPosition != null)
			{
				return new FixedTextPointer(false, LogicalDirection.Forward, flowPosition);
			}
			return null;
		}

		// Token: 0x06005296 RID: 21142 RVA: 0x002586DC File Offset: 0x002576DC
		private bool IntersectGlyphs(Glyphs g, double top, double left, double bottom, double right, out int begin, out int end, out bool includeEnd, out double baseline, out double height)
		{
			begin = 0;
			end = 0;
			includeEnd = false;
			GlyphRun glyphRun = g.ToGlyphRun();
			Rect rect = glyphRun.ComputeAlignmentBox();
			rect.Offset(glyphRun.BaselineOrigin.X, glyphRun.BaselineOrigin.Y);
			baseline = glyphRun.BaselineOrigin.Y;
			height = rect.Height;
			double y = rect.Y + 0.5 * rect.Height;
			GeneralTransform generalTransform = g.TransformToAncestor(this._page);
			Point point;
			generalTransform.TryTransform(new Point(rect.Left, y), out point);
			Point point2;
			generalTransform.TryTransform(new Point(rect.Right, y), out point2);
			bool flag = false;
			if (point.X < left)
			{
				if (point2.X < left)
				{
					return false;
				}
				flag = true;
			}
			else if (point.X > right)
			{
				if (point2.X > right)
				{
					return false;
				}
				flag = true;
			}
			else if (point2.X < left || point2.X > right)
			{
				flag = true;
			}
			double num3;
			double num4;
			if (flag)
			{
				double num = (left - point.X) / (point2.X - point.X);
				double num2 = (right - point.X) / (point2.X - point.X);
				if (num2 > num)
				{
					num3 = num;
					num4 = num2;
				}
				else
				{
					num3 = num2;
					num4 = num;
				}
			}
			else
			{
				num3 = 0.0;
				num4 = 1.0;
			}
			flag = false;
			if (point.Y < top)
			{
				if (point2.Y < top)
				{
					return false;
				}
				flag = true;
			}
			else if (point.Y > bottom)
			{
				if (point2.Y > bottom)
				{
					return false;
				}
				flag = true;
			}
			else if (point2.Y < top || point2.Y > bottom)
			{
				flag = true;
			}
			if (flag)
			{
				double num5 = (top - point.Y) / (point2.Y - point.Y);
				double num6 = (bottom - point.Y) / (point2.Y - point.Y);
				if (num6 > num5)
				{
					if (num5 > num3)
					{
						num3 = num5;
					}
					if (num6 < num4)
					{
						num4 = num6;
					}
				}
				else
				{
					if (num6 > num3)
					{
						num3 = num6;
					}
					if (num5 < num4)
					{
						num4 = num5;
					}
				}
			}
			num3 = rect.Left + rect.Width * num3;
			num4 = rect.Left + rect.Width * num4;
			bool ltr = (glyphRun.BidiLevel & 1) == 0;
			begin = this.GlyphRunHitTest(glyphRun, num3, ltr);
			end = this.GlyphRunHitTest(glyphRun, num4, ltr);
			if (begin > end)
			{
				int num7 = begin;
				begin = end;
				end = num7;
			}
			int num8 = (glyphRun.Characters == null) ? 0 : glyphRun.Characters.Count;
			includeEnd = (end == num8);
			return true;
		}

		// Token: 0x06005297 RID: 21143 RVA: 0x002589A0 File Offset: 0x002579A0
		private int GlyphRunHitTest(GlyphRun run, double xoffset, bool LTR)
		{
			double distance = LTR ? (xoffset - run.BaselineOrigin.X) : (run.BaselineOrigin.X - xoffset);
			bool flag;
			CharacterHit caretCharacterHitFromDistance = run.GetCaretCharacterHitFromDistance(distance, out flag);
			return caretCharacterHitFromDistance.FirstCharacterIndex + caretCharacterHitFromDistance.TrailingLength;
		}

		// Token: 0x06005298 RID: 21144 RVA: 0x002589EC File Offset: 0x002579EC
		private void QueryCopy(object sender, CanExecuteRoutedEventArgs e)
		{
			if (this.HasSelection)
			{
				e.CanExecute = true;
			}
		}

		// Token: 0x06005299 RID: 21145 RVA: 0x00258A00 File Offset: 0x00257A00
		private void OnLeftMouseDown(object sender, MouseButtonEventArgs e)
		{
			e.Handled = true;
			FixedDocumentPage fixedPanelDocumentPage = this.GetFixedPanelDocumentPage(e.GetPosition(this._scope));
			if (fixedPanelDocumentPage != null)
			{
				this._uiScope.Focus();
				this._scope.CaptureMouse();
				this.ClearSelection();
				this._panel = fixedPanelDocumentPage.Owner;
				this._page = fixedPanelDocumentPage.FixedPage;
				this._isSelecting = true;
				this._origin = e.GetPosition(this._page);
				this._pageIndex = fixedPanelDocumentPage.PageIndex;
			}
		}

		// Token: 0x0600529A RID: 21146 RVA: 0x00258A85 File Offset: 0x00257A85
		private void OnLeftMouseUp(object sender, MouseButtonEventArgs e)
		{
			e.Handled = true;
			this._scope.ReleaseMouseCapture();
			if (this._isSelecting)
			{
				this._isSelecting = false;
				if (this._page != null)
				{
					this.ExtendSelection(e.GetPosition(this._page));
				}
			}
		}

		// Token: 0x0600529B RID: 21147 RVA: 0x00258AC2 File Offset: 0x00257AC2
		private void OnMouseMove(object sender, MouseEventArgs e)
		{
			e.Handled = true;
			if (e.LeftButton == MouseButtonState.Released)
			{
				this._isSelecting = false;
				return;
			}
			if (this._isSelecting && this._page != null)
			{
				this.ExtendSelection(e.GetPosition(this._page));
			}
		}

		// Token: 0x0600529C RID: 21148 RVA: 0x00258AFD File Offset: 0x00257AFD
		private void OnQueryCursor(object sender, QueryCursorEventArgs e)
		{
			if (this._isSelecting || this.GetFixedPanelDocumentPage(e.GetPosition(this._scope)) != null)
			{
				e.Cursor = Cursors.Cross;
			}
			else
			{
				e.Cursor = Cursors.Arrow;
			}
			e.Handled = true;
		}

		// Token: 0x0600529D RID: 21149 RVA: 0x00258B3C File Offset: 0x00257B3C
		private FixedDocumentPage GetFixedPanelDocumentPage(Point pt)
		{
			DocumentGrid documentGrid = this._scope as DocumentGrid;
			if (documentGrid != null)
			{
				DocumentPage documentPageFromPoint = documentGrid.GetDocumentPageFromPoint(pt);
				FixedDocumentPage fixedDocumentPage = documentPageFromPoint as FixedDocumentPage;
				if (fixedDocumentPage == null)
				{
					FixedDocumentSequenceDocumentPage fixedDocumentSequenceDocumentPage = documentPageFromPoint as FixedDocumentSequenceDocumentPage;
					if (fixedDocumentSequenceDocumentPage != null)
					{
						fixedDocumentPage = (fixedDocumentSequenceDocumentPage.ChildDocumentPage as FixedDocumentPage);
					}
				}
				return fixedDocumentPage;
			}
			return null;
		}

		// Token: 0x1700137E RID: 4990
		// (get) Token: 0x0600529E RID: 21150 RVA: 0x00258B83 File Offset: 0x00257B83
		internal FixedPage Page
		{
			get
			{
				return this._page;
			}
		}

		// Token: 0x1700137F RID: 4991
		// (get) Token: 0x0600529F RID: 21151 RVA: 0x00258B8B File Offset: 0x00257B8B
		internal Rect SelectionRect
		{
			get
			{
				return this._selectionRect;
			}
		}

		// Token: 0x17001380 RID: 4992
		// (get) Token: 0x060052A0 RID: 21152 RVA: 0x00258B93 File Offset: 0x00257B93
		internal bool HasSelection
		{
			get
			{
				return this._page != null && this._panel != null && !this._selectionRect.IsEmpty;
			}
		}

		// Token: 0x04002EA9 RID: 11945
		private FixedDocument _panel;

		// Token: 0x04002EAA RID: 11946
		private FixedPage _page;

		// Token: 0x04002EAB RID: 11947
		private Rect _selectionRect;

		// Token: 0x04002EAC RID: 11948
		private bool _isSelecting;

		// Token: 0x04002EAD RID: 11949
		private Point _origin;

		// Token: 0x04002EAE RID: 11950
		private UIElement _scope;

		// Token: 0x04002EAF RID: 11951
		private FrameworkElement _uiScope;

		// Token: 0x04002EB0 RID: 11952
		private int _pageIndex;

		// Token: 0x02000B4C RID: 2892
		private class TextPositionPair
		{
			// Token: 0x04004880 RID: 18560
			public ITextPointer first;

			// Token: 0x04004881 RID: 18561
			public ITextPointer second;
		}
	}
}
