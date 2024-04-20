using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Markup;
using MS.Internal;
using MS.Internal.KnownBoxes;
using MS.Internal.PresentationFramework;
using MS.Utility;

namespace System.Windows.Controls
{
	// Token: 0x02000735 RID: 1845
	[Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
	public class ContentPresenter : FrameworkElement
	{
		// Token: 0x06006155 RID: 24917 RVA: 0x0029C9B8 File Offset: 0x0029B9B8
		static ContentPresenter()
		{
			DataTemplate dataTemplate = new DataTemplate();
			FrameworkElementFactory frameworkElementFactory = ContentPresenter.CreateAccessTextFactory();
			frameworkElementFactory.SetValue(AccessText.TextProperty, new TemplateBindingExtension(ContentPresenter.ContentProperty));
			dataTemplate.VisualTree = frameworkElementFactory;
			dataTemplate.Seal();
			ContentPresenter.s_AccessTextTemplate = dataTemplate;
			DataTemplate dataTemplate2 = new DataTemplate();
			frameworkElementFactory = ContentPresenter.CreateTextBlockFactory();
			frameworkElementFactory.SetValue(TextBlock.TextProperty, new TemplateBindingExtension(ContentPresenter.ContentProperty));
			dataTemplate2.VisualTree = frameworkElementFactory;
			dataTemplate2.Seal();
			ContentPresenter.s_StringTemplate = dataTemplate2;
			DataTemplate dataTemplate3 = new DataTemplate();
			frameworkElementFactory = ContentPresenter.CreateTextBlockFactory();
			Binding binding = new Binding();
			binding.XPath = ".";
			frameworkElementFactory.SetBinding(TextBlock.TextProperty, binding);
			dataTemplate3.VisualTree = frameworkElementFactory;
			dataTemplate3.Seal();
			ContentPresenter.s_XmlNodeTemplate = dataTemplate3;
			ContentPresenter.UseContentTemplate useContentTemplate = new ContentPresenter.UseContentTemplate();
			useContentTemplate.Seal();
			ContentPresenter.s_UIElementTemplate = useContentTemplate;
			ContentPresenter.DefaultTemplate defaultTemplate = new ContentPresenter.DefaultTemplate();
			defaultTemplate.Seal();
			ContentPresenter.s_DefaultTemplate = defaultTemplate;
			ContentPresenter.s_DefaultTemplateSelector = new ContentPresenter.DefaultSelector();
		}

		// Token: 0x06006156 RID: 24918 RVA: 0x0029CBF4 File Offset: 0x0029BBF4
		public ContentPresenter()
		{
			this.Initialize();
		}

		// Token: 0x06006157 RID: 24919 RVA: 0x0029CC04 File Offset: 0x0029BC04
		private void Initialize()
		{
			PropertyMetadata metadata = ContentPresenter.TemplateProperty.GetMetadata(base.DependencyObjectType);
			DataTemplate dataTemplate = (DataTemplate)metadata.DefaultValue;
			if (dataTemplate != null)
			{
				ContentPresenter.OnTemplateChanged(this, new DependencyPropertyChangedEventArgs(ContentPresenter.TemplateProperty, metadata, null, dataTemplate));
			}
			base.DataContext = null;
		}

		// Token: 0x1700167F RID: 5759
		// (get) Token: 0x06006158 RID: 24920 RVA: 0x0029CC4B File Offset: 0x0029BC4B
		// (set) Token: 0x06006159 RID: 24921 RVA: 0x0029CC5D File Offset: 0x0029BC5D
		public bool RecognizesAccessKey
		{
			get
			{
				return (bool)base.GetValue(ContentPresenter.RecognizesAccessKeyProperty);
			}
			set
			{
				base.SetValue(ContentPresenter.RecognizesAccessKeyProperty, BooleanBoxes.Box(value));
			}
		}

		// Token: 0x17001680 RID: 5760
		// (get) Token: 0x0600615A RID: 24922 RVA: 0x0029C7E8 File Offset: 0x0029B7E8
		// (set) Token: 0x0600615B RID: 24923 RVA: 0x0029C7F5 File Offset: 0x0029B7F5
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

		// Token: 0x0600615C RID: 24924 RVA: 0x0029CC70 File Offset: 0x0029BC70
		private static void OnContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ContentPresenter contentPresenter = (ContentPresenter)d;
			if (!contentPresenter._templateIsCurrent)
			{
				return;
			}
			bool flag;
			if (e.NewValue == BindingExpressionBase.DisconnectedItem)
			{
				flag = false;
			}
			else if (contentPresenter.ContentTemplate != null)
			{
				flag = false;
			}
			else if (contentPresenter.ContentTemplateSelector != null)
			{
				flag = true;
			}
			else if (contentPresenter.Template == ContentPresenter.UIElementContentTemplate)
			{
				flag = true;
				contentPresenter.Template = null;
			}
			else if (contentPresenter.Template == ContentPresenter.DefaultContentTemplate)
			{
				flag = true;
			}
			else
			{
				Type type;
				object obj = ContentPresenter.DataTypeForItem(e.OldValue, contentPresenter, out type);
				object obj2 = ContentPresenter.DataTypeForItem(e.NewValue, contentPresenter, out type);
				flag = (obj != obj2);
				if (!flag && contentPresenter.RecognizesAccessKey && typeof(string) == obj2 && contentPresenter.IsUsingDefaultStringTemplate)
				{
					string text = (string)e.OldValue;
					string text2 = (string)e.NewValue;
					bool flag2 = text.IndexOf(AccessText.AccessKeyMarker) > -1;
					bool flag3 = text2.IndexOf(AccessText.AccessKeyMarker) > -1;
					if (flag2 != flag3)
					{
						flag = true;
					}
				}
			}
			if (flag)
			{
				contentPresenter._templateIsCurrent = false;
			}
			if (contentPresenter._templateIsCurrent && contentPresenter.Template != ContentPresenter.UIElementContentTemplate)
			{
				contentPresenter.DataContext = e.NewValue;
			}
		}

		// Token: 0x17001681 RID: 5761
		// (get) Token: 0x0600615D RID: 24925 RVA: 0x0029C8A3 File Offset: 0x0029B8A3
		// (set) Token: 0x0600615E RID: 24926 RVA: 0x0029C8B5 File Offset: 0x0029B8B5
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

		// Token: 0x0600615F RID: 24927 RVA: 0x0029CD9F File Offset: 0x0029BD9F
		private static void OnContentTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ContentPresenter contentPresenter = (ContentPresenter)d;
			contentPresenter._templateIsCurrent = false;
			contentPresenter.OnContentTemplateChanged((DataTemplate)e.OldValue, (DataTemplate)e.NewValue);
		}

