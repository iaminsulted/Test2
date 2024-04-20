using System;
using System.Windows;

namespace MS.Internal.Annotations.Component
{
	// Token: 0x020002CE RID: 718
	internal abstract class PresentationContext
	{
		// Token: 0x17000503 RID: 1283
		// (get) Token: 0x06001B06 RID: 6918
		public abstract UIElement Host { get; }

		// Token: 0x17000504 RID: 1284
		// (get) Token: 0x06001B07 RID: 6919
		public abstract PresentationContext EnclosingContext { get; }

		// Token: 0x06001B08 RID: 6920
		public abstract void AddToHost(IAnnotationComponent component);

		// Token: 0x06001B09 RID: 6921
		public abstract void RemoveFromHost(IAnnotationComponent component, bool reorder);

		// Token: 0x06001B0A RID: 6922
		public abstract void InvalidateTransform(IAnnotationComponent component);

		// Token: 0x06001B0B RID: 6923
		public abstract void BringToFront(IAnnotationComponent component);

		// Token: 0x06001B0C RID: 6924
		public abstract void SendToBack(IAnnotationComponent component);
	}
}
