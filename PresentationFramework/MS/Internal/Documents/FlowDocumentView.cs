using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;
using MS.Internal.PtsHost;
using MS.Internal.Text;

namespace MS.Internal.Documents
{
	// Token: 0x020001C8 RID: 456
	internal class FlowDocumentView : FrameworkElement, IScrollInfo, IServiceProvider
	{
		// Token: 0x06000FA6 RID: 4006 RVA: 0x0013C3BF File Offset: 0x0013B3BF
		internal FlowDocumentView()
		{
		}

		// Token: 0x06000FA7 RID: 4007 RVA: 0x0013E908 File Offset: 0x0013D908
		protected sealed override Size MeasureOverride(Size constraint)
		{
			Size result = default(Size);
			if (this._suspendLayout)
			{
				result = base.DesiredSize;
			}
			else if (this.Document != null)
			{
				this.EnsureFormatter();
				this._formatter.Format(constraint);
				if (this._scrollData != null)
				{
					result.Width = Math.Min(constraint.Width, this._formatter.DocumentPage.Size.Width);
					result.Height = Math.Min(constraint.Height, this._formatter.DocumentPage.Size.Height);
				}
				else
				{
					result = this._formatter.DocumentPage.Size;
				}
			}
			return result;
		}

		// Token: 0x06000FA8 RID: 4008 RVA: 0x0013E9C0 File Offset: 0x0013D9C0
		protected sealed override Size ArrangeOverride(Size arrangeSize)
		{
			Rect empty = Rect.Empty;
			bool flag = false;
			Size size = arrangeSize;
			if (!this._suspendLayout)
			{
				TextDpi.SnapToTextDpi(ref size);
				if (this.Document != null)
				{
					this.EnsureFormatter();
					if (this._scrollData != null)
					{
						if (!DoubleUtil.AreClose(this._scrollData.Viewport, size))
						{
							this._scrollData.Viewport = size;
							flag = true;
						}
						if (!DoubleUtil.AreClose(this._scrollData.Extent, this._formatter.DocumentPage.Size))
						{
							this._scrollData.Extent = this._formatter.DocumentPage.Size;
							flag = true;
							if (Math.Abs(this._scrollData.ExtentWidth - this._scrollData.ViewportWidth) < 1.0)
							{
								this._scrollData.ExtentWidth = this._scrollData.ViewportWidth;
							}
							if (Math.Abs(this._scrollData.ExtentHeight - this._scrollData.ViewportHeight) < 1.0)
							{
								this._scrollData.ExtentHeight = this._scrollData.ViewportHeight;
							}
						}
						Vector vector = new Vector(Math.Max(0.0, Math.Min(this._scrollData.ExtentWidth - this._scrollData.ViewportWidth, this._scrollData.HorizontalOffset)), Math.Max(0.0, Math.Min(this._scrollData.ExtentHeight - this._scrollData.ViewportHeight, this._scrollData.VerticalOffset)));
						if (!DoubleUtil.AreClose(vector, this._scrollData.Offset))
						{
							this._scrollData.Offset = vector;
							flag = true;
						}
						if (flag && this._scrollData.ScrollOwner != null)
						{
							this._scrollData.ScrollOwner.InvalidateScrollInfo();
						}
						empty = new Rect(this._scrollData.HorizontalOffset, this._scrollData.VerticalOffset, size.Width, size.Height);
					}
					this._formatter.Arrange(size, empty);
					if (this._pageVisual != this._formatter.DocumentPage.Visual)
					{
						if (this._textView != null)
						{
							this._textView.OnPageConnected();
						}
						if (this._pageVisual != null)
						{
							base.RemoveVisualChild(this._pageVisual);
						}
						this._pageVisual = (PageVisual)this._formatter.DocumentPage.Visual;
						base.AddVisualChild(this._pageVisual);
					}
					if (this._scrollData != null)
					{
						this._pageVisual.Offset = new Vector(-this._scrollData.HorizontalOffset, -this._scrollData.VerticalOffset);
					}
					PtsHelper.UpdateMirroringTransform(base.FlowDirection, FlowDirection.LeftToRight, this._pageVisual, size.Width);
				}
				else
				{
					if (this._pageVisual != null)
					{
						if (this._textView != null)
						{
							this._textView.OnPageDisconnected();
						}
						base.RemoveVisualChild(this._pageVisual);
						this._pageVisual = null;
					}
					if (this._scrollData != null)
					{
						if (!DoubleUtil.AreClose(this._scrollData.Viewport, size))
						{
							this._scrollData.Viewport = size;
							flag = true;
						}
						if (!DoubleUtil.AreClose(this._scrollData.Extent, default(Size)))
						{
							this._scrollData.Extent = default(Size);
							flag = true;
						}
						if (!DoubleUtil.AreClose(this._scrollData.Offset, default(Vector)))
						{
							this._scrollData.Offset = default(Vector);
							flag = true;
						}
						if (flag && this._scrollData.ScrollOwner != null)
						{
							this._scrollData.ScrollOwner.InvalidateScrollInfo();
						}
					}
				}
			}
			return arrangeSize;
		}