		// Token: 0x06006160 RID: 24928 RVA: 0x0029CDCB File Offset: 0x0029BDCB
		protected virtual void OnContentTemplateChanged(DataTemplate oldContentTemplate, DataTemplate newContentTemplate)
		{
			Helper.CheckTemplateAndTemplateSelector("Content", ContentPresenter.ContentTemplateProperty, ContentPresenter.ContentTemplateSelectorProperty, this);
			this.Template = null;
		}

		// Token: 0x17001682 RID: 5762
		// (get) Token: 0x06006161 RID: 24929 RVA: 0x0029C8FF File Offset: 0x0029B8FF
		// (set) Token: 0x06006162 RID: 24930 RVA: 0x0029C911 File Offset: 0x0029B911
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

		// Token: 0x06006163 RID: 24931 RVA: 0x00105F35 File Offset: 0x00104F35
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeContentTemplateSelector()
		{
			return false;
		}

		// Token: 0x06006164 RID: 24932 RVA: 0x0029CDE9 File Offset: 0x0029BDE9
		private static void OnContentTemplateSelectorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ContentPresenter contentPresenter = (ContentPresenter)d;
			contentPresenter._templateIsCurrent = false;
			contentPresenter.OnContentTemplateSelectorChanged((DataTemplateSelector)e.OldValue, (DataTemplateSelector)e.NewValue);
		}

		// Token: 0x06006165 RID: 24933 RVA: 0x0029CDCB File Offset: 0x0029BDCB
		protected virtual void OnContentTemplateSelectorChanged(DataTemplateSelector oldContentTemplateSelector, DataTemplateSelector newContentTemplateSelector)
		{
			Helper.CheckTemplateAndTemplateSelector("Content", ContentPresenter.ContentTemplateProperty, ContentPresenter.ContentTemplateSelectorProperty, this);
			this.Template = null;
		}

		// Token: 0x17001683 RID: 5763
		// (get) Token: 0x06006166 RID: 24934 RVA: 0x0029CE15 File Offset: 0x0029BE15
		// (set) Token: 0x06006167 RID: 24935 RVA: 0x0029CE27 File Offset: 0x0029BE27
		[Bindable(true)]
		[CustomCategory("Content")]
		public string ContentStringFormat
		{
			get
			{
				return (string)base.GetValue(ContentPresenter.ContentStringFormatProperty);
			}
			set
			{
				base.SetValue(ContentPresenter.ContentStringFormatProperty, value);
			}
		}

		// Token: 0x06006168 RID: 24936 RVA: 0x0029CE35 File Offset: 0x0029BE35
		private static void OnContentStringFormatChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((ContentPresenter)d).OnContentStringFormatChanged((string)e.OldValue, (string)e.NewValue);
		}

		// Token: 0x06006169 RID: 24937 RVA: 0x0029CE5A File Offset: 0x0029BE5A
		protected virtual void OnContentStringFormatChanged(string oldContentStringFormat, string newContentStringFormat)
		{
			ContentPresenter.XMLFormattingTemplateField.ClearValue(this);
			ContentPresenter.StringFormattingTemplateField.ClearValue(this);
			ContentPresenter.AccessTextFormattingTemplateField.ClearValue(this);
		}

		// Token: 0x17001684 RID: 5764
		// (get) Token: 0x0600616A RID: 24938 RVA: 0x0029CE7D File Offset: 0x0029BE7D
		// (set) Token: 0x0600616B RID: 24939 RVA: 0x0029CE8F File Offset: 0x0029BE8F
		public string ContentSource
		{
			get
			{
				return base.GetValue(ContentPresenter.ContentSourceProperty) as string;
			}
			set
			{
				base.SetValue(ContentPresenter.ContentSourceProperty, value);
			}
		}

		// Token: 0x0600616C RID: 24940 RVA: 0x0029CEA0 File Offset: 0x0029BEA0
		internal override void OnPreApplyTemplate()
		{
			base.OnPreApplyTemplate();
			if (base.TemplatedParent == null)
			{
				base.InvalidateProperty(ContentPresenter.ContentProperty);
			}
			if (this._language != null && this._language != base.Language)
			{
				this._templateIsCurrent = false;
			}
			if (!this._templateIsCurrent)
			{
				this.EnsureTemplate();
				this._templateIsCurrent = true;
			}
		}

		// Token: 0x0600616D RID: 24941 RVA: 0x0029CEF8 File Offset: 0x0029BEF8
		protected override Size MeasureOverride(Size constraint)
		{
			return Helper.MeasureElementWithSingleChild(this, constraint);
		}

		// Token: 0x0600616E RID: 24942 RVA: 0x0029CF01 File Offset: 0x0029BF01
		protected override Size ArrangeOverride(Size arrangeSize)
		{
			return Helper.ArrangeElementWithSingleChild(this, arrangeSize);
		}

		// Token: 0x0600616F RID: 24943 RVA: 0x0029CF0C File Offset: 0x0029BF0C
		protected virtual DataTemplate ChooseTemplate()
		{
			object content = this.Content;
			DataTemplate dataTemplate = this.ContentTemplate;
			if (dataTemplate == null && this.ContentTemplateSelector != null)
			{
				dataTemplate = this.ContentTemplateSelector.SelectTemplate(content, this);
			}
			if (dataTemplate == null)
			{
				dataTemplate = ContentPresenter.DefaultTemplateSelector.SelectTemplate(content, this);
			}
			return dataTemplate;
		}

		// Token: 0x17001685 RID: 5765
		// (get) Token: 0x06006170 RID: 24944 RVA: 0x0029CF53 File Offset: 0x0029BF53
		internal static DataTemplate AccessTextContentTemplate
		{
			get
			{
				return ContentPresenter.s_AccessTextTemplate;
			}
		}

		// Token: 0x17001686 RID: 5766
		// (get) Token: 0x06006171 RID: 24945 RVA: 0x0029CF5A File Offset: 0x0029BF5A
		internal static DataTemplate StringContentTemplate
		{
			get
			{
				return ContentPresenter.s_StringTemplate;
			}
		}

		// Token: 0x17001687 RID: 5767
		// (get) Token: 0x06006172 RID: 24946 RVA: 0x0029CF61 File Offset: 0x0029BF61
		internal override FrameworkTemplate TemplateInternal
		{
			get
			{
				return this.Template;
			}
		}

		// Token: 0x17001688 RID: 5768
		// (get) Token: 0x06006173 RID: 24947 RVA: 0x0029CF69 File Offset: 0x0029BF69
		// (set) Token: 0x06006174 RID: 24948 RVA: 0x0029CF71 File Offset: 0x0029BF71
		internal override FrameworkTemplate TemplateCache
		{
			get
			{
				return this._templateCache;
			}
			set
			{
				this._templateCache = (DataTemplate)value;
			}
		}

		// Token: 0x17001689 RID: 5769
		// (get) Token: 0x06006175 RID: 24949 RVA: 0x0029CF7F File Offset: 0x0029BF7F
		internal bool TemplateIsCurrent
		{
			get
			{
				return this._templateIsCurrent;
			}
		}

		// Token: 0x06006176 RID: 24950 RVA: 0x0029CF88 File Offset: 0x0029BF88
		internal void PrepareContentPresenter(object item, DataTemplate itemTemplate, DataTemplateSelector itemTemplateSelector, string stringFormat)
		{
			if (item != this)
			{
				if (this._contentIsItem || !base.HasNonDefaultValue(ContentPresenter.ContentProperty))
				{
					this.Content = item;
					this._contentIsItem = true;
				}
				if (itemTemplate != null)
				{
					base.SetValue(ContentPresenter.ContentTemplateProperty, itemTemplate);
				}
				if (itemTemplateSelector != null)
				{
					base.SetValue(ContentPresenter.ContentTemplateSelectorProperty, itemTemplateSelector);
				}
				if (stringFormat != null)
				{
					base.SetValue(ContentPresenter.ContentStringFormatProperty, stringFormat);
				}
			}
		}

		// Token: 0x06006177 RID: 24951 RVA: 0x0029CFEB File Offset: 0x0029BFEB
		internal void ClearContentPresenter(object item)
		{
			if (item != this && this._contentIsItem)
			{
				this.Content = BindingExpressionBase.DisconnectedItem;
			}
		}

		// Token: 0x06006178 RID: 24952 RVA: 0x0029D004 File Offset: 0x0029C004
		internal static object DataTypeForItem(object item, DependencyObject target, out Type type)
		{
			if (item == null)
			{
				type = null;
				return null;
			}
			type = ReflectionHelper.GetReflectionType(item);
			object result;
			if (SystemXmlLinqHelper.IsXElement(item))
			{
				result = SystemXmlLinqHelper.GetXElementTagName(item);
				type = null;
			}
			else if (SystemXmlHelper.IsXmlNode(item))
			{
				result = SystemXmlHelper.GetXmlTagName(item, target);
				type = null;
			}
			else if (type == typeof(object))
			{
				result = null;
			}
			else
			{
				result = type;
			}
			return result;
		}

		// Token: 0x06006179 RID: 24953 RVA: 0x0029D065 File Offset: 0x0029C065
		internal void ReevaluateTemplate()
		{
			if (this.Template != this.ChooseTemplate())
			{
				this._templateIsCurrent = false;
				base.InvalidateMeasure();
			}
		}

		// Token: 0x1700168A RID: 5770
		// (get) Token: 0x0600617A RID: 24954 RVA: 0x0029D082 File Offset: 0x0029C082
		private static DataTemplate XmlNodeContentTemplate
		{
			get
			{
				return ContentPresenter.s_XmlNodeTemplate;
			}
		}

		// Token: 0x1700168B RID: 5771
		// (get) Token: 0x0600617B RID: 24955 RVA: 0x0029D089 File Offset: 0x0029C089
		private static DataTemplate UIElementContentTemplate
		{
			get
			{
				return ContentPresenter.s_UIElementTemplate;
			}
		}

		// Token: 0x1700168C RID: 5772
		// (get) Token: 0x0600617C RID: 24956 RVA: 0x0029D090 File Offset: 0x0029C090
		private static DataTemplate DefaultContentTemplate
		{
			get
			{
				return ContentPresenter.s_DefaultTemplate;
			}
		}

		// Token: 0x1700168D RID: 5773
		// (get) Token: 0x0600617D RID: 24957 RVA: 0x0029D097 File Offset: 0x0029C097
		private static ContentPresenter.DefaultSelector DefaultTemplateSelector
		{
			get
			{
				return ContentPresenter.s_DefaultTemplateSelector;
			}
		}

		// Token: 0x1700168E RID: 5774
		// (get) Token: 0x0600617E RID: 24958 RVA: 0x0029D0A0 File Offset: 0x0029C0A0
		private DataTemplate FormattingAccessTextContentTemplate
		{
			get
			{
				DataTemplate dataTemplate = ContentPresenter.AccessTextFormattingTemplateField.GetValue(this);
				if (dataTemplate == null)
				{
					Binding binding = new Binding();
					binding.StringFormat = this.ContentStringFormat;
					FrameworkElementFactory frameworkElementFactory = ContentPresenter.CreateAccessTextFactory();
					frameworkElementFactory.SetBinding(AccessText.TextProperty, binding);
					dataTemplate = new DataTemplate();
					dataTemplate.VisualTree = frameworkElementFactory;
					dataTemplate.Seal();
					ContentPresenter.AccessTextFormattingTemplateField.SetValue(this, dataTemplate);
				}
				return dataTemplate;
			}
		}

		// Token: 0x1700168F RID: 5775
		// (get) Token: 0x0600617F RID: 24959 RVA: 0x0029D100 File Offset: 0x0029C100
		private DataTemplate FormattingStringContentTemplate
		{
			get
			{
				DataTemplate dataTemplate = ContentPresenter.StringFormattingTemplateField.GetValue(this);
				if (dataTemplate == null)
				{
					Binding binding = new Binding();
					binding.StringFormat = this.ContentStringFormat;
					FrameworkElementFactory frameworkElementFactory = ContentPresenter.CreateTextBlockFactory();
					frameworkElementFactory.SetBinding(TextBlock.TextProperty, binding);
					dataTemplate = new DataTemplate();
					dataTemplate.VisualTree = frameworkElementFactory;
					dataTemplate.Seal();
					ContentPresenter.StringFormattingTemplateField.SetValue(this, dataTemplate);
				}
				return dataTemplate;
			}
		}

		// Token: 0x17001690 RID: 5776
		// (get) Token: 0x06006180 RID: 24960 RVA: 0x0029D160 File Offset: 0x0029C160
		private DataTemplate FormattingXmlNodeContentTemplate
		{
			get
			{
				DataTemplate dataTemplate = ContentPresenter.XMLFormattingTemplateField.GetValue(this);
				if (dataTemplate == null)
				{
					Binding binding = new Binding();
					binding.XPath = ".";
					binding.StringFormat = this.ContentStringFormat;
					FrameworkElementFactory frameworkElementFactory = ContentPresenter.CreateTextBlockFactory();
					frameworkElementFactory.SetBinding(TextBlock.TextProperty, binding);
					dataTemplate = new DataTemplate();
					dataTemplate.VisualTree = frameworkElementFactory;
					dataTemplate.Seal();
					ContentPresenter.XMLFormattingTemplateField.SetValue(this, dataTemplate);
				}
				return dataTemplate;
			}
		}

		// Token: 0x17001691 RID: 5777
		// (get) Token: 0x06006181 RID: 24961 RVA: 0x0029CF69 File Offset: 0x0029BF69
		// (set) Token: 0x06006182 RID: 24962 RVA: 0x0029D1CB File Offset: 0x0029C1CB
		private DataTemplate Template
		{
			get
			{
				return this._templateCache;
			}
			set
			{
				base.SetValue(ContentPresenter.TemplateProperty, value);
			}
		}

		// Token: 0x06006183 RID: 24963 RVA: 0x0029D1D9 File Offset: 0x0029C1D9
		internal override void OnTemplateChangedInternal(FrameworkTemplate oldTemplate, FrameworkTemplate newTemplate)
		{
			this.OnTemplateChanged((DataTemplate)oldTemplate, (DataTemplate)newTemplate);
		}

		// Token: 0x06006184 RID: 24964 RVA: 0x0029D1ED File Offset: 0x0029C1ED
		private static void OnTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			StyleHelper.UpdateTemplateCache((ContentPresenter)d, (FrameworkTemplate)e.OldValue, (FrameworkTemplate)e.NewValue, ContentPresenter.TemplateProperty);
		}

		// Token: 0x06006185 RID: 24965 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		protected virtual void OnTemplateChanged(DataTemplate oldTemplate, DataTemplate newTemplate)
		{
		}

		// Token: 0x06006186 RID: 24966 RVA: 0x0029D218 File Offset: 0x0029C218
		private void EnsureTemplate()
		{
			DataTemplate template = this.Template;
			DataTemplate dataTemplate = null;
			this._templateIsCurrent = false;
			while (!this._templateIsCurrent)
			{
				this._templateIsCurrent = true;
				dataTemplate = this.ChooseTemplate();
				if (template != dataTemplate)
				{
					this.Template = null;
				}
				if (dataTemplate != ContentPresenter.UIElementContentTemplate)
				{
					base.DataContext = this.Content;
				}
				else
				{
					base.ClearValue(FrameworkElement.DataContextProperty);
				}
			}
			this.Template = dataTemplate;
			if (template == dataTemplate)
			{
				StyleHelper.DoTemplateInvalidations(this, template);
			}
		}

		// Token: 0x06006187 RID: 24967 RVA: 0x0029D28C File Offset: 0x0029C28C
		private DataTemplate SelectTemplateForString(string s)
		{
			string contentStringFormat = this.ContentStringFormat;
			DataTemplate result;
			if (this.RecognizesAccessKey && s.IndexOf(AccessText.AccessKeyMarker) > -1)
			{
				result = (string.IsNullOrEmpty(contentStringFormat) ? ContentPresenter.AccessTextContentTemplate : this.FormattingAccessTextContentTemplate);
			}
			else
			{
				result = (string.IsNullOrEmpty(contentStringFormat) ? ContentPresenter.StringContentTemplate : this.FormattingStringContentTemplate);
			}
			return result;
		}

		// Token: 0x17001692 RID: 5778
		// (get) Token: 0x06006188 RID: 24968 RVA: 0x0029D2E8 File Offset: 0x0029C2E8
		private bool IsUsingDefaultStringTemplate
		{
			get
			{
				if (this.Template == ContentPresenter.StringContentTemplate || this.Template == ContentPresenter.AccessTextContentTemplate)
				{
					return true;
				}
				DataTemplate value = ContentPresenter.StringFormattingTemplateField.GetValue(this);
				if (value != null && value == this.Template)
				{
					return true;
				}
				value = ContentPresenter.AccessTextFormattingTemplateField.GetValue(this);
				return value != null && value == this.Template;
			}
		}

		// Token: 0x06006189 RID: 24969 RVA: 0x0029D346 File Offset: 0x0029C346
		private DataTemplate SelectTemplateForXML()
		{
			if (!string.IsNullOrEmpty(this.ContentStringFormat))
			{
				return this.FormattingXmlNodeContentTemplate;
			}
			return ContentPresenter.XmlNodeContentTemplate;
		}

		// Token: 0x0600618A RID: 24970 RVA: 0x0029D361 File Offset: 0x0029C361
		internal static FrameworkElementFactory CreateAccessTextFactory()
		{
			return new FrameworkElementFactory(typeof(AccessText));
		}

		// Token: 0x0600618B RID: 24971 RVA: 0x0029D372 File Offset: 0x0029C372
		internal static FrameworkElementFactory CreateTextBlockFactory()
		{
			return new FrameworkElementFactory(typeof(TextBlock));
		}

		// Token: 0x0600618C RID: 24972 RVA: 0x0029D383 File Offset: 0x0029C383
		private static TextBlock CreateTextBlock(ContentPresenter container)
		{
			return new TextBlock();
		}

		// Token: 0x0600618D RID: 24973 RVA: 0x0029D38A File Offset: 0x0029C38A
		private void CacheLanguage(XmlLanguage language)
		{
			this._language = language;
		}

		// Token: 0x17001693 RID: 5779
		// (get) Token: 0x0600618E RID: 24974 RVA: 0x001FD464 File Offset: 0x001FC464
		internal override int EffectiveValuesInitialSize
		{
			get
			{
				return 28;
			}
		}

		// Token: 0x0400326B RID: 12907
		[CommonDependencyProperty]
		public static readonly DependencyProperty RecognizesAccessKeyProperty = DependencyProperty.Register("RecognizesAccessKey", typeof(bool), typeof(ContentPresenter), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));

		// Token: 0x0400326C RID: 12908
		[CommonDependencyProperty]
		public static readonly DependencyProperty ContentProperty = ContentControl.ContentProperty.AddOwner(typeof(ContentPresenter), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(ContentPresenter.OnContentChanged)));

		// Token: 0x0400326D RID: 12909
		[CommonDependencyProperty]
		public static readonly DependencyProperty ContentTemplateProperty = ContentControl.ContentTemplateProperty.AddOwner(typeof(ContentPresenter), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(ContentPresenter.OnContentTemplateChanged)));

		// Token: 0x0400326E RID: 12910
		[CommonDependencyProperty]
		public static readonly DependencyProperty ContentTemplateSelectorProperty = ContentControl.ContentTemplateSelectorProperty.AddOwner(typeof(ContentPresenter), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(ContentPresenter.OnContentTemplateSelectorChanged)));

		// Token: 0x0400326F RID: 12911
		[CommonDependencyProperty]
		public static readonly DependencyProperty ContentStringFormatProperty = DependencyProperty.Register("ContentStringFormat", typeof(string), typeof(ContentPresenter), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(ContentPresenter.OnContentStringFormatChanged)));

		// Token: 0x04003270 RID: 12912
		[CommonDependencyProperty]
		public static readonly DependencyProperty ContentSourceProperty = DependencyProperty.Register("ContentSource", typeof(string), typeof(ContentPresenter), new FrameworkPropertyMetadata("Content"));

		// Token: 0x04003271 RID: 12913
		internal static readonly DependencyProperty TemplateProperty = DependencyProperty.Register("Template", typeof(DataTemplate), typeof(ContentPresenter), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(ContentPresenter.OnTemplateChanged)));

		// Token: 0x04003272 RID: 12914
		private DataTemplate _templateCache;

		// Token: 0x04003273 RID: 12915
		private bool _templateIsCurrent;

		// Token: 0x04003274 RID: 12916
		private bool _contentIsItem;

		// Token: 0x04003275 RID: 12917
		private XmlLanguage _language;

		// Token: 0x04003276 RID: 12918
		private static DataTemplate s_AccessTextTemplate;

		// Token: 0x04003277 RID: 12919
		private static DataTemplate s_StringTemplate;

		// Token: 0x04003278 RID: 12920
		private static DataTemplate s_XmlNodeTemplate;

		// Token: 0x04003279 RID: 12921
		private static DataTemplate s_UIElementTemplate;

		// Token: 0x0400327A RID: 12922
		private static DataTemplate s_DefaultTemplate;

		// Token: 0x0400327B RID: 12923
		private static ContentPresenter.DefaultSelector s_DefaultTemplateSelector;

		// Token: 0x0400327C RID: 12924
		private static readonly UncommonField<DataTemplate> XMLFormattingTemplateField = new UncommonField<DataTemplate>();

		// Token: 0x0400327D RID: 12925
		private static readonly UncommonField<DataTemplate> StringFormattingTemplateField = new UncommonField<DataTemplate>();

		// Token: 0x0400327E RID: 12926
		private static readonly UncommonField<DataTemplate> AccessTextFormattingTemplateField = new UncommonField<DataTemplate>();

		// Token: 0x02000BC3 RID: 3011
		private class UseContentTemplate : DataTemplate
		{
			// Token: 0x06008F4B RID: 36683 RVA: 0x00343D37 File Offset: 0x00342D37
			public UseContentTemplate()
			{
				base.CanBuildVisualTree = true;
			}

			// Token: 0x06008F4C RID: 36684 RVA: 0x00343D48 File Offset: 0x00342D48
			internal override bool BuildVisualTree(FrameworkElement container)
			{
				object content = ((ContentPresenter)container).Content;
				UIElement uielement = content as UIElement;
				if (uielement == null)
				{
					uielement = (UIElement)TypeDescriptor.GetConverter(ReflectionHelper.GetReflectionType(content)).ConvertTo(content, typeof(UIElement));
				}
				StyleHelper.AddCustomTemplateRoot(container, uielement);
				return true;
			}
		}

		// Token: 0x02000BC4 RID: 3012
		private class DefaultTemplate : DataTemplate
		{
			// Token: 0x06008F4D RID: 36685 RVA: 0x00343D37 File Offset: 0x00342D37
			public DefaultTemplate()
			{
				base.CanBuildVisualTree = true;
			}

			// Token: 0x06008F4E RID: 36686 RVA: 0x00343D94 File Offset: 0x00342D94
			internal override bool BuildVisualTree(FrameworkElement container)
			{
				bool flag = EventTrace.IsEnabled(EventTrace.Keyword.KeywordGeneral, EventTrace.Level.Info);
				if (flag)
				{
					EventTrace.EventProvider.TraceEvent(EventTrace.Event.WClientStringBegin, EventTrace.Keyword.KeywordGeneral, EventTrace.Level.Info, "ContentPresenter.BuildVisualTree");
				}
				bool result;
				try
				{
					ContentPresenter contentPresenter = (ContentPresenter)container;
					result = (this.DefaultExpansion(contentPresenter.Content, contentPresenter) != null);
				}
				finally
				{
					if (flag)
					{
						EventTrace.EventProvider.TraceEvent(EventTrace.Event.WClientStringEnd, EventTrace.Keyword.KeywordGeneral, EventTrace.Level.Info, string.Format(CultureInfo.InvariantCulture, "ContentPresenter.BuildVisualTree for CP {0}", container.GetHashCode()));
					}
				}
				return result;
			}

			// Token: 0x06008F4F RID: 36687 RVA: 0x00343E18 File Offset: 0x00342E18
			private UIElement DefaultExpansion(object content, ContentPresenter container)
			{
				if (content == null)
				{
					return null;
				}
				TextBlock textBlock = ContentPresenter.CreateTextBlock(container);
				textBlock.IsContentPresenterContainer = true;
				if (container != null)
				{
					StyleHelper.AddCustomTemplateRoot(container, textBlock, false, true);
				}
				this.DoDefaultExpansion(textBlock, content, container);
				return textBlock;
			}

			// Token: 0x06008F50 RID: 36688 RVA: 0x00343E50 File Offset: 0x00342E50
			private void DoDefaultExpansion(TextBlock textBlock, object content, ContentPresenter container)
			{
				Inline item;
				if ((item = (content as Inline)) != null)
				{
					textBlock.Inlines.Add(item);
					return;
				}
				bool flag = false;
				XmlLanguage language = container.Language;
				CultureInfo specificCulture = language.GetSpecificCulture();
				container.CacheLanguage(language);
				string text;
				if ((text = container.ContentStringFormat) != null)
				{
					try
					{
						text = Helper.GetEffectiveStringFormat(text);
						textBlock.Text = string.Format(specificCulture, text, content);
						flag = true;
					}
					catch (FormatException)
					{
					}
				}
				if (!flag)
				{
					TypeConverter converter = TypeDescriptor.GetConverter(ReflectionHelper.GetReflectionType(content));
					ContentPresenter.DefaultTemplate.TypeContext context = new ContentPresenter.DefaultTemplate.TypeContext(content);
					if (converter != null && converter.CanConvertTo(context, typeof(string)))
					{
						textBlock.Text = (string)converter.ConvertTo(context, specificCulture, content, typeof(string));
						return;
					}
					textBlock.Text = string.Format(specificCulture, "{0}", content);
				}
			}

			// Token: 0x02000C93 RID: 3219
			private class TypeContext : ITypeDescriptorContext, IServiceProvider
			{
				// Token: 0x0600957B RID: 38267 RVA: 0x0034E00A File Offset: 0x0034D00A
				public TypeContext(object instance)
				{
					this._instance = instance;
				}

				// Token: 0x1700200F RID: 8207
				// (get) Token: 0x0600957C RID: 38268 RVA: 0x00109403 File Offset: 0x00108403
				IContainer ITypeDescriptorContext.Container
				{
					get
					{
						return null;
					}
				}

				// Token: 0x17002010 RID: 8208
				// (get) Token: 0x0600957D RID: 38269 RVA: 0x0034E019 File Offset: 0x0034D019
				object ITypeDescriptorContext.Instance
				{
					get
					{
						return this._instance;
					}
				}

				// Token: 0x17002011 RID: 8209
				// (get) Token: 0x0600957E RID: 38270 RVA: 0x00109403 File Offset: 0x00108403
				PropertyDescriptor ITypeDescriptorContext.PropertyDescriptor
				{
					get
					{
						return null;
					}
				}

				// Token: 0x0600957F RID: 38271 RVA: 0x000F6B2C File Offset: 0x000F5B2C
				void ITypeDescriptorContext.OnComponentChanged()
				{
				}

				// Token: 0x06009580 RID: 38272 RVA: 0x00105F35 File Offset: 0x00104F35
				bool ITypeDescriptorContext.OnComponentChanging()
				{
					return false;
				}

				// Token: 0x06009581 RID: 38273 RVA: 0x00109403 File Offset: 0x00108403
				object IServiceProvider.GetService(Type serviceType)
				{
					return null;
				}

				// Token: 0x04004FD2 RID: 20434
				private object _instance;
			}
		}

		// Token: 0x02000BC5 RID: 3013
		private class DefaultSelector : DataTemplateSelector
		{
			// Token: 0x06008F51 RID: 36689 RVA: 0x00343F2C File Offset: 0x00342F2C
			public override DataTemplate SelectTemplate(object item, DependencyObject container)
			{
				DataTemplate dataTemplate = null;
				if (item != null)
				{
					dataTemplate = (DataTemplate)FrameworkElement.FindTemplateResourceInternal(container, item, typeof(DataTemplate));
				}
				if (dataTemplate == null)
				{
					string s;
					TypeConverter converter;
					if ((s = (item as string)) != null)
					{
						dataTemplate = ((ContentPresenter)container).SelectTemplateForString(s);
					}
					else if (item is UIElement)
					{
						dataTemplate = ContentPresenter.UIElementContentTemplate;
					}
					else if (SystemXmlHelper.IsXmlNode(item))
					{
						dataTemplate = ((ContentPresenter)container).SelectTemplateForXML();
					}
					else if (item is Inline)
					{
						dataTemplate = ContentPresenter.DefaultContentTemplate;
					}
					else if (item != null && (converter = TypeDescriptor.GetConverter(ReflectionHelper.GetReflectionType(item))) != null && converter.CanConvertTo(typeof(UIElement)))
					{
						dataTemplate = ContentPresenter.UIElementContentTemplate;
					}
					else
					{
						dataTemplate = ContentPresenter.DefaultContentTemplate;
					}
				}
				return dataTemplate;
			}
		}
	}
}
