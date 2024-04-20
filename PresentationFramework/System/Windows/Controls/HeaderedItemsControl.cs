using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Threading;
using MS.Internal;
using MS.Internal.Controls;
using MS.Internal.KnownBoxes;
using MS.Internal.PresentationFramework;

namespace System.Windows.Controls
{
	// Token: 0x02000792 RID: 1938
	[DefaultProperty("Header")]
	[Localizability(LocalizationCategory.Menu)]
	public class HeaderedItemsControl : ItemsControl
	{
		// Token: 0x170018E6 RID: 6374
		// (get) Token: 0x06006BAD RID: 27565 RVA: 0x002C679F File Offset: 0x002C579F
		// (set) Token: 0x06006BAE RID: 27566 RVA: 0x002C67AC File Offset: 0x002C57AC
		[CustomCategory("Content")]
		[Bindable(true)]
		public object Header
		{
			get
			{
				return base.GetValue(HeaderedItemsControl.HeaderProperty);
			}
			set
			{
				base.SetValue(HeaderedItemsControl.HeaderProperty, value);
			}
		}

		// Token: 0x06006BAF RID: 27567 RVA: 0x002C67BA File Offset: 0x002C57BA
		private static void OnHeaderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			HeaderedItemsControl headeredItemsControl = (HeaderedItemsControl)d;
			headeredItemsControl.SetValue(HeaderedItemsControl.HasHeaderPropertyKey, (e.NewValue != null) ? BooleanBoxes.TrueBox : BooleanBoxes.FalseBox);
			headeredItemsControl.OnHeaderChanged(e.OldValue, e.NewValue);
		}

		// Token: 0x06006BB0 RID: 27568 RVA: 0x002C67F5 File Offset: 0x002C57F5
		protected virtual void OnHeaderChanged(object oldHeader, object newHeader)
		{
			if (!this.IsHeaderLogical())
			{
				return;
			}
			base.RemoveLogicalChild(oldHeader);
			base.AddLogicalChild(newHeader);
		}

		// Token: 0x170018E7 RID: 6375
		// (get) Token: 0x06006BB1 RID: 27569 RVA: 0x002C680E File Offset: 0x002C580E
		[Bindable(false)]
		[Browsable(false)]
		public bool HasHeader
		{
			get
			{
				return (bool)base.GetValue(HeaderedItemsControl.HasHeaderProperty);
			}
		}

		// Token: 0x170018E8 RID: 6376
		// (get) Token: 0x06006BB2 RID: 27570 RVA: 0x002C6820 File Offset: 0x002C5820
		// (set) Token: 0x06006BB3 RID: 27571 RVA: 0x002C6832 File Offset: 0x002C5832
		[Bindable(true)]
		[CustomCategory("Content")]
		public DataTemplate HeaderTemplate
		{
			get
			{
				return (DataTemplate)base.GetValue(HeaderedItemsControl.HeaderTemplateProperty);
			}
			set
			{
				base.SetValue(HeaderedItemsControl.HeaderTemplateProperty, value);
			}
		}

