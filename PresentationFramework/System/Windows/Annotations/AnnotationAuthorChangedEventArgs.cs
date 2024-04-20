using System;
using System.ComponentModel;

namespace System.Windows.Annotations
{
	// Token: 0x02000868 RID: 2152
	public sealed class AnnotationAuthorChangedEventArgs : EventArgs
	{
		// Token: 0x06007EF6 RID: 32502 RVA: 0x0031BBCC File Offset: 0x0031ABCC
		public AnnotationAuthorChangedEventArgs(Annotation annotation, AnnotationAction action, object author)
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
			this._author = author;
			this._action = action;
		}

		// Token: 0x17001D53 RID: 7507
		// (get) Token: 0x06007EF7 RID: 32503 RVA: 0x0031BC20 File Offset: 0x0031AC20
		public Annotation Annotation
		{
			get
			{
				return this._annotation;
			}
		}

		// Token: 0x17001D54 RID: 7508
		// (get) Token: 0x06007EF8 RID: 32504 RVA: 0x0031BC28 File Offset: 0x0031AC28
		public object Author
		{
			get
			{
				return this._author;
			}
		}

		// Token: 0x17001D55 RID: 7509
		// (get) Token: 0x06007EF9 RID: 32505 RVA: 0x0031BC30 File Offset: 0x0031AC30
		public AnnotationAction Action
		{
			get
			{
				return this._action;
			}
		}

		// Token: 0x04003B62 RID: 15202
		private Annotation _annotation;

		// Token: 0x04003B63 RID: 15203
		private object _author;

		// Token: 0x04003B64 RID: 15204
		private AnnotationAction _action;
	}
}
