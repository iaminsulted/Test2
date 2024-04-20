using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Threading;
using MS.Internal;
using MS.Internal.Controls;
using MS.Internal.KnownBoxes;
using MS.Internal.PresentationFramework;

namespace System.Windows.Controls
{
	// Token: 0x02000791 RID: 1937
	[Localizability(LocalizationCategory.Text)]
	public class HeaderedContentControl : ContentControl
	{
		// Token: 0x06006B8F RID: 27535 RVA: 0x002C62C8 File Offset: 0x002C52C8
		static HeaderedContentControl()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HeaderedContentControl), new FrameworkPropertyMetadata(typeof(HeaderedContentControl)));
			HeaderedContentControl._dType = DependencyObjectType.FromSystemTypeInternal(typeof(HeaderedContentControl));
		}

		// Token: 0x170018DD RID: 6365
		// (get) Token: 0x06006B91 RID: 27537 RVA: 0x002C641C File Offset: 0x002C541C
		// (set) Token: 0x06006B92 RID: 27538 RVA: 0x002C6429 File Offset: 0x002C5429
		[Category("Content")]
		[Bindable(true)]
		[Localizability(LocalizationCategory.Label)]
		public object Header
		{
			get
			{
				return base.GetValue(HeaderedContentControl.HeaderProperty);
			}
			set
			{
				base.SetValue(HeaderedContentControl.HeaderProperty, value);
			}
		}

		// Token: 0x06006B93 RID: 27539 RVA: 0x002C6437 File Offset: 0x002C5437
		private static void OnHeaderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			HeaderedContentControl headeredContentControl = (HeaderedContentControl)d;
			headeredContentControl.SetValue(HeaderedContentControl.HasHeaderPropertyKey, (e.NewValue != null) ? BooleanBoxes.TrueBox : BooleanBoxes.FalseBox);
			headeredContentControl.OnHeaderChanged(e.OldValue, e.NewValue);
		}

		// Token: 0x06006B94 RID: 27540 RVA: 0x002C6472 File Offset: 0x002C5472
		protected virtual void OnHeaderChanged(object oldHeader, object newHeader)
		{
			base.RemoveLogicalChild(oldHeader);
			base.AddLogicalChild(newHeader);
		}

		// Token: 0x170018DE RID: 6366
		// (get) Token: 0x06006B95 RID: 27541 RVA: 0x002C6482 File Offset: 0x002C5482
		[Bindable(false)]
		[Browsable(false)]
		public bool HasHeader
		{
			get
			{
				return (bool)base.GetValue(HeaderedContentControl.HasHeaderProperty);
			}
		}

		// Token: 0x170018DF RID: 6367
		// (get) Token: 0x06006B96 RID: 27542 RVA: 0x002C6494 File Offset: 0x002C5494
		// (set) Token: 0x06006B97 RID: 27543 RVA: 0x002C64A6 File Offset: 0x002C54A6
		[Category("Content")]
		[Bindable(true)]
		public DataTemplate HeaderTemplate
		{
			get
			{
				return (DataTemplate)base.GetValue(HeaderedContentControl.HeaderTemplateProperty);
			}
			set
			{
				base.SetValue(HeaderedContentControl.HeaderTemplateProperty, value);
			}
		}

		// Token: 0x06006B98 RID: 27544 RVA: 0x002C64B4 File Offset: 0x002C54B4
		private static void OnHeaderTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((HeaderedContentControl)d).OnHeaderTemplateChanged((DataTemplate)e.OldValue, (DataTemplate)e.NewValue);
		}

		// Token: 0x06006B99 RID: 27545 RVA: 0x002C64D9 File Offset: 0x002C54D9
		protected virtual void OnHeaderTemplateChanged(DataTemplate oldHeaderTemplate, DataTemplate newHeaderTemplate)
		{
			Helper.CheckTemplateAndTemplateSelector("Header", HeaderedContentControl.HeaderTemplateProperty, HeaderedContentControl.HeaderTemplateSelectorProperty, this);
		}

		// Token: 0x170018E0 RID: 6368
		// (get) Token: 0x06006B9A RID: 27546 RVA: 0x002C64F0 File Offset: 0x002C54F0
		// (set) Token: 0x06006B9B RID: 27547 RVA: 0x002C6502 File Offset: 0x002C5502
		[Category("Content")]
		[Bindable(true)]
		public DataTemplateSelector HeaderTemplateSelector
		{
			get
			{
				return (DataTemplateSelector)base.GetValue(HeaderedContentControl.HeaderTemplateSelectorProperty);
			}
			set
			{
				base.SetValue(HeaderedContentControl.HeaderTemplateSelectorProperty, value);
			}
		}

		// Token: 0x06006B9C RID: 27548 RVA: 0x002C6510 File Offset: 0x002C5510
		private static void OnHeaderTemplateSelectorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((HeaderedContentControl)d).OnHeaderTemplateSelectorChanged((DataTemplateSelector)e.OldValue, (DataTemplateSelector)e.NewValue);
		}

		// Token: 0x06006B9D RID: 27549 RVA: 0x002C64D9 File Offset: 0x002C54D9
		protected virtual void OnHeaderTemplateSelectorChanged(DataTemplateSelector oldHeaderTemplateSelector, DataTemplateSelector newHeaderTemplateSelector)
		{
			Helper.CheckTemplateAndTemplateSelector("Header", HeaderedContentControl.HeaderTemplateProperty, HeaderedContentControl.HeaderTemplateSelectorProperty, this);
		}

		// Token: 0x170018E1 RID: 6369
		// (get) Token: 0x06006B9E RID: 27550 RVA: 0x002C6535 File Offset: 0x002C5535
		// (set) Token: 0x06006B9F RID: 27551 RVA: 0x002C6547 File Offset: 0x002C5547
		[Bindable(true)]
		[CustomCategory("Content")]
		public string HeaderStringFormat
		{
			get
			{
				return (string)base.GetValue(HeaderedContentControl.HeaderStringFormatProperty);
			}
			set
			{
				base.SetValue(HeaderedContentControl.HeaderStringFormatProperty, value);
			}
		}

		// Token: 0x06006BA0 RID: 27552 RVA: 0x002C6555 File Offset: 0x002C5555
		private static void OnHeaderStringFormatChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((HeaderedContentControl)d).OnHeaderStringFormatChanged((string)e.OldValue, (string)e.NewValue);
		}

		// Token: 0x06006BA1 RID: 27553 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		protected virtual void OnHeaderStringFormatChanged(string oldHeaderStringFormat, string newHeaderStringFormat)
		{
		}

		// Token: 0x170018E2 RID: 6370
		// (get) Token: 0x06006BA2 RID: 27554 RVA: 0x002C657C File Offset: 0x002C557C
		protected internal override IEnumerator LogicalChildren
		{
			get
			{
				object header = this.Header;
				if (this.HeaderIsNotLogical || header == null)
				{
					return base.LogicalChildren;
				}
				return new HeaderedContentModelTreeEnumerator(this, base.ContentIsNotLogical ? null : base.Content, header);
			}
		}

		// Token: 0x06006BA3 RID: 27555 RVA: 0x002C65BA File Offset: 0x002C55BA
		internal override string GetPlainText()
		{
			return ContentControl.ContentObjectToString(this.Header);
		}

		// Token: 0x170018E3 RID: 6371
		// (get) Token: 0x06006BA4 RID: 27556 RVA: 0x002C65C7 File Offset: 0x002C55C7
		// (set) Token: 0x06006BA5 RID: 27557 RVA: 0x002C65D0 File Offset: 0x002C55D0
		internal bool HeaderIsNotLogical
		{
			get
			{
				return base.ReadControlFlag(Control.ControlBoolFlags.HeaderIsNotLogical);
			}
			set
			{
				base.WriteControlFlag(Control.ControlBoolFlags.HeaderIsNotLogical, value);
			}
		}

		// Token: 0x170018E4 RID: 6372
		// (get) Token: 0x06006BA6 RID: 27558 RVA: 0x002C65DA File Offset: 0x002C55DA
		// (set) Token: 0x06006BA7 RID: 27559 RVA: 0x002C65E4 File Offset: 0x002C55E4
		internal bool HeaderIsItem
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

		// Token: 0x06006BA8 RID: 27560 RVA: 0x002C65F0 File Offset: 0x002C55F0
		internal void PrepareHeaderedContentControl(object item, DataTemplate itemTemplate, DataTemplateSelector itemTemplateSelector, string stringFormat)
		{
			if (item != this)
			{
				base.ContentIsNotLogical = true;
				this.HeaderIsNotLogical = true;
				if (base.ContentIsItem || !base.HasNonDefaultValue(ContentControl.ContentProperty))
				{
					base.Content = item;
					base.ContentIsItem = true;
				}
				if (!(item is Visual) && (this.HeaderIsItem || !base.HasNonDefaultValue(HeaderedContentControl.HeaderProperty)))
				{
					this.Header = item;
					this.HeaderIsItem = true;
				}
				if (itemTemplate != null)
				{
					base.SetValue(HeaderedContentControl.HeaderTemplateProperty, itemTemplate);
				}
				if (itemTemplateSelector != null)
				{
					base.SetValue(HeaderedContentControl.HeaderTemplateSelectorProperty, itemTemplateSelector);
				}
				if (stringFormat != null)
				{
					base.SetValue(HeaderedContentControl.HeaderStringFormatProperty, stringFormat);
					return;
				}
			}
			else
			{
				base.ContentIsNotLogical = false;
			}
		}

		// Token: 0x06006BA9 RID: 27561 RVA: 0x002C6697 File Offset: 0x002C5697
		internal void ClearHeaderedContentControl(object item)
		{
			if (item != this)
			{
				if (base.ContentIsItem)
				{
					base.Content = BindingExpressionBase.DisconnectedItem;
				}
				if (this.HeaderIsItem)
				{
					this.Header = BindingExpressionBase.DisconnectedItem;
				}
			}
		}

		// Token: 0x06006BAA RID: 27562 RVA: 0x002C66C4 File Offset: 0x002C56C4
		public override string ToString()
		{
			string text = base.GetType().ToString();
			string headerText = string.Empty;
			string contentText = string.Empty;
			bool valuesDefined = false;
			if (base.CheckAccess())
			{
				headerText = ContentControl.ContentObjectToString(this.Header);
				contentText = ContentControl.ContentObjectToString(base.Content);
				valuesDefined = true;
			}
			else
			{
				base.Dispatcher.Invoke(DispatcherPriority.Send, new TimeSpan(0, 0, 0, 0, 20), new DispatcherOperationCallback(delegate(object o)
				{
					headerText = ContentControl.ContentObjectToString(this.Header);
					contentText = ContentControl.ContentObjectToString(this.Content);
					valuesDefined = true;
					return null;
				}), null);
			}
			if (valuesDefined)
			{
				return SR.Get("ToStringFormatString_HeaderedContentControl", new object[]
				{
					text,
					headerText,
					contentText
				});
			}
			return text;
		}

		// Token: 0x170018E5 RID: 6373
		// (get) Token: 0x06006BAB RID: 27563 RVA: 0x002C6790 File Offset: 0x002C5790
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return HeaderedContentControl._dType;
			}
		}

		// Token: 0x040035B8 RID: 13752
		[CommonDependencyProperty]
		public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(object), typeof(HeaderedContentControl), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(HeaderedContentControl.OnHeaderChanged)));

		// Token: 0x040035B9 RID: 13753
		internal static readonly DependencyPropertyKey HasHeaderPropertyKey = DependencyProperty.RegisterReadOnly("HasHeader", typeof(bool), typeof(HeaderedContentControl), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));

		// Token: 0x040035BA RID: 13754
		[CommonDependencyProperty]
		public static readonly DependencyProperty HasHeaderProperty = HeaderedContentControl.HasHeaderPropertyKey.DependencyProperty;

		// Token: 0x040035BB RID: 13755
		[CommonDependencyProperty]
		public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register("HeaderTemplate", typeof(DataTemplate), typeof(HeaderedContentControl), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(HeaderedContentControl.OnHeaderTemplateChanged)));

		// Token: 0x040035BC RID: 13756
		[CommonDependencyProperty]
		public static readonly DependencyProperty HeaderTemplateSelectorProperty = DependencyProperty.Register("HeaderTemplateSelector", typeof(DataTemplateSelector), typeof(HeaderedContentControl), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(HeaderedContentControl.OnHeaderTemplateSelectorChanged)));

		// Token: 0x040035BD RID: 13757
		[CommonDependencyProperty]
		public static readonly DependencyProperty HeaderStringFormatProperty = DependencyProperty.Register("HeaderStringFormat", typeof(string), typeof(HeaderedContentControl), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(HeaderedContentControl.OnHeaderStringFormatChanged)));

		// Token: 0x040035BE RID: 13758
		private static DependencyObjectType _dType;
	}
}
