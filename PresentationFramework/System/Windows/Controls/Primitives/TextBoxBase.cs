using System;
using System.Collections.ObjectModel;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using MS.Internal;
using MS.Internal.Documents;
using MS.Internal.KnownBoxes;

namespace System.Windows.Controls.Primitives
{
	// Token: 0x02000859 RID: 2137
	[TemplatePart(Name = "PART_ContentHost", Type = typeof(FrameworkElement))]
	[Localizability(LocalizationCategory.Text)]
	public abstract class TextBoxBase : Control
	{
		// Token: 0x06007D9E RID: 32158 RVA: 0x00315110 File Offset: 0x00314110
		static TextBoxBase()
		{
			TextBoxBase.TextChangedEvent = EventManager.RegisterRoutedEvent("TextChanged", RoutingStrategy.Bubble, typeof(TextChangedEventHandler), typeof(TextBoxBase));
			TextBoxBase.SelectionChangedEvent = EventManager.RegisterRoutedEvent("SelectionChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TextBoxBase));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(TextBoxBase), new FrameworkPropertyMetadata(typeof(TextBoxBase)));
			TextBoxBase._dType = DependencyObjectType.FromSystemTypeInternal(typeof(TextBoxBase));
			Control.PaddingProperty.OverrideMetadata(typeof(TextBoxBase), new FrameworkPropertyMetadata(new PropertyChangedCallback(TextBoxBase.OnScrollViewerPropertyChanged)));
			InputMethod.IsInputMethodEnabledProperty.OverrideMetadata(typeof(TextBoxBase), new FrameworkPropertyMetadata(new PropertyChangedCallback(TextBoxBase.OnInputMethodEnabledPropertyChanged)));
			UIElement.IsEnabledProperty.OverrideMetadata(typeof(TextBoxBase), new UIPropertyMetadata(new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));
			UIElement.IsMouseOverPropertyKey.OverrideMetadata(typeof(TextBoxBase), new UIPropertyMetadata(new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));
		}

		// Token: 0x06007D9F RID: 32159 RVA: 0x00315559 File Offset: 0x00314559
		internal TextBoxBase()
		{
			base.CoerceValue(TextBoxBase.HorizontalScrollBarVisibilityProperty);
		}

		// Token: 0x06007DA0 RID: 32160 RVA: 0x0031556C File Offset: 0x0031456C
		public void AppendText(string textData)
		{
			if (textData == null)
			{
				return;
			}
			new TextRange(this._textContainer.End, this._textContainer.End).Text = textData;
		}

		// Token: 0x06007DA1 RID: 32161 RVA: 0x00315593 File Offset: 0x00314593
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this.AttachToVisualTree();
		}

		// Token: 0x06007DA2 RID: 32162 RVA: 0x003155A1 File Offset: 0x003145A1
		public void Copy()
		{
			TextEditorCopyPaste.Copy(this.TextEditor, false);
		}

		// Token: 0x06007DA3 RID: 32163 RVA: 0x003155AF File Offset: 0x003145AF
		public void Cut()
		{
			TextEditorCopyPaste.Cut(this.TextEditor, false);
		}

		// Token: 0x06007DA4 RID: 32164 RVA: 0x003155BD File Offset: 0x003145BD
		public void Paste()
		{
			TextEditorCopyPaste.Paste(this.TextEditor);
		}

		// Token: 0x06007DA5 RID: 32165 RVA: 0x003155CC File Offset: 0x003145CC
		public void SelectAll()
		{
			using (this.TextSelectionInternal.DeclareChangeBlock())
			{
				this.TextSelectionInternal.Select(this._textContainer.Start, this._textContainer.End);
			}
		}

		// Token: 0x06007DA6 RID: 32166 RVA: 0x00315624 File Offset: 0x00314624
		public void LineLeft()
		{
			if (this.ScrollViewer != null)
			{
				base.UpdateLayout();
				this.ScrollViewer.LineLeft();
			}
		}

		// Token: 0x06007DA7 RID: 32167 RVA: 0x0031563F File Offset: 0x0031463F
		public void LineRight()
		{
			if (this.ScrollViewer != null)
			{
				base.UpdateLayout();
				this.ScrollViewer.LineRight();
			}
		}

		// Token: 0x06007DA8 RID: 32168 RVA: 0x0031565A File Offset: 0x0031465A
		public void PageLeft()
		{
			if (this.ScrollViewer != null)
			{
				base.UpdateLayout();
				this.ScrollViewer.PageLeft();
			}
		}

		// Token: 0x06007DA9 RID: 32169 RVA: 0x00315675 File Offset: 0x00314675
		public void PageRight()
		{
			if (this.ScrollViewer != null)
			{
				base.UpdateLayout();
				this.ScrollViewer.PageRight();
			}
		}

		// Token: 0x06007DAA RID: 32170 RVA: 0x00315690 File Offset: 0x00314690
		public void LineUp()
		{
			base.UpdateLayout();
			this.DoLineUp();
		}

		// Token: 0x06007DAB RID: 32171 RVA: 0x0031569E File Offset: 0x0031469E
		public void LineDown()
		{
			base.UpdateLayout();
			this.DoLineDown();
		}

		// Token: 0x06007DAC RID: 32172 RVA: 0x003156AC File Offset: 0x003146AC
		public void PageUp()
		{
			if (this.ScrollViewer != null)
			{
				base.UpdateLayout();
				this.ScrollViewer.PageUp();
			}
		}

		// Token: 0x06007DAD RID: 32173 RVA: 0x003156C7 File Offset: 0x003146C7
		public void PageDown()
		{
			if (this.ScrollViewer != null)
			{
				base.UpdateLayout();
				this.ScrollViewer.PageDown();
			}
		}

		// Token: 0x06007DAE RID: 32174 RVA: 0x003156E2 File Offset: 0x003146E2
		public void ScrollToHome()
		{
			if (this.ScrollViewer != null)
			{
				base.UpdateLayout();
				this.ScrollViewer.ScrollToHome();
			}
		}

		// Token: 0x06007DAF RID: 32175 RVA: 0x003156FD File Offset: 0x003146FD
		public void ScrollToEnd()
		{
			if (this.ScrollViewer != null)
			{
				base.UpdateLayout();
				this.ScrollViewer.ScrollToEnd();
			}
		}

		// Token: 0x06007DB0 RID: 32176 RVA: 0x00315718 File Offset: 0x00314718
		public void ScrollToHorizontalOffset(double offset)
		{
			if (double.IsNaN(offset))
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (this.ScrollViewer != null)
			{
				base.UpdateLayout();
				this.ScrollViewer.ScrollToHorizontalOffset(offset);
			}
		}

		// Token: 0x06007DB1 RID: 32177 RVA: 0x00315747 File Offset: 0x00314747
		public void ScrollToVerticalOffset(double offset)
		{
			if (double.IsNaN(offset))
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (this.ScrollViewer != null)
			{
				base.UpdateLayout();
				this.ScrollViewer.ScrollToVerticalOffset(offset);
			}
		}

