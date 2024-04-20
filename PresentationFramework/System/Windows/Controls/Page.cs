using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Navigation;
using MS.Internal;
using MS.Internal.KnownBoxes;

namespace System.Windows.Controls
{
	// Token: 0x020007B4 RID: 1972
	[ContentProperty("Content")]
	public class Page : FrameworkElement, IWindowService, IAddChild
	{
		// Token: 0x06006FF2 RID: 28658 RVA: 0x002D6730 File Offset: 0x002D5730
		static Page()
		{
			Window.IWindowServiceProperty.OverrideMetadata(typeof(Page), new FrameworkPropertyMetadata(new PropertyChangedCallback(Page._OnWindowServiceChanged)));
			UIElement.FocusableProperty.OverrideMetadata(typeof(Page), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(Page), new FrameworkPropertyMetadata(typeof(Page)));
			Page._dType = DependencyObjectType.FromSystemTypeInternal(typeof(Page));
		}

		// Token: 0x06006FF3 RID: 28659 RVA: 0x002D68DC File Offset: 0x002D58DC
		public Page()
		{
			PropertyMetadata metadata = Page.TemplateProperty.GetMetadata(base.DependencyObjectType);
			ControlTemplate controlTemplate = (ControlTemplate)metadata.DefaultValue;
			if (controlTemplate != null)
			{
				Page.OnTemplateChanged(this, new DependencyPropertyChangedEventArgs(Page.TemplateProperty, metadata, null, controlTemplate));
			}
		}

		// Token: 0x06006FF4 RID: 28660 RVA: 0x002D6922 File Offset: 0x002D5922
		void IAddChild.AddChild(object obj)
		{
			base.VerifyAccess();
			if (this.Content == null || obj == null)
			{
				this.Content = obj;
				return;
			}
			throw new InvalidOperationException(SR.Get("PageCannotHaveMultipleContent"));
		}

		// Token: 0x06006FF5 RID: 28661 RVA: 0x00175B1C File Offset: 0x00174B1C
		void IAddChild.AddText(string str)
		{
			XamlSerializerUtil.ThrowIfNonWhiteSpaceInAddText(str, this);
		}

		// Token: 0x170019D4 RID: 6612
		// (get) Token: 0x06006FF6 RID: 28662 RVA: 0x002D694C File Offset: 0x002D594C
		protected internal override IEnumerator LogicalChildren
		{
			get
			{
				base.VerifyAccess();
				return new SingleChildEnumerator(this.Content);
			}
		}

		// Token: 0x170019D5 RID: 6613
		// (get) Token: 0x06006FF7 RID: 28663 RVA: 0x002D695F File Offset: 0x002D595F
		// (set) Token: 0x06006FF8 RID: 28664 RVA: 0x002D6972 File Offset: 0x002D5972
		public object Content
		{
			get
			{
				base.VerifyAccess();
				return base.GetValue(Page.ContentProperty);
			}
			set
			{
				base.VerifyAccess();
				base.SetValue(Page.ContentProperty, value);
			}
		}

		// Token: 0x170019D6 RID: 6614
		// (get) Token: 0x06006FF9 RID: 28665 RVA: 0x002D6986 File Offset: 0x002D5986
		// (set) Token: 0x06006FFA RID: 28666 RVA: 0x002D69B1 File Offset: 0x002D59B1
		string IWindowService.Title
		{
			get
			{
				base.VerifyAccess();
				if (this.WindowService == null)
				{
					throw new InvalidOperationException(SR.Get("CannotQueryPropertiesWhenPageNotInTreeWithWindow"));
				}
				return this.WindowService.Title;
			}
			set
			{
				base.VerifyAccess();
				if (this.WindowService == null)
				{
					this.PageHelperObject._windowTitle = value;
					this.PropertyIsSet(SetPropertyFlags.WindowTitle);
					return;
				}
				if (this._isTopLevel)
				{
					this.WindowService.Title = value;
					this.PropertyIsSet(SetPropertyFlags.WindowTitle);
				}
			}
		}

