using System;
using System.Windows.Automation.Peers;
using System.Windows.Ink;
using System.Windows.Media;
using System.Windows.Threading;
using MS.Internal.Ink;

namespace System.Windows.Controls
{
	// Token: 0x02000799 RID: 1945
	public class InkPresenter : Decorator
	{
		// Token: 0x06006C95 RID: 27797 RVA: 0x002C988C File Offset: 0x002C888C
		public InkPresenter()
		{
			this._renderer = new Renderer();
			this.SetStrokesChangedHandlers(this.Strokes, null);
			this._contrastCallback = new InkPresenter.InkPresenterHighContrastCallback(this);
			HighContrastHelper.RegisterHighContrastCallback(this._contrastCallback);
			if (SystemParameters.HighContrast)
			{
				this._contrastCallback.TurnHighContrastOn(SystemColors.WindowTextColor);
			}
			this._constraintSize = Size.Empty;
		}

		// Token: 0x06006C96 RID: 27798 RVA: 0x002C98F0 File Offset: 0x002C88F0
		public void AttachVisuals(Visual visual, DrawingAttributes drawingAttributes)
		{
			base.VerifyAccess();
			this.EnsureRootVisual();
			this._renderer.AttachIncrementalRendering(visual, drawingAttributes);
		}

		// Token: 0x06006C97 RID: 27799 RVA: 0x002C990B File Offset: 0x002C890B
		public void DetachVisuals(Visual visual)
		{
			base.VerifyAccess();
			this.EnsureRootVisual();
			this._renderer.DetachIncrementalRendering(visual);
		}

		// Token: 0x1700190E RID: 6414
		// (get) Token: 0x06006C98 RID: 27800 RVA: 0x002C9925 File Offset: 0x002C8925
		// (set) Token: 0x06006C99 RID: 27801 RVA: 0x002C9937 File Offset: 0x002C8937
		public StrokeCollection Strokes
		{
			get
			{
				return (StrokeCollection)base.GetValue(InkPresenter.StrokesProperty);
			}
			set
			{
				base.SetValue(InkPresenter.StrokesProperty, value);
			}
		}

		// Token: 0x06006C9A RID: 27802 RVA: 0x002C9948 File Offset: 0x002C8948
		private static void OnStrokesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			InkPresenter inkPresenter = (InkPresenter)d;
			StrokeCollection oldStrokes = (StrokeCollection)e.OldValue;
			StrokeCollection newStrokes = (StrokeCollection)e.NewValue;
			inkPresenter.SetStrokesChangedHandlers(newStrokes, oldStrokes);
			inkPresenter.OnStrokeChanged(inkPresenter, EventArgs.Empty);
		}

		// Token: 0x06006C9B RID: 27803 RVA: 0x002C9988 File Offset: 0x002C8988
		protected override Size MeasureOverride(Size constraint)
		{
			StrokeCollection strokes = this.Strokes;
			Size result = base.MeasureOverride(constraint);
			if (strokes != null && strokes.Count != 0)
			{
				Rect strokesBounds = this.StrokesBounds;
				if (!strokesBounds.IsEmpty && strokesBounds.Right > 0.0 && strokesBounds.Bottom > 0.0)
				{
					Size size = new Size(strokesBounds.Right, strokesBounds.Bottom);
					result.Width = Math.Max(result.Width, size.Width);
					result.Height = Math.Max(result.Height, size.Height);
				}
			}
			if (this.Child != null)
			{
				this._constraintSize = constraint;
			}
			else
			{
				this._constraintSize = Size.Empty;
			}
			return result;
		}

		// Token: 0x06006C9C RID: 27804 RVA: 0x002C9A4C File Offset: 0x002C8A4C
		protected override Size ArrangeOverride(Size arrangeSize)
		{
			base.VerifyAccess();
			this.EnsureRootVisual();
			Size size = arrangeSize;
			if (!this._constraintSize.IsEmpty)
			{
				size = new Size(Math.Min(arrangeSize.Width, this._constraintSize.Width), Math.Min(arrangeSize.Height, this._constraintSize.Height));
			}
			UIElement child = this.Child;
			if (child != null)
			{
				child.Arrange(new Rect(size));
			}
			return arrangeSize;
		}

