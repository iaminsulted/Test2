using System;

namespace System.Windows.Annotations.Storage
{
	// Token: 0x02000877 RID: 2167
	public class StoreContentChangedEventArgs : EventArgs
	{
		// Token: 0x06007FDA RID: 32730 RVA: 0x003208D4 File Offset: 0x0031F8D4
		public StoreContentChangedEventArgs(StoreContentAction action, Annotation annotation)
		{
			if (annotation == null)
			{
				throw new ArgumentNullException("annotation");
			}
			this._action = action;
			this._annotation = annotation;
		}

		// Token: 0x17001D73 RID: 7539
		// (get) Token: 0x06007FDB RID: 32731 RVA: 0x003208F8 File Offset: 0x0031F8F8
		public Annotation Annotation
		{
			get
			{
				return this._annotation;
			}
		}

		// Token: 0x17001D74 RID: 7540
		// (get) Token: 0x06007FDC RID: 32732 RVA: 0x00320900 File Offset: 0x0031F900
		public StoreContentAction Action
		{
			get
			{
				return this._action;
			}
		}

		// Token: 0x04003B9C RID: 15260
		private StoreContentAction _action;

		// Token: 0x04003B9D RID: 15261
		private Annotation _annotation;
	}
}
