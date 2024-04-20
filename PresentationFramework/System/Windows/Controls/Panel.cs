using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
using System.Windows.Media;
using MS.Internal.Controls;
using MS.Internal.KnownBoxes;
using MS.Internal.PresentationFramework;

namespace System.Windows.Controls
{
	// Token: 0x020007B9 RID: 1977
	[Localizability(LocalizationCategory.Ignore)]
	[ContentProperty("Children")]
	public abstract class Panel : FrameworkElement, IAddChild
	{
		// Token: 0x06007041 RID: 28737 RVA: 0x002D7143 File Offset: 0x002D6143
		protected Panel()
		{
			this._zConsonant = (int)Panel.ZIndexProperty.GetDefaultValue(base.DependencyObjectType);
		}

		// Token: 0x06007042 RID: 28738 RVA: 0x002D7168 File Offset: 0x002D6168
		protected override void OnRender(DrawingContext dc)
		{
			Brush background = this.Background;
			if (background != null)
			{
				Size renderSize = base.RenderSize;
				dc.DrawRectangle(background, null, new Rect(0.0, 0.0, renderSize.Width, renderSize.Height));
			}
		}

		// Token: 0x06007043 RID: 28739 RVA: 0x002D71B4 File Offset: 0x002D61B4
		void IAddChild.AddChild(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (this.IsItemsHost)
			{
				throw new InvalidOperationException(SR.Get("Panel_BoundPanel_NoChildren"));
			}
			UIElement uielement = value as UIElement;
			if (uielement == null)
			{
				throw new ArgumentException(SR.Get("UnexpectedParameterType", new object[]
				{
					value.GetType(),
					typeof(UIElement)
				}), "value");
			}
			this.Children.Add(uielement);
		}

		// Token: 0x06007044 RID: 28740 RVA: 0x00175B1C File Offset: 0x00174B1C
		void IAddChild.AddText(string text)
		{
			XamlSerializerUtil.ThrowIfNonWhiteSpaceInAddText(text, this);
		}

		// Token: 0x170019EE RID: 6638
		// (get) Token: 0x06007045 RID: 28741 RVA: 0x002D722F File Offset: 0x002D622F
		// (set) Token: 0x06007046 RID: 28742 RVA: 0x002D7241 File Offset: 0x002D6241
		public Brush Background
		{
			get
			{
				return (Brush)base.GetValue(Panel.BackgroundProperty);
			}
			set
			{
				base.SetValue(Panel.BackgroundProperty, value);
			}
		}

		// Token: 0x170019EF RID: 6639
		// (get) Token: 0x06007047 RID: 28743 RVA: 0x002D724F File Offset: 0x002D624F
		protected internal override IEnumerator LogicalChildren
		{
			get
			{
				if (this.VisualChildrenCount == 0 || this.IsItemsHost)
				{
					return EmptyEnumerator.Instance;
				}
				return this.Children.GetEnumerator();
			}
		}

		// Token: 0x170019F0 RID: 6640
		// (get) Token: 0x06007048 RID: 28744 RVA: 0x002D7272 File Offset: 0x002D6272
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public UIElementCollection Children
		{
			get
			{
				return this.InternalChildren;
			}
		}

		// Token: 0x06007049 RID: 28745 RVA: 0x002D727A File Offset: 0x002D627A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeChildren()
		{
			return !this.IsItemsHost && this.Children != null && this.Children.Count > 0;
		}

		// Token: 0x170019F1 RID: 6641
		// (get) Token: 0x0600704A RID: 28746 RVA: 0x002D729D File Offset: 0x002D629D
		// (set) Token: 0x0600704B RID: 28747 RVA: 0x002D72AF File Offset: 0x002D62AF
		[Bindable(false)]
		[Category("Behavior")]
		public bool IsItemsHost
		{
			get
			{
				return (bool)base.GetValue(Panel.IsItemsHostProperty);
			}
			set
			{
				base.SetValue(Panel.IsItemsHostProperty, BooleanBoxes.Box(value));
			}
		}

