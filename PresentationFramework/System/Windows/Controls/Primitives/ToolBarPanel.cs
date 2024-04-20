using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows.Data;
using System.Windows.Media;
using MS.Internal;
using MS.Internal.KnownBoxes;
using MS.Internal.Telemetry.PresentationFramework;

namespace System.Windows.Controls.Primitives
{
	// Token: 0x02000860 RID: 2144
	public class ToolBarPanel : StackPanel
	{
		// Token: 0x06007E71 RID: 32369 RVA: 0x003187C8 File Offset: 0x003177C8
		static ToolBarPanel()
		{
			ControlsTraceLogger.AddControl(TelemetryControls.ToolBarPanel);
		}

		// Token: 0x17001D2C RID: 7468
		// (get) Token: 0x06007E73 RID: 32371 RVA: 0x003187E0 File Offset: 0x003177E0
		// (set) Token: 0x06007E74 RID: 32372 RVA: 0x003187E8 File Offset: 0x003177E8
		internal double MinLength { get; private set; }

		// Token: 0x17001D2D RID: 7469
		// (get) Token: 0x06007E75 RID: 32373 RVA: 0x003187F1 File Offset: 0x003177F1
		// (set) Token: 0x06007E76 RID: 32374 RVA: 0x003187F9 File Offset: 0x003177F9
		internal double MaxLength { get; private set; }

		// Token: 0x06007E77 RID: 32375 RVA: 0x00318804 File Offset: 0x00317804
		private bool MeasureGeneratedItems(bool asNeededPass, Size constraint, bool horizontal, double maxExtent, ref Size panelDesiredSize, out double overflowExtent)
		{
			ToolBarOverflowPanel toolBarOverflowPanel = this.ToolBarOverflowPanel;
			bool flag = false;
			bool result = false;
			bool flag2 = false;
			overflowExtent = 0.0;
			UIElementCollection internalChildren = base.InternalChildren;
			int num = internalChildren.Count;
			int num2 = 0;
			for (int i = 0; i < this._generatedItemsCollection.Count; i++)
			{
				UIElement uielement = this._generatedItemsCollection[i];
				OverflowMode overflowMode = ToolBar.GetOverflowMode(uielement);
				bool flag3 = overflowMode == OverflowMode.AsNeeded;
				if (flag3 == asNeededPass)
				{
					DependencyObject parent = VisualTreeHelper.GetParent(uielement);
					if (overflowMode != OverflowMode.Always && !flag)
					{
						ToolBar.SetIsOverflowItem(uielement, BooleanBoxes.FalseBox);
						uielement.Measure(constraint);
						Size desiredSize = uielement.DesiredSize;
						if (flag3)
						{
							double value;
							if (horizontal)
							{
								value = desiredSize.Width + panelDesiredSize.Width;
							}
							else
							{
								value = desiredSize.Height + panelDesiredSize.Height;
							}
							if (DoubleUtil.GreaterThan(value, maxExtent))
							{
								flag = true;
							}
						}
						if (!flag)
						{
							if (horizontal)
							{
								panelDesiredSize.Width += desiredSize.Width;
								panelDesiredSize.Height = Math.Max(panelDesiredSize.Height, desiredSize.Height);
							}
							else
							{
								panelDesiredSize.Width = Math.Max(panelDesiredSize.Width, desiredSize.Width);
								panelDesiredSize.Height += desiredSize.Height;
							}
							if (parent != this)
							{
								if (parent == toolBarOverflowPanel && toolBarOverflowPanel != null)
								{
									toolBarOverflowPanel.Children.Remove(uielement);
								}
								if (num2 < num)
								{
									internalChildren.InsertInternal(num2, uielement);
								}
								else
								{
									internalChildren.AddInternal(uielement);
								}
								num++;
							}
							num2++;
						}
					}
					if (overflowMode == OverflowMode.Always || flag)
					{
						result = true;
						if (uielement.MeasureDirty)
						{
							ToolBar.SetIsOverflowItem(uielement, BooleanBoxes.FalseBox);
							uielement.Measure(constraint);
						}
						Size desiredSize2 = uielement.DesiredSize;
						if (horizontal)
						{
							overflowExtent += desiredSize2.Width;
							panelDesiredSize.Height = Math.Max(panelDesiredSize.Height, desiredSize2.Height);
						}
						else
						{
							overflowExtent += desiredSize2.Height;
							panelDesiredSize.Width = Math.Max(panelDesiredSize.Width, desiredSize2.Width);
						}
						ToolBar.SetIsOverflowItem(uielement, BooleanBoxes.TrueBox);
						if (parent == this)
						{
							internalChildren.RemoveNoVerify(uielement);
							num--;
							flag2 = true;
						}
						else if (parent == null)
						{
							flag2 = true;
						}
					}
				}
				else if (num2 < num && internalChildren[num2] == uielement)
				{
					num2++;
				}
			}
			if (flag2 && toolBarOverflowPanel != null)
			{
				toolBarOverflowPanel.InvalidateMeasure();
			}
			return result;
		}