		// Token: 0x170019D7 RID: 6615
		// (get) Token: 0x06006FFB RID: 28667 RVA: 0x002D69F0 File Offset: 0x002D59F0
		// (set) Token: 0x06006FFC RID: 28668 RVA: 0x002D69FE File Offset: 0x002D59FE
		[Localizability(LocalizationCategory.Title)]
		public string WindowTitle
		{
			get
			{
				base.VerifyAccess();
				return ((IWindowService)this).Title;
			}
			set
			{
				base.VerifyAccess();
				((IWindowService)this).Title = value;
			}
		}

		// Token: 0x06006FFD RID: 28669 RVA: 0x002D6A0D File Offset: 0x002D5A0D
		internal bool ShouldJournalWindowTitle()
		{
			return this.IsPropertySet(SetPropertyFlags.WindowTitle);
		}

		// Token: 0x170019D8 RID: 6616
		// (get) Token: 0x06006FFE RID: 28670 RVA: 0x002D6A16 File Offset: 0x002D5A16
		// (set) Token: 0x06006FFF RID: 28671 RVA: 0x002D6A44 File Offset: 0x002D5A44
		double IWindowService.Height
		{
			get
			{
				base.VerifyAccess();
				if (this.WindowService == null)
				{
					throw new InvalidOperationException(SR.Get("CannotQueryPropertiesWhenPageNotInTreeWithWindow"));
				}
				return this.WindowService.Height;
			}
			set
			{
				base.VerifyAccess();
				if (this.WindowService == null)
				{
					this.PageHelperObject._windowHeight = value;
					this.PropertyIsSet(SetPropertyFlags.WindowHeight);
					return;
				}
				if (this._isTopLevel)
				{
					if (!this.WindowService.UserResized)
					{
						this.WindowService.Height = value;
					}
					this.PropertyIsSet(SetPropertyFlags.WindowHeight);
				}
			}
		}

		// Token: 0x170019D9 RID: 6617
		// (get) Token: 0x06007000 RID: 28672 RVA: 0x002D6A9B File Offset: 0x002D5A9B
		// (set) Token: 0x06007001 RID: 28673 RVA: 0x002D6AA9 File Offset: 0x002D5AA9
		public double WindowHeight
		{
			get
			{
				base.VerifyAccess();
				return ((IWindowService)this).Height;
			}
			set
			{
				base.VerifyAccess();
				((IWindowService)this).Height = value;
			}
		}

		// Token: 0x170019DA RID: 6618
		// (get) Token: 0x06007002 RID: 28674 RVA: 0x002D6AB8 File Offset: 0x002D5AB8
		// (set) Token: 0x06007003 RID: 28675 RVA: 0x002D6AE4 File Offset: 0x002D5AE4
		double IWindowService.Width
		{
			get
			{
				base.VerifyAccess();
				if (this.WindowService == null)
				{
					throw new InvalidOperationException(SR.Get("CannotQueryPropertiesWhenPageNotInTreeWithWindow"));
				}
				return this.WindowService.Width;
			}
			set
			{
				base.VerifyAccess();
				if (this.WindowService == null)
				{
					this.PageHelperObject._windowWidth = value;
					this.PropertyIsSet(SetPropertyFlags.WindowWidth);
					return;
				}
				if (this._isTopLevel)
				{
					if (!this.WindowService.UserResized)
					{
						this.WindowService.Width = value;
					}
					this.PropertyIsSet(SetPropertyFlags.WindowWidth);
				}
			}
		}

		// Token: 0x170019DB RID: 6619
		// (get) Token: 0x06007004 RID: 28676 RVA: 0x002D6B3B File Offset: 0x002D5B3B
		// (set) Token: 0x06007005 RID: 28677 RVA: 0x002D6B49 File Offset: 0x002D5B49
		public double WindowWidth
		{
			get
			{
				base.VerifyAccess();
				return ((IWindowService)this).Width;
			}
			set
			{
				base.VerifyAccess();
				((IWindowService)this).Width = value;
			}
		}

		// Token: 0x170019DC RID: 6620
		// (get) Token: 0x06007006 RID: 28678 RVA: 0x002D6B58 File Offset: 0x002D5B58
		// (set) Token: 0x06007007 RID: 28679 RVA: 0x002D6B6A File Offset: 0x002D5B6A
		[Category("Appearance")]
		public Brush Background
		{
			get
			{
				return (Brush)base.GetValue(Page.BackgroundProperty);
			}
			set
			{
				base.SetValue(Page.BackgroundProperty, value);
			}
		}

