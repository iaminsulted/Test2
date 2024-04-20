using System;
using System.Windows.Markup;
using MS.Internal.Documents;

namespace System.Windows.Documents
{
	// Token: 0x0200062A RID: 1578
	[ContentProperty("Child")]
	[TextElementEditingBehavior(IsMergeable = false)]
	public class InlineUIContainer : Inline
	{
		// Token: 0x06004E10 RID: 19984 RVA: 0x00243005 File Offset: 0x00242005
		public InlineUIContainer()
		{
		}

		// Token: 0x06004E11 RID: 19985 RVA: 0x0024300D File Offset: 0x0024200D
		public InlineUIContainer(UIElement childUIElement) : this(childUIElement, null)
		{
		}

		// Token: 0x06004E12 RID: 19986 RVA: 0x00243018 File Offset: 0x00242018
		public InlineUIContainer(UIElement childUIElement, TextPointer insertionPosition)
		{
			if (insertionPosition != null)
			{
				insertionPosition.TextContainer.BeginChange();
			}
			try
			{
				if (insertionPosition != null)
				{
					insertionPosition.InsertInline(this);
				}
				this.Child = childUIElement;
			}
			finally
			{
				if (insertionPosition != null)
				{
					insertionPosition.TextContainer.EndChange();
				}
			}
		}

		// Token: 0x17001217 RID: 4631
		// (get) Token: 0x06004E13 RID: 19987 RVA: 0x0022C836 File Offset: 0x0022B836
		// (set) Token: 0x06004E14 RID: 19988 RVA: 0x0024306C File Offset: 0x0024206C
		public UIElement Child
		{
			get
			{
				return base.ContentStart.GetAdjacentElement(LogicalDirection.Forward) as UIElement;
			}
			set
			{
				TextContainer textContainer = base.TextContainer;
				textContainer.BeginChange();
				try
				{
					TextPointer contentStart = base.ContentStart;
					UIElement child = this.Child;
					if (child != null)
					{
						textContainer.DeleteContentInternal(contentStart, base.ContentEnd);
						TextElement.ContainerTextElementField.ClearValue(child);
					}
					if (value != null)
					{
						TextElement.ContainerTextElementField.SetValue(value, this);
						contentStart.InsertUIElement(value);
					}
				}
				finally
				{
					textContainer.EndChange();
				}
			}
		}

		// Token: 0x17001218 RID: 4632
		// (get) Token: 0x06004E15 RID: 19989 RVA: 0x002430E0 File Offset: 0x002420E0
		internal UIElementIsland UIElementIsland
		{
			get
			{
				this.UpdateUIElementIsland();
				return this._uiElementIsland;
			}
		}

		// Token: 0x06004E16 RID: 19990 RVA: 0x002430F0 File Offset: 0x002420F0
		private void UpdateUIElementIsland()
		{
			UIElement child = this.Child;
			if (this._uiElementIsland == null || this._uiElementIsland.Root != child)
			{
				if (this._uiElementIsland != null)
				{
					this._uiElementIsland.Dispose();
					this._uiElementIsland = null;
				}
				if (child != null)
				{
					this._uiElementIsland = new UIElementIsland(child);
				}
			}
		}

		// Token: 0x04002835 RID: 10293
		private UIElementIsland _uiElementIsland;
	}
}