		// Token: 0x0600704C RID: 28748 RVA: 0x002D72C2 File Offset: 0x002D62C2
		private static void OnIsItemsHostChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((Panel)d).OnIsItemsHostChanged((bool)e.OldValue, (bool)e.NewValue);
		}

		// Token: 0x0600704D RID: 28749 RVA: 0x002D72E8 File Offset: 0x002D62E8
		protected virtual void OnIsItemsHostChanged(bool oldIsItemsHost, bool newIsItemsHost)
		{
			DependencyObject itemsOwnerInternal = ItemsControl.GetItemsOwnerInternal(this);
			ItemsControl itemsControl = itemsOwnerInternal as ItemsControl;
			Panel panel = null;
			if (itemsControl != null)
			{
				IItemContainerGenerator itemContainerGenerator = itemsControl.ItemContainerGenerator;
				if (itemContainerGenerator != null && itemContainerGenerator == itemContainerGenerator.GetItemContainerGeneratorForPanel(this))
				{
					panel = itemsControl.ItemsHost;
					itemsControl.ItemsHost = this;
				}
			}
			else
			{
				GroupItem groupItem = itemsOwnerInternal as GroupItem;
				if (groupItem != null)
				{
					IItemContainerGenerator generator = groupItem.Generator;
					if (generator != null && generator == generator.GetItemContainerGeneratorForPanel(this))
					{
						panel = groupItem.ItemsHost;
						groupItem.ItemsHost = this;
					}
				}
			}
			if (panel != null && panel != this)
			{
				panel.VerifyBoundState();
			}
			this.VerifyBoundState();
		}

		// Token: 0x170019F2 RID: 6642
		// (get) Token: 0x0600704E RID: 28750 RVA: 0x002D7376 File Offset: 0x002D6376
		public Orientation LogicalOrientationPublic
		{
			get
			{
				return this.LogicalOrientation;
			}
		}

		// Token: 0x170019F3 RID: 6643
		// (get) Token: 0x0600704F RID: 28751 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		protected internal virtual Orientation LogicalOrientation
		{
			get
			{
				return Orientation.Vertical;
			}
		}

		// Token: 0x170019F4 RID: 6644
		// (get) Token: 0x06007050 RID: 28752 RVA: 0x002D737E File Offset: 0x002D637E
		public bool HasLogicalOrientationPublic
		{
			get
			{
				return this.HasLogicalOrientation;
			}
		}

		// Token: 0x170019F5 RID: 6645
		// (get) Token: 0x06007051 RID: 28753 RVA: 0x00105F35 File Offset: 0x00104F35
		protected internal virtual bool HasLogicalOrientation
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170019F6 RID: 6646
		// (get) Token: 0x06007052 RID: 28754 RVA: 0x002D7386 File Offset: 0x002D6386
		protected internal UIElementCollection InternalChildren
		{
			get
			{
				this.VerifyBoundState();
				if (this.IsItemsHost)
				{
					this.EnsureGenerator();
				}
				else if (this._uiElementCollection == null)
				{
					this.EnsureEmptyChildren(this);
				}
				return this._uiElementCollection;
			}
		}

		// Token: 0x170019F7 RID: 6647
		// (get) Token: 0x06007053 RID: 28755 RVA: 0x002D73B4 File Offset: 0x002D63B4
		protected override int VisualChildrenCount
		{
			get
			{
				if (this._uiElementCollection == null)
				{
					return 0;
				}
				return this._uiElementCollection.Count;
			}
		}

		// Token: 0x06007054 RID: 28756 RVA: 0x002D73CC File Offset: 0x002D63CC
		protected override Visual GetVisualChild(int index)
		{
			if (this._uiElementCollection == null)
			{
				throw new ArgumentOutOfRangeException("index", index, SR.Get("Visual_ArgumentOutOfRange"));
			}
			if (this.IsZStateDirty)
			{
				this.RecomputeZState();
			}
			int index2 = (this._zLut != null) ? this._zLut[index] : index;
			return this._uiElementCollection[index2];
		}