		// Token: 0x06006BB4 RID: 27572 RVA: 0x002C6840 File Offset: 0x002C5840
		private static void OnHeaderTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((HeaderedItemsControl)d).OnHeaderTemplateChanged((DataTemplate)e.OldValue, (DataTemplate)e.NewValue);
		}

		// Token: 0x06006BB5 RID: 27573 RVA: 0x002C6865 File Offset: 0x002C5865
		protected virtual void OnHeaderTemplateChanged(DataTemplate oldHeaderTemplate, DataTemplate newHeaderTemplate)
		{
			Helper.CheckTemplateAndTemplateSelector("Header", HeaderedItemsControl.HeaderTemplateProperty, HeaderedItemsControl.HeaderTemplateSelectorProperty, this);
		}

		// Token: 0x170018E9 RID: 6377
		// (get) Token: 0x06006BB6 RID: 27574 RVA: 0x002C687C File Offset: 0x002C587C
		// (set) Token: 0x06006BB7 RID: 27575 RVA: 0x002C688E File Offset: 0x002C588E
		[Bindable(true)]
		[CustomCategory("Content")]
		public DataTemplateSelector HeaderTemplateSelector
		{
			get
			{
				return (DataTemplateSelector)base.GetValue(HeaderedItemsControl.HeaderTemplateSelectorProperty);
			}
			set
			{
				base.SetValue(HeaderedItemsControl.HeaderTemplateSelectorProperty, value);
			}
		}

		// Token: 0x06006BB8 RID: 27576 RVA: 0x002C689C File Offset: 0x002C589C
		private static void OnHeaderTemplateSelectorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((HeaderedItemsControl)d).OnHeaderTemplateSelectorChanged((DataTemplateSelector)e.OldValue, (DataTemplateSelector)e.NewValue);
		}

		// Token: 0x06006BB9 RID: 27577 RVA: 0x002C6865 File Offset: 0x002C5865
		protected virtual void OnHeaderTemplateSelectorChanged(DataTemplateSelector oldHeaderTemplateSelector, DataTemplateSelector newHeaderTemplateSelector)
		{
			Helper.CheckTemplateAndTemplateSelector("Header", HeaderedItemsControl.HeaderTemplateProperty, HeaderedItemsControl.HeaderTemplateSelectorProperty, this);
		}

		// Token: 0x170018EA RID: 6378
		// (get) Token: 0x06006BBA RID: 27578 RVA: 0x002C68C1 File Offset: 0x002C58C1
		// (set) Token: 0x06006BBB RID: 27579 RVA: 0x002C68D3 File Offset: 0x002C58D3
		[CustomCategory("Content")]
		[Bindable(true)]
		public string HeaderStringFormat
		{
			get
			{
				return (string)base.GetValue(HeaderedItemsControl.HeaderStringFormatProperty);
			}
			set
			{
				base.SetValue(HeaderedItemsControl.HeaderStringFormatProperty, value);
			}
		}

		// Token: 0x06006BBC RID: 27580 RVA: 0x002C68E1 File Offset: 0x002C58E1
		private static void OnHeaderStringFormatChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((HeaderedItemsControl)d).OnHeaderStringFormatChanged((string)e.OldValue, (string)e.NewValue);
		}

		// Token: 0x06006BBD RID: 27581 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		protected virtual void OnHeaderStringFormatChanged(string oldHeaderStringFormat, string newHeaderStringFormat)
		{
		}

		// Token: 0x06006BBE RID: 27582 RVA: 0x002C6908 File Offset: 0x002C5908
		internal void PrepareHeaderedItemsControl(object item, ItemsControl parentItemsControl)
		{
			bool flag = item != this;
			base.WriteControlFlag(Control.ControlBoolFlags.HeaderIsNotLogical, flag);
			base.PrepareItemsControl(item, parentItemsControl);
			if (flag)
			{
				if (this.HeaderIsItem || !base.HasNonDefaultValue(HeaderedItemsControl.HeaderProperty))
				{
					this.Header = item;
					this.HeaderIsItem = true;
				}
				DataTemplate itemTemplate = parentItemsControl.ItemTemplate;
				DataTemplateSelector itemTemplateSelector = parentItemsControl.ItemTemplateSelector;
				string itemStringFormat = parentItemsControl.ItemStringFormat;
				if (itemTemplate != null)
				{
					base.SetValue(HeaderedItemsControl.HeaderTemplateProperty, itemTemplate);
				}
				if (itemTemplateSelector != null)
				{
					base.SetValue(HeaderedItemsControl.HeaderTemplateSelectorProperty, itemTemplateSelector);
				}
				if (itemStringFormat != null && Helper.HasDefaultValue(this, HeaderedItemsControl.HeaderStringFormatProperty))
				{
					base.SetValue(HeaderedItemsControl.HeaderStringFormatProperty, itemStringFormat);
				}
				this.PrepareHierarchy(item, parentItemsControl);
			}
		}

		// Token: 0x06006BBF RID: 27583 RVA: 0x002C69AA File Offset: 0x002C59AA
		internal void ClearHeaderedItemsControl(object item)
		{
			base.ClearItemsControl(item);
			if (item != this && this.HeaderIsItem)
			{
				this.Header = BindingExpressionBase.DisconnectedItem;
			}
		}

		// Token: 0x06006BC0 RID: 27584 RVA: 0x002C69CA File Offset: 0x002C59CA
		internal override string GetPlainText()
		{
			return ContentControl.ContentObjectToString(this.Header);
		}

		// Token: 0x06006BC1 RID: 27585 RVA: 0x002C69D8 File Offset: 0x002C59D8
		public override string ToString()
		{
			string text = base.GetType().ToString();
			string headerText = string.Empty;
			int itemCount = 0;
			bool valuesDefined = false;
			if (base.CheckAccess())
			{
				headerText = ContentControl.ContentObjectToString(this.Header);
				itemCount = (base.HasItems ? base.Items.Count : 0);
				valuesDefined = true;
			}
			else
			{
				base.Dispatcher.Invoke(DispatcherPriority.Send, new TimeSpan(0, 0, 0, 0, 20), new DispatcherOperationCallback(delegate(object o)
				{
					headerText = ContentControl.ContentObjectToString(this.Header);
					itemCount = (this.HasItems ? this.Items.Count : 0);
					valuesDefined = true;
					return null;
				}), null);
			}
			if (valuesDefined)
			{
				return SR.Get("ToStringFormatString_HeaderedItemsControl", new object[]
				{
					text,
					headerText,
					itemCount
				});
			}
			return text;
		}

		// Token: 0x170018EB RID: 6379
		// (get) Token: 0x06006BC2 RID: 27586 RVA: 0x002C6AB0 File Offset: 0x002C5AB0
		protected internal override IEnumerator LogicalChildren
		{
			get
			{
				object header = this.Header;
				if (base.ReadControlFlag(Control.ControlBoolFlags.HeaderIsNotLogical) || header == null)
				{
					return base.LogicalChildren;
				}
				return new HeaderedItemsModelTreeEnumerator(this, base.LogicalChildren, header);
			}
		}

		// Token: 0x06006BC3 RID: 27587 RVA: 0x002C6AE4 File Offset: 0x002C5AE4
		private void PrepareHierarchy(object item, ItemsControl parentItemsControl)
		{
			DataTemplate dataTemplate = this.HeaderTemplate;
			if (dataTemplate == null)
			{
				DataTemplateSelector headerTemplateSelector = this.HeaderTemplateSelector;
				if (headerTemplateSelector != null)
				{
					dataTemplate = headerTemplateSelector.SelectTemplate(item, this);
				}
				if (dataTemplate == null)
				{
					dataTemplate = (DataTemplate)FrameworkElement.FindTemplateResourceInternal(this, item, typeof(DataTemplate));
				}
			}
			HierarchicalDataTemplate hierarchicalDataTemplate = dataTemplate as HierarchicalDataTemplate;
			if (hierarchicalDataTemplate != null)
			{
				bool flag = base.ItemTemplate == parentItemsControl.ItemTemplate;
				bool flag2 = base.ItemContainerStyle == parentItemsControl.ItemContainerStyle;
				if (hierarchicalDataTemplate.ItemsSource != null && !base.HasNonDefaultValue(ItemsControl.ItemsSourceProperty))
				{
					base.SetBinding(ItemsControl.ItemsSourceProperty, hierarchicalDataTemplate.ItemsSource);
				}
				if (hierarchicalDataTemplate.IsItemStringFormatSet && base.ItemStringFormat == parentItemsControl.ItemStringFormat)
				{
					base.ClearValue(ItemsControl.ItemTemplateProperty);
					base.ClearValue(ItemsControl.ItemTemplateSelectorProperty);
					base.ClearValue(ItemsControl.ItemStringFormatProperty);
					if (hierarchicalDataTemplate.ItemStringFormat != null)
					{
						base.ItemStringFormat = hierarchicalDataTemplate.ItemStringFormat;
					}
				}
				if (hierarchicalDataTemplate.IsItemTemplateSelectorSet && base.ItemTemplateSelector == parentItemsControl.ItemTemplateSelector)
				{
					base.ClearValue(ItemsControl.ItemTemplateProperty);
					base.ClearValue(ItemsControl.ItemTemplateSelectorProperty);
					if (hierarchicalDataTemplate.ItemTemplateSelector != null)
					{
						base.ItemTemplateSelector = hierarchicalDataTemplate.ItemTemplateSelector;
					}
				}
				if (hierarchicalDataTemplate.IsItemTemplateSet && flag)
				{
					base.ClearValue(ItemsControl.ItemTemplateProperty);
					if (hierarchicalDataTemplate.ItemTemplate != null)
					{
						base.ItemTemplate = hierarchicalDataTemplate.ItemTemplate;
					}
				}
				if (hierarchicalDataTemplate.IsItemContainerStyleSelectorSet && base.ItemContainerStyleSelector == parentItemsControl.ItemContainerStyleSelector)
				{
					base.ClearValue(ItemsControl.ItemContainerStyleProperty);
					base.ClearValue(ItemsControl.ItemContainerStyleSelectorProperty);
					if (hierarchicalDataTemplate.ItemContainerStyleSelector != null)
					{
						base.ItemContainerStyleSelector = hierarchicalDataTemplate.ItemContainerStyleSelector;
					}
				}
				if (hierarchicalDataTemplate.IsItemContainerStyleSet && flag2)
				{
					base.ClearValue(ItemsControl.ItemContainerStyleProperty);
					if (hierarchicalDataTemplate.ItemContainerStyle != null)
					{
						base.ItemContainerStyle = hierarchicalDataTemplate.ItemContainerStyle;
					}
				}
				if (hierarchicalDataTemplate.IsAlternationCountSet && base.AlternationCount == parentItemsControl.AlternationCount)
				{
					base.ClearValue(ItemsControl.AlternationCountProperty);
					if (true)
					{
						base.AlternationCount = hierarchicalDataTemplate.AlternationCount;
					}
				}
				if (hierarchicalDataTemplate.IsItemBindingGroupSet && base.ItemBindingGroup == parentItemsControl.ItemBindingGroup)
				{
					base.ClearValue(ItemsControl.ItemBindingGroupProperty);
					if (hierarchicalDataTemplate.ItemBindingGroup != null)
					{
						base.ItemBindingGroup = hierarchicalDataTemplate.ItemBindingGroup;
					}
				}
			}
		}

		// Token: 0x06006BC4 RID: 27588 RVA: 0x002C6D18 File Offset: 0x002C5D18
		private bool IsBound(DependencyProperty dp, Binding binding)
		{
			BindingExpressionBase bindingExpression = BindingOperations.GetBindingExpression(this, dp);
			return bindingExpression != null && bindingExpression.ParentBindingBase == binding;
		}

		// Token: 0x06006BC5 RID: 27589 RVA: 0x002C6D3B File Offset: 0x002C5D3B
		private bool IsHeaderLogical()
		{
			if (base.ReadControlFlag(Control.ControlBoolFlags.HeaderIsNotLogical))
			{
				return false;
			}
			if (BindingOperations.IsDataBound(this, HeaderedItemsControl.HeaderProperty))
			{
				base.WriteControlFlag(Control.ControlBoolFlags.HeaderIsNotLogical, true);
				return false;
			}
			return true;
		}

		// Token: 0x170018EC RID: 6380
		// (get) Token: 0x06006BC6 RID: 27590 RVA: 0x002C65DA File Offset: 0x002C55DA
		// (set) Token: 0x06006BC7 RID: 27591 RVA: 0x002C65E4 File Offset: 0x002C55E4
		private bool HeaderIsItem
		{
			get
			{
				return base.ReadControlFlag(Control.ControlBoolFlags.HeaderIsItem);
			}
			set
			{
				base.WriteControlFlag(Control.ControlBoolFlags.HeaderIsItem, value);
			}
		}

		// Token: 0x040035BF RID: 13759
		[CommonDependencyProperty]
		public static readonly DependencyProperty HeaderProperty = HeaderedContentControl.HeaderProperty.AddOwner(typeof(HeaderedItemsControl), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(HeaderedItemsControl.OnHeaderChanged)));

		// Token: 0x040035C0 RID: 13760
		private static readonly DependencyPropertyKey HasHeaderPropertyKey = HeaderedContentControl.HasHeaderPropertyKey;

		// Token: 0x040035C1 RID: 13761
		[CommonDependencyProperty]
		public static readonly DependencyProperty HasHeaderProperty = HeaderedContentControl.HasHeaderProperty.AddOwner(typeof(HeaderedItemsControl));

		// Token: 0x040035C2 RID: 13762
		[CommonDependencyProperty]
		public static readonly DependencyProperty HeaderTemplateProperty = HeaderedContentControl.HeaderTemplateProperty.AddOwner(typeof(HeaderedItemsControl), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(HeaderedItemsControl.OnHeaderTemplateChanged)));

		// Token: 0x040035C3 RID: 13763
		[CommonDependencyProperty]
		public static readonly DependencyProperty HeaderTemplateSelectorProperty = HeaderedContentControl.HeaderTemplateSelectorProperty.AddOwner(typeof(HeaderedItemsControl), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(HeaderedItemsControl.OnHeaderTemplateSelectorChanged)));

		// Token: 0x040035C4 RID: 13764
		[CommonDependencyProperty]
		public static readonly DependencyProperty HeaderStringFormatProperty = DependencyProperty.Register("HeaderStringFormat", typeof(string), typeof(HeaderedItemsControl), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(HeaderedItemsControl.OnHeaderStringFormatChanged)));
	}
}
