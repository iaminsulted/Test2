using System;
using System.Collections;
using System.ComponentModel;
using System.Text;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Documents.DocumentStructures;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Navigation;
using MS.Internal;
using MS.Internal.Utility;

namespace System.Windows.Documents
{
	// Token: 0x020005FD RID: 1533
	[ContentProperty("Children")]
	public sealed class FixedPage : FrameworkElement, IAddChildInternal, IAddChild, IFixedNavigate, IUriContext
	{
		// Token: 0x06004AB9 RID: 19129 RVA: 0x00234708 File Offset: 0x00233708
		static FixedPage()
		{
			FrameworkPropertyMetadata frameworkPropertyMetadata = new FrameworkPropertyMetadata(FlowDirection.LeftToRight, FrameworkPropertyMetadataOptions.AffectsParentArrange);
			frameworkPropertyMetadata.CoerceValueCallback = new CoerceValueCallback(FixedPage.CoerceFlowDirection);
			FrameworkElement.FlowDirectionProperty.OverrideMetadata(typeof(FixedPage), frameworkPropertyMetadata);
		}

		// Token: 0x06004ABA RID: 19130 RVA: 0x00234918 File Offset: 0x00233918
		public FixedPage()
		{
			this.Init();
		}

		// Token: 0x06004ABB RID: 19131 RVA: 0x00234926 File Offset: 0x00233926
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new FixedPageAutomationPeer(this);
		}

		// Token: 0x06004ABC RID: 19132 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		protected override void OnPreviewMouseWheel(MouseWheelEventArgs e)
		{
		}

		// Token: 0x06004ABD RID: 19133 RVA: 0x00234930 File Offset: 0x00233930
		protected override void OnRender(DrawingContext dc)
		{
			Brush background = this.Background;
			if (background != null)
			{
				dc.DrawRectangle(background, null, new Rect(0.0, 0.0, base.RenderSize.Width, base.RenderSize.Height));
			}
		}

		// Token: 0x06004ABE RID: 19134 RVA: 0x00234984 File Offset: 0x00233984
		void IAddChild.AddChild(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
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

		// Token: 0x06004ABF RID: 19135 RVA: 0x00175B1C File Offset: 0x00174B1C
		void IAddChild.AddText(string text)
		{
			XamlSerializerUtil.ThrowIfNonWhiteSpaceInAddText(text, this);
		}

		// Token: 0x06004AC0 RID: 19136 RVA: 0x002349E7 File Offset: 0x002339E7
		[AttachedPropertyBrowsableForChildren]
		[TypeConverter("System.Windows.LengthConverter, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, Custom=null")]
		public static double GetLeft(UIElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (double)element.GetValue(FixedPage.LeftProperty);
		}

		// Token: 0x06004AC1 RID: 19137 RVA: 0x00234A07 File Offset: 0x00233A07
		public static void SetLeft(UIElement element, double length)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(FixedPage.LeftProperty, length);
		}

		// Token: 0x06004AC2 RID: 19138 RVA: 0x00234A28 File Offset: 0x00233A28
		[TypeConverter("System.Windows.LengthConverter, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, Custom=null")]
		[AttachedPropertyBrowsableForChildren]
		public static double GetTop(UIElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (double)element.GetValue(FixedPage.TopProperty);
		}