		// Token: 0x06000FA9 RID: 4009 RVA: 0x0013ED61 File Offset: 0x0013DD61
		protected override Visual GetVisualChild(int index)
		{
			if (index != 0)
			{
				throw new ArgumentOutOfRangeException("index", index, SR.Get("Visual_ArgumentOutOfRange"));
			}
			return this._pageVisual;
		}

		// Token: 0x170002B5 RID: 693
		// (get) Token: 0x06000FAA RID: 4010 RVA: 0x0013ED87 File Offset: 0x0013DD87
		protected override int VisualChildrenCount
		{
			get
			{
				if (this._pageVisual != null)
				{
					return 1;
				}
				return 0;
			}
		}

		// Token: 0x06000FAB RID: 4011 RVA: 0x0013ED94 File Offset: 0x0013DD94
		internal void SuspendLayout()
		{
			this._suspendLayout = true;
			if (this._pageVisual != null)
			{
				this._pageVisual.Opacity = 0.5;
			}
		}

		// Token: 0x06000FAC RID: 4012 RVA: 0x0013EDB9 File Offset: 0x0013DDB9
		internal void ResumeLayout()
		{
			this._suspendLayout = false;
			if (this._pageVisual != null)
			{
				this._pageVisual.Opacity = 1.0;
			}
			base.InvalidateMeasure();
		}

		// Token: 0x170002B6 RID: 694
		// (get) Token: 0x06000FAD RID: 4013 RVA: 0x0013EDE4 File Offset: 0x0013DDE4
		// (set) Token: 0x06000FAE RID: 4014 RVA: 0x0013EDEC File Offset: 0x0013DDEC
		internal FlowDocument Document
		{
			get
			{
				return this._document;
			}
			set
			{
				if (this._formatter != null)
				{
					this.HandleFormatterSuspended(this._formatter, EventArgs.Empty);
				}
				this._suspendLayout = false;
				this._textView = null;
				this._document = value;
				base.InvalidateMeasure();
				base.InvalidateVisual();
			}
		}

		// Token: 0x170002B7 RID: 695
		// (get) Token: 0x06000FAF RID: 4015 RVA: 0x0013EE28 File Offset: 0x0013DE28
		internal FlowDocumentPage DocumentPage
		{
			get
			{
				if (this._document != null)
				{
					this.EnsureFormatter();
					return this._formatter.DocumentPage;
				}
				return null;
			}
		}

		// Token: 0x06000FB0 RID: 4016 RVA: 0x0013EE48 File Offset: 0x0013DE48
		private void EnsureFormatter()
		{
			Invariant.Assert(this._document != null);
			if (this._formatter == null)
			{
				this._formatter = this._document.BottomlessFormatter;
				this._formatter.ContentInvalidated += this.HandleContentInvalidated;
				this._formatter.Suspended += this.HandleFormatterSuspended;
			}
			Invariant.Assert(this._formatter == this._document.BottomlessFormatter);
		}

		// Token: 0x06000FB1 RID: 4017 RVA: 0x0013EEC2 File Offset: 0x0013DEC2
		private void HandleContentInvalidated(object sender, EventArgs e)
		{
			Invariant.Assert(sender == this._formatter);
			base.InvalidateMeasure();
			base.InvalidateVisual();
		}

		// Token: 0x06000FB2 RID: 4018 RVA: 0x0013EEE0 File Offset: 0x0013DEE0
		private void HandleFormatterSuspended(object sender, EventArgs e)
		{
			Invariant.Assert(sender == this._formatter);
			this._formatter.ContentInvalidated -= this.HandleContentInvalidated;
			this._formatter.Suspended -= this.HandleFormatterSuspended;
			this._formatter = null;
			if (this._pageVisual != null && !this._suspendLayout)
			{
				if (this._textView != null)
				{
					this._textView.OnPageDisconnected();
				}
				base.RemoveVisualChild(this._pageVisual);
				this._pageVisual = null;
			}
		}

		// Token: 0x06000FB3 RID: 4019 RVA: 0x0013EF66 File Offset: 0x0013DF66
		void IScrollInfo.LineUp()
		{
			if (this._scrollData != null)
			{
				this._scrollData.LineUp(this);
			}
		}

