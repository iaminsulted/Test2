using System;
using System.Windows.Markup;

namespace System.Windows.Documents
{
	// Token: 0x020005DC RID: 1500
	[ContentProperty("Child")]
	public class BlockUIContainer : Block
	{
		// Token: 0x06004881 RID: 18561 RVA: 0x0022C811 File Offset: 0x0022B811
		public BlockUIContainer()
		{
		}

		// Token: 0x06004882 RID: 18562 RVA: 0x0022C819 File Offset: 0x0022B819
		public BlockUIContainer(UIElement uiElement)
		{
			if (uiElement == null)
			{
				throw new ArgumentNullException("uiElement");
			}
			this.Child = uiElement;
		}

		// Token: 0x17001049 RID: 4169
		// (get) Token: 0x06004883 RID: 18563 RVA: 0x0022C836 File Offset: 0x0022B836
		// (set) Token: 0x06004884 RID: 18564 RVA: 0x0022C84C File Offset: 0x0022B84C
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
	}
}