		// Token: 0x06004AC3 RID: 19139 RVA: 0x00234A48 File Offset: 0x00233A48
		public static void SetTop(UIElement element, double length)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(FixedPage.TopProperty, length);
		}

		// Token: 0x06004AC4 RID: 19140 RVA: 0x00234A69 File Offset: 0x00233A69
		[TypeConverter("System.Windows.LengthConverter, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, Custom=null")]
		[AttachedPropertyBrowsableForChildren]
		public static double GetRight(UIElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (double)element.GetValue(FixedPage.RightProperty);
		}

		// Token: 0x06004AC5 RID: 19141 RVA: 0x00234A89 File Offset: 0x00233A89
		public static void SetRight(UIElement element, double length)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(FixedPage.RightProperty, length);
		}

		// Token: 0x06004AC6 RID: 19142 RVA: 0x00234AAA File Offset: 0x00233AAA
		[TypeConverter("System.Windows.LengthConverter, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, Custom=null")]
		[AttachedPropertyBrowsableForChildren]
		public static double GetBottom(UIElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (double)element.GetValue(FixedPage.BottomProperty);
		}

		// Token: 0x06004AC7 RID: 19143 RVA: 0x00234ACA File Offset: 0x00233ACA
		public static void SetBottom(UIElement element, double length)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(FixedPage.BottomProperty, length);
		}

		// Token: 0x06004AC8 RID: 19144 RVA: 0x00234AEB File Offset: 0x00233AEB
		[AttachedPropertyBrowsableForChildren]
		public static Uri GetNavigateUri(UIElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (Uri)element.GetValue(FixedPage.NavigateUriProperty);
		}

		// Token: 0x06004AC9 RID: 19145 RVA: 0x00234B0B File Offset: 0x00233B0B
		public static void SetNavigateUri(UIElement element, Uri uri)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(FixedPage.NavigateUriProperty, uri);
		}

		// Token: 0x1700111E RID: 4382
		// (get) Token: 0x06004ACA RID: 19146 RVA: 0x0022A4E3 File Offset: 0x002294E3
		// (set) Token: 0x06004ACB RID: 19147 RVA: 0x0022A4F5 File Offset: 0x002294F5
		Uri IUriContext.BaseUri
		{
			get
			{
				return (Uri)base.GetValue(BaseUriHelper.BaseUriProperty);
			}
			set
			{
				base.SetValue(BaseUriHelper.BaseUriProperty, value);
			}
		}

		// Token: 0x1700111F RID: 4383
		// (get) Token: 0x06004ACC RID: 19148 RVA: 0x00234B27 File Offset: 0x00233B27
		protected internal override IEnumerator LogicalChildren
		{
			get
			{
				return this.Children.GetEnumerator();
			}
		}

		// Token: 0x17001120 RID: 4384
		// (get) Token: 0x06004ACD RID: 19149 RVA: 0x00234B34 File Offset: 0x00233B34
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public UIElementCollection Children
		{
			get
			{
				if (this._uiElementCollection == null)
				{
					this._uiElementCollection = this.CreateUIElementCollection(this);
				}
				return this._uiElementCollection;
			}
		}

		// Token: 0x17001121 RID: 4385
		// (get) Token: 0x06004ACE RID: 19150 RVA: 0x00234B51 File Offset: 0x00233B51
		// (set) Token: 0x06004ACF RID: 19151 RVA: 0x00234B5E File Offset: 0x00233B5E
		public object PrintTicket
		{
			get
			{
				return base.GetValue(FixedPage.PrintTicketProperty);
			}
			set
			{
				base.SetValue(FixedPage.PrintTicketProperty, value);
			}
		}

		// Token: 0x17001122 RID: 4386
		// (get) Token: 0x06004AD0 RID: 19152 RVA: 0x00234B6C File Offset: 0x00233B6C
		// (set) Token: 0x06004AD1 RID: 19153 RVA: 0x00234B7E File Offset: 0x00233B7E
		public Brush Background
		{
			get
			{
				return (Brush)base.GetValue(FixedPage.BackgroundProperty);
			}
			set
			{
				base.SetValue(FixedPage.BackgroundProperty, value);
			}
		}

		// Token: 0x17001123 RID: 4387
		// (get) Token: 0x06004AD2 RID: 19154 RVA: 0x00234B8C File Offset: 0x00233B8C
		// (set) Token: 0x06004AD3 RID: 19155 RVA: 0x00234B9E File Offset: 0x00233B9E
		public Rect ContentBox
		{
			get
			{
				return (Rect)base.GetValue(FixedPage.ContentBoxProperty);
			}
			set
			{
				base.SetValue(FixedPage.ContentBoxProperty, value);
			}
		}

		// Token: 0x17001124 RID: 4388
		// (get) Token: 0x06004AD4 RID: 19156 RVA: 0x00234BB1 File Offset: 0x00233BB1
		// (set) Token: 0x06004AD5 RID: 19157 RVA: 0x00234BC3 File Offset: 0x00233BC3
		public Rect BleedBox
		{
			get
			{
				return (Rect)base.GetValue(FixedPage.BleedBoxProperty);
			}
			set
			{
				base.SetValue(FixedPage.BleedBoxProperty, value);
			}
		}

		// Token: 0x06004AD6 RID: 19158 RVA: 0x00234BD8 File Offset: 0x00233BD8
		protected internal override void OnVisualParentChanged(DependencyObject oldParent)
		{
			base.OnVisualParentChanged(oldParent);
			if (oldParent == null)
			{
				bool highlightVisual = HighlightVisual.GetHighlightVisual(this) != null;
				AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this);
				if (!highlightVisual && adornerLayer != null)
				{
					PageContent pageContent = LogicalTreeHelper.GetParent(this) as PageContent;
					if (pageContent != null)
					{
						FixedDocument fixedDocument = LogicalTreeHelper.GetParent(pageContent) as FixedDocument;
						if (fixedDocument != null && adornerLayer != null)
						{
							int zOrder = 1073741823;
							adornerLayer.Add(new HighlightVisual(fixedDocument, this), zOrder);
						}
					}
				}
			}
		}

		// Token: 0x06004AD7 RID: 19159 RVA: 0x00234C36 File Offset: 0x00233C36
		private static object CoerceFlowDirection(DependencyObject page, object flowDirection)
		{
			return FlowDirection.LeftToRight;
		}

		// Token: 0x06004AD8 RID: 19160 RVA: 0x00234C40 File Offset: 0x00233C40
		internal static Uri GetLinkUri(IInputElement element, Uri inputUri)
		{
			DependencyObject dependencyObject = element as DependencyObject;
			if (inputUri != null)
			{
				Uri uri = inputUri;
				if (!inputUri.IsAbsoluteUri)
				{
					uri = new Uri(new Uri("http://microsoft.com/"), inputUri);
				}
				string fragment = uri.Fragment;
				int num = (fragment == null) ? 0 : fragment.Length;
				if (num != 0)
				{
					string text = inputUri.ToString();
					inputUri = new Uri(text.Substring(0, text.IndexOf('#')), UriKind.RelativeOrAbsolute);
					if (!inputUri.IsAbsoluteUri)
					{
						string startPartUriString = FixedPage.GetStartPartUriString(dependencyObject);
						if (startPartUriString != null)
						{
							inputUri = new Uri(startPartUriString, UriKind.RelativeOrAbsolute);
						}
					}
				}
				Uri baseUri = BaseUriHelper.GetBaseUri(dependencyObject);
				Uri uri2 = BindUriHelper.GetUriToNavigate(dependencyObject, baseUri, inputUri);
				if (num != 0)
				{
					StringBuilder stringBuilder = new StringBuilder(uri2.ToString());
					stringBuilder.Append(fragment);
					uri2 = new Uri(stringBuilder.ToString(), UriKind.RelativeOrAbsolute);
				}
				return uri2;
			}
			return null;
		}

		// Token: 0x17001125 RID: 4389
		// (get) Token: 0x06004AD9 RID: 19161 RVA: 0x00234D08 File Offset: 0x00233D08
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

		// Token: 0x06004ADA RID: 19162 RVA: 0x00234D1F File Offset: 0x00233D1F
		protected override Visual GetVisualChild(int index)
		{
			if (this._uiElementCollection == null)
			{
				throw new ArgumentOutOfRangeException("index", index, SR.Get("Visual_ArgumentOutOfRange"));
			}
			return this._uiElementCollection[index];
		}

		// Token: 0x06004ADB RID: 19163 RVA: 0x00234D50 File Offset: 0x00233D50
		private UIElementCollection CreateUIElementCollection(FrameworkElement logicalParent)
		{
			return new UIElementCollection(this, logicalParent);
		}

		// Token: 0x06004ADC RID: 19164 RVA: 0x00234D5C File Offset: 0x00233D5C
		protected override Size MeasureOverride(Size constraint)
		{
			Size availableSize = new Size(double.PositiveInfinity, double.PositiveInfinity);
			foreach (object obj in this.Children)
			{
				((UIElement)obj).Measure(availableSize);
			}
			return default(Size);
		}

		// Token: 0x06004ADD RID: 19165 RVA: 0x00234DD8 File Offset: 0x00233DD8
		protected override Size ArrangeOverride(Size arrangeSize)
		{
			foreach (object obj in this.Children)
			{
				UIElement uielement = (UIElement)obj;
				double x = 0.0;
				double y = 0.0;
				double left = FixedPage.GetLeft(uielement);
				if (!DoubleUtil.IsNaN(left))
				{
					x = left;
				}
				else
				{
					double right = FixedPage.GetRight(uielement);
					if (!DoubleUtil.IsNaN(right))
					{
						x = arrangeSize.Width - uielement.DesiredSize.Width - right;
					}
				}
				double top = FixedPage.GetTop(uielement);
				if (!DoubleUtil.IsNaN(top))
				{
					y = top;
				}
				else
				{
					double bottom = FixedPage.GetBottom(uielement);
					if (!DoubleUtil.IsNaN(bottom))
					{
						y = arrangeSize.Height - uielement.DesiredSize.Height - bottom;
					}
				}
				uielement.Arrange(new Rect(new Point(x, y), uielement.DesiredSize));
			}
			return arrangeSize;
		}

		// Token: 0x06004ADE RID: 19166 RVA: 0x00234EE4 File Offset: 0x00233EE4
		void IFixedNavigate.NavigateAsync(string elementID)
		{
			FixedHyperLink.NavigateToElement(this, elementID);
		}

		// Token: 0x06004ADF RID: 19167 RVA: 0x00234EF0 File Offset: 0x00233EF0
		UIElement IFixedNavigate.FindElementByID(string elementID, out FixedPage rootFixedPage)
		{
			UIElement result = null;
			rootFixedPage = this;
			UIElementCollection children = this.Children;
			int i = 0;
			int count = children.Count;
			while (i < count)
			{
				DependencyObject dependencyObject = LogicalTreeHelper.FindLogicalNode(children[i], elementID);
				if (dependencyObject != null)
				{
					result = (dependencyObject as UIElement);
					break;
				}
				i++;
			}
			return result;
		}

		// Token: 0x06004AE0 RID: 19168 RVA: 0x00234F39 File Offset: 0x00233F39
		internal FixedNode CreateFixedNode(int pageIndex, UIElement e)
		{
			return this._CreateFixedNode(pageIndex, e);
		}

		// Token: 0x06004AE1 RID: 19169 RVA: 0x00234F43 File Offset: 0x00233F43
		internal Glyphs GetGlyphsElement(FixedNode node)
		{
			return this.GetElement(node) as Glyphs;
		}

		// Token: 0x06004AE2 RID: 19170 RVA: 0x00234F54 File Offset: 0x00233F54
		internal DependencyObject GetElement(FixedNode node)
		{
			int num = node[1];
			if (num < 0 || num > this.Children.Count)
			{
				return null;
			}
			DependencyObject dependencyObject = this.Children[num];
			for (int i = 2; i <= node.ChildLevels; i++)
			{
				num = node[i];
				if (dependencyObject is Canvas)
				{
					dependencyObject = ((Canvas)dependencyObject).Children[num];
				}
				else
				{
					IEnumerable children = LogicalTreeHelper.GetChildren(dependencyObject);
					if (children == null)
					{
						return null;
					}
					int num2 = -1;
					IEnumerator enumerator = children.GetEnumerator();
					while (enumerator.MoveNext())
					{
						num2++;
						if (num2 == num)
						{
							dependencyObject = (DependencyObject)enumerator.Current;
							break;
						}
					}
				}
			}
			return dependencyObject;
		}

		// Token: 0x17001126 RID: 4390
		// (get) Token: 0x06004AE3 RID: 19171 RVA: 0x00234FFF File Offset: 0x00233FFF
		// (set) Token: 0x06004AE4 RID: 19172 RVA: 0x00235007 File Offset: 0x00234007
		internal string StartPartUriString
		{
			get
			{
				return this._startPartUriString;
			}
			set
			{
				this._startPartUriString = value;
			}
		}

		// Token: 0x06004AE5 RID: 19173 RVA: 0x00235010 File Offset: 0x00234010
		private void Init()
		{
			if (XpsValidatingLoader.DocumentMode)
			{
				base.InheritanceBehavior = InheritanceBehavior.SkipAllNext;
			}
		}

		// Token: 0x06004AE6 RID: 19174 RVA: 0x00235020 File Offset: 0x00234020
		internal StoryFragments GetPageStructure()
		{
			return FixedDocument.GetStoryFragments(this);
		}

		// Token: 0x06004AE7 RID: 19175 RVA: 0x00235028 File Offset: 0x00234028
		internal int[] _CreateChildIndex(DependencyObject e)
		{
			ArrayList arrayList = new ArrayList();
			while (e != this)
			{
				DependencyObject parent = LogicalTreeHelper.GetParent(e);
				int num = -1;
				if (parent is FixedPage)
				{
					num = ((FixedPage)parent).Children.IndexOf((UIElement)e);
				}
				else if (parent is Canvas)
				{
					num = ((Canvas)parent).Children.IndexOf((UIElement)e);
				}
				else
				{
					IEnumerator enumerator = LogicalTreeHelper.GetChildren(parent).GetEnumerator();
					while (enumerator.MoveNext())
					{
						num++;
						if (enumerator.Current == e)
						{
							break;
						}
					}
				}
				arrayList.Insert(0, num);
				e = parent;
			}
			while (e != this)
			{
			}
			return (int[])arrayList.ToArray(typeof(int));
		}

		// Token: 0x06004AE8 RID: 19176 RVA: 0x002350DB File Offset: 0x002340DB
		private FixedNode _CreateFixedNode(int pageIndex, UIElement e)
		{
			return FixedNode.Create(pageIndex, this._CreateChildIndex(e));
		}

		// Token: 0x06004AE9 RID: 19177 RVA: 0x002350EC File Offset: 0x002340EC
		private static string GetStartPartUriString(DependencyObject current)
		{
			DependencyObject dependencyObject = current;
			FixedPage fixedPage = current as FixedPage;
			while (fixedPage == null && dependencyObject != null)
			{
				dependencyObject = dependencyObject.InheritanceParent;
				fixedPage = (dependencyObject as FixedPage);
			}
			if (fixedPage == null)
			{
				return null;
			}
			if (fixedPage.StartPartUriString == null)
			{
				DependencyObject parent = LogicalTreeHelper.GetParent(current);
				while (parent != null)
				{
					FixedDocumentSequence fixedDocumentSequence = parent as FixedDocumentSequence;
					if (fixedDocumentSequence != null)
					{
						Uri baseUri = ((IUriContext)fixedDocumentSequence).BaseUri;
						if (!(baseUri != null))
						{
							break;
						}
						string text = baseUri.ToString();
						string fragment = baseUri.Fragment;
						if (fragment != null && fragment.Length != 0)
						{
							fixedPage.StartPartUriString = text.Substring(0, text.IndexOf('#'));
							break;
						}
						fixedPage.StartPartUriString = baseUri.ToString();
						break;
					}
					else
					{
						parent = LogicalTreeHelper.GetParent(parent);
					}
				}
				if (fixedPage.StartPartUriString == null)
				{
					fixedPage.StartPartUriString = string.Empty;
				}
			}
			if (fixedPage.StartPartUriString == string.Empty)
			{
				return null;
			}
			return fixedPage.StartPartUriString;
		}

		// Token: 0x04002737 RID: 10039
		public static readonly DependencyProperty PrintTicketProperty = DependencyProperty.RegisterAttached("PrintTicket", typeof(object), typeof(FixedPage), new FrameworkPropertyMetadata(null));

		// Token: 0x04002738 RID: 10040
		public static readonly DependencyProperty BackgroundProperty = Panel.BackgroundProperty.AddOwner(typeof(FixedPage), new FrameworkPropertyMetadata(Brushes.White, FrameworkPropertyMetadataOptions.AffectsRender));

		// Token: 0x04002739 RID: 10041
		public static readonly DependencyProperty LeftProperty = DependencyProperty.RegisterAttached("Left", typeof(double), typeof(FixedPage), new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsParentArrange));

		// Token: 0x0400273A RID: 10042
		public static readonly DependencyProperty TopProperty = DependencyProperty.RegisterAttached("Top", typeof(double), typeof(FixedPage), new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsParentArrange));

		// Token: 0x0400273B RID: 10043
		public static readonly DependencyProperty RightProperty = DependencyProperty.RegisterAttached("Right", typeof(double), typeof(FixedPage), new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsParentArrange));

		// Token: 0x0400273C RID: 10044
		public static readonly DependencyProperty BottomProperty = DependencyProperty.RegisterAttached("Bottom", typeof(double), typeof(FixedPage), new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsParentArrange));

		// Token: 0x0400273D RID: 10045
		public static readonly DependencyProperty ContentBoxProperty = DependencyProperty.Register("ContentBox", typeof(Rect), typeof(FixedPage), new FrameworkPropertyMetadata(Rect.Empty));

		// Token: 0x0400273E RID: 10046
		public static readonly DependencyProperty BleedBoxProperty = DependencyProperty.Register("BleedBox", typeof(Rect), typeof(FixedPage), new FrameworkPropertyMetadata(Rect.Empty));

		// Token: 0x0400273F RID: 10047
		public static readonly DependencyProperty NavigateUriProperty = DependencyProperty.RegisterAttached("NavigateUri", typeof(Uri), typeof(FixedPage), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(Hyperlink.OnNavigateUriChanged), new CoerceValueCallback(Hyperlink.CoerceNavigateUri)));

		// Token: 0x04002740 RID: 10048
		private string _startPartUriString;

		// Token: 0x04002741 RID: 10049
		private UIElementCollection _uiElementCollection;
	}
}