		// Token: 0x06000FB4 RID: 4020 RVA: 0x0013EF7C File Offset: 0x0013DF7C
		void IScrollInfo.LineDown()
		{
			if (this._scrollData != null)
			{
				this._scrollData.LineDown(this);
			}
		}

		// Token: 0x06000FB5 RID: 4021 RVA: 0x0013EF92 File Offset: 0x0013DF92
		void IScrollInfo.LineLeft()
		{
			if (this._scrollData != null)
			{
				this._scrollData.LineLeft(this);
			}
		}

		// Token: 0x06000FB6 RID: 4022 RVA: 0x0013EFA8 File Offset: 0x0013DFA8
		void IScrollInfo.LineRight()
		{
			if (this._scrollData != null)
			{
				this._scrollData.LineRight(this);
			}
		}

		// Token: 0x06000FB7 RID: 4023 RVA: 0x0013EFBE File Offset: 0x0013DFBE
		void IScrollInfo.PageUp()
		{
			if (this._scrollData != null)
			{
				this._scrollData.PageUp(this);
			}
		}

		// Token: 0x06000FB8 RID: 4024 RVA: 0x0013EFD4 File Offset: 0x0013DFD4
		void IScrollInfo.PageDown()
		{
			if (this._scrollData != null)
			{
				this._scrollData.PageDown(this);
			}
		}

		// Token: 0x06000FB9 RID: 4025 RVA: 0x0013EFEA File Offset: 0x0013DFEA
		void IScrollInfo.PageLeft()
		{
			if (this._scrollData != null)
			{
				this._scrollData.PageLeft(this);
			}
		}

		// Token: 0x06000FBA RID: 4026 RVA: 0x0013F000 File Offset: 0x0013E000
		void IScrollInfo.PageRight()
		{
			if (this._scrollData != null)
			{
				this._scrollData.PageRight(this);
			}
		}

		// Token: 0x06000FBB RID: 4027 RVA: 0x0013F016 File Offset: 0x0013E016
		void IScrollInfo.MouseWheelUp()
		{
			if (this._scrollData != null)
			{
				this._scrollData.MouseWheelUp(this);
			}
		}

		// Token: 0x06000FBC RID: 4028 RVA: 0x0013F02C File Offset: 0x0013E02C
		void IScrollInfo.MouseWheelDown()
		{
			if (this._scrollData != null)
			{
				this._scrollData.MouseWheelDown(this);
			}
		}

		// Token: 0x06000FBD RID: 4029 RVA: 0x0013F042 File Offset: 0x0013E042
		void IScrollInfo.MouseWheelLeft()
		{
			if (this._scrollData != null)
			{
				this._scrollData.MouseWheelLeft(this);
			}
		}

		// Token: 0x06000FBE RID: 4030 RVA: 0x0013F058 File Offset: 0x0013E058
		void IScrollInfo.MouseWheelRight()
		{
			if (this._scrollData != null)
			{
				this._scrollData.MouseWheelRight(this);
			}
		}

		// Token: 0x06000FBF RID: 4031 RVA: 0x0013F06E File Offset: 0x0013E06E
		void IScrollInfo.SetHorizontalOffset(double offset)
		{
			if (this._scrollData != null)
			{
				this._scrollData.SetHorizontalOffset(this, offset);
			}
		}

		// Token: 0x06000FC0 RID: 4032 RVA: 0x0013F085 File Offset: 0x0013E085
		void IScrollInfo.SetVerticalOffset(double offset)
		{
			if (this._scrollData != null)
			{
				this._scrollData.SetVerticalOffset(this, offset);
			}
		}

		// Token: 0x06000FC1 RID: 4033 RVA: 0x0013F09C File Offset: 0x0013E09C
		Rect IScrollInfo.MakeVisible(Visual visual, Rect rectangle)
		{
			if (this._scrollData == null)
			{
				rectangle = Rect.Empty;
			}
			else
			{
				rectangle = this._scrollData.MakeVisible(this, visual, rectangle);
			}
			return rectangle;
		}

		// Token: 0x170002B8 RID: 696
		// (get) Token: 0x06000FC2 RID: 4034 RVA: 0x0013F0C0 File Offset: 0x0013E0C0
		// (set) Token: 0x06000FC3 RID: 4035 RVA: 0x0013F0D7 File Offset: 0x0013E0D7
		bool IScrollInfo.CanVerticallyScroll
		{
			get
			{
				return this._scrollData != null && this._scrollData.CanVerticallyScroll;
			}
			set
			{
				if (this._scrollData != null)
				{
					this._scrollData.CanVerticallyScroll = value;
				}
			}
		}