		// Token: 0x170019DD RID: 6621
		// (get) Token: 0x06007008 RID: 28680 RVA: 0x002D6B78 File Offset: 0x002D5B78
		// (set) Token: 0x06007009 RID: 28681 RVA: 0x002D6B8A File Offset: 0x002D5B8A
		public string Title
		{
			get
			{
				return (string)base.GetValue(Page.TitleProperty);
			}
			set
			{
				base.SetValue(Page.TitleProperty, value);
			}
		}

		// Token: 0x0600700A RID: 28682 RVA: 0x002D6B98 File Offset: 0x002D5B98
		private static void OnTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((Page)d).PropertyIsSet(SetPropertyFlags.Title);
		}

		// Token: 0x170019DE RID: 6622
		// (get) Token: 0x0600700B RID: 28683 RVA: 0x002D6BA8 File Offset: 0x002D5BA8
		// (set) Token: 0x0600700C RID: 28684 RVA: 0x002D6BEA File Offset: 0x002D5BEA
		public bool ShowsNavigationUI
		{
			get
			{
				base.VerifyAccess();
				if (this.WindowService == null)
				{
					throw new InvalidOperationException(SR.Get("CannotQueryPropertiesWhenPageNotInTreeWithWindow"));
				}
				NavigationWindow navigationWindow = this.WindowService as NavigationWindow;
				return navigationWindow != null && navigationWindow.ShowsNavigationUI;
			}
			set
			{
				base.VerifyAccess();
				if (this.WindowService == null)
				{
					this.PageHelperObject._showsNavigationUI = value;
					this.PropertyIsSet(SetPropertyFlags.ShowsNavigationUI);
					return;
				}
				if (this._isTopLevel)
				{
					this.SetShowsNavigationUI(value);
					this.PropertyIsSet(SetPropertyFlags.ShowsNavigationUI);
				}
			}
		}

		// Token: 0x170019DF RID: 6623
		// (get) Token: 0x0600700D RID: 28685 RVA: 0x002D6C26 File Offset: 0x002D5C26
		// (set) Token: 0x0600700E RID: 28686 RVA: 0x002D6C2E File Offset: 0x002D5C2E
		public bool KeepAlive
		{
			get
			{
				return JournalEntry.GetKeepAlive(this);
			}
			set
			{
				JournalEntry.SetKeepAlive(this, value);
			}
		}

		// Token: 0x170019E0 RID: 6624
		// (get) Token: 0x0600700F RID: 28687 RVA: 0x002D6C37 File Offset: 0x002D5C37
		public NavigationService NavigationService
		{
			get
			{
				return NavigationService.GetNavigationService(this);
			}
		}

		// Token: 0x170019E1 RID: 6625
		// (get) Token: 0x06007010 RID: 28688 RVA: 0x002D6C3F File Offset: 0x002D5C3F
		// (set) Token: 0x06007011 RID: 28689 RVA: 0x002D6C51 File Offset: 0x002D5C51
		[Category("Appearance")]
		[Bindable(true)]
		public Brush Foreground
		{
			get
			{
				return (Brush)base.GetValue(Page.ForegroundProperty);
			}
			set
			{
				base.SetValue(Page.ForegroundProperty, value);
			}
		}

		// Token: 0x170019E2 RID: 6626
		// (get) Token: 0x06007012 RID: 28690 RVA: 0x002D6C5F File Offset: 0x002D5C5F
		// (set) Token: 0x06007013 RID: 28691 RVA: 0x002D6C71 File Offset: 0x002D5C71
		[Bindable(true)]
		[Category("Appearance")]
		[Localizability(LocalizationCategory.Font, Modifiability = Modifiability.Unmodifiable)]
		public FontFamily FontFamily
		{
			get
			{
				return (FontFamily)base.GetValue(Page.FontFamilyProperty);
			}
			set
			{
				base.SetValue(Page.FontFamilyProperty, value);
			}
		}

		// Token: 0x170019E3 RID: 6627
		// (get) Token: 0x06007014 RID: 28692 RVA: 0x002D6C7F File Offset: 0x002D5C7F
		// (set) Token: 0x06007015 RID: 28693 RVA: 0x002D6C91 File Offset: 0x002D5C91
		[Bindable(true)]
		[TypeConverter(typeof(FontSizeConverter))]
		[Category("Appearance")]
		[Localizability(LocalizationCategory.None)]
		public double FontSize
		{
			get
			{
				return (double)base.GetValue(Page.FontSizeProperty);
			}
			set
			{
				base.SetValue(Page.FontSizeProperty, value);
			}
		}