		// Token: 0x06006C9D RID: 27805 RVA: 0x0015D311 File Offset: 0x0015C311
		protected override Geometry GetLayoutClip(Size layoutSlotSize)
		{
			if (base.ClipToBounds)
			{
				return base.GetLayoutClip(layoutSlotSize);
			}
			return null;
		}

		// Token: 0x06006C9E RID: 27806 RVA: 0x002C9AC0 File Offset: 0x002C8AC0
		protected override Visual GetVisualChild(int index)
		{
			int visualChildrenCount = this.VisualChildrenCount;
			if (visualChildrenCount != 2)
			{
				if (index == 0 && visualChildrenCount == 1)
				{
					if (this._hasAddedRoot)
					{
						return this._renderer.RootVisual;
					}
					if (base.Child != null)
					{
						return base.Child;
					}
				}
				throw new ArgumentOutOfRangeException("index", index, SR.Get("Visual_ArgumentOutOfRange"));
			}
			if (index == 0)
			{
				return base.Child;
			}
			if (index != 1)
			{
				throw new ArgumentOutOfRangeException("index", index, SR.Get("Visual_ArgumentOutOfRange"));
			}
			return this._renderer.RootVisual;
		}

		// Token: 0x1700190F RID: 6415
		// (get) Token: 0x06006C9F RID: 27807 RVA: 0x002C9B53 File Offset: 0x002C8B53
		protected override int VisualChildrenCount
		{
			get
			{
				if (base.Child != null)
				{
					if (this._hasAddedRoot)
					{
						return 2;
					}
					return 1;
				}
				else
				{
					if (this._hasAddedRoot)
					{
						return 1;
					}
					return 0;
				}
			}
		}

