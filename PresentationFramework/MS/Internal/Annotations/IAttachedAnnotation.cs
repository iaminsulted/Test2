using System;
using System.Windows;
using System.Windows.Annotations;
using System.Windows.Annotations.Storage;

namespace MS.Internal.Annotations
{
	// Token: 0x020002BD RID: 701
	internal interface IAttachedAnnotation : IAnchorInfo
	{
		// Token: 0x170004CC RID: 1228
		// (get) Token: 0x06001A29 RID: 6697
		object AttachedAnchor { get; }

		// Token: 0x170004CD RID: 1229
		// (get) Token: 0x06001A2A RID: 6698
		object FullyAttachedAnchor { get; }

		// Token: 0x170004CE RID: 1230
		// (get) Token: 0x06001A2B RID: 6699
		AttachmentLevel AttachmentLevel { get; }

		// Token: 0x170004CF RID: 1231
		// (get) Token: 0x06001A2C RID: 6700
		DependencyObject Parent { get; }

		// Token: 0x170004D0 RID: 1232
		// (get) Token: 0x06001A2D RID: 6701
		Point AnchorPoint { get; }

		// Token: 0x06001A2E RID: 6702
		bool IsAnchorEqual(object o);

		// Token: 0x170004D1 RID: 1233
		// (get) Token: 0x06001A2F RID: 6703
		AnnotationStore Store { get; }
	}
}