		// Token: 0x170002B9 RID: 697
		// (get) Token: 0x06000FC4 RID: 4036 RVA: 0x0013F0ED File Offset: 0x0013E0ED
		// (set) Token: 0x06000FC5 RID: 4037 RVA: 0x0013F104 File Offset: 0x0013E104
		bool IScrollInfo.CanHorizontallyScroll
		{
			get
			{
				return this._scrollData != null && this._scrollData.CanHorizontallyScroll;
			}
			set
			{
				if (this._scrollData != null)
				{
					this._scrollData.CanHorizontallyScroll = value;
				}
			}
		}

		// Token: 0x170002BA RID: 698
		// (get) Token: 0x06000FC6 RID: 4038 RVA: 0x0013F11A File Offset: 0x0013E11A
		double IScrollInfo.ExtentWidth
		{
			get
			{
				if (this._scrollData == null)
				{
					return 0.0;
				}
				return this._scrollData.ExtentWidth;
			}
		}

		// Token: 0x170002BB RID: 699
		// (get) Token: 0x06000FC7 RID: 4039 RVA: 0x0013F139 File Offset: 0x0013E139
		double IScrollInfo.ExtentHeight
		{
			get
			{
				if (this._scrollData == null)
				{
					return 0.0;
				}
				return this._scrollData.ExtentHeight;
			}
		}

		// Token: 0x170002BC RID: 700
		// (get) Token: 0x06000FC8 RID: 4040 RVA: 0x0013F158 File Offset: 0x0013E158
		double IScrollInfo.ViewportWidth
		{
			get
			{
				if (this._scrollData == null)
				{
					return 0.0;
				}
				return this._scrollData.ViewportWidth;
			}
		}

		// Token: 0x170002BD RID: 701
		// (get) Token: 0x06000FC9 RID: 4041 RVA: 0x0013F177 File Offset: 0x0013E177
		double IScrollInfo.ViewportHeight
		{
			get
			{
				if (this._scrollData == null)
				{
					return 0.0;
				}
				return this._scrollData.ViewportHeight;
			}
		}

		// Token: 0x170002BE RID: 702
		// (get) Token: 0x06000FCA RID: 4042 RVA: 0x0013F196 File Offset: 0x0013E196
		double IScrollInfo.HorizontalOffset
		{
			get
			{
				if (this._scrollData == null)
				{
					return 0.0;
				}
				return this._scrollData.HorizontalOffset;
			}
		}

		// Token: 0x170002BF RID: 703
		// (get) Token: 0x06000FCB RID: 4043 RVA: 0x0013F1B5 File Offset: 0x0013E1B5
		double IScrollInfo.VerticalOffset
		{
			get
			{
				if (this._scrollData == null)
				{
					return 0.0;
				}
				return this._scrollData.VerticalOffset;
			}
		}

		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x06000FCC RID: 4044 RVA: 0x0013F1D4 File Offset: 0x0013E1D4
		// (set) Token: 0x06000FCD RID: 4045 RVA: 0x0013F1EB File Offset: 0x0013E1EB
		ScrollViewer IScrollInfo.ScrollOwner
		{
			get
			{
				if (this._scrollData == null)
				{
					return null;
				}
				return this._scrollData.ScrollOwner;
			}
			set
			{
				if (this._scrollData == null)
				{
					this._scrollData = new ScrollData();
				}
				this._scrollData.SetScrollOwner(this, value);
			}
		}

		// Token: 0x06000FCE RID: 4046 RVA: 0x0013F210 File Offset: 0x0013E210
		object IServiceProvider.GetService(Type serviceType)
		{
			object result = null;
			if (serviceType == typeof(ITextView))
			{
				if (this._textView == null && this._document != null)
				{
					this._textView = new DocumentPageTextView(this, this._document.StructuralCache.TextContainer);
				}
				result = this._textView;
			}
			else if (serviceType == typeof(ITextContainer) && this.Document != null)
			{
				result = this.Document.StructuralCache.TextContainer;
			}
			return result;
		}

		// Token: 0x04000AAF RID: 2735
		private FlowDocument _document;

		// Token: 0x04000AB0 RID: 2736
		private PageVisual _pageVisual;

		// Token: 0x04000AB1 RID: 2737
		private FlowDocumentFormatter _formatter;

		// Token: 0x04000AB2 RID: 2738
		private ScrollData _scrollData;

		// Token: 0x04000AB3 RID: 2739
		private DocumentPageTextView _textView;

		// Token: 0x04000AB4 RID: 2740
		private bool _suspendLayout;
	}
}
