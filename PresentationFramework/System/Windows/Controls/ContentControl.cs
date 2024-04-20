using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Markup;
using MS.Internal;
using MS.Internal.Controls;
using MS.Internal.KnownBoxes;
using MS.Internal.PresentationFramework;
using MS.Internal.Telemetry.PresentationFramework;

namespace System.Windows.Controls
{
	// Token: 0x02000734 RID: 1844
	[DefaultProperty("Content")]
	[ContentProperty("Content")]
	[Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
	public class ContentControl : Control, IAddChild
	{
		// Token: 0x06006133 RID: 24883 RVA: 0x0029C520 File Offset: 0x0029B520
		static ContentControl()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(ContentControl), new FrameworkPropertyMetadata(typeof(ContentControl)));
			ContentControl._dType = DependencyObjectType.FromSystemTypeInternal(typeof(ContentControl));
			ControlsTraceLogger.AddControl(TelemetryControls.ContentControl);
		}

		// Token: 0x17001675 RID: 5749
		// (get) Token: 0x06006134 RID: 24884 RVA: 0x0029C680 File Offset: 0x0029B680
		protected internal override IEnumerator LogicalChildren
		{
			get
			{
				object content = this.Content;
				if (this.ContentIsNotLogical || content == null)
				{
					return EmptyEnumerator.Instance;
				}
				if (base.TemplatedParent != null)
				{
					DependencyObject dependencyObject = content as DependencyObject;
					if (dependencyObject != null)
					{
						DependencyObject parent = LogicalTreeHelper.GetParent(dependencyObject);
						if (parent != null && parent != this)
						{
							return EmptyEnumerator.Instance;
						}
					}
				}
				return new ContentModelTreeEnumerator(this, content);
			}
		}

		// Token: 0x06006135 RID: 24885 RVA: 0x0029C6D2 File Offset: 0x0029B6D2
		internal override string GetPlainText()
		{
			return ContentControl.ContentObjectToString(this.Content);
		}

		// Token: 0x06006136 RID: 24886 RVA: 0x0029C6E0 File Offset: 0x0029B6E0
		internal static string ContentObjectToString(object content)
		{
			if (content == null)
			{
				return string.Empty;
			}
			FrameworkElement frameworkElement = content as FrameworkElement;
			if (frameworkElement != null)
			{
				return frameworkElement.GetPlainText();
			}
			return content.ToString();
		}

		// Token: 0x06006137 RID: 24887 RVA: 0x0029C710 File Offset: 0x0029B710
		internal void PrepareContentControl(object item, DataTemplate itemTemplate, DataTemplateSelector itemTemplateSelector, string itemStringFormat)
		{
			if (item != this)
			{
				this.ContentIsNotLogical = true;
				if (this.ContentIsItem || !base.HasNonDefaultValue(ContentControl.ContentProperty))
				{
					this.Content = item;
					this.ContentIsItem = true;
				}
				if (itemTemplate != null)
				{
					base.SetValue(ContentControl.ContentTemplateProperty, itemTemplate);
				}
				if (itemTemplateSelector != null)
				{
					base.SetValue(ContentControl.ContentTemplateSelectorProperty, itemTemplateSelector);
				}
				if (itemStringFormat != null)
				{
					base.SetValue(ContentControl.ContentStringFormatProperty, itemStringFormat);
					return;
				}
			}
			else
			{
				this.ContentIsNotLogical = false;
			}
		}

		// Token: 0x06006138 RID: 24888 RVA: 0x0029C782 File Offset: 0x0029B782
		internal void ClearContentControl(object item)
		{
			if (item != this && this.ContentIsItem)
			{
				this.Content = BindingExpressionBase.DisconnectedItem;
			}
		}

		// Token: 0x06006139 RID: 24889 RVA: 0x0029C79B File Offset: 0x0029B79B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual bool ShouldSerializeContent()
		{
			return base.ReadLocalValue(ContentControl.ContentProperty) != DependencyProperty.UnsetValue;
		}

		// Token: 0x0600613A RID: 24890 RVA: 0x0029C7B2 File Offset: 0x0029B7B2
		void IAddChild.AddChild(object value)
		{
			this.AddChild(value);
		}

		// Token: 0x0600613B RID: 24891 RVA: 0x0029C7BB File Offset: 0x0029B7BB
		protected virtual void AddChild(object value)
		{
			if (this.Content == null || value == null)
			{
				this.Content = value;
				return;
			}
			throw new InvalidOperationException(SR.Get("ContentControlCannotHaveMultipleContent"));
		}

		// Token: 0x0600613C RID: 24892 RVA: 0x0029C7DF File Offset: 0x0029B7DF
		void IAddChild.AddText(string text)
		{
			this.AddText(text);
		}