		// Token: 0x06007E78 RID: 32376 RVA: 0x00318A78 File Offset: 0x00317A78
		protected override Size MeasureOverride(Size constraint)
		{
			Size result = default(Size);
			if (base.IsItemsHost)
			{
				Size constraint2 = constraint;
				bool flag = base.Orientation == Orientation.Horizontal;
				double maxExtent;
				if (flag)
				{
					constraint2.Width = double.PositiveInfinity;
					maxExtent = constraint.Width;
				}
				else
				{
					constraint2.Height = double.PositiveInfinity;
					maxExtent = constraint.Height;
				}
				double num;
				bool flag2 = this.MeasureGeneratedItems(false, constraint2, flag, maxExtent, ref result, out num);
				this.MinLength = (flag ? result.Width : result.Height);
				bool flag3 = this.MeasureGeneratedItems(true, constraint2, flag, maxExtent, ref result, out num);
				this.MaxLength = (flag ? result.Width : result.Height) + num;
				ToolBar toolBar = this.ToolBar;
				if (toolBar != null)
				{
					toolBar.SetValue(ToolBar.HasOverflowItemsPropertyKey, flag2 || flag3);
				}
			}
			else
			{
				result = base.MeasureOverride(constraint);
			}
			return result;
		}

		// Token: 0x06007E79 RID: 32377 RVA: 0x00318B5C File Offset: 0x00317B5C
		protected override Size ArrangeOverride(Size arrangeSize)
		{
			UIElementCollection internalChildren = base.InternalChildren;
			bool flag = base.Orientation == Orientation.Horizontal;
			Rect finalRect = new Rect(arrangeSize);
			double num = 0.0;
			int i = 0;
			int count = internalChildren.Count;
			while (i < count)
			{
				UIElement uielement = internalChildren[i];
				if (flag)
				{
					finalRect.X += num;
					num = uielement.DesiredSize.Width;
					finalRect.Width = num;
					finalRect.Height = Math.Max(arrangeSize.Height, uielement.DesiredSize.Height);
				}
				else
				{
					finalRect.Y += num;
					num = uielement.DesiredSize.Height;
					finalRect.Height = num;
					finalRect.Width = Math.Max(arrangeSize.Width, uielement.DesiredSize.Width);
				}
				uielement.Arrange(finalRect);
				i++;
			}
			return arrangeSize;
		}

		// Token: 0x06007E7A RID: 32378 RVA: 0x00318C5C File Offset: 0x00317C5C
		internal override void OnPreApplyTemplate()
		{
			base.OnPreApplyTemplate();
			if (base.TemplatedParent is ToolBar && !base.HasNonDefaultValue(StackPanel.OrientationProperty))
			{
				Binding binding = new Binding();
				binding.RelativeSource = RelativeSource.TemplatedParent;
				binding.Path = new PropertyPath(ToolBar.OrientationProperty);
				base.SetBinding(StackPanel.OrientationProperty, binding);
			}
		}

		// Token: 0x06007E7B RID: 32379 RVA: 0x00318CB8 File Offset: 0x00317CB8
		internal override void GenerateChildren()
		{
			base.GenerateChildren();
			UIElementCollection internalChildren = base.InternalChildren;
			if (this._generatedItemsCollection == null)
			{
				this._generatedItemsCollection = new List<UIElement>(internalChildren.Count);
			}
			else
			{
				this._generatedItemsCollection.Clear();
			}
			ToolBarOverflowPanel toolBarOverflowPanel = this.ToolBarOverflowPanel;
			if (toolBarOverflowPanel != null)
			{
				toolBarOverflowPanel.Children.Clear();
				toolBarOverflowPanel.InvalidateMeasure();
			}
			int count = internalChildren.Count;
			for (int i = 0; i < count; i++)
			{
				UIElement uielement = internalChildren[i];
				ToolBar.SetIsOverflowItem(uielement, BooleanBoxes.FalseBox);
				this._generatedItemsCollection.Add(uielement);
			}
		}

		// Token: 0x06007E7C RID: 32380 RVA: 0x00318D48 File Offset: 0x00317D48
		internal override bool OnItemsChangedInternal(object sender, ItemsChangedEventArgs args)
		{
			switch (args.Action)
			{
			case NotifyCollectionChangedAction.Add:
				this.AddChildren(args.Position, args.ItemCount);
				break;
			case NotifyCollectionChangedAction.Remove:
				this.RemoveChildren(args.Position, args.ItemUICount);
				break;
			case NotifyCollectionChangedAction.Replace:
				this.ReplaceChildren(args.Position, args.ItemCount, args.ItemUICount);
				break;
			case NotifyCollectionChangedAction.Move:
				this.MoveChildren(args.OldPosition, args.Position, args.ItemUICount);
				break;
			case NotifyCollectionChangedAction.Reset:
				base.OnItemsChangedInternal(sender, args);
				break;
			}
			return true;
		}