		// Token: 0x06007055 RID: 28757 RVA: 0x00234D50 File Offset: 0x00233D50
		protected virtual UIElementCollection CreateUIElementCollection(FrameworkElement logicalParent)
		{
			return new UIElementCollection(this, logicalParent);
		}

		// Token: 0x170019F8 RID: 6648
		// (get) Token: 0x06007056 RID: 28758 RVA: 0x002D742A File Offset: 0x002D642A
		internal IItemContainerGenerator Generator
		{
			get
			{
				return this._itemContainerGenerator;
			}
		}

		// Token: 0x170019F9 RID: 6649
		// (get) Token: 0x06007057 RID: 28759 RVA: 0x002D7432 File Offset: 0x002D6432
		// (set) Token: 0x06007058 RID: 28760 RVA: 0x002D743B File Offset: 0x002D643B
		internal bool VSP_IsVirtualizing
		{
			get
			{
				return this.GetBoolField(Panel.BoolField.IsVirtualizing);
			}
			set
			{
				this.SetBoolField(Panel.BoolField.IsVirtualizing, value);
			}
		}

		// Token: 0x170019FA RID: 6650
		// (get) Token: 0x06007059 RID: 28761 RVA: 0x002D7445 File Offset: 0x002D6445
		// (set) Token: 0x0600705A RID: 28762 RVA: 0x002D744E File Offset: 0x002D644E
		internal bool VSP_HasMeasured
		{
			get
			{
				return this.GetBoolField(Panel.BoolField.HasMeasured);
			}
			set
			{
				this.SetBoolField(Panel.BoolField.HasMeasured, value);
			}
		}

		// Token: 0x170019FB RID: 6651
		// (get) Token: 0x0600705B RID: 28763 RVA: 0x002D7458 File Offset: 0x002D6458
		// (set) Token: 0x0600705C RID: 28764 RVA: 0x002D7462 File Offset: 0x002D6462
		internal bool VSP_MustDisableVirtualization
		{
			get
			{
				return this.GetBoolField(Panel.BoolField.MustDisableVirtualization);
			}
			set
			{
				this.SetBoolField(Panel.BoolField.MustDisableVirtualization, value);
			}
		}

		// Token: 0x170019FC RID: 6652
		// (get) Token: 0x0600705D RID: 28765 RVA: 0x002D746D File Offset: 0x002D646D
		// (set) Token: 0x0600705E RID: 28766 RVA: 0x002D7477 File Offset: 0x002D6477
		internal bool VSP_IsPixelBased
		{
			get
			{
				return this.GetBoolField(Panel.BoolField.IsPixelBased);
			}
			set
			{
				this.SetBoolField(Panel.BoolField.IsPixelBased, value);
			}
		}

		// Token: 0x170019FD RID: 6653
		// (get) Token: 0x0600705F RID: 28767 RVA: 0x002D7482 File Offset: 0x002D6482
		// (set) Token: 0x06007060 RID: 28768 RVA: 0x002D748C File Offset: 0x002D648C
		internal bool VSP_InRecyclingMode
		{
			get
			{
				return this.GetBoolField(Panel.BoolField.InRecyclingMode);
			}
			set
			{
				this.SetBoolField(Panel.BoolField.InRecyclingMode, value);
			}
		}

		// Token: 0x170019FE RID: 6654
		// (get) Token: 0x06007061 RID: 28769 RVA: 0x002D7497 File Offset: 0x002D6497
		// (set) Token: 0x06007062 RID: 28770 RVA: 0x002D74A4 File Offset: 0x002D64A4
		internal bool VSP_MeasureCaches
		{
			get
			{
				return this.GetBoolField(Panel.BoolField.MeasureCaches);
			}
			set
			{
				this.SetBoolField(Panel.BoolField.MeasureCaches, value);
			}
		}

