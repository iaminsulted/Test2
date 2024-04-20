using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows.Documents;
using System.Windows.Media;
using MS.Internal;
using MS.Internal.Documents;

namespace System.Windows.Controls.Primitives
{
	// Token: 0x02000822 RID: 2082
	public class BulletDecorator : Decorator
	{
		// Token: 0x17001C1E RID: 7198
		// (get) Token: 0x0600795F RID: 31071 RVA: 0x00302D55 File Offset: 0x00301D55
		// (set) Token: 0x06007960 RID: 31072 RVA: 0x00302D67 File Offset: 0x00301D67
		public Brush Background
		{
			get
			{
				return (Brush)base.GetValue(BulletDecorator.BackgroundProperty);
			}
			set
			{
				base.SetValue(BulletDecorator.BackgroundProperty, value);
			}
		}

		// Token: 0x17001C1F RID: 7199
		// (get) Token: 0x06007961 RID: 31073 RVA: 0x00302D75 File Offset: 0x00301D75
		// (set) Token: 0x06007962 RID: 31074 RVA: 0x00302D80 File Offset: 0x00301D80
		public UIElement Bullet
		{
			get
			{
				return this._bullet;
			}
			set
			{
				if (this._bullet != value)
				{
					if (this._bullet != null)
					{
						base.RemoveVisualChild(this._bullet);
						base.RemoveLogicalChild(this._bullet);
					}
					this._bullet = value;
					base.AddLogicalChild(value);
					base.AddVisualChild(value);
					UIElement child = this.Child;
					if (child != null)
					{
						base.RemoveVisualChild(child);
						base.AddVisualChild(child);
					}
					base.InvalidateMeasure();
				}
			}
		}

		// Token: 0x17001C20 RID: 7200
		// (get) Token: 0x06007963 RID: 31075 RVA: 0x00302DE9 File Offset: 0x00301DE9
		protected internal override IEnumerator LogicalChildren
		{
			get
			{
				if (this._bullet == null)
				{
					return base.LogicalChildren;
				}
				if (this.Child == null)
				{
					return new SingleChildEnumerator(this._bullet);
				}
				return new BulletDecorator.DoubleChildEnumerator(this._bullet, this.Child);
			}
		}

		// Token: 0x06007964 RID: 31076 RVA: 0x00302E20 File Offset: 0x00301E20
		protected override void OnRender(DrawingContext dc)
		{
			Brush background = this.Background;
			if (background != null)
			{
				dc.DrawRectangle(background, null, new Rect(0.0, 0.0, base.RenderSize.Width, base.RenderSize.Height));
			}
		}

		// Token: 0x17001C21 RID: 7201
		// (get) Token: 0x06007965 RID: 31077 RVA: 0x00302E72 File Offset: 0x00301E72
		protected override int VisualChildrenCount
		{
			get
			{
				return ((this.Child == null) ? 0 : 1) + ((this._bullet == null) ? 0 : 1);
			}
		}

		// Token: 0x06007966 RID: 31078 RVA: 0x00302E90 File Offset: 0x00301E90
		protected override Visual GetVisualChild(int index)
		{
			if (index < 0 || index > this.VisualChildrenCount - 1)
			{
				throw new ArgumentOutOfRangeException("index", index, SR.Get("Visual_ArgumentOutOfRange"));
			}
			if (index == 0 && this._bullet != null)
			{
				return this._bullet;
			}
			return this.Child;
		}

		// Token: 0x06007967 RID: 31079 RVA: 0x00302EE0 File Offset: 0x00301EE0
		protected override Size MeasureOverride(Size constraint)
		{
			Size size = default(Size);
			Size size2 = default(Size);
			UIElement bullet = this.Bullet;
			UIElement child = this.Child;
			if (bullet != null)
			{
				bullet.Measure(constraint);
				size = bullet.DesiredSize;
			}
			if (child != null)
			{
				Size availableSize = constraint;
				availableSize.Width = Math.Max(0.0, availableSize.Width - size.Width);
				child.Measure(availableSize);
				size2 = child.DesiredSize;
			}
			return new Size(size.Width + size2.Width, Math.Max(size.Height, size2.Height));
		}

		// Token: 0x06007968 RID: 31080 RVA: 0x00302F7C File Offset: 0x00301F7C
		protected override Size ArrangeOverride(Size arrangeSize)
		{
			UIElement bullet = this.Bullet;
			UIElement child = this.Child;
			double x = 0.0;
			double num = 0.0;
			Size size = default(Size);
			if (bullet != null)
			{
				bullet.Arrange(new Rect(bullet.DesiredSize));
				size = bullet.RenderSize;
				x = size.Width;
			}
			if (child != null)
			{
				Size size2 = arrangeSize;
				if (bullet != null)
				{
					size2.Width = Math.Max(child.DesiredSize.Width, arrangeSize.Width - bullet.DesiredSize.Width);
					size2.Height = Math.Max(child.DesiredSize.Height, arrangeSize.Height);
				}
				child.Arrange(new Rect(x, 0.0, size2.Width, size2.Height));
				double num2 = this.GetFirstLineHeight(child) * 0.5;
				num += Math.Max(0.0, num2 - size.Height * 0.5);
			}
			if (bullet != null && !DoubleUtil.IsZero(num))
			{
				bullet.Arrange(new Rect(0.0, num, bullet.DesiredSize.Width, bullet.DesiredSize.Height));
			}
			return arrangeSize;
		}