		// Token: 0x06007DB2 RID: 32178 RVA: 0x00315778 File Offset: 0x00314778
		public bool Undo()
		{
			UndoManager undoManager = UndoManager.GetUndoManager(this);
			if (undoManager != null && undoManager.UndoCount > undoManager.MinUndoStackCount)
			{
				this.TextEditor.Undo();
				return true;
			}
			return false;
		}

		// Token: 0x06007DB3 RID: 32179 RVA: 0x003157AC File Offset: 0x003147AC
		public bool Redo()
		{
			UndoManager undoManager = UndoManager.GetUndoManager(this);
			if (undoManager != null && undoManager.RedoCount > 0)
			{
				this.TextEditor.Redo();
				return true;
			}
			return false;
		}

		// Token: 0x06007DB4 RID: 32180 RVA: 0x003157DC File Offset: 0x003147DC
		public void LockCurrentUndoUnit()
		{
			UndoManager undoManager = UndoManager.GetUndoManager(this);
			if (undoManager != null)
			{
				IParentUndoUnit openedUnit = undoManager.OpenedUnit;
				if (openedUnit != null)
				{
					while (openedUnit.OpenedUnit != null)
					{
						openedUnit = openedUnit.OpenedUnit;
					}
					if (openedUnit.LastUnit is IParentUndoUnit)
					{
						openedUnit.OnNextAdd();
						return;
					}
				}
				else if (undoManager.LastUnit is IParentUndoUnit)
				{
					((IParentUndoUnit)undoManager.LastUnit).OnNextAdd();
				}
			}
		}

		// Token: 0x06007DB5 RID: 32181 RVA: 0x0031583F File Offset: 0x0031483F
		public void BeginChange()
		{
			this.TextEditor.Selection.BeginChange();
		}

		// Token: 0x06007DB6 RID: 32182 RVA: 0x00315851 File Offset: 0x00314851
		public void EndChange()
		{
			if (this.TextEditor.Selection.ChangeBlockLevel == 0)
			{
				throw new InvalidOperationException(SR.Get("TextBoxBase_UnmatchedEndChange"));
			}
			this.TextEditor.Selection.EndChange();
		}

		// Token: 0x06007DB7 RID: 32183 RVA: 0x00315885 File Offset: 0x00314885
		public IDisposable DeclareChangeBlock()
		{
			return this.TextEditor.Selection.DeclareChangeBlock();
		}

		// Token: 0x17001CF8 RID: 7416
		// (get) Token: 0x06007DB8 RID: 32184 RVA: 0x00315897 File Offset: 0x00314897
		// (set) Token: 0x06007DB9 RID: 32185 RVA: 0x003158A9 File Offset: 0x003148A9
		public bool IsReadOnly
		{
			get
			{
				return (bool)base.GetValue(TextEditor.IsReadOnlyProperty);
			}
			set
			{
				base.SetValue(TextEditor.IsReadOnlyProperty, value);
			}
		}

		// Token: 0x17001CF9 RID: 7417
		// (get) Token: 0x06007DBA RID: 32186 RVA: 0x003158B7 File Offset: 0x003148B7
		// (set) Token: 0x06007DBB RID: 32187 RVA: 0x003158C9 File Offset: 0x003148C9
		public bool IsReadOnlyCaretVisible
		{
			get
			{
				return (bool)base.GetValue(TextBoxBase.IsReadOnlyCaretVisibleProperty);
			}
			set
			{
				base.SetValue(TextBoxBase.IsReadOnlyCaretVisibleProperty, value);
			}
		}

		// Token: 0x17001CFA RID: 7418
		// (get) Token: 0x06007DBC RID: 32188 RVA: 0x003158D7 File Offset: 0x003148D7
		// (set) Token: 0x06007DBD RID: 32189 RVA: 0x003158E9 File Offset: 0x003148E9
		public bool AcceptsReturn
		{
			get
			{
				return (bool)base.GetValue(TextBoxBase.AcceptsReturnProperty);
			}
			set
			{
				base.SetValue(TextBoxBase.AcceptsReturnProperty, value);
			}
		}

		// Token: 0x17001CFB RID: 7419
		// (get) Token: 0x06007DBE RID: 32190 RVA: 0x003158F7 File Offset: 0x003148F7
		// (set) Token: 0x06007DBF RID: 32191 RVA: 0x00315909 File Offset: 0x00314909
		public bool AcceptsTab
		{
			get
			{
				return (bool)base.GetValue(TextBoxBase.AcceptsTabProperty);
			}
			set
			{
				base.SetValue(TextBoxBase.AcceptsTabProperty, value);
			}
		}

		// Token: 0x17001CFC RID: 7420
		// (get) Token: 0x06007DC0 RID: 32192 RVA: 0x00315917 File Offset: 0x00314917
		public SpellCheck SpellCheck
		{
			get
			{
				return new SpellCheck(this);
			}
		}

		// Token: 0x17001CFD RID: 7421
		// (get) Token: 0x06007DC1 RID: 32193 RVA: 0x0031591F File Offset: 0x0031491F
		// (set) Token: 0x06007DC2 RID: 32194 RVA: 0x00315931 File Offset: 0x00314931
		public ScrollBarVisibility HorizontalScrollBarVisibility
		{
			get
			{
				return (ScrollBarVisibility)base.GetValue(TextBoxBase.HorizontalScrollBarVisibilityProperty);
			}
			set
			{
				base.SetValue(TextBoxBase.HorizontalScrollBarVisibilityProperty, value);
			}
		}

		// Token: 0x17001CFE RID: 7422
		// (get) Token: 0x06007DC3 RID: 32195 RVA: 0x00315944 File Offset: 0x00314944
		// (set) Token: 0x06007DC4 RID: 32196 RVA: 0x00315956 File Offset: 0x00314956
		public ScrollBarVisibility VerticalScrollBarVisibility
		{
			get
			{
				return (ScrollBarVisibility)base.GetValue(TextBoxBase.VerticalScrollBarVisibilityProperty);
			}
			set
			{
				base.SetValue(TextBoxBase.VerticalScrollBarVisibilityProperty, value);
			}
		}

		// Token: 0x17001CFF RID: 7423
		// (get) Token: 0x06007DC5 RID: 32197 RVA: 0x00315969 File Offset: 0x00314969
		public double ExtentWidth
		{
			get
			{
				if (this.ScrollViewer == null)
				{
					return 0.0;
				}
				return this.ScrollViewer.ExtentWidth;
			}
		}

		// Token: 0x17001D00 RID: 7424
		// (get) Token: 0x06007DC6 RID: 32198 RVA: 0x00315988 File Offset: 0x00314988
		public double ExtentHeight
		{
			get
			{
				if (this.ScrollViewer == null)
				{
					return 0.0;
				}
				return this.ScrollViewer.ExtentHeight;
			}
		}

		// Token: 0x17001D01 RID: 7425
		// (get) Token: 0x06007DC7 RID: 32199 RVA: 0x003159A7 File Offset: 0x003149A7
		public double ViewportWidth
		{
			get
			{
				if (this.ScrollViewer == null)
				{
					return 0.0;
				}
				return this.ScrollViewer.ViewportWidth;
			}
		}