		// Token: 0x06007063 RID: 28771 RVA: 0x002D74B2 File Offset: 0x002D64B2
		private bool VerifyBoundState()
		{
			if (ItemsControl.GetItemsOwnerInternal(this) != null)
			{
				if (this._itemContainerGenerator == null)
				{
					this.ClearChildren();
				}
				return this._itemContainerGenerator != null;
			}
			if (this._itemContainerGenerator != null)
			{
				this.DisconnectFromGenerator();
				this.ClearChildren();
			}
			return false;
		}

		// Token: 0x170019FF RID: 6655
		// (get) Token: 0x06007064 RID: 28772 RVA: 0x002D74EC File Offset: 0x002D64EC
		internal bool IsDataBound
		{
			get
			{
				return this.IsItemsHost && this._itemContainerGenerator != null;
			}
		}

		// Token: 0x06007065 RID: 28773 RVA: 0x002D7501 File Offset: 0x002D6501
		internal static bool IsAboutToGenerateContent(Panel panel)
		{
			return panel.IsItemsHost && panel._itemContainerGenerator == null;
		}

		// Token: 0x06007066 RID: 28774 RVA: 0x002D7518 File Offset: 0x002D6518
		private void ConnectToGenerator()
		{
			ItemsControl itemsOwner = ItemsControl.GetItemsOwner(this);
			if (itemsOwner == null)
			{
				throw new InvalidOperationException(SR.Get("Panel_ItemsControlNotFound"));
			}
			IItemContainerGenerator itemContainerGenerator = itemsOwner.ItemContainerGenerator;
			if (itemContainerGenerator != null)
			{
				this._itemContainerGenerator = itemContainerGenerator.GetItemContainerGeneratorForPanel(this);
				if (this._itemContainerGenerator != null)
				{
					this._itemContainerGenerator.ItemsChanged += this.OnItemsChanged;
					((IItemContainerGenerator)this._itemContainerGenerator).RemoveAll();
				}
			}
		}

		// Token: 0x06007067 RID: 28775 RVA: 0x002D757E File Offset: 0x002D657E
		private void DisconnectFromGenerator()
		{
			this._itemContainerGenerator.ItemsChanged -= this.OnItemsChanged;
			((IItemContainerGenerator)this._itemContainerGenerator).RemoveAll();
			this._itemContainerGenerator = null;
		}

		// Token: 0x06007068 RID: 28776 RVA: 0x002D75A9 File Offset: 0x002D65A9
		private void EnsureEmptyChildren(FrameworkElement logicalParent)
		{
			if (this._uiElementCollection == null || this._uiElementCollection.LogicalParent != logicalParent)
			{
				this._uiElementCollection = this.CreateUIElementCollection(logicalParent);
				return;
			}
			this.ClearChildren();
		}

		// Token: 0x06007069 RID: 28777 RVA: 0x002D75D5 File Offset: 0x002D65D5
		internal void EnsureGenerator()
		{
			if (this._itemContainerGenerator == null)
			{
				this.ConnectToGenerator();
				this.EnsureEmptyChildren(null);
				this.GenerateChildren();
			}
		}

		// Token: 0x0600706A RID: 28778 RVA: 0x002D75F2 File Offset: 0x002D65F2
		private void ClearChildren()
		{
			if (this._itemContainerGenerator != null)
			{
				((IItemContainerGenerator)this._itemContainerGenerator).RemoveAll();
			}
			if (this._uiElementCollection != null && this._uiElementCollection.Count > 0)
			{
				this._uiElementCollection.ClearInternal();
				this.OnClearChildrenInternal();
			}
		}

		// Token: 0x0600706B RID: 28779 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		internal virtual void OnClearChildrenInternal()
		{
		}

		// Token: 0x0600706C RID: 28780 RVA: 0x002D7630 File Offset: 0x002D6630
		internal virtual void GenerateChildren()
		{
			IItemContainerGenerator itemContainerGenerator = this._itemContainerGenerator;
			if (itemContainerGenerator != null)
			{
				using (itemContainerGenerator.StartAt(new GeneratorPosition(-1, 0), GeneratorDirection.Forward))
				{
					UIElement uielement;
					while ((uielement = (itemContainerGenerator.GenerateNext() as UIElement)) != null)
					{
						this._uiElementCollection.AddInternal(uielement);
						itemContainerGenerator.PrepareItemContainer(uielement);
					}
				}
			}
		}