		// Token: 0x170019E4 RID: 6628
		// (get) Token: 0x06007016 RID: 28694 RVA: 0x002D6CA4 File Offset: 0x002D5CA4
		// (set) Token: 0x06007017 RID: 28695 RVA: 0x002D6CAC File Offset: 0x002D5CAC
		public ControlTemplate Template
		{
			get
			{
				return this._templateCache;
			}
			set
			{
				base.SetValue(Page.TemplateProperty, value);
			}
		}

		// Token: 0x170019E5 RID: 6629
		// (get) Token: 0x06007018 RID: 28696 RVA: 0x002D6CBA File Offset: 0x002D5CBA
		internal override FrameworkTemplate TemplateInternal
		{
			get
			{
				return this.Template;
			}
		}

		// Token: 0x170019E6 RID: 6630
		// (get) Token: 0x06007019 RID: 28697 RVA: 0x002D6CA4 File Offset: 0x002D5CA4
		// (set) Token: 0x0600701A RID: 28698 RVA: 0x002D6CC2 File Offset: 0x002D5CC2
		internal override FrameworkTemplate TemplateCache
		{
			get
			{
				return this._templateCache;
			}
			set
			{
				this._templateCache = (ControlTemplate)value;
			}
		}

		// Token: 0x0600701B RID: 28699 RVA: 0x002D6CD0 File Offset: 0x002D5CD0
		internal override void OnTemplateChangedInternal(FrameworkTemplate oldTemplate, FrameworkTemplate newTemplate)
		{
			this.OnTemplateChanged((ControlTemplate)oldTemplate, (ControlTemplate)newTemplate);
		}