		// Token: 0x17001D02 RID: 7426
		// (get) Token: 0x06007DC8 RID: 32200 RVA: 0x003159C6 File Offset: 0x003149C6
		public double ViewportHeight
		{
			get
			{
				if (this.ScrollViewer == null)
				{
					return 0.0;
				}
				return this.ScrollViewer.ViewportHeight;
			}
		}

		// Token: 0x17001D03 RID: 7427
		// (get) Token: 0x06007DC9 RID: 32201 RVA: 0x003159E5 File Offset: 0x003149E5
		public double HorizontalOffset
		{
			get
			{
				if (this.ScrollViewer == null)
				{
					return 0.0;
				}
				return this.ScrollViewer.HorizontalOffset;
			}
		}

		// Token: 0x17001D04 RID: 7428
		// (get) Token: 0x06007DCA RID: 32202 RVA: 0x00315A04 File Offset: 0x00314A04
		public double VerticalOffset
		{
			get
			{
				if (this.ScrollViewer == null)
				{
					return 0.0;
				}
				return this.ScrollViewer.VerticalOffset;
			}
		}

		// Token: 0x17001D05 RID: 7429
		// (get) Token: 0x06007DCB RID: 32203 RVA: 0x00315A24 File Offset: 0x00314A24
		public bool CanUndo
		{
			get
			{
				UndoManager undoManager = UndoManager.GetUndoManager(this);
				return undoManager != null && this._pendingUndoAction != UndoAction.Clear && (undoManager.UndoCount > undoManager.MinUndoStackCount || (undoManager.State != UndoState.Undo && this._pendingUndoAction == UndoAction.Create));
			}
		}

		// Token: 0x17001D06 RID: 7430
		// (get) Token: 0x06007DCC RID: 32204 RVA: 0x00315A68 File Offset: 0x00314A68
		public bool CanRedo
		{
			get
			{
				UndoManager undoManager = UndoManager.GetUndoManager(this);
				return undoManager != null && this._pendingUndoAction != UndoAction.Clear && (undoManager.RedoCount > 0 || (undoManager.State == UndoState.Undo && this._pendingUndoAction == UndoAction.Create));
			}
		}

		// Token: 0x17001D07 RID: 7431
		// (get) Token: 0x06007DCD RID: 32205 RVA: 0x00315AA6 File Offset: 0x00314AA6
		// (set) Token: 0x06007DCE RID: 32206 RVA: 0x00315AB8 File Offset: 0x00314AB8
		public bool IsUndoEnabled
		{
			get
			{
				return (bool)base.GetValue(TextBoxBase.IsUndoEnabledProperty);
			}
			set
			{
				base.SetValue(TextBoxBase.IsUndoEnabledProperty, value);
			}
		}

		// Token: 0x17001D08 RID: 7432
		// (get) Token: 0x06007DCF RID: 32207 RVA: 0x00315AC6 File Offset: 0x00314AC6
		// (set) Token: 0x06007DD0 RID: 32208 RVA: 0x00315AD8 File Offset: 0x00314AD8
		public int UndoLimit
		{
			get
			{
				return (int)base.GetValue(TextBoxBase.UndoLimitProperty);
			}
			set
			{
				base.SetValue(TextBoxBase.UndoLimitProperty, value);
			}
		}

		// Token: 0x17001D09 RID: 7433
		// (get) Token: 0x06007DD1 RID: 32209 RVA: 0x00315AEB File Offset: 0x00314AEB
		// (set) Token: 0x06007DD2 RID: 32210 RVA: 0x00315AFD File Offset: 0x00314AFD
		public bool AutoWordSelection
		{
			get
			{
				return (bool)base.GetValue(TextBoxBase.AutoWordSelectionProperty);
			}
			set
			{
				base.SetValue(TextBoxBase.AutoWordSelectionProperty, value);
			}
		}

		// Token: 0x17001D0A RID: 7434
		// (get) Token: 0x06007DD3 RID: 32211 RVA: 0x00315B0B File Offset: 0x00314B0B
		// (set) Token: 0x06007DD4 RID: 32212 RVA: 0x00315B1D File Offset: 0x00314B1D
		public Brush SelectionBrush
		{
			get
			{
				return (Brush)base.GetValue(TextBoxBase.SelectionBrushProperty);
			}
			set
			{
				base.SetValue(TextBoxBase.SelectionBrushProperty, value);
			}
		}

		// Token: 0x17001D0B RID: 7435
		// (get) Token: 0x06007DD5 RID: 32213 RVA: 0x00315B2B File Offset: 0x00314B2B
		// (set) Token: 0x06007DD6 RID: 32214 RVA: 0x00315B3D File Offset: 0x00314B3D
		public Brush SelectionTextBrush
		{
			get
			{
				return (Brush)base.GetValue(TextBoxBase.SelectionTextBrushProperty);
			}
			set
			{
				base.SetValue(TextBoxBase.SelectionTextBrushProperty, value);
			}
		}

		// Token: 0x17001D0C RID: 7436
		// (get) Token: 0x06007DD7 RID: 32215 RVA: 0x00315B4B File Offset: 0x00314B4B
		// (set) Token: 0x06007DD8 RID: 32216 RVA: 0x00315B5D File Offset: 0x00314B5D
		public double SelectionOpacity
		{
			get
			{
				return (double)base.GetValue(TextBoxBase.SelectionOpacityProperty);
			}
			set
			{
				base.SetValue(TextBoxBase.SelectionOpacityProperty, value);
			}
		}

		// Token: 0x17001D0D RID: 7437
		// (get) Token: 0x06007DD9 RID: 32217 RVA: 0x00315B70 File Offset: 0x00314B70
		// (set) Token: 0x06007DDA RID: 32218 RVA: 0x00315B82 File Offset: 0x00314B82
		public Brush CaretBrush
		{
			get
			{
				return (Brush)base.GetValue(TextBoxBase.CaretBrushProperty);
			}
			set
			{
				base.SetValue(TextBoxBase.CaretBrushProperty, value);
			}
		}

		// Token: 0x17001D0E RID: 7438
		// (get) Token: 0x06007DDB RID: 32219 RVA: 0x00315B90 File Offset: 0x00314B90
		public bool IsSelectionActive
		{
			get
			{
				return (bool)base.GetValue(TextBoxBase.IsSelectionActiveProperty);
			}
		}

		// Token: 0x17001D0F RID: 7439
		// (get) Token: 0x06007DDC RID: 32220 RVA: 0x00315BA2 File Offset: 0x00314BA2
		// (set) Token: 0x06007DDD RID: 32221 RVA: 0x00315BB4 File Offset: 0x00314BB4
		public bool IsInactiveSelectionHighlightEnabled
		{
			get
			{
				return (bool)base.GetValue(TextBoxBase.IsInactiveSelectionHighlightEnabledProperty);
			}
			set
			{
				base.SetValue(TextBoxBase.IsInactiveSelectionHighlightEnabledProperty, value);
			}
		}

