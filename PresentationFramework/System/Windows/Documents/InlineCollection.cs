using System;
using System.Collections;
using System.Windows.Controls;
using System.Windows.Markup;

namespace System.Windows.Documents
{
	// Token: 0x02000629 RID: 1577
	[ContentWrapper(typeof(Run))]
	[ContentWrapper(typeof(InlineUIContainer))]
	[WhitespaceSignificantCollection]
	public class InlineCollection : TextElementCollection<Inline>, IList, ICollection, IEnumerable
	{
		// Token: 0x06004E07 RID: 19975 RVA: 0x00242DF1 File Offset: 0x00241DF1
		internal InlineCollection(DependencyObject owner, bool isOwnerParent) : base(owner, isOwnerParent)
		{
		}

		// Token: 0x06004E08 RID: 19976 RVA: 0x00242DFC File Offset: 0x00241DFC
		internal override int OnAdd(object value)
		{
			string text = value as string;
			int result;
			if (text != null)
			{
				result = this.AddText(text, true);
			}
			else
			{
				base.TextContainer.BeginChange();
				try
				{
					UIElement uielement = value as UIElement;
					if (uielement != null)
					{
						result = this.AddUIElement(uielement, true);
					}
					else
					{
						result = base.OnAdd(value);
					}
				}
				finally
				{
					base.TextContainer.EndChange();
				}
			}
			return result;
		}

		// Token: 0x06004E09 RID: 19977 RVA: 0x00242E68 File Offset: 0x00241E68
		public void Add(string text)
		{
			this.AddText(text, false);
		}

		// Token: 0x06004E0A RID: 19978 RVA: 0x00242E73 File Offset: 0x00241E73
		public void Add(UIElement uiElement)
		{
			this.AddUIElement(uiElement, false);
		}

		// Token: 0x17001215 RID: 4629
		// (get) Token: 0x06004E0B RID: 19979 RVA: 0x00242E7E File Offset: 0x00241E7E
		public Inline FirstInline
		{
			get
			{
				return base.FirstChild;
			}
		}

		// Token: 0x17001216 RID: 4630
		// (get) Token: 0x06004E0C RID: 19980 RVA: 0x00242E86 File Offset: 0x00241E86
		public Inline LastInline
		{
			get
			{
				return base.LastChild;
			}
		}

		// Token: 0x06004E0D RID: 19981 RVA: 0x00242E90 File Offset: 0x00241E90
		internal override void ValidateChild(Inline child)
		{
			base.ValidateChild(child);
			if (base.Parent is TextElement)
			{
				TextSchema.ValidateChild((TextElement)base.Parent, child, true, true);
				return;
			}
			if (!TextSchema.IsValidChildOfContainer(base.Parent.GetType(), child.GetType()))
			{
				throw new InvalidOperationException(SR.Get("TextSchema_ChildTypeIsInvalid", new object[]
				{
					base.Parent.GetType().Name,
					child.GetType().Name
				}));
			}
		}

		// Token: 0x06004E0E RID: 19982 RVA: 0x00242F18 File Offset: 0x00241F18
		private int AddText(string text, bool returnIndex)
		{
			if (text == null)
			{
				throw new ArgumentNullException("text");
			}
			if (base.Parent is TextBlock)
			{
				TextBlock textBlock = (TextBlock)base.Parent;
				if (!textBlock.HasComplexContent)
				{
					textBlock.Text += text;
					return 0;
				}
			}
			base.TextContainer.BeginChange();
			int result;
			try
			{
				Run run = Inline.CreateImplicitRun(base.Parent);
				int num;
				if (returnIndex)
				{
					num = base.OnAdd(run);
				}
				else
				{
					base.Add(run);
					num = -1;
				}
				run.Text = text;
				result = num;
			}
			finally
			{
				base.TextContainer.EndChange();
			}
			return result;
		}

		// Token: 0x06004E0F RID: 19983 RVA: 0x00242FC0 File Offset: 0x00241FC0
		private int AddUIElement(UIElement uiElement, bool returnIndex)
		{
			if (uiElement == null)
			{
				throw new ArgumentNullException("uiElement");
			}
			InlineUIContainer inlineUIContainer = Inline.CreateImplicitInlineUIContainer(base.Parent);
			int result;
			if (returnIndex)
			{
				result = base.OnAdd(inlineUIContainer);
			}
			else
			{
				base.Add(inlineUIContainer);
				result = -1;
			}
			inlineUIContainer.Child = uiElement;
			return result;
		}
	}
}