		// Token: 0x0600613D RID: 24893 RVA: 0x0029C7B2 File Offset: 0x0029B7B2
		protected virtual void AddText(string text)
		{
			this.AddChild(text);
		}

		// Token: 0x17001676 RID: 5750
		// (get) Token: 0x0600613E RID: 24894 RVA: 0x0029C7E8 File Offset: 0x0029B7E8
		// (set) Token: 0x0600613F RID: 24895 RVA: 0x0029C7F5 File Offset: 0x0029B7F5
		[Bindable(true)]
		[CustomCategory("Content")]
		public object Content
		{
			get
			{
				return base.GetValue(ContentControl.ContentProperty);
			}
			set
			{
				base.SetValue(ContentControl.ContentProperty, value);
			}
		}

		// Token: 0x06006140 RID: 24896 RVA: 0x0029C803 File Offset: 0x0029B803
		private static void OnContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ContentControl contentControl = (ContentControl)d;
			contentControl.SetValue(ContentControl.HasContentPropertyKey, (e.NewValue != null) ? BooleanBoxes.TrueBox : BooleanBoxes.FalseBox);
			contentControl.OnContentChanged(e.OldValue, e.NewValue);
		}

		// Token: 0x06006141 RID: 24897 RVA: 0x0029C840 File Offset: 0x0029B840
		protected virtual void OnContentChanged(object oldContent, object newContent)
		{
			base.RemoveLogicalChild(oldContent);
			if (this.ContentIsNotLogical)
			{
				return;
			}
			DependencyObject dependencyObject = newContent as DependencyObject;
			if (dependencyObject != null)
			{
				DependencyObject parent = LogicalTreeHelper.GetParent(dependencyObject);
				if (parent != null)
				{
					if (base.TemplatedParent != null && FrameworkObject.IsEffectiveAncestor(parent, this))
					{
						return;
					}
					LogicalTreeHelper.RemoveLogicalChild(parent, newContent);
				}
			}
			base.AddLogicalChild(newContent);
		}

		// Token: 0x17001677 RID: 5751
		// (get) Token: 0x06006142 RID: 24898 RVA: 0x0029C891 File Offset: 0x0029B891
		[ReadOnly(true)]
		[Browsable(false)]
		public bool HasContent
		{
			get
			{
				return (bool)base.GetValue(ContentControl.HasContentProperty);
			}
		}

		// Token: 0x17001678 RID: 5752
		// (get) Token: 0x06006143 RID: 24899 RVA: 0x0029C8A3 File Offset: 0x0029B8A3
		// (set) Token: 0x06006144 RID: 24900 RVA: 0x0029C8B5 File Offset: 0x0029B8B5
		[CustomCategory("Content")]
		[Bindable(true)]
		public DataTemplate ContentTemplate
		{
			get
			{
				return (DataTemplate)base.GetValue(ContentControl.ContentTemplateProperty);
			}
			set
			{
				base.SetValue(ContentControl.ContentTemplateProperty, value);
			}
		}

		// Token: 0x06006145 RID: 24901 RVA: 0x0029C8C3 File Offset: 0x0029B8C3
		private static void OnContentTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((ContentControl)d).OnContentTemplateChanged((DataTemplate)e.OldValue, (DataTemplate)e.NewValue);
		}

		// Token: 0x06006146 RID: 24902 RVA: 0x0029C8E8 File Offset: 0x0029B8E8
		protected virtual void OnContentTemplateChanged(DataTemplate oldContentTemplate, DataTemplate newContentTemplate)
		{
			Helper.CheckTemplateAndTemplateSelector("Content", ContentControl.ContentTemplateProperty, ContentControl.ContentTemplateSelectorProperty, this);
		}

		// Token: 0x17001679 RID: 5753
		// (get) Token: 0x06006147 RID: 24903 RVA: 0x0029C8FF File Offset: 0x0029B8FF
		// (set) Token: 0x06006148 RID: 24904 RVA: 0x0029C911 File Offset: 0x0029B911
		[Bindable(true)]
		[CustomCategory("Content")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public DataTemplateSelector ContentTemplateSelector
		{
			get
			{
				return (DataTemplateSelector)base.GetValue(ContentControl.ContentTemplateSelectorProperty);
			}
			set
			{
				base.SetValue(ContentControl.ContentTemplateSelectorProperty, value);
			}
		}

		// Token: 0x06006149 RID: 24905 RVA: 0x0029C91F File Offset: 0x0029B91F
		private static void OnContentTemplateSelectorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((ContentControl)d).OnContentTemplateSelectorChanged((DataTemplateSelector)e.NewValue, (DataTemplateSelector)e.NewValue);
		}

		// Token: 0x0600614A RID: 24906 RVA: 0x0029C8E8 File Offset: 0x0029B8E8
		protected virtual void OnContentTemplateSelectorChanged(DataTemplateSelector oldContentTemplateSelector, DataTemplateSelector newContentTemplateSelector)
		{
			Helper.CheckTemplateAndTemplateSelector("Content", ContentControl.ContentTemplateProperty, ContentControl.ContentTemplateSelectorProperty, this);
		}

		// Token: 0x1700167A RID: 5754
		// (get) Token: 0x0600614B RID: 24907 RVA: 0x0029C944 File Offset: 0x0029B944
		// (set) Token: 0x0600614C RID: 24908 RVA: 0x0029C956 File Offset: 0x0029B956
		[CustomCategory("Content")]
		[Bindable(true)]
		public string ContentStringFormat
		{
			get
			{
				return (string)base.GetValue(ContentControl.ContentStringFormatProperty);
			}
			set
			{
				base.SetValue(ContentControl.ContentStringFormatProperty, value);
			}
		}

		// Token: 0x0600614D RID: 24909 RVA: 0x0029C964 File Offset: 0x0029B964
		private static void OnContentStringFormatChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((ContentControl)d).OnContentStringFormatChanged((string)e.OldValue, (string)e.NewValue);
		}

		// Token: 0x0600614E RID: 24910 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		protected virtual void OnContentStringFormatChanged(string oldContentStringFormat, string newContentStringFormat)
		{
		}

		// Token: 0x1700167B RID: 5755
		// (get) Token: 0x0600614F RID: 24911 RVA: 0x0029C989 File Offset: 0x0029B989
		// (set) Token: 0x06006150 RID: 24912 RVA: 0x0029C992 File Offset: 0x0029B992
		internal bool ContentIsNotLogical
		{
			get
			{
				return base.ReadControlFlag(Control.ControlBoolFlags.ContentIsNotLogical);
			}
			set
			{
				base.WriteControlFlag(Control.ControlBoolFlags.ContentIsNotLogical, value);
			}
		}

		// Token: 0x1700167C RID: 5756
		// (get) Token: 0x06006151 RID: 24913 RVA: 0x0029C99C File Offset: 0x0029B99C
		// (set) Token: 0x06006152 RID: 24914 RVA: 0x0029C9A6 File Offset: 0x0029B9A6
		internal bool ContentIsItem
		{
			get
			{
				return base.ReadControlFlag(Control.ControlBoolFlags.ContentIsItem);
			}
			set
			{
				base.WriteControlFlag(Control.ControlBoolFlags.ContentIsItem, value);
			}
		}

		// Token: 0x1700167D RID: 5757
		// (get) Token: 0x06006153 RID: 24915 RVA: 0x001FC019 File Offset: 0x001FB019
		internal override int EffectiveValuesInitialSize
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x1700167E RID: 5758
		// (get) Token: 0x06006154 RID: 24916 RVA: 0x0029C9B1 File Offset: 0x0029B9B1
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return ContentControl._dType;
			}
		}

		// Token: 0x04003264 RID: 12900
		[CommonDependencyProperty]
		public static readonly DependencyProperty ContentProperty = DependencyProperty.Register("Content", typeof(object), typeof(ContentControl), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(ContentControl.OnContentChanged)));

		// Token: 0x04003265 RID: 12901
		private static readonly DependencyPropertyKey HasContentPropertyKey = DependencyProperty.RegisterReadOnly("HasContent", typeof(bool), typeof(ContentControl), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.None));

		// Token: 0x04003266 RID: 12902
		[CommonDependencyProperty]
		public static readonly DependencyProperty HasContentProperty = ContentControl.HasContentPropertyKey.DependencyProperty;

		// Token: 0x04003267 RID: 12903
		[CommonDependencyProperty]
		public static readonly DependencyProperty ContentTemplateProperty = DependencyProperty.Register("ContentTemplate", typeof(DataTemplate), typeof(ContentControl), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(ContentControl.OnContentTemplateChanged)));

		// Token: 0x04003268 RID: 12904
		[CommonDependencyProperty]
		public static readonly DependencyProperty ContentTemplateSelectorProperty = DependencyProperty.Register("ContentTemplateSelector", typeof(DataTemplateSelector), typeof(ContentControl), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(ContentControl.OnContentTemplateSelectorChanged)));

		// Token: 0x04003269 RID: 12905
		[CommonDependencyProperty]
		public static readonly DependencyProperty ContentStringFormatProperty = DependencyProperty.Register("ContentStringFormat", typeof(string), typeof(ContentControl), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(ContentControl.OnContentStringFormatChanged)));

		// Token: 0x0400326A RID: 12906
		private static DependencyObjectType _dType;
	}
}