		// Token: 0x0600706D RID: 28781 RVA: 0x002D7698 File Offset: 0x002D6698
		private void OnItemsChanged(object sender, ItemsChangedEventArgs args)
		{
			if (this.VerifyBoundState() && this.OnItemsChangedInternal(sender, args))
			{
				base.InvalidateMeasure();
			}
		}

		// Token: 0x0600706E RID: 28782 RVA: 0x002D76B4 File Offset: 0x002D66B4
		internal virtual bool OnItemsChangedInternal(object sender, ItemsChangedEventArgs args)
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
				this.ResetChildren();
				break;
			}
			return true;
		}

		// Token: 0x0600706F RID: 28783 RVA: 0x002D7748 File Offset: 0x002D6748
		private void AddChildren(GeneratorPosition pos, int itemCount)
		{
			IItemContainerGenerator itemContainerGenerator = this._itemContainerGenerator;
			using (itemContainerGenerator.StartAt(pos, GeneratorDirection.Forward))
			{
				for (int i = 0; i < itemCount; i++)
				{
					UIElement uielement = itemContainerGenerator.GenerateNext() as UIElement;
					if (uielement != null)
					{
						this._uiElementCollection.InsertInternal(pos.Index + 1 + i, uielement);
						itemContainerGenerator.PrepareItemContainer(uielement);
					}
					else
					{
						this._itemContainerGenerator.Verify();
					}
				}
			}
		}

		// Token: 0x06007070 RID: 28784 RVA: 0x002D77C8 File Offset: 0x002D67C8
		private void RemoveChildren(GeneratorPosition pos, int containerCount)
		{
			this._uiElementCollection.RemoveRangeInternal(pos.Index, containerCount);
		}

		// Token: 0x06007071 RID: 28785 RVA: 0x002D77E0 File Offset: 0x002D67E0
		private void ReplaceChildren(GeneratorPosition pos, int itemCount, int containerCount)
		{
			IItemContainerGenerator itemContainerGenerator = this._itemContainerGenerator;
			using (itemContainerGenerator.StartAt(pos, GeneratorDirection.Forward, true))
			{
				for (int i = 0; i < itemCount; i++)
				{
					bool flag;
					UIElement uielement = itemContainerGenerator.GenerateNext(out flag) as UIElement;
					if (uielement != null && !flag)
					{
						this._uiElementCollection.SetInternal(pos.Index + i, uielement);
						itemContainerGenerator.PrepareItemContainer(uielement);
					}
					else
					{
						this._itemContainerGenerator.Verify();
					}
				}
			}
		}

		// Token: 0x06007072 RID: 28786 RVA: 0x002D7868 File Offset: 0x002D6868
		private void MoveChildren(GeneratorPosition fromPos, GeneratorPosition toPos, int containerCount)
		{
			if (fromPos == toPos)
			{
				return;
			}
			int num = ((IItemContainerGenerator)this._itemContainerGenerator).IndexFromGeneratorPosition(toPos);
			UIElement[] array = new UIElement[containerCount];
			for (int i = 0; i < containerCount; i++)
			{
				array[i] = this._uiElementCollection[fromPos.Index + i];
			}
			this._uiElementCollection.RemoveRangeInternal(fromPos.Index, containerCount);
			for (int j = 0; j < containerCount; j++)
			{
				this._uiElementCollection.InsertInternal(num + j, array[j]);
			}
		}

		// Token: 0x06007073 RID: 28787 RVA: 0x002D78E6 File Offset: 0x002D68E6
		private void ResetChildren()
		{
			this.EnsureEmptyChildren(null);
			this.GenerateChildren();
		}

		// Token: 0x06007074 RID: 28788 RVA: 0x002D78F5 File Offset: 0x002D68F5
		private bool GetBoolField(Panel.BoolField field)
		{
			return (this._boolFieldStore & field) > ~(Panel.BoolField.IsZStateDirty | Panel.BoolField.IsZStateDiverse | Panel.BoolField.IsVirtualizing | Panel.BoolField.HasMeasured | Panel.BoolField.IsPixelBased | Panel.BoolField.InRecyclingMode | Panel.BoolField.MustDisableVirtualization | Panel.BoolField.MeasureCaches);
		}

		// Token: 0x06007075 RID: 28789 RVA: 0x002D7902 File Offset: 0x002D6902
		private void SetBoolField(Panel.BoolField field, bool value)
		{
			if (value)
			{
				this._boolFieldStore |= field;
				return;
			}
			this._boolFieldStore &= ~field;
		}

		// Token: 0x17001A00 RID: 6656
		// (get) Token: 0x06007076 RID: 28790 RVA: 0x001FCA9D File Offset: 0x001FBA9D
		internal override int EffectiveValuesInitialSize
		{
			get
			{
				return 9;
			}
		}

		// Token: 0x06007077 RID: 28791 RVA: 0x002D7928 File Offset: 0x002D6928
		protected internal override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
		{
			if (!this.IsZStateDirty)
			{
				if (this.IsZStateDiverse)
				{
					this.IsZStateDirty = true;
				}
				else if (visualAdded != null && (int)visualAdded.GetValue(Panel.ZIndexProperty) != this._zConsonant)
				{
					this.IsZStateDirty = true;
				}
			}
			base.OnVisualChildrenChanged(visualAdded, visualRemoved);
			if (this.IsZStateDirty)
			{
				this.RecomputeZState();
				this.InvalidateZState();
			}
		}

		// Token: 0x06007078 RID: 28792 RVA: 0x002D798C File Offset: 0x002D698C
		public static void SetZIndex(UIElement element, int value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Panel.ZIndexProperty, value);
		}

		// Token: 0x06007079 RID: 28793 RVA: 0x002D79AD File Offset: 0x002D69AD
		public static int GetZIndex(UIElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (int)element.GetValue(Panel.ZIndexProperty);
		}

		// Token: 0x0600707A RID: 28794 RVA: 0x002D79D0 File Offset: 0x002D69D0
		private static void OnZIndexPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			int num = (int)e.OldValue;
			int num2 = (int)e.NewValue;
			if (num == num2)
			{
				return;
			}
			UIElement uielement = d as UIElement;
			if (uielement == null)
			{
				return;
			}
			Panel panel = uielement.InternalVisualParent as Panel;
			if (panel == null)
			{
				return;
			}
			panel.InvalidateZState();
		}

		// Token: 0x0600707B RID: 28795 RVA: 0x002D7A1B File Offset: 0x002D6A1B
		internal void InvalidateZState()
		{
			if (!this.IsZStateDirty && this._uiElementCollection != null)
			{
				base.InvalidateZOrder();
			}
			this.IsZStateDirty = true;
		}

		// Token: 0x17001A01 RID: 6657
		// (get) Token: 0x0600707C RID: 28796 RVA: 0x002D7A3A File Offset: 0x002D6A3A
		// (set) Token: 0x0600707D RID: 28797 RVA: 0x002D7A43 File Offset: 0x002D6A43
		private bool IsZStateDirty
		{
			get
			{
				return this.GetBoolField(Panel.BoolField.IsZStateDirty);
			}
			set
			{
				this.SetBoolField(Panel.BoolField.IsZStateDirty, value);
			}
		}

		// Token: 0x17001A02 RID: 6658
		// (get) Token: 0x0600707E RID: 28798 RVA: 0x002D7A4D File Offset: 0x002D6A4D
		// (set) Token: 0x0600707F RID: 28799 RVA: 0x002D7A56 File Offset: 0x002D6A56
		private bool IsZStateDiverse
		{
			get
			{
				return this.GetBoolField(Panel.BoolField.IsZStateDiverse);
			}
			set
			{
				this.SetBoolField(Panel.BoolField.IsZStateDiverse, value);
			}
		}

		// Token: 0x06007080 RID: 28800 RVA: 0x002D7A60 File Offset: 0x002D6A60
		private void RecomputeZState()
		{
			int num = (this._uiElementCollection != null) ? this._uiElementCollection.Count : 0;
			bool flag = false;
			bool flag2 = false;
			int num2 = (int)Panel.ZIndexProperty.GetDefaultValue(base.DependencyObjectType);
			int num3 = num2;
			List<long> list = null;
			if (num > 0)
			{
				if (this._uiElementCollection[0] != null)
				{
					num3 = (int)this._uiElementCollection[0].GetValue(Panel.ZIndexProperty);
				}
				if (num > 1)
				{
					list = new List<long>(num);
					list.Add((long)num3 << 32);
					int num4 = num3;
					int num5 = 1;
					do
					{
						int num6 = (this._uiElementCollection[num5] != null) ? ((int)this._uiElementCollection[num5].GetValue(Panel.ZIndexProperty)) : num2;
						list.Add(((long)num6 << 32) + (long)num5);
						flag2 |= (num6 < num4);
						num4 = num6;
						flag |= (num6 != num3);
					}
					while (++num5 < num);
				}
			}
			if (flag2)
			{
				list.Sort();
				if (this._zLut == null || this._zLut.Length != num)
				{
					this._zLut = new int[num];
				}
				for (int i = 0; i < num; i++)
				{
					this._zLut[i] = (int)(list[i] & (long)((ulong)-1));
				}
			}
			else
			{
				this._zLut = null;
			}
			this.IsZStateDiverse = flag;
			this._zConsonant = num3;
			this.IsZStateDirty = false;
		}

		// Token: 0x040036DF RID: 14047
		[CommonDependencyProperty]
		public static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register("Background", typeof(Brush), typeof(Panel), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender));

		// Token: 0x040036E0 RID: 14048
		public static readonly DependencyProperty IsItemsHostProperty = DependencyProperty.Register("IsItemsHost", typeof(bool), typeof(Panel), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.NotDataBindable, new PropertyChangedCallback(Panel.OnIsItemsHostChanged)));

		// Token: 0x040036E1 RID: 14049
		private UIElementCollection _uiElementCollection;

		// Token: 0x040036E2 RID: 14050
		private ItemContainerGenerator _itemContainerGenerator;

		// Token: 0x040036E3 RID: 14051
		private Panel.BoolField _boolFieldStore;

		// Token: 0x040036E4 RID: 14052
		private const int c_zDefaultValue = 0;

		// Token: 0x040036E5 RID: 14053
		private int _zConsonant;

		// Token: 0x040036E6 RID: 14054
		private int[] _zLut;

		// Token: 0x040036E7 RID: 14055
		public static readonly DependencyProperty ZIndexProperty = DependencyProperty.RegisterAttached("ZIndex", typeof(int), typeof(Panel), new FrameworkPropertyMetadata(0, new PropertyChangedCallback(Panel.OnZIndexPropertyChanged)));

		// Token: 0x02000C11 RID: 3089
		[Flags]
		private enum BoolField : byte
		{
			// Token: 0x04004AD9 RID: 19161
			IsZStateDirty = 1,
			// Token: 0x04004ADA RID: 19162
			IsZStateDiverse = 2,
			// Token: 0x04004ADB RID: 19163
			IsVirtualizing = 4,
			// Token: 0x04004ADC RID: 19164
			HasMeasured = 8,
			// Token: 0x04004ADD RID: 19165
			IsPixelBased = 16,
			// Token: 0x04004ADE RID: 19166
			InRecyclingMode = 32,
			// Token: 0x04004ADF RID: 19167
			MustDisableVirtualization = 64,
			// Token: 0x04004AE0 RID: 19168
			MeasureCaches = 128
		}
	}
}