		// Token: 0x1400015C RID: 348
		// (add) Token: 0x06007DDE RID: 32222 RVA: 0x00315BC2 File Offset: 0x00314BC2
		// (remove) Token: 0x06007DDF RID: 32223 RVA: 0x00315BD0 File Offset: 0x00314BD0
		public event TextChangedEventHandler TextChanged
		{
			add
			{
				base.AddHandler(TextBoxBase.TextChangedEvent, value);
			}
			remove
			{
				base.RemoveHandler(TextBoxBase.TextChangedEvent, value);
			}
		}

		// Token: 0x1400015D RID: 349
		// (add) Token: 0x06007DE0 RID: 32224 RVA: 0x00315BDE File Offset: 0x00314BDE
		// (remove) Token: 0x06007DE1 RID: 32225 RVA: 0x00315BEC File Offset: 0x00314BEC
		public event RoutedEventHandler SelectionChanged
		{
			add
			{
				base.AddHandler(TextBoxBase.SelectionChangedEvent, value);
			}
			remove
			{
				base.RemoveHandler(TextBoxBase.SelectionChangedEvent, value);
			}
		}

		// Token: 0x06007DE2 RID: 32226 RVA: 0x00315BFC File Offset: 0x00314BFC
		internal override void ChangeVisualState(bool useTransitions)
		{
			if (!base.IsEnabled)
			{
				VisualStateManager.GoToState(this, "Disabled", useTransitions);
			}
			else if (this.IsReadOnly)
			{
				VisualStateManager.GoToState(this, "ReadOnly", useTransitions);
			}
			else if (base.IsMouseOver)
			{
				VisualStateManager.GoToState(this, "MouseOver", useTransitions);
			}
			else
			{
				VisualStateManager.GoToState(this, "Normal", useTransitions);
			}
			if (base.IsKeyboardFocused)
			{
				VisualStateManager.GoToState(this, "Focused", useTransitions);
			}
			else
			{
				VisualStateManager.GoToState(this, "Unfocused", useTransitions);
			}
			base.ChangeVisualState(useTransitions);
		}

		// Token: 0x06007DE3 RID: 32227 RVA: 0x0017D6EF File Offset: 0x0017C6EF
		protected virtual void OnTextChanged(TextChangedEventArgs e)
		{
			base.RaiseEvent(e);
		}

		// Token: 0x06007DE4 RID: 32228 RVA: 0x0017D6EF File Offset: 0x0017C6EF
		protected virtual void OnSelectionChanged(RoutedEventArgs e)
		{
			base.RaiseEvent(e);
		}

		// Token: 0x06007DE5 RID: 32229 RVA: 0x00315C86 File Offset: 0x00314C86
		protected override void OnTemplateChanged(ControlTemplate oldTemplate, ControlTemplate newTemplate)
		{
			base.OnTemplateChanged(oldTemplate, newTemplate);
			if (oldTemplate != null && newTemplate != null && oldTemplate.VisualTree != newTemplate.VisualTree)
			{
				this.DetachFromVisualTree();
			}
		}

		// Token: 0x06007DE6 RID: 32230 RVA: 0x00315CAC File Offset: 0x00314CAC
		protected override void OnMouseWheel(MouseWheelEventArgs e)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			if (this.ScrollViewer != null && ((e.Delta > 0 && this.VerticalOffset != 0.0) || (e.Delta < 0 && this.VerticalOffset < this.ScrollViewer.ScrollableHeight)))
			{
				Invariant.Assert(this.RenderScope is IScrollInfo);
				if (e.Delta > 0)
				{
					((IScrollInfo)this.RenderScope).MouseWheelUp();
				}
				else
				{
					((IScrollInfo)this.RenderScope).MouseWheelDown();
				}
				e.Handled = true;
			}
			base.OnMouseWheel(e);
		}

