using System;
using MS.Internal;
using MS.Internal.Documents;

namespace System.Windows.Documents
{
	// Token: 0x020006D9 RID: 1753
	internal class UIElementPropertyUndoUnit : IUndoUnit
	{
		// Token: 0x06005C57 RID: 23639 RVA: 0x0028634C File Offset: 0x0028534C
		private UIElementPropertyUndoUnit(UIElement uiElement, DependencyProperty property, object oldValue)
		{
			this._uiElement = uiElement;
			this._property = property;
			this._oldValue = oldValue;
		}

		// Token: 0x06005C58 RID: 23640 RVA: 0x00286369 File Offset: 0x00285369
		public void Do()
		{
			if (this._oldValue != DependencyProperty.UnsetValue)
			{
				this._uiElement.SetValue(this._property, this._oldValue);
				return;
			}
			this._uiElement.ClearValue(this._property);
		}

		// Token: 0x06005C59 RID: 23641 RVA: 0x00142B2E File Offset: 0x00141B2E
		public bool Merge(IUndoUnit unit)
		{
			Invariant.Assert(unit != null);
			return false;
		}

		// Token: 0x06005C5A RID: 23642 RVA: 0x002863A1 File Offset: 0x002853A1
		internal static void Add(ITextContainer textContainer, UIElement uiElement, DependencyProperty property, HorizontalAlignment newValue)
		{
			UIElementPropertyUndoUnit.AddPrivate(textContainer, uiElement, property, newValue);
		}

		// Token: 0x06005C5B RID: 23643 RVA: 0x002863B1 File Offset: 0x002853B1
		internal static void Add(ITextContainer textContainer, UIElement uiElement, DependencyProperty property, FlowDirection newValue)
		{
			UIElementPropertyUndoUnit.AddPrivate(textContainer, uiElement, property, newValue);
		}

		// Token: 0x06005C5C RID: 23644 RVA: 0x002863C4 File Offset: 0x002853C4
		private static void AddPrivate(ITextContainer textContainer, UIElement uiElement, DependencyProperty property, object newValue)
		{
			UndoManager orClearUndoManager = TextTreeUndo.GetOrClearUndoManager(textContainer);
			if (orClearUndoManager == null)
			{
				return;
			}
			object obj = uiElement.ReadLocalValue(property);
			if (obj is Expression)
			{
				if (orClearUndoManager.IsEnabled)
				{
					orClearUndoManager.Clear();
				}
				return;
			}
			if (obj.Equals(newValue))
			{
				return;
			}
			orClearUndoManager.Add(new UIElementPropertyUndoUnit(uiElement, property, obj));
		}

		// Token: 0x040030C7 RID: 12487
		private readonly UIElement _uiElement;

		// Token: 0x040030C8 RID: 12488
		private readonly DependencyProperty _property;

		// Token: 0x040030C9 RID: 12489
		private readonly object _oldValue;
	}
}
