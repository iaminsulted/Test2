using System;

namespace System.Windows.Annotations
{
	// Token: 0x02000864 RID: 2148
	public interface IAnchorInfo
	{
		// Token: 0x17001D48 RID: 7496
		// (get) Token: 0x06007ECF RID: 32463
		Annotation Annotation { get; }

		// Token: 0x17001D49 RID: 7497
		// (get) Token: 0x06007ED0 RID: 32464
		AnnotationResource Anchor { get; }

		// Token: 0x17001D4A RID: 7498
		// (get) Token: 0x06007ED1 RID: 32465
		object ResolvedAnchor { get; }
	}
}
