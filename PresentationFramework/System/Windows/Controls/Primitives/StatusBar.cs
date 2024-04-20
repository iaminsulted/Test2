using System;
using System.Windows.Automation.Peers;
using MS.Internal.KnownBoxes;
using MS.Internal.Telemetry.PresentationFramework;

namespace System.Windows.Controls.Primitives
{
	// Token: 0x02000856 RID: 2134
	[StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(StatusBarItem))]
	public class StatusBar : ItemsControl
	{
		// Token: 0x06007D81 RID: 32129 RVA: 0x003143CC File Offset: 0x003133CC
		static StatusBar()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(StatusBar), new FrameworkPropertyMetadata(typeof(StatusBar)));
			StatusBar._dType = DependencyObjectType.FromSystemTypeInternal(typeof(StatusBar));
			Control.IsTabStopProperty.OverrideMetadata(typeof(StatusBar), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));
			ItemsPanelTemplate itemsPanelTemplate = new ItemsPanelTemplate(new FrameworkElementFactory(typeof(DockPanel)));
			itemsPanelTemplate.Seal();
			ItemsControl.ItemsPanelProperty.OverrideMetadata(typeof(StatusBar), new FrameworkPropertyMetadata(itemsPanelTemplate));
			ControlsTraceLogger.AddControl((TelemetryControls)((ulong)int.MinValue));
		}

		// Token: 0x17001CF2 RID: 7410
		// (get) Token: 0x06007D82 RID: 32130 RVA: 0x003144AA File Offset: 0x003134AA
		// (set) Token: 0x06007D83 RID: 32131 RVA: 0x003144BC File Offset: 0x003134BC
		public ItemContainerTemplateSelector ItemContainerTemplateSelector
		{
			get
			{
				return (ItemContainerTemplateSelector)base.GetValue(StatusBar.ItemContainerTemplateSelectorProperty);
			}
			set
			{
				base.SetValue(StatusBar.ItemContainerTemplateSelectorProperty, value);
			}
		}

		// Token: 0x17001CF3 RID: 7411
		// (get) Token: 0x06007D84 RID: 32132 RVA: 0x003144CA File Offset: 0x003134CA
		// (set) Token: 0x06007D85 RID: 32133 RVA: 0x003144DC File Offset: 0x003134DC
		public bool UsesItemContainerTemplate
		{
			get
			{
				return (bool)base.GetValue(StatusBar.UsesItemContainerTemplateProperty);
			}
			set
			{
				base.SetValue(StatusBar.UsesItemContainerTemplateProperty, value);
			}
		}

		// Token: 0x06007D86 RID: 32134 RVA: 0x003144EA File Offset: 0x003134EA
		protected override bool IsItemItsOwnContainerOverride(object item)
		{
			bool flag = item is StatusBarItem || item is Separator;
			if (!flag)
			{
				this._currentItem = item;
			}
			return flag;
		}

		// Token: 0x06007D87 RID: 32135 RVA: 0x0031450C File Offset: 0x0031350C
		protected override DependencyObject GetContainerForItemOverride()
		{
			object currentItem = this._currentItem;
			this._currentItem = null;
			if (this.UsesItemContainerTemplate)
			{
				DataTemplate dataTemplate = this.ItemContainerTemplateSelector.SelectTemplate(currentItem, this);
				if (dataTemplate != null)
				{
					object obj = dataTemplate.LoadContent();
					if (obj is StatusBarItem || obj is Separator)
					{
						return obj as DependencyObject;
					}
					throw new InvalidOperationException(SR.Get("InvalidItemContainer", new object[]
					{
						base.GetType().Name,
						typeof(StatusBarItem).Name,
						typeof(Separator).Name,
						obj
					}));
				}
			}
			return new StatusBarItem();
		}

		// Token: 0x06007D88 RID: 32136 RVA: 0x003145B0 File Offset: 0x003135B0
		protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
		{
			base.PrepareContainerForItemOverride(element, item);
			Separator separator = element as Separator;
			if (separator != null)
			{
				bool flag;
				if (separator.GetValueSource(FrameworkElement.StyleProperty, null, out flag) <= BaseValueSourceInternal.ImplicitReference)
				{
					separator.SetResourceReference(FrameworkElement.StyleProperty, StatusBar.SeparatorStyleKey);
				}
				separator.DefaultStyleKey = StatusBar.SeparatorStyleKey;
			}
		}

		// Token: 0x06007D89 RID: 32137 RVA: 0x002D5AF0 File Offset: 0x002D4AF0
		protected override bool ShouldApplyItemContainerStyle(DependencyObject container, object item)
		{
			return !(item is Separator) && base.ShouldApplyItemContainerStyle(container, item);
		}

		// Token: 0x06007D8A RID: 32138 RVA: 0x003145FB File Offset: 0x003135FB
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new StatusBarAutomationPeer(this);
		}

		// Token: 0x17001CF4 RID: 7412
		// (get) Token: 0x06007D8B RID: 32139 RVA: 0x00314603 File Offset: 0x00313603
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return StatusBar._dType;
			}
		}

		// Token: 0x17001CF5 RID: 7413
		// (get) Token: 0x06007D8C RID: 32140 RVA: 0x0031460A File Offset: 0x0031360A
		public static ResourceKey SeparatorStyleKey
		{
			get
			{
				return SystemResourceKey.StatusBarSeparatorStyleKey;
			}
		}

		// Token: 0x04003AF1 RID: 15089
		public static readonly DependencyProperty ItemContainerTemplateSelectorProperty = MenuBase.ItemContainerTemplateSelectorProperty.AddOwner(typeof(StatusBar), new FrameworkPropertyMetadata(new DefaultItemContainerTemplateSelector()));

		// Token: 0x04003AF2 RID: 15090
		public static readonly DependencyProperty UsesItemContainerTemplateProperty = MenuBase.UsesItemContainerTemplateProperty.AddOwner(typeof(StatusBar));

		// Token: 0x04003AF3 RID: 15091
		private object _currentItem;

		// Token: 0x04003AF4 RID: 15092
		private static DependencyObjectType _dType;
	}
}
