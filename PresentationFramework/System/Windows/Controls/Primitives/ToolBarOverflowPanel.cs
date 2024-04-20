using System;
using System.Collections.Generic;
using MS.Internal;

namespace System.Windows.Controls.Primitives
{
	// Token: 0x0200085F RID: 2143
	public class ToolBarOverflowPanel : Panel
	{
		// Token: 0x17001D29 RID: 7465
		// (get) Token: 0x06007E66 RID: 32358 RVA: 0x003182EF File Offset: 0x003172EF
		// (set) Token: 0x06007E67 RID: 32359 RVA: 0x00318301 File Offset: 0x00317301
		public double WrapWidth
		{
			get
			{
				return (double)base.GetValue(ToolBarOverflowPanel.WrapWidthProperty);
			}
			set
			{
				base.SetValue(ToolBarOverflowPanel.WrapWidthProperty, value);
			}
		}

		// Token: 0x06007E68 RID: 32360 RVA: 0x00318314 File Offset: 0x00317314
		private static bool IsWrapWidthValid(object value)
		{
			double num = (double)value;
			return DoubleUtil.IsNaN(num) || (DoubleUtil.GreaterThanOrClose(num, 0.0) && !double.IsPositiveInfinity(num));
		}

		// Token: 0x06007E69 RID: 32361 RVA: 0x00318350 File Offset: 0x00317350
		protected override Size MeasureOverride(Size constraint)
		{
			Size size = default(Size);
			this._panelSize = default(Size);
			this._wrapWidth = (double.IsNaN(this.WrapWidth) ? constraint.Width : this.WrapWidth);
			UIElementCollection internalChildren = base.InternalChildren;
			int num = internalChildren.Count;
			ToolBarPanel toolBarPanel = this.ToolBarPanel;
			if (toolBarPanel != null)
			{
				List<UIElement> generatedItemsCollection = toolBarPanel.GeneratedItemsCollection;
				int num2 = (generatedItemsCollection != null) ? generatedItemsCollection.Count : 0;
				int num3 = 0;
				for (int i = 0; i < num2; i++)
				{
					UIElement uielement = generatedItemsCollection[i];
					if (uielement != null && ToolBar.GetIsOverflowItem(uielement) && !(uielement is Separator))
					{
						if (num3 < num)
						{
							if (internalChildren[num3] != uielement)
							{
								internalChildren.Insert(num3, uielement);
								num++;
							}
						}
						else
						{
							internalChildren.Add(uielement);
							num++;
						}
						num3++;
					}
				}
			}
			for (int j = 0; j < num; j++)
			{
				UIElement uielement2 = internalChildren[j];
				uielement2.Measure(constraint);
				Size desiredSize = uielement2.DesiredSize;
				if (DoubleUtil.GreaterThan(desiredSize.Width, this._wrapWidth))
				{
					this._wrapWidth = desiredSize.Width;
				}
			}
			this._wrapWidth = Math.Min(this._wrapWidth, constraint.Width);
			for (int k = 0; k < internalChildren.Count; k++)
			{
				Size desiredSize2 = internalChildren[k].DesiredSize;
				if (DoubleUtil.GreaterThan(size.Width + desiredSize2.Width, this._wrapWidth))
				{
					this._panelSize.Width = Math.Max(size.Width, this._panelSize.Width);
					this._panelSize.Height = this._panelSize.Height + size.Height;
					size = desiredSize2;
					if (DoubleUtil.GreaterThan(desiredSize2.Width, this._wrapWidth))
					{
						this._panelSize.Width = Math.Max(desiredSize2.Width, this._panelSize.Width);
						this._panelSize.Height = this._panelSize.Height + desiredSize2.Height;
						size = default(Size);
					}
				}
				else
				{
					size.Width += desiredSize2.Width;
					size.Height = Math.Max(desiredSize2.Height, size.Height);
				}
			}
			this._panelSize.Width = Math.Max(size.Width, this._panelSize.Width);
			this._panelSize.Height = this._panelSize.Height + size.Height;
			return this._panelSize;
		}

		// Token: 0x06007E6A RID: 32362 RVA: 0x003185E0 File Offset: 0x003175E0
		protected override Size ArrangeOverride(Size arrangeBounds)
		{
			int start = 0;
			Size size = default(Size);
			double num = 0.0;
			this._wrapWidth = Math.Min(this._wrapWidth, arrangeBounds.Width);
			UIElementCollection children = base.Children;
			for (int i = 0; i < children.Count; i++)
			{
				Size desiredSize = children[i].DesiredSize;
				if (DoubleUtil.GreaterThan(size.Width + desiredSize.Width, this._wrapWidth))
				{
					this.arrangeLine(num, size.Height, start, i);
					num += size.Height;
					start = i;
					size = desiredSize;
				}
				else
				{
					size.Width += desiredSize.Width;
					size.Height = Math.Max(desiredSize.Height, size.Height);
				}
			}
			this.arrangeLine(num, size.Height, start, children.Count);
			return this._panelSize;
		}

		// Token: 0x06007E6B RID: 32363 RVA: 0x003186D3 File Offset: 0x003176D3
		protected override UIElementCollection CreateUIElementCollection(FrameworkElement logicalParent)
		{
			return new UIElementCollection(this, (base.TemplatedParent == null) ? logicalParent : null);
		}

		// Token: 0x06007E6C RID: 32364 RVA: 0x003186E8 File Offset: 0x003176E8
		private void arrangeLine(double y, double lineHeight, int start, int end)
		{
			double num = 0.0;
			UIElementCollection children = base.Children;
			for (int i = start; i < end; i++)
			{
				UIElement uielement = children[i];
				uielement.Arrange(new Rect(num, y, uielement.DesiredSize.Width, lineHeight));
				num += uielement.DesiredSize.Width;
			}
		}

		// Token: 0x17001D2A RID: 7466
		// (get) Token: 0x06007E6D RID: 32365 RVA: 0x0031874A File Offset: 0x0031774A
		private ToolBar ToolBar
		{
			get
			{
				return base.TemplatedParent as ToolBar;
			}
		}

		// Token: 0x17001D2B RID: 7467
		// (get) Token: 0x06007E6E RID: 32366 RVA: 0x00318758 File Offset: 0x00317758
		private ToolBarPanel ToolBarPanel
		{
			get
			{
				ToolBar toolBar = this.ToolBar;
				if (toolBar != null)
				{
					return toolBar.ToolBarPanel;
				}
				return null;
			}
		}

		// Token: 0x04003B3A RID: 15162
		public static readonly DependencyProperty WrapWidthProperty = DependencyProperty.Register("WrapWidth", typeof(double), typeof(ToolBarOverflowPanel), new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsMeasure), new ValidateValueCallback(ToolBarOverflowPanel.IsWrapWidthValid));

		// Token: 0x04003B3B RID: 15163
		private double _wrapWidth;

		// Token: 0x04003B3C RID: 15164
		private Size _panelSize;
	}
}