		// Token: 0x0600701C RID: 28700 RVA: 0x002D6CE4 File Offset: 0x002D5CE4
		private static void OnTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			StyleHelper.UpdateTemplateCache((Page)d, (FrameworkTemplate)e.OldValue, (FrameworkTemplate)e.NewValue, Page.TemplateProperty);
		}

		// Token: 0x0600701D RID: 28701 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		protected virtual void OnTemplateChanged(ControlTemplate oldTemplate, ControlTemplate newTemplate)
		{
		}

		// Token: 0x0600701E RID: 28702 RVA: 0x002D6D10 File Offset: 0x002D5D10
		protected override Size MeasureOverride(Size constraint)
		{
			base.VerifyAccess();
			if (this.VisualChildrenCount > 0)
			{
				UIElement uielement = this.GetVisualChild(0) as UIElement;
				if (uielement != null)
				{
					uielement.Measure(constraint);
					return uielement.DesiredSize;
				}
			}
			return new Size(0.0, 0.0);
		}

		// Token: 0x0600701F RID: 28703 RVA: 0x002D6D64 File Offset: 0x002D5D64
		protected override Size ArrangeOverride(Size arrangeBounds)
		{
			base.VerifyAccess();
			if (this.VisualChildrenCount > 0)
			{
				UIElement uielement = this.GetVisualChild(0) as UIElement;
				if (uielement != null)
				{
					uielement.Arrange(new Rect(default(Point), arrangeBounds));
				}
			}
			return arrangeBounds;
		}

		// Token: 0x06007020 RID: 28704 RVA: 0x002D6DA8 File Offset: 0x002D5DA8
		protected internal sealed override void OnVisualParentChanged(DependencyObject oldParent)
		{
			base.VerifyAccess();
			base.OnVisualParentChanged(oldParent);
			Visual visual = VisualTreeHelper.GetParent(this) as Visual;
			if (visual == null || base.Parent is Window || (this.NavigationService != null && this.NavigationService.Content == this))
			{
				return;
			}
			bool flag = false;
			FrameworkElement frameworkElement = visual as FrameworkElement;
			if (frameworkElement != null)
			{
				DependencyObject dependencyObject = frameworkElement;
				while (frameworkElement != null && frameworkElement.TemplatedParent != null)
				{
					dependencyObject = frameworkElement.TemplatedParent;
					frameworkElement = (dependencyObject as FrameworkElement);
					if (frameworkElement is Frame)
					{
						break;
					}
				}
				if (dependencyObject is Window || dependencyObject is Frame)
				{
					flag = true;
				}
			}
			if (!flag)
			{
				throw new InvalidOperationException(SR.Get("ParentOfPageMustBeWindowOrFrame"));
			}
		}

		// Token: 0x06007021 RID: 28705 RVA: 0x002D6E4B File Offset: 0x002D5E4B
		private static void OnContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((Page)d).OnContentChanged(e.OldValue, e.NewValue);
		}

		// Token: 0x06007022 RID: 28706 RVA: 0x002C6472 File Offset: 0x002C5472
		private void OnContentChanged(object oldContent, object newContent)
		{
			base.RemoveLogicalChild(oldContent);
			base.AddLogicalChild(newContent);
		}

		// Token: 0x06007023 RID: 28707 RVA: 0x002D6E66 File Offset: 0x002D5E66
		private static void _OnWindowServiceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			(d as Page).OnWindowServiceChanged(e.NewValue as IWindowService);
		}

		// Token: 0x06007024 RID: 28708 RVA: 0x002D6E7F File Offset: 0x002D5E7F
		private void OnWindowServiceChanged(IWindowService iws)
		{
			this._currentIws = iws;
			this.DetermineTopLevel();
			if (this._currentIws != null && this._isTopLevel)
			{
				this.PropagateProperties();
			}
		}

		// Token: 0x06007025 RID: 28709 RVA: 0x002D6EA4 File Offset: 0x002D5EA4
		private void DetermineTopLevel()
		{
			FrameworkElement frameworkElement = base.Parent as FrameworkElement;
			if (frameworkElement != null && frameworkElement.InheritanceBehavior == InheritanceBehavior.Default)
			{
				this._isTopLevel = true;
				return;
			}
			this._isTopLevel = false;
		}

		// Token: 0x06007026 RID: 28710 RVA: 0x002D6ED8 File Offset: 0x002D5ED8
		private void PropagateProperties()
		{
			if (this._pho == null)
			{
				return;
			}
			if (this.IsPropertySet(SetPropertyFlags.WindowTitle))
			{
				this._currentIws.Title = this.PageHelperObject._windowTitle;
			}
			if (this.IsPropertySet(SetPropertyFlags.WindowHeight) && !this._currentIws.UserResized)
			{
				this._currentIws.Height = this.PageHelperObject._windowHeight;
			}
			if (this.IsPropertySet(SetPropertyFlags.WindowWidth) && !this._currentIws.UserResized)
			{
				this._currentIws.Width = this.PageHelperObject._windowWidth;
			}
			if (this.IsPropertySet(SetPropertyFlags.ShowsNavigationUI))
			{
				this.SetShowsNavigationUI(this.PageHelperObject._showsNavigationUI);
			}
		}

		// Token: 0x170019E7 RID: 6631
		// (get) Token: 0x06007027 RID: 28711 RVA: 0x002D6F80 File Offset: 0x002D5F80
		bool IWindowService.UserResized
		{
			get
			{
				Invariant.Assert(this._currentIws != null, "_currentIws cannot be null here.");
				return this._currentIws.UserResized;
			}
		}

		// Token: 0x06007028 RID: 28712 RVA: 0x002D6FA0 File Offset: 0x002D5FA0
		private void SetShowsNavigationUI(bool showsNavigationUI)
		{
			NavigationWindow navigationWindow = this._currentIws as NavigationWindow;
			if (navigationWindow != null)
			{
				navigationWindow.ShowsNavigationUI = showsNavigationUI;
			}
		}

		// Token: 0x06007029 RID: 28713 RVA: 0x002D6FC3 File Offset: 0x002D5FC3
		private bool IsPropertySet(SetPropertyFlags property)
		{
			return (this._setPropertyFlags & property) > SetPropertyFlags.None;
		}

		// Token: 0x0600702A RID: 28714 RVA: 0x002D6FD0 File Offset: 0x002D5FD0
		private void PropertyIsSet(SetPropertyFlags property)
		{
			this._setPropertyFlags |= property;
		}

		// Token: 0x0600702B RID: 28715 RVA: 0x002D6A0D File Offset: 0x002D5A0D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeWindowTitle()
		{
			return this.IsPropertySet(SetPropertyFlags.WindowTitle);
		}

		// Token: 0x0600702C RID: 28716 RVA: 0x002D6FE0 File Offset: 0x002D5FE0
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeWindowHeight()
		{
			return this.IsPropertySet(SetPropertyFlags.WindowHeight);
		}

		// Token: 0x0600702D RID: 28717 RVA: 0x002D6FE9 File Offset: 0x002D5FE9
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeWindowWidth()
		{
			return this.IsPropertySet(SetPropertyFlags.WindowWidth);
		}

		// Token: 0x0600702E RID: 28718 RVA: 0x002D6FF2 File Offset: 0x002D5FF2
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeTitle()
		{
			return this.IsPropertySet(SetPropertyFlags.Title);
		}

		// Token: 0x0600702F RID: 28719 RVA: 0x002D6FFB File Offset: 0x002D5FFB
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeShowsNavigationUI()
		{
			return this.IsPropertySet(SetPropertyFlags.ShowsNavigationUI);
		}

		// Token: 0x170019E8 RID: 6632
		// (get) Token: 0x06007030 RID: 28720 RVA: 0x002D7005 File Offset: 0x002D6005
		private IWindowService WindowService
		{
			get
			{
				return this._currentIws;
			}
		}

		// Token: 0x170019E9 RID: 6633
		// (get) Token: 0x06007031 RID: 28721 RVA: 0x002D700D File Offset: 0x002D600D
		private PageHelperObject PageHelperObject
		{
			get
			{
				if (this._pho == null)
				{
					this._pho = new PageHelperObject();
				}
				return this._pho;
			}
		}

		// Token: 0x170019EA RID: 6634
		// (get) Token: 0x06007032 RID: 28722 RVA: 0x001A5A01 File Offset: 0x001A4A01
		internal override int EffectiveValuesInitialSize
		{
			get
			{
				return 19;
			}
		}

		// Token: 0x170019EB RID: 6635
		// (get) Token: 0x06007033 RID: 28723 RVA: 0x002D7028 File Offset: 0x002D6028
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return Page._dType;
			}
		}

		// Token: 0x040036BE RID: 14014
		public static readonly DependencyProperty ContentProperty = ContentControl.ContentProperty.AddOwner(typeof(Page), new FrameworkPropertyMetadata(new PropertyChangedCallback(Page.OnContentChanged)));

		// Token: 0x040036BF RID: 14015
		public static readonly DependencyProperty BackgroundProperty = Panel.BackgroundProperty.AddOwner(typeof(Page), new FrameworkPropertyMetadata(Panel.BackgroundProperty.GetDefaultValue(typeof(Panel)), FrameworkPropertyMetadataOptions.None));

		// Token: 0x040036C0 RID: 14016
		public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(Page), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(Page.OnTitleChanged)));

		// Token: 0x040036C1 RID: 14017
		public static readonly DependencyProperty KeepAliveProperty = JournalEntry.KeepAliveProperty.AddOwner(typeof(Page));

		// Token: 0x040036C2 RID: 14018
		public static readonly DependencyProperty ForegroundProperty = TextElement.ForegroundProperty.AddOwner(typeof(Page));

		// Token: 0x040036C3 RID: 14019
		public static readonly DependencyProperty FontFamilyProperty = TextElement.FontFamilyProperty.AddOwner(typeof(Page));

		// Token: 0x040036C4 RID: 14020
		public static readonly DependencyProperty FontSizeProperty = TextElement.FontSizeProperty.AddOwner(typeof(Page));

		// Token: 0x040036C5 RID: 14021
		public static readonly DependencyProperty TemplateProperty = Control.TemplateProperty.AddOwner(typeof(Page), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(Page.OnTemplateChanged)));

		// Token: 0x040036C6 RID: 14022
		private IWindowService _currentIws;

		// Token: 0x040036C7 RID: 14023
		private PageHelperObject _pho;

		// Token: 0x040036C8 RID: 14024
		private SetPropertyFlags _setPropertyFlags;

		// Token: 0x040036C9 RID: 14025
		private bool _isTopLevel;

		// Token: 0x040036CA RID: 14026
		private ControlTemplate _templateCache;

		// Token: 0x040036CB RID: 14027
		private static DependencyObjectType _dType;
	}
}