		// Token: 0x06006CA0 RID: 27808 RVA: 0x002C9B74 File Offset: 0x002C8B74
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new InkPresenterAutomationPeer(this);
		}

		// Token: 0x06006CA1 RID: 27809 RVA: 0x002C9B7C File Offset: 0x002C8B7C
		internal bool ContainsAttachedVisual(Visual visual)
		{
			base.VerifyAccess();
			return this._renderer.ContainsAttachedIncrementalRenderingVisual(visual);
		}

		// Token: 0x06006CA2 RID: 27810 RVA: 0x002C9B90 File Offset: 0x002C8B90
		internal bool AttachedVisualIsPositionedCorrectly(Visual visual, DrawingAttributes drawingAttributes)
		{
			base.VerifyAccess();
			return this._renderer.AttachedVisualIsPositionedCorrectly(visual, drawingAttributes);
		}

		// Token: 0x06006CA3 RID: 27811 RVA: 0x002C9BA5 File Offset: 0x002C8BA5
		private void SetStrokesChangedHandlers(StrokeCollection newStrokes, StrokeCollection oldStrokes)
		{
			if (oldStrokes != null)
			{
				oldStrokes.StrokesChanged -= this.OnStrokesChanged;
			}
			newStrokes.StrokesChanged += this.OnStrokesChanged;
			this._renderer.Strokes = newStrokes;
			this.SetStrokeChangedHandlers(newStrokes, oldStrokes);
		}

		// Token: 0x06006CA4 RID: 27812 RVA: 0x002C9BE2 File Offset: 0x002C8BE2
		private void OnStrokesChanged(object sender, StrokeCollectionChangedEventArgs eventArgs)
		{
			this.SetStrokeChangedHandlers(eventArgs.Added, eventArgs.Removed);
			this.OnStrokeChanged(this, EventArgs.Empty);
		}

		// Token: 0x06006CA5 RID: 27813 RVA: 0x002C9C04 File Offset: 0x002C8C04
		private void SetStrokeChangedHandlers(StrokeCollection addedStrokes, StrokeCollection removedStrokes)
		{
			int count;
			if (removedStrokes != null)
			{
				count = removedStrokes.Count;
				for (int i = 0; i < count; i++)
				{
					this.StopListeningOnStrokeEvents(removedStrokes[i]);
				}
			}
			count = addedStrokes.Count;
			for (int i = 0; i < count; i++)
			{
				this.StartListeningOnStrokeEvents(addedStrokes[i]);
			}
		}

		// Token: 0x06006CA6 RID: 27814 RVA: 0x002C9C54 File Offset: 0x002C8C54
		private void OnStrokeChanged(object sender, EventArgs e)
		{
			this.OnStrokeChanged();
		}

		// Token: 0x06006CA7 RID: 27815 RVA: 0x002C9C5C File Offset: 0x002C8C5C
		private void OnStrokeChanged()
		{
			this._cachedBounds = null;
			base.InvalidateMeasure();
		}

		// Token: 0x06006CA8 RID: 27816 RVA: 0x002C9C70 File Offset: 0x002C8C70
		private void StartListeningOnStrokeEvents(Stroke stroke)
		{
			stroke.Invalidated += this.OnStrokeChanged;
		}

		// Token: 0x06006CA9 RID: 27817 RVA: 0x002C9C84 File Offset: 0x002C8C84
		private void StopListeningOnStrokeEvents(Stroke stroke)
		{
			stroke.Invalidated -= this.OnStrokeChanged;
		}

		// Token: 0x06006CAA RID: 27818 RVA: 0x002C9C98 File Offset: 0x002C8C98
		private void EnsureRootVisual()
		{
			if (!this._hasAddedRoot)
			{
				this._renderer.RootVisual._parentIndex = 0;
				base.AddVisualChild(this._renderer.RootVisual);
				this._hasAddedRoot = true;
			}
		}

		// Token: 0x17001910 RID: 6416
		// (get) Token: 0x06006CAB RID: 27819 RVA: 0x002C9CCB File Offset: 0x002C8CCB
		private Rect StrokesBounds
		{
			get
			{
				if (this._cachedBounds == null)
				{
					this._cachedBounds = new Rect?(this.Strokes.GetBounds());
				}
				return this._cachedBounds.Value;
			}
		}

		// Token: 0x0400360B RID: 13835
		public static readonly DependencyProperty StrokesProperty = DependencyProperty.Register("Strokes", typeof(StrokeCollection), typeof(InkPresenter), new FrameworkPropertyMetadata(new StrokeCollectionDefaultValueFactory(), new PropertyChangedCallback(InkPresenter.OnStrokesChanged)), (object value) => value != null);

		// Token: 0x0400360C RID: 13836
		private Renderer _renderer;

		// Token: 0x0400360D RID: 13837
		private Rect? _cachedBounds;

		// Token: 0x0400360E RID: 13838
		private bool _hasAddedRoot;

		// Token: 0x0400360F RID: 13839
		private InkPresenter.InkPresenterHighContrastCallback _contrastCallback;

		// Token: 0x04003610 RID: 13840
		private Size _constraintSize;

		// Token: 0x02000BF8 RID: 3064
		private class InkPresenterHighContrastCallback : HighContrastCallback
		{
			// Token: 0x06008FF2 RID: 36850 RVA: 0x003458B7 File Offset: 0x003448B7
			internal InkPresenterHighContrastCallback(InkPresenter inkPresenter)
			{
				this._thisInkPresenter = inkPresenter;
			}

			// Token: 0x06008FF3 RID: 36851 RVA: 0x00345830 File Offset: 0x00344830
			private InkPresenterHighContrastCallback()
			{
			}

			// Token: 0x06008FF4 RID: 36852 RVA: 0x003458C6 File Offset: 0x003448C6
			internal override void TurnHighContrastOn(Color highContrastColor)
			{
				this._thisInkPresenter._renderer.TurnHighContrastOn(highContrastColor);
				this._thisInkPresenter.OnStrokeChanged();
			}

			// Token: 0x06008FF5 RID: 36853 RVA: 0x003458E4 File Offset: 0x003448E4
			internal override void TurnHighContrastOff()
			{
				this._thisInkPresenter._renderer.TurnHighContrastOff();
				this._thisInkPresenter.OnStrokeChanged();
			}

			// Token: 0x17001F74 RID: 8052
			// (get) Token: 0x06008FF6 RID: 36854 RVA: 0x00345901 File Offset: 0x00344901
			internal override Dispatcher Dispatcher
			{
				get
				{
					return this._thisInkPresenter.Dispatcher;
				}
			}

			// Token: 0x04004A94 RID: 19092
			private InkPresenter _thisInkPresenter;
		}
	}
}
