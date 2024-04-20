using System;
using System.ComponentModel;

namespace System.Windows.Annotations
{
	// Token: 0x0200086E RID: 2158
	public sealed class AnnotationResourceChangedEventArgs : EventArgs
	{
		// Token: 0x06007F50 RID: 32592 RVA: 0x0031D8D4 File Offset: 0x0031C8D4
		public AnnotationResourceChangedEventArgs(Annotation annotation, AnnotationAction action, AnnotationResource resource)
		{
			if (annotation == null)
			{
				throw new ArgumentNullException("annotation");
			}
			if (action < AnnotationAction.Added || action > AnnotationAction.Modified)
			{
				throw new InvalidEnumArgumentException("action", (int)action, typeof(AnnotationAction));
			}
			this._annotation = annotation;
			this._resource = resource;
			this._action = action;
		}

		// Token: 0x17001D63 RID: 7523
		// (get) Token: 0x06007F51 RID: 32593 RVA: 0x0031D928 File Offset: 0x0031C928
		public Annotation Annotation
		{
			get
			{
				return this._annotation;
			}
		}

		// Token: 0x17001D64 RID: 7524
		// (get) Token: 0x06007F52 RID: 32594 RVA: 0x0031D930 File Offset: 0x0031C930
		public AnnotationResource Resource
		{
			get
			{
				return this._resource;
			}
		}

		// Token: 0x17001D65 RID: 7525
		// (get) Token: 0x06007F53 RID: 32595 RVA: 0x0031D938 File Offset: 0x0031C938
		public AnnotationAction Action
		{
			get
			{
				return this._action;
			}
		}

		// Token: 0x04003B72 RID: 15218
		private Annotation _annotation;

		// Token: 0x04003B73 RID: 15219
		private AnnotationResource _resource;

		// Token: 0x04003B74 RID: 15220
		private AnnotationAction _action;
	}
}