		// Token: 0x06007DE7 RID: 32231 RVA: 0x00315D51 File Offset: 0x00314D51
		protected override void OnPreviewKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			if (e.Handled)
			{
				return;
			}
			if (this._textEditor != null)
			{
				this._textEditor.OnPreviewKeyDown(e);
			}
		}

		// Token: 0x06007DE8 RID: 32232 RVA: 0x00315D77 File Offset: 0x00314D77
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			if (e.Handled)
			{
				return;
			}
			if (this._textEditor != null)
			{
				this._textEditor.OnKeyDown(e);
			}
		}

		// Token: 0x06007DE9 RID: 32233 RVA: 0x00315D9D File Offset: 0x00314D9D
		protected override void OnKeyUp(KeyEventArgs e)
		{
			base.OnKeyUp(e);
			if (e.Handled)
			{
				return;
			}
			if (this._textEditor != null)
			{
				this._textEditor.OnKeyUp(e);
			}
		}

		// Token: 0x06007DEA RID: 32234 RVA: 0x00315DC3 File Offset: 0x00314DC3
		protected override void OnTextInput(TextCompositionEventArgs e)
		{
			base.OnTextInput(e);
			if (e.Handled)
			{
				return;
			}
			if (this._textEditor != null)
			{
				this._textEditor.OnTextInput(e);
			}
		}

		// Token: 0x06007DEB RID: 32235 RVA: 0x00315DE9 File Offset: 0x00314DE9
		protected override void OnMouseDown(MouseButtonEventArgs e)
		{
			base.OnMouseDown(e);
			if (e.Handled)
			{
				return;
			}
			if (this._textEditor != null)
			{
				this._textEditor.OnMouseDown(e);
			}
		}

		// Token: 0x06007DEC RID: 32236 RVA: 0x00315E0F File Offset: 0x00314E0F
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (e.Handled)
			{
				return;
			}
			if (this._textEditor != null)
			{
				this._textEditor.OnMouseMove(e);
			}
		}

		// Token: 0x06007DED RID: 32237 RVA: 0x00315E35 File Offset: 0x00314E35
		protected override void OnMouseUp(MouseButtonEventArgs e)
		{
			base.OnMouseUp(e);
			if (e.Handled)
			{
				return;
			}
			if (this._textEditor != null)
			{
				this._textEditor.OnMouseUp(e);
			}
		}

		// Token: 0x06007DEE RID: 32238 RVA: 0x00315E5B File Offset: 0x00314E5B
		protected override void OnQueryCursor(QueryCursorEventArgs e)
		{
			base.OnQueryCursor(e);
			if (e.Handled)
			{
				return;
			}
			if (this._textEditor != null)
			{
				this._textEditor.OnQueryCursor(e);
			}
		}

		// Token: 0x06007DEF RID: 32239 RVA: 0x00315E81 File Offset: 0x00314E81
		protected override void OnQueryContinueDrag(QueryContinueDragEventArgs e)
		{
			base.OnQueryContinueDrag(e);
			if (e.Handled)
			{
				return;
			}
			if (this._textEditor != null)
			{
				this._textEditor.OnQueryContinueDrag(e);
			}
		}

		// Token: 0x06007DF0 RID: 32240 RVA: 0x00315EA7 File Offset: 0x00314EA7
		protected override void OnGiveFeedback(GiveFeedbackEventArgs e)
		{
			base.OnGiveFeedback(e);
			if (e.Handled)
			{
				return;
			}
			if (this._textEditor != null)
			{
				this._textEditor.OnGiveFeedback(e);
			}
		}

		// Token: 0x06007DF1 RID: 32241 RVA: 0x00315ECD File Offset: 0x00314ECD
		protected override void OnDragEnter(DragEventArgs e)
		{
			base.OnDragEnter(e);
			if (e.Handled)
			{
				return;
			}
			if (this._textEditor != null)
			{
				this._textEditor.OnDragEnter(e);
			}
		}

		// Token: 0x06007DF2 RID: 32242 RVA: 0x00315EF3 File Offset: 0x00314EF3
		protected override void OnDragOver(DragEventArgs e)
		{
			base.OnDragOver(e);
			if (e.Handled)
			{
				return;
			}
			if (this._textEditor != null)
			{
				this._textEditor.OnDragOver(e);
			}
		}

		// Token: 0x06007DF3 RID: 32243 RVA: 0x00315F19 File Offset: 0x00314F19
		protected override void OnDragLeave(DragEventArgs e)
		{
			base.OnDragLeave(e);
			if (e.Handled)
			{
				return;
			}
			if (this._textEditor != null)
			{
				this._textEditor.OnDragLeave(e);
			}
		}

		// Token: 0x06007DF4 RID: 32244 RVA: 0x00315F3F File Offset: 0x00314F3F
		protected override void OnDrop(DragEventArgs e)
		{
			base.OnDrop(e);
			if (e.Handled)
			{
				return;
			}
			if (this._textEditor != null)
			{
				this._textEditor.OnDrop(e);
			}
		}

		// Token: 0x06007DF5 RID: 32245 RVA: 0x00315F65 File Offset: 0x00314F65
		protected override void OnContextMenuOpening(ContextMenuEventArgs e)
		{
			base.OnContextMenuOpening(e);
			if (e.Handled)
			{
				return;
			}
			if (this._textEditor != null)
			{
				this._textEditor.OnContextMenuOpening(e);
			}
		}

		// Token: 0x06007DF6 RID: 32246 RVA: 0x00315F8B File Offset: 0x00314F8B
		protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
		{
			base.OnGotKeyboardFocus(e);
			if (e.Handled)
			{
				return;
			}
			if (this._textEditor != null)
			{
				this._textEditor.OnGotKeyboardFocus(e);
			}
		}

		// Token: 0x06007DF7 RID: 32247 RVA: 0x00315FB1 File Offset: 0x00314FB1
		protected override void OnLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
		{
			base.OnLostKeyboardFocus(e);
			if (e.Handled)
			{
				return;
			}
			if (this._textEditor != null)
			{
				this._textEditor.OnLostKeyboardFocus(e);
			}
		}

		// Token: 0x06007DF8 RID: 32248 RVA: 0x00315FD7 File Offset: 0x00314FD7
		protected override void OnLostFocus(RoutedEventArgs e)
		{
			base.OnLostFocus(e);
			if (e.Handled)
			{
				return;
			}
			if (this._textEditor != null)
			{
				this._textEditor.OnLostFocus(e);
			}
		}

		// Token: 0x06007DF9 RID: 32249
		internal abstract FrameworkElement CreateRenderScope();

		// Token: 0x06007DFA RID: 32250 RVA: 0x00316000 File Offset: 0x00315000
		internal virtual void OnTextContainerChanged(object sender, TextContainerChangedEventArgs e)
		{
			if (!e.HasContentAddedOrRemoved && !e.HasLocalPropertyValueChange)
			{
				return;
			}
			UndoManager undoManager = UndoManager.GetUndoManager(this);
			UndoAction undoAction;
			if (undoManager != null)
			{
				if (this._textEditor.UndoState == UndoState.Redo)
				{
					undoAction = UndoAction.Redo;
				}
				else if (this._textEditor.UndoState == UndoState.Undo)
				{
					undoAction = UndoAction.Undo;
				}
				else if (undoManager.OpenedUnit == null)
				{
					undoAction = UndoAction.Clear;
				}
				else if (undoManager.LastReopenedUnit == undoManager.OpenedUnit)
				{
					undoAction = UndoAction.Merge;
				}
				else
				{
					undoAction = UndoAction.Create;
				}
			}
			else
			{
				undoAction = UndoAction.Create;
			}
			this._pendingUndoAction = undoAction;
			try
			{
				this.OnTextChanged(new TextChangedEventArgs(TextBoxBase.TextChangedEvent, undoAction, new ReadOnlyCollection<TextChange>(e.Changes.Values)));
			}
			finally
			{
				this._pendingUndoAction = UndoAction.None;
			}
		}

		// Token: 0x06007DFB RID: 32251 RVA: 0x003160B4 File Offset: 0x003150B4
		internal void InitializeTextContainer(TextContainer textContainer)
		{
			Invariant.Assert(textContainer != null);
			Invariant.Assert(textContainer.TextSelection == null);
			if (this._textContainer != null)
			{
				Invariant.Assert(this._textEditor != null);
				Invariant.Assert(this._textEditor.TextContainer == this._textContainer);
				Invariant.Assert(this._textEditor.TextContainer.TextSelection == this._textEditor.Selection);
				this.DetachFromVisualTree();
				this._textEditor.OnDetach();
			}
			this._textContainer = textContainer;
			this._textContainer.Changed += this.OnTextContainerChanged;
			this._textEditor = new TextEditor(this._textContainer, this, true);
			this._textEditor.Selection.Changed += this.OnSelectionChangedInternal;
			UndoManager undoManager = UndoManager.GetUndoManager(this);
			if (undoManager != null)
			{
				undoManager.UndoLimit = this.UndoLimit;
			}
		}

		// Token: 0x06007DFC RID: 32252 RVA: 0x003161A0 File Offset: 0x003151A0
		internal TextPointer GetTextPositionFromPointInternal(Point point, bool snapToText)
		{
			GeneralTransform generalTransform = base.TransformToDescendant(this.RenderScope);
			if (generalTransform != null)
			{
				generalTransform.TryTransform(point, out point);
			}
			TextPointer result;
			if (TextEditor.GetTextView(this.RenderScope).Validate(point))
			{
				result = (TextPointer)TextEditor.GetTextView(this.RenderScope).GetTextPositionFromPoint(point, snapToText);
			}
			else
			{
				result = (snapToText ? this.TextContainer.Start : null);
			}
			return result;
		}

		// Token: 0x06007DFD RID: 32253 RVA: 0x00316208 File Offset: 0x00315208
		internal bool GetRectangleFromTextPosition(TextPointer position, out Rect rect)
		{
			if (position == null)
			{
				throw new ArgumentNullException("position");
			}
			if (TextEditor.GetTextView(this.RenderScope).Validate(position))
			{
				rect = TextEditor.GetTextView(this.RenderScope).GetRectangleFromTextPosition(position);
				Point inPoint = new Point(0.0, 0.0);
				GeneralTransform generalTransform = base.TransformToDescendant(this.RenderScope);
				if (generalTransform != null)
				{
					generalTransform.TryTransform(inPoint, out inPoint);
				}
				rect.X -= inPoint.X;
				rect.Y -= inPoint.Y;
			}
			else
			{
				rect = Rect.Empty;
			}
			return rect != Rect.Empty;
		}

		// Token: 0x06007DFE RID: 32254 RVA: 0x003162C8 File Offset: 0x003152C8
		internal virtual void AttachToVisualTree()
		{
			this.DetachFromVisualTree();
			this.SetRenderScopeToContentHost();
			if (this.ScrollViewer != null)
			{
				this.ScrollViewer.ScrollChanged += this.OnScrollChanged;
				base.SetValue(TextEditor.PageHeightProperty, this.ScrollViewer.ViewportHeight);
				this.ScrollViewer.Focusable = false;
				this.ScrollViewer.HandlesMouseWheelScrolling = false;
				if (this.ScrollViewer.Background == null)
				{
					this.ScrollViewer.Background = Brushes.Transparent;
				}
				TextBoxBase.OnScrollViewerPropertyChanged(this, new DependencyPropertyChangedEventArgs(ScrollViewer.HorizontalScrollBarVisibilityProperty, null, base.GetValue(TextBoxBase.HorizontalScrollBarVisibilityProperty)));
				TextBoxBase.OnScrollViewerPropertyChanged(this, new DependencyPropertyChangedEventArgs(ScrollViewer.VerticalScrollBarVisibilityProperty, null, base.GetValue(TextBoxBase.VerticalScrollBarVisibilityProperty)));
				TextBoxBase.OnScrollViewerPropertyChanged(this, new DependencyPropertyChangedEventArgs(Control.PaddingProperty, null, base.GetValue(Control.PaddingProperty)));
				return;
			}
			base.ClearValue(TextEditor.PageHeightProperty);
		}

		// Token: 0x06007DFF RID: 32255 RVA: 0x003163B4 File Offset: 0x003153B4
		internal virtual void DoLineUp()
		{
			if (this.ScrollViewer != null)
			{
				this.ScrollViewer.LineUp();
			}
		}

		// Token: 0x06007E00 RID: 32256 RVA: 0x003163C9 File Offset: 0x003153C9
		internal virtual void DoLineDown()
		{
			if (this.ScrollViewer != null)
			{
				this.ScrollViewer.LineDown();
			}
		}

		// Token: 0x06007E01 RID: 32257 RVA: 0x003163E0 File Offset: 0x003153E0
		internal override void AddToEventRouteCore(EventRoute route, RoutedEventArgs args)
		{
			base.AddToEventRouteCore(route, args);
			Visual visual = this.RenderScope;
			while (visual != this && visual != null)
			{
				if (visual is UIElement)
				{
					((UIElement)visual).AddToEventRoute(route, args);
				}
				visual = (VisualTreeHelper.GetParent(visual) as Visual);
			}
		}

		// Token: 0x06007E02 RID: 32258 RVA: 0x00316428 File Offset: 0x00315428
		internal void ChangeUndoEnabled(bool value)
		{
			if (this.TextSelectionInternal.ChangeBlockLevel > 0)
			{
				throw new InvalidOperationException(SR.Get("TextBoxBase_CantSetIsUndoEnabledInsideChangeBlock"));
			}
			UndoManager undoManager = UndoManager.GetUndoManager(this);
			if (undoManager != null)
			{
				if (!value && undoManager.IsEnabled)
				{
					undoManager.Clear();
				}
				undoManager.IsEnabled = value;
			}
		}

		// Token: 0x06007E03 RID: 32259 RVA: 0x00316478 File Offset: 0x00315478
		internal void ChangeUndoLimit(object value)
		{
			UndoManager undoManager = UndoManager.GetUndoManager(this);
			if (undoManager != null)
			{
				if (undoManager.OpenedUnit != null)
				{
					throw new InvalidOperationException(SR.Get("TextBoxBase_CantSetIsUndoEnabledInsideChangeBlock"));
				}
				int undoLimit;
				if (value == DependencyProperty.UnsetValue)
				{
					undoLimit = UndoManager.UndoLimitDefaultValue;
				}
				else
				{
					undoLimit = (int)value;
				}
				undoManager.UndoLimit = undoLimit;
			}
		}

		// Token: 0x17001D10 RID: 7440
		// (get) Token: 0x06007E04 RID: 32260 RVA: 0x003164C5 File Offset: 0x003154C5
		internal ScrollViewer ScrollViewer
		{
			get
			{
				if (this._scrollViewer == null && this._textEditor != null)
				{
					this._scrollViewer = (this._textEditor._Scroller as ScrollViewer);
				}
				return this._scrollViewer;
			}
		}

		// Token: 0x17001D11 RID: 7441
		// (get) Token: 0x06007E05 RID: 32261 RVA: 0x003164F3 File Offset: 0x003154F3
		internal TextSelection TextSelectionInternal
		{
			get
			{
				return (TextSelection)this._textEditor.Selection;
			}
		}

		// Token: 0x17001D12 RID: 7442
		// (get) Token: 0x06007E06 RID: 32262 RVA: 0x00316505 File Offset: 0x00315505
		internal TextContainer TextContainer
		{
			get
			{
				return this._textContainer;
			}
		}

		// Token: 0x17001D13 RID: 7443
		// (get) Token: 0x06007E07 RID: 32263 RVA: 0x0031650D File Offset: 0x0031550D
		internal FrameworkElement RenderScope
		{
			get
			{
				return this._renderScope;
			}
		}

		// Token: 0x17001D14 RID: 7444
		// (get) Token: 0x06007E08 RID: 32264 RVA: 0x00316515 File Offset: 0x00315515
		// (set) Token: 0x06007E09 RID: 32265 RVA: 0x0031651D File Offset: 0x0031551D
		internal UndoAction PendingUndoAction
		{
			get
			{
				return this._pendingUndoAction;
			}
			set
			{
				this._pendingUndoAction = value;
			}
		}

		// Token: 0x17001D15 RID: 7445
		// (get) Token: 0x06007E0A RID: 32266 RVA: 0x00316526 File Offset: 0x00315526
		internal TextEditor TextEditor
		{
			get
			{
				return this._textEditor;
			}
		}

		// Token: 0x17001D16 RID: 7446
		// (get) Token: 0x06007E0B RID: 32267 RVA: 0x0031652E File Offset: 0x0031552E
		internal bool IsContentHostAvailable
		{
			get
			{
				return this._textBoxContentHost != null;
			}
		}

		// Token: 0x06007E0C RID: 32268 RVA: 0x0031653C File Offset: 0x0031553C
		private void DetachFromVisualTree()
		{
			if (this._textEditor != null)
			{
				this._textEditor.Selection.DetachFromVisualTree();
			}
			if (this.ScrollViewer != null)
			{
				this.ScrollViewer.ScrollChanged -= this.OnScrollChanged;
			}
			this._scrollViewer = null;
			this.ClearContentHost();
		}

		// Token: 0x06007E0D RID: 32269 RVA: 0x00316590 File Offset: 0x00315590
		private void InitializeRenderScope()
		{
			if (this._renderScope == null)
			{
				return;
			}
			ITextView textView = (ITextView)((IServiceProvider)this._renderScope).GetService(typeof(ITextView));
			this.TextContainer.TextView = textView;
			this._textEditor.TextView = textView;
			if (this.ScrollViewer != null)
			{
				this.ScrollViewer.CanContentScroll = true;
			}
		}

		// Token: 0x06007E0E RID: 32270 RVA: 0x003165F4 File Offset: 0x003155F4
		private void UninitializeRenderScope()
		{
			this._textEditor.TextView = null;
			TextBoxView textBoxView;
			if ((textBoxView = (this._renderScope as TextBoxView)) != null)
			{
				textBoxView.RemoveTextContainerListeners();
				return;
			}
			FlowDocumentView flowDocumentView;
			if ((flowDocumentView = (this._renderScope as FlowDocumentView)) != null)
			{
				if (flowDocumentView.Document != null)
				{
					flowDocumentView.Document.Uninitialize();
					flowDocumentView.Document = null;
					return;
				}
			}
			else
			{
				Invariant.Assert(this._renderScope == null, "_renderScope must be null here");
			}
		}

		// Token: 0x06007E0F RID: 32271 RVA: 0x00316660 File Offset: 0x00315660
		private static Brush GetDefaultSelectionBrush()
		{
			SolidColorBrush solidColorBrush = new SolidColorBrush(SystemColors.HighlightColor);
			solidColorBrush.Freeze();
			return solidColorBrush;
		}

		// Token: 0x06007E10 RID: 32272 RVA: 0x00316672 File Offset: 0x00315672
		private static Brush GetDefaultSelectionTextBrush()
		{
			SolidColorBrush solidColorBrush = new SolidColorBrush(SystemColors.HighlightTextColor);
			solidColorBrush.Freeze();
			return solidColorBrush;
		}

		// Token: 0x06007E11 RID: 32273 RVA: 0x00316684 File Offset: 0x00315684
		private static object OnPageHeightGetValue(DependencyObject d)
		{
			return ((TextBoxBase)d).ViewportHeight;
		}

		// Token: 0x06007E12 RID: 32274 RVA: 0x00316698 File Offset: 0x00315698
		private void SetRenderScopeToContentHost()
		{
			FrameworkElement renderScope = this.CreateRenderScope();
			this.ClearContentHost();
			this._textBoxContentHost = (base.GetTemplateChild("PART_ContentHost") as FrameworkElement);
			this._renderScope = renderScope;
			if (this._textBoxContentHost is ScrollViewer)
			{
				ScrollViewer scrollViewer = (ScrollViewer)this._textBoxContentHost;
				if (scrollViewer.Content != null)
				{
					this._renderScope = null;
					this._textBoxContentHost = null;
					throw new NotSupportedException(SR.Get("TextBoxScrollViewerMarkedAsTextBoxContentMustHaveNoContent"));
				}
				scrollViewer.Content = this._renderScope;
			}
			else if (this._textBoxContentHost is Decorator)
			{
				Decorator decorator = (Decorator)this._textBoxContentHost;
				if (decorator.Child != null)
				{
					this._renderScope = null;
					this._textBoxContentHost = null;
					throw new NotSupportedException(SR.Get("TextBoxDecoratorMarkedAsTextBoxContentMustHaveNoContent"));
				}
				decorator.Child = this._renderScope;
			}
			else
			{
				this._renderScope = null;
				if (this._textBoxContentHost != null)
				{
					this._textBoxContentHost = null;
					throw new NotSupportedException(SR.Get("TextBoxInvalidTextContainer"));
				}
			}
			this.InitializeRenderScope();
		}

		// Token: 0x06007E13 RID: 32275 RVA: 0x00316798 File Offset: 0x00315798
		private void ClearContentHost()
		{
			this.UninitializeRenderScope();
			if (this._textBoxContentHost is ScrollViewer)
			{
				((ScrollViewer)this._textBoxContentHost).Content = null;
			}
			else if (this._textBoxContentHost is Decorator)
			{
				((Decorator)this._textBoxContentHost).Child = null;
			}
			else
			{
				Invariant.Assert(this._textBoxContentHost == null, "_textBoxContentHost must be null here");
			}
			this._textBoxContentHost = null;
		}

		// Token: 0x06007E14 RID: 32276 RVA: 0x00316805 File Offset: 0x00315805
		private static void OnIsReadOnlyCaretVisiblePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			TextBoxBase textBoxBase = (TextBoxBase)d;
			textBoxBase.TextSelectionInternal.UpdateCaretState(CaretScrollMethod.None);
			((ITextSelection)textBoxBase.TextSelectionInternal).RefreshCaret();
		}

		// Token: 0x06007E15 RID: 32277 RVA: 0x00316823 File Offset: 0x00315823
		internal virtual void OnScrollChanged(object sender, ScrollChangedEventArgs e)
		{
			if (e.ViewportHeightChange != 0.0)
			{
				base.SetValue(TextEditor.PageHeightProperty, e.ViewportHeight);
			}
		}

		// Token: 0x06007E16 RID: 32278 RVA: 0x0031684C File Offset: 0x0031584C
		private void OnSelectionChangedInternal(object sender, EventArgs e)
		{
			this.OnSelectionChanged(new RoutedEventArgs(TextBoxBase.SelectionChangedEvent));
		}

		// Token: 0x17001D17 RID: 7447
		// (get) Token: 0x06007E17 RID: 32279 RVA: 0x0031685E File Offset: 0x0031585E
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return TextBoxBase._dType;
			}
		}

		// Token: 0x06007E18 RID: 32280 RVA: 0x00316868 File Offset: 0x00315868
		internal static void OnScrollViewerPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			TextBoxBase textBoxBase = d as TextBoxBase;
			if (textBoxBase != null && textBoxBase.ScrollViewer != null)
			{
				object newValue = e.NewValue;
				if (newValue == DependencyProperty.UnsetValue)
				{
					textBoxBase.ScrollViewer.ClearValue(e.Property);
					return;
				}
				textBoxBase.ScrollViewer.SetValue(e.Property, newValue);
			}
		}

		// Token: 0x06007E19 RID: 32281 RVA: 0x003168BD File Offset: 0x003158BD
		private static void OnIsUndoEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((TextBoxBase)d).ChangeUndoEnabled((bool)e.NewValue);
		}

		// Token: 0x06007E1A RID: 32282 RVA: 0x003129B1 File Offset: 0x003119B1
		private static bool UndoLimitValidateValue(object value)
		{
			return (int)value >= -1;
		}

		// Token: 0x06007E1B RID: 32283 RVA: 0x003168D6 File Offset: 0x003158D6
		private static void OnUndoLimitChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((TextBoxBase)d).ChangeUndoLimit(e.NewValue);
		}

		// Token: 0x06007E1C RID: 32284 RVA: 0x003168EC File Offset: 0x003158EC
		private static void OnInputMethodEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			TextBoxBase textBoxBase = (TextBoxBase)d;
			if (textBoxBase.TextEditor != null && textBoxBase.TextEditor.TextStore != null && (bool)e.NewValue && Keyboard.FocusedElement == textBoxBase)
			{
				textBoxBase.TextEditor.TextStore.OnGotFocus();
			}
		}

		// Token: 0x06007E1D RID: 32285 RVA: 0x0031693C File Offset: 0x0031593C
		private static void UpdateCaretElement(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			TextBoxBase textBoxBase = (TextBoxBase)d;
			if (textBoxBase.TextSelectionInternal != null)
			{
				CaretElement caretElement = textBoxBase.TextSelectionInternal.CaretElement;
				if (caretElement != null)
				{
					if (e.Property == TextBoxBase.CaretBrushProperty)
					{
						caretElement.UpdateCaretBrush(TextSelection.GetCaretBrush(textBoxBase.TextEditor));
					}
					caretElement.InvalidateVisual();
				}
				TextBoxView textBoxView = ((textBoxBase != null) ? textBoxBase.RenderScope : null) as TextBoxView;
				TextBoxView textBoxView2 = textBoxView;
				if (textBoxView2 != null && ((ITextView)textBoxView2).RendersOwnSelection)
				{
					textBoxView.InvalidateArrange();
				}
			}
		}

		// Token: 0x04003AF9 RID: 15097
		public static readonly DependencyProperty IsReadOnlyProperty = TextEditor.IsReadOnlyProperty.AddOwner(typeof(TextBoxBase), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits, new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));

		// Token: 0x04003AFA RID: 15098
		public static readonly DependencyProperty IsReadOnlyCaretVisibleProperty = DependencyProperty.Register("IsReadOnlyCaretVisible", typeof(bool), typeof(TextBoxBase), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(TextBoxBase.OnIsReadOnlyCaretVisiblePropertyChanged)));

		// Token: 0x04003AFB RID: 15099
		public static readonly DependencyProperty AcceptsReturnProperty = KeyboardNavigation.AcceptsReturnProperty.AddOwner(typeof(TextBoxBase));

		// Token: 0x04003AFC RID: 15100
		public static readonly DependencyProperty AcceptsTabProperty = DependencyProperty.Register("AcceptsTab", typeof(bool), typeof(TextBoxBase), new FrameworkPropertyMetadata(false));

		// Token: 0x04003AFD RID: 15101
		public static readonly DependencyProperty HorizontalScrollBarVisibilityProperty = ScrollViewer.HorizontalScrollBarVisibilityProperty.AddOwner(typeof(TextBoxBase), new FrameworkPropertyMetadata(ScrollBarVisibility.Hidden, new PropertyChangedCallback(TextBoxBase.OnScrollViewerPropertyChanged)));

		// Token: 0x04003AFE RID: 15102
		public static readonly DependencyProperty VerticalScrollBarVisibilityProperty = ScrollViewer.VerticalScrollBarVisibilityProperty.AddOwner(typeof(TextBoxBase), new FrameworkPropertyMetadata(ScrollBarVisibility.Hidden, new PropertyChangedCallback(TextBoxBase.OnScrollViewerPropertyChanged)));

		// Token: 0x04003AFF RID: 15103
		public static readonly DependencyProperty IsUndoEnabledProperty = DependencyProperty.Register("IsUndoEnabled", typeof(bool), typeof(TextBoxBase), new FrameworkPropertyMetadata(true, new PropertyChangedCallback(TextBoxBase.OnIsUndoEnabledChanged)));

		// Token: 0x04003B00 RID: 15104
		public static readonly DependencyProperty UndoLimitProperty = DependencyProperty.Register("UndoLimit", typeof(int), typeof(TextBoxBase), new FrameworkPropertyMetadata(UndoManager.UndoLimitDefaultValue, new PropertyChangedCallback(TextBoxBase.OnUndoLimitChanged)), new ValidateValueCallback(TextBoxBase.UndoLimitValidateValue));

		// Token: 0x04003B01 RID: 15105
		public static readonly DependencyProperty AutoWordSelectionProperty = DependencyProperty.Register("AutoWordSelection", typeof(bool), typeof(TextBoxBase), new FrameworkPropertyMetadata(false));

		// Token: 0x04003B02 RID: 15106
		public static readonly DependencyProperty SelectionBrushProperty = DependencyProperty.Register("SelectionBrush", typeof(Brush), typeof(TextBoxBase), new FrameworkPropertyMetadata(TextBoxBase.GetDefaultSelectionBrush(), new PropertyChangedCallback(TextBoxBase.UpdateCaretElement)));

		// Token: 0x04003B03 RID: 15107
		public static readonly DependencyProperty SelectionTextBrushProperty = DependencyProperty.Register("SelectionTextBrush", typeof(Brush), typeof(TextBoxBase), new FrameworkPropertyMetadata(TextBoxBase.GetDefaultSelectionTextBrush(), new PropertyChangedCallback(TextBoxBase.UpdateCaretElement)));

		// Token: 0x04003B04 RID: 15108
		internal const double AdornerSelectionOpacityDefaultValue = 0.4;

		// Token: 0x04003B05 RID: 15109
		internal const double NonAdornerSelectionOpacityDefaultValue = 1.0;

		// Token: 0x04003B06 RID: 15110
		private static double SelectionOpacityDefaultValue = FrameworkAppContextSwitches.UseAdornerForTextboxSelectionRendering ? 0.4 : 1.0;

		// Token: 0x04003B07 RID: 15111
		public static readonly DependencyProperty SelectionOpacityProperty = DependencyProperty.Register("SelectionOpacity", typeof(double), typeof(TextBoxBase), new FrameworkPropertyMetadata(TextBoxBase.SelectionOpacityDefaultValue, new PropertyChangedCallback(TextBoxBase.UpdateCaretElement)));

		// Token: 0x04003B08 RID: 15112
		public static readonly DependencyProperty CaretBrushProperty = DependencyProperty.Register("CaretBrush", typeof(Brush), typeof(TextBoxBase), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(TextBoxBase.UpdateCaretElement)));

		// Token: 0x04003B09 RID: 15113
		internal static readonly DependencyPropertyKey IsSelectionActivePropertyKey = DependencyProperty.RegisterAttachedReadOnly("IsSelectionActive", typeof(bool), typeof(TextBoxBase), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));

		// Token: 0x04003B0A RID: 15114
		public static readonly DependencyProperty IsSelectionActiveProperty = TextBoxBase.IsSelectionActivePropertyKey.DependencyProperty;

		// Token: 0x04003B0B RID: 15115
		public static readonly DependencyProperty IsInactiveSelectionHighlightEnabledProperty = DependencyProperty.Register("IsInactiveSelectionHighlightEnabled", typeof(bool), typeof(TextBoxBase));

		// Token: 0x04003B0E RID: 15118
		private static DependencyObjectType _dType;

		// Token: 0x04003B0F RID: 15119
		private TextContainer _textContainer;

		// Token: 0x04003B10 RID: 15120
		private TextEditor _textEditor;

		// Token: 0x04003B11 RID: 15121
		private FrameworkElement _textBoxContentHost;

		// Token: 0x04003B12 RID: 15122
		private FrameworkElement _renderScope;

		// Token: 0x04003B13 RID: 15123
		private ScrollViewer _scrollViewer;

		// Token: 0x04003B14 RID: 15124
		private UndoAction _pendingUndoAction;

		// Token: 0x04003B15 RID: 15125
		internal const string ContentHostTemplateName = "PART_ContentHost";
	}
}