		// Token: 0x06007969 RID: 31081 RVA: 0x003030D4 File Offset: 0x003020D4
		private double GetFirstLineHeight(UIElement element)
		{
			UIElement uielement = this.FindText(element);
			ReadOnlyCollection<LineResult> readOnlyCollection = null;
			if (uielement != null)
			{
				TextBlock textBlock = (TextBlock)uielement;
				if (textBlock.IsLayoutDataValid)
				{
					readOnlyCollection = textBlock.GetLineResults();
				}
			}
			else
			{
				uielement = this.FindFlowDocumentScrollViewer(element);
				if (uielement != null)
				{
					TextDocumentView textDocumentView = ((IServiceProvider)uielement).GetService(typeof(ITextView)) as TextDocumentView;
					if (textDocumentView != null && textDocumentView.IsValid)
					{
						ReadOnlyCollection<ColumnResult> columns = textDocumentView.Columns;
						if (columns != null && columns.Count > 0)
						{
							ReadOnlyCollection<ParagraphResult> paragraphs = columns[0].Paragraphs;
							if (paragraphs != null && paragraphs.Count > 0)
							{
								ContainerParagraphResult containerParagraphResult = paragraphs[0] as ContainerParagraphResult;
								if (containerParagraphResult != null)
								{
									TextParagraphResult textParagraphResult = containerParagraphResult.Paragraphs[0] as TextParagraphResult;
									if (textParagraphResult != null)
									{
										readOnlyCollection = textParagraphResult.Lines;
									}
								}
							}
						}
					}
				}
			}
			if (readOnlyCollection != null && readOnlyCollection.Count > 0)
			{
				Point inPoint = default(Point);
				uielement.TransformToAncestor(element).TryTransform(inPoint, out inPoint);
				return readOnlyCollection[0].LayoutBox.Height + inPoint.Y * 2.0;
			}
			return element.RenderSize.Height;
		}

		// Token: 0x0600796A RID: 31082 RVA: 0x00303204 File Offset: 0x00302204
		private TextBlock FindText(Visual root)
		{
			TextBlock textBlock = root as TextBlock;
			if (textBlock != null)
			{
				return textBlock;
			}
			ContentPresenter contentPresenter = root as ContentPresenter;
			if (contentPresenter != null)
			{
				if (VisualTreeHelper.GetChildrenCount(contentPresenter) == 1)
				{
					DependencyObject child = VisualTreeHelper.GetChild(contentPresenter, 0);
					TextBlock textBlock2 = child as TextBlock;
					if (textBlock2 == null)
					{
						AccessText accessText = child as AccessText;
						if (accessText != null && VisualTreeHelper.GetChildrenCount(accessText) == 1)
						{
							textBlock2 = (VisualTreeHelper.GetChild(accessText, 0) as TextBlock);
						}
					}
					return textBlock2;
				}
			}
			else
			{
				AccessText accessText2 = root as AccessText;
				if (accessText2 != null && VisualTreeHelper.GetChildrenCount(accessText2) == 1)
				{
					return VisualTreeHelper.GetChild(accessText2, 0) as TextBlock;
				}
			}
			return null;
		}

		// Token: 0x0600796B RID: 31083 RVA: 0x00303290 File Offset: 0x00302290
		private FlowDocumentScrollViewer FindFlowDocumentScrollViewer(Visual root)
		{
			FlowDocumentScrollViewer flowDocumentScrollViewer = root as FlowDocumentScrollViewer;
			if (flowDocumentScrollViewer != null)
			{
				return flowDocumentScrollViewer;
			}
			ContentPresenter contentPresenter = root as ContentPresenter;
			if (contentPresenter != null && VisualTreeHelper.GetChildrenCount(contentPresenter) == 1)
			{
				return VisualTreeHelper.GetChild(contentPresenter, 0) as FlowDocumentScrollViewer;
			}
			return null;
		}

		// Token: 0x040039A6 RID: 14758
		public static readonly DependencyProperty BackgroundProperty = Panel.BackgroundProperty.AddOwner(typeof(BulletDecorator), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

		// Token: 0x040039A7 RID: 14759
		private UIElement _bullet;

		// Token: 0x02000C44 RID: 3140
		private class DoubleChildEnumerator : IEnumerator
		{
			// Token: 0x0600916C RID: 37228 RVA: 0x00349411 File Offset: 0x00348411
			internal DoubleChildEnumerator(object child1, object child2)
			{
				this._child1 = child1;
				this._child2 = child2;
			}

			// Token: 0x17001FD8 RID: 8152
			// (get) Token: 0x0600916D RID: 37229 RVA: 0x00349430 File Offset: 0x00348430
			object IEnumerator.Current
			{
				get
				{
					int index = this._index;
					if (index == 0)
					{
						return this._child1;
					}
					if (index != 1)
					{
						return null;
					}
					return this._child2;
				}
			}

			// Token: 0x0600916E RID: 37230 RVA: 0x0034945C File Offset: 0x0034845C
			bool IEnumerator.MoveNext()
			{
				this._index++;
				return this._index < 2;
			}

			// Token: 0x0600916F RID: 37231 RVA: 0x00349475 File Offset: 0x00348475
			void IEnumerator.Reset()
			{
				this._index = -1;
			}

			// Token: 0x04004C15 RID: 19477
			private int _index = -1;

			// Token: 0x04004C16 RID: 19478
			private object _child1;

			// Token: 0x04004C17 RID: 19479
			private object _child2;
		}
	}
}