		// Token: 0x06007E7D RID: 32381 RVA: 0x00318DE0 File Offset: 0x00317DE0
		private void AddChildren(GeneratorPosition pos, int itemCount)
		{
			IItemContainerGenerator generator = base.Generator;
			using (generator.StartAt(pos, GeneratorDirection.Forward))
			{
				for (int i = 0; i < itemCount; i++)
				{
					UIElement uielement = generator.GenerateNext() as UIElement;
					if (uielement != null)
					{
						this._generatedItemsCollection.Insert(pos.Index + 1 + i, uielement);
						generator.PrepareItemContainer(uielement);
					}
					else
					{
						ItemContainerGenerator itemContainerGenerator = base.Generator as ItemContainerGenerator;
						if (itemContainerGenerator != null)
						{
							itemContainerGenerator.Verify();
						}
					}
				}
			}
		}

		// Token: 0x06007E7E RID: 32382 RVA: 0x00318E6C File Offset: 0x00317E6C
		private void RemoveChild(UIElement child)
		{
			DependencyObject parent = VisualTreeHelper.GetParent(child);
			if (parent == this)
			{
				base.InternalChildren.RemoveInternal(child);
				return;
			}
			ToolBarOverflowPanel toolBarOverflowPanel = this.ToolBarOverflowPanel;
			if (parent == toolBarOverflowPanel && toolBarOverflowPanel != null)
			{
				toolBarOverflowPanel.Children.Remove(child);
			}
		}

		// Token: 0x06007E7F RID: 32383 RVA: 0x00318EAC File Offset: 0x00317EAC
		private void RemoveChildren(GeneratorPosition pos, int containerCount)
		{
			for (int i = 0; i < containerCount; i++)
			{
				this.RemoveChild(this._generatedItemsCollection[pos.Index + i]);
			}
			this._generatedItemsCollection.RemoveRange(pos.Index, containerCount);
		}

		// Token: 0x06007E80 RID: 32384 RVA: 0x00318EF4 File Offset: 0x00317EF4
		private void ReplaceChildren(GeneratorPosition pos, int itemCount, int containerCount)
		{
			IItemContainerGenerator generator = base.Generator;
			using (generator.StartAt(pos, GeneratorDirection.Forward, true))
			{
				for (int i = 0; i < itemCount; i++)
				{
					bool flag;
					UIElement uielement = generator.GenerateNext(out flag) as UIElement;
					if (uielement != null && !flag)
					{
						this.RemoveChild(this._generatedItemsCollection[pos.Index + i]);
						this._generatedItemsCollection[pos.Index + i] = uielement;
						generator.PrepareItemContainer(uielement);
					}
					else
					{
						ItemContainerGenerator itemContainerGenerator = base.Generator as ItemContainerGenerator;
						if (itemContainerGenerator != null)
						{
							itemContainerGenerator.Verify();
						}
					}
				}
			}
		}

		// Token: 0x06007E81 RID: 32385 RVA: 0x00318FA4 File Offset: 0x00317FA4
		private void MoveChildren(GeneratorPosition fromPos, GeneratorPosition toPos, int containerCount)
		{
			if (fromPos == toPos)
			{
				return;
			}
			int num = base.Generator.IndexFromGeneratorPosition(toPos);
			UIElement[] array = new UIElement[containerCount];
			for (int i = 0; i < containerCount; i++)
			{
				UIElement uielement = this._generatedItemsCollection[fromPos.Index + i];
				this.RemoveChild(uielement);
				array[i] = uielement;
			}
			this._generatedItemsCollection.RemoveRange(fromPos.Index, containerCount);
			for (int j = 0; j < containerCount; j++)
			{
				this._generatedItemsCollection.Insert(num + j, array[j]);
			}
		}

		// Token: 0x17001D2E RID: 7470
		// (get) Token: 0x06007E82 RID: 32386 RVA: 0x0031874A File Offset: 0x0031774A
		private ToolBar ToolBar
		{
			get
			{
				return base.TemplatedParent as ToolBar;
			}
		}

		// Token: 0x17001D2F RID: 7471
		// (get) Token: 0x06007E83 RID: 32387 RVA: 0x00319034 File Offset: 0x00318034
		private ToolBarOverflowPanel ToolBarOverflowPanel
		{
			get
			{
				ToolBar toolBar = this.ToolBar;
				if (toolBar != null)
				{
					return toolBar.ToolBarOverflowPanel;
				}
				return null;
			}
		}

		// Token: 0x17001D30 RID: 7472
		// (get) Token: 0x06007E84 RID: 32388 RVA: 0x00319053 File Offset: 0x00318053
		internal List<UIElement> GeneratedItemsCollection
		{
			get
			{
				return this._generatedItemsCollection;
			}
		}

		// Token: 0x04003B3F RID: 15167
		private List<UIElement> _generatedItemsCollection;
	}
}
