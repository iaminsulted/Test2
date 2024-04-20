using System;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace System.Windows.Documents
{
	// Token: 0x020005F3 RID: 1523
	internal sealed class FixedElement : DependencyObject
	{
		// Token: 0x06004A6C RID: 19052 RVA: 0x00233193 File Offset: 0x00232193
		internal FixedElement(FixedElement.ElementType type, FixedTextPointer start, FixedTextPointer end, int pageIndex)
		{
			this._type = type;
			this._start = start;
			this._end = end;
			this._pageIndex = pageIndex;
		}

		// Token: 0x06004A6D RID: 19053 RVA: 0x002331B8 File Offset: 0x002321B8
		internal void Append(FixedElement e)
		{
			if (this._type == FixedElement.ElementType.InlineUIContainer)
			{
				this._object = e._object;
			}
		}

		// Token: 0x06004A6E RID: 19054 RVA: 0x002331D0 File Offset: 0x002321D0
		internal object GetObject()
		{
			if (this._type == FixedElement.ElementType.Hyperlink || this._type == FixedElement.ElementType.Paragraph || (this._type >= FixedElement.ElementType.Table && this._type <= FixedElement.ElementType.TableCell))
			{
				if (this._object == null)
				{
					this._object = this.BuildObjectTree();
				}
				return this._object;
			}
			if (this._type != FixedElement.ElementType.Object && this._type != FixedElement.ElementType.InlineUIContainer)
			{
				return null;
			}
			Image image = this.GetImage();
			object result = image;
			if (this._type == FixedElement.ElementType.InlineUIContainer)
			{
				result = new InlineUIContainer
				{
					Child = image
				};
			}
			return result;
		}

		// Token: 0x06004A6F RID: 19055 RVA: 0x00233254 File Offset: 0x00232254
		internal object BuildObjectTree()
		{
			FixedElement.ElementType type = this._type;
			IAddChild addChild;
			if (type != FixedElement.ElementType.Paragraph)
			{
				switch (type)
				{
				case FixedElement.ElementType.Table:
					addChild = new Table();
					goto IL_C2;
				case FixedElement.ElementType.TableRowGroup:
					addChild = new TableRowGroup();
					goto IL_C2;
				case FixedElement.ElementType.TableRow:
					addChild = new TableRow();
					goto IL_C2;
				case FixedElement.ElementType.TableCell:
					addChild = new TableCell();
					goto IL_C2;
				case FixedElement.ElementType.Hyperlink:
				{
					Hyperlink hyperlink = new Hyperlink();
					hyperlink.NavigateUri = (base.GetValue(FixedElement.NavigateUriProperty) as Uri);
					hyperlink.RequestNavigate += this.ClickHyperlink;
					AutomationProperties.SetHelpText(hyperlink, (string)base.GetValue(FixedElement.HelpTextProperty));
					AutomationProperties.SetName(hyperlink, (string)base.GetValue(FixedElement.NameProperty));
					addChild = hyperlink;
					goto IL_C2;
				}
				}
				addChild = null;
			}
			else
			{
				addChild = new Paragraph();
			}
			IL_C2:
			ITextPointer textPointer = ((ITextPointer)this._start).CreatePointer();
			while (textPointer.CompareTo(this._end) < 0)
			{
				TextPointerContext pointerContext = textPointer.GetPointerContext(LogicalDirection.Forward);
				if (pointerContext == TextPointerContext.Text)
				{
					addChild.AddText(textPointer.GetTextInRun(LogicalDirection.Forward));
				}
				else if (pointerContext == TextPointerContext.EmbeddedElement)
				{
					addChild.AddChild(textPointer.GetAdjacentElement(LogicalDirection.Forward));
				}
				else if (pointerContext == TextPointerContext.ElementStart)
				{
					object adjacentElement = textPointer.GetAdjacentElement(LogicalDirection.Forward);
					if (adjacentElement != null)
					{
						addChild.AddChild(adjacentElement);
						textPointer.MoveToNextContextPosition(LogicalDirection.Forward);
						textPointer.MoveToElementEdge(ElementEdge.BeforeEnd);
					}
				}
				textPointer.MoveToNextContextPosition(LogicalDirection.Forward);
			}
			return addChild;
		}

		// Token: 0x06004A70 RID: 19056 RVA: 0x002333A0 File Offset: 0x002323A0
		private Image GetImage()
		{
			Image image = null;
			Uri uri = this._object as Uri;
			if (uri != null)
			{
				image = new Image();
				image.Source = new BitmapImage(uri);
				image.Width = image.Source.Width;
				image.Height = image.Source.Height;
				AutomationProperties.SetName(image, (string)base.GetValue(FixedElement.NameProperty));
				AutomationProperties.SetHelpText(image, (string)base.GetValue(FixedElement.HelpTextProperty));
			}
			return image;
		}

		// Token: 0x06004A71 RID: 19057 RVA: 0x00233428 File Offset: 0x00232428
		private void ClickHyperlink(object sender, RequestNavigateEventArgs args)
		{
			FixedDocument fixedDocument = this._start.FixedTextContainer.FixedDocument;
			int pageNumber = fixedDocument.GetPageNumber(this._start);
			Hyperlink.RaiseNavigate(fixedDocument.SyncGetPage(pageNumber, false), args.Uri, null);
		}

		// Token: 0x17001106 RID: 4358
		// (get) Token: 0x06004A72 RID: 19058 RVA: 0x00233465 File Offset: 0x00232465
		internal bool IsTextElement
		{
			get
			{
				return this._type != FixedElement.ElementType.Object && this._type != FixedElement.ElementType.Container;
			}
		}

		// Token: 0x17001107 RID: 4359
		// (get) Token: 0x06004A73 RID: 19059 RVA: 0x00233480 File Offset: 0x00232480
		internal Type Type
		{
			get
			{
				switch (this._type)
				{
				case FixedElement.ElementType.Paragraph:
					return typeof(Paragraph);
				case FixedElement.ElementType.Inline:
					return typeof(Inline);
				case FixedElement.ElementType.Run:
					return typeof(Run);
				case FixedElement.ElementType.Span:
					return typeof(Span);
				case FixedElement.ElementType.Bold:
					return typeof(Bold);
				case FixedElement.ElementType.Italic:
					return typeof(Italic);
				case FixedElement.ElementType.Underline:
					return typeof(Underline);
				case FixedElement.ElementType.Object:
					return typeof(object);
				case FixedElement.ElementType.Section:
					return typeof(Section);
				case FixedElement.ElementType.Figure:
					return typeof(Figure);
				case FixedElement.ElementType.Table:
					return typeof(Table);
				case FixedElement.ElementType.TableRowGroup:
					return typeof(TableRowGroup);
				case FixedElement.ElementType.TableRow:
					return typeof(TableRow);
				case FixedElement.ElementType.TableCell:
					return typeof(TableCell);
				case FixedElement.ElementType.List:
					return typeof(List);
				case FixedElement.ElementType.ListItem:
					return typeof(ListItem);
				case FixedElement.ElementType.Hyperlink:
					return typeof(Hyperlink);
				case FixedElement.ElementType.InlineUIContainer:
					return typeof(InlineUIContainer);
				}
				return typeof(object);
			}
		}

		// Token: 0x17001108 RID: 4360
		// (get) Token: 0x06004A74 RID: 19060 RVA: 0x002335C3 File Offset: 0x002325C3
		internal FixedTextPointer Start
		{
			get
			{
				return this._start;
			}
		}

		// Token: 0x17001109 RID: 4361
		// (get) Token: 0x06004A75 RID: 19061 RVA: 0x002335CB File Offset: 0x002325CB
		internal FixedTextPointer End
		{
			get
			{
				return this._end;
			}
		}

		// Token: 0x1700110A RID: 4362
		// (get) Token: 0x06004A76 RID: 19062 RVA: 0x002335D3 File Offset: 0x002325D3
		internal int PageIndex
		{
			get
			{
				return this._pageIndex;
			}
		}

		// Token: 0x1700110B RID: 4363
		// (set) Token: 0x06004A77 RID: 19063 RVA: 0x002335DB File Offset: 0x002325DB
		internal object Object
		{
			set
			{
				this._object = value;
			}
		}

		// Token: 0x040026FE RID: 9982
		public static readonly DependencyProperty LanguageProperty = FrameworkElement.LanguageProperty.AddOwner(typeof(FixedElement));

		// Token: 0x040026FF RID: 9983
		public static readonly DependencyProperty FontFamilyProperty = TextElement.FontFamilyProperty.AddOwner(typeof(FixedElement));

		// Token: 0x04002700 RID: 9984
		public static readonly DependencyProperty FontStyleProperty = TextElement.FontStyleProperty.AddOwner(typeof(FixedElement));

		// Token: 0x04002701 RID: 9985
		public static readonly DependencyProperty FontWeightProperty = TextElement.FontWeightProperty.AddOwner(typeof(FixedElement));

		// Token: 0x04002702 RID: 9986
		public static readonly DependencyProperty FontStretchProperty = TextElement.FontStretchProperty.AddOwner(typeof(FixedElement));

		// Token: 0x04002703 RID: 9987
		public static readonly DependencyProperty FontSizeProperty = TextElement.FontSizeProperty.AddOwner(typeof(FixedElement));

		// Token: 0x04002704 RID: 9988
		public static readonly DependencyProperty ForegroundProperty = TextElement.ForegroundProperty.AddOwner(typeof(FixedElement));

		// Token: 0x04002705 RID: 9989
		public static readonly DependencyProperty FlowDirectionProperty = FrameworkElement.FlowDirectionProperty.AddOwner(typeof(FixedElement));

		// Token: 0x04002706 RID: 9990
		public static readonly DependencyProperty CellSpacingProperty = Table.CellSpacingProperty.AddOwner(typeof(FixedElement));

		// Token: 0x04002707 RID: 9991
		public static readonly DependencyProperty BorderThicknessProperty = Block.BorderThicknessProperty.AddOwner(typeof(FixedElement));

		// Token: 0x04002708 RID: 9992
		public static readonly DependencyProperty BorderBrushProperty = Block.BorderBrushProperty.AddOwner(typeof(FixedElement));

		// Token: 0x04002709 RID: 9993
		public static readonly DependencyProperty ColumnSpanProperty = TableCell.ColumnSpanProperty.AddOwner(typeof(FixedElement));

		// Token: 0x0400270A RID: 9994
		public static readonly DependencyProperty NavigateUriProperty = Hyperlink.NavigateUriProperty.AddOwner(typeof(FixedElement));

		// Token: 0x0400270B RID: 9995
		public static readonly DependencyProperty NameProperty = AutomationProperties.NameProperty.AddOwner(typeof(FixedElement));

		// Token: 0x0400270C RID: 9996
		public static readonly DependencyProperty HelpTextProperty = AutomationProperties.HelpTextProperty.AddOwner(typeof(FixedElement));

		// Token: 0x0400270D RID: 9997
		private FixedElement.ElementType _type;

		// Token: 0x0400270E RID: 9998
		private FixedTextPointer _start;

		// Token: 0x0400270F RID: 9999
		private FixedTextPointer _end;

		// Token: 0x04002710 RID: 10000
		private object _object;

		// Token: 0x04002711 RID: 10001
		private int _pageIndex;

		// Token: 0x02000B34 RID: 2868
		internal enum ElementType
		{
			// Token: 0x04004805 RID: 18437
			Paragraph,
			// Token: 0x04004806 RID: 18438
			Inline,
			// Token: 0x04004807 RID: 18439
			Run,
			// Token: 0x04004808 RID: 18440
			Span,
			// Token: 0x04004809 RID: 18441
			Bold,
			// Token: 0x0400480A RID: 18442
			Italic,
			// Token: 0x0400480B RID: 18443
			Underline,
			// Token: 0x0400480C RID: 18444
			Object,
			// Token: 0x0400480D RID: 18445
			Container,
			// Token: 0x0400480E RID: 18446
			Section,
			// Token: 0x0400480F RID: 18447
			Figure,
			// Token: 0x04004810 RID: 18448
			Table,
			// Token: 0x04004811 RID: 18449
			TableRowGroup,
			// Token: 0x04004812 RID: 18450
			TableRow,
			// Token: 0x04004813 RID: 18451
			TableCell,
			// Token: 0x04004814 RID: 18452
			List,
			// Token: 0x04004815 RID: 18453
			ListItem,
			// Token: 0x04004816 RID: 18454
			Header,
			// Token: 0x04004817 RID: 18455
			Footer,
			// Token: 0x04004818 RID: 18456
			Hyperlink,
			// Token: 0x04004819 RID: 18457
			InlineUIContainer
		}
	}
}
