using System;
using System.Windows;
using MS.Internal.Documents;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000118 RID: 280
	internal sealed class InlineObject : EmbeddedObject
	{
		// Token: 0x06000738 RID: 1848 RVA: 0x0010CC83 File Offset: 0x0010BC83
		internal InlineObject(int dcp, UIElementIsland uiElementIsland, TextParagraph para) : base(dcp)
		{
			this._para = para;
			this._uiElementIsland = uiElementIsland;
			this._uiElementIsland.DesiredSizeChanged += this._para.OnUIElementDesiredSizeChanged;
		}

		// Token: 0x06000739 RID: 1849 RVA: 0x0010CCB6 File Offset: 0x0010BCB6
		internal override void Dispose()
		{
			if (this._uiElementIsland != null)
			{
				this._uiElementIsland.DesiredSizeChanged -= this._para.OnUIElementDesiredSizeChanged;
			}
			base.Dispose();
		}

		// Token: 0x0600073A RID: 1850 RVA: 0x0010CCE2 File Offset: 0x0010BCE2
		internal override void Update(EmbeddedObject newObject)
		{
			InlineObject inlineObject = newObject as InlineObject;
			ErrorHandler.Assert(inlineObject != null, ErrorHandler.EmbeddedObjectTypeMismatch);
			ErrorHandler.Assert(inlineObject._uiElementIsland == this._uiElementIsland, ErrorHandler.EmbeddedObjectOwnerMismatch);
			inlineObject._uiElementIsland = null;
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x0600073B RID: 1851 RVA: 0x0010CD16 File Offset: 0x0010BD16
		internal override DependencyObject Element
		{
			get
			{
				return this._uiElementIsland.Root;
			}
		}

		// Token: 0x04000748 RID: 1864
		private UIElementIsland _uiElementIsland;

		// Token: 0x04000749 RID: 1865
		private TextParagraph _para;
	}
}
