using System;
using System.Collections;
using System.Windows;
using System.Windows.Media;

namespace MS.Internal.Annotations.Component
{
	// Token: 0x020002CB RID: 715
	internal interface IAnnotationComponent
	{
		// Token: 0x170004F0 RID: 1264
		// (get) Token: 0x06001ACD RID: 6861
		IList AttachedAnnotations { get; }

		// Token: 0x170004F1 RID: 1265
		// (get) Token: 0x06001ACE RID: 6862
		// (set) Token: 0x06001ACF RID: 6863
		PresentationContext PresentationContext { get; set; }

		// Token: 0x170004F2 RID: 1266
		// (get) Token: 0x06001AD0 RID: 6864
		// (set) Token: 0x06001AD1 RID: 6865
		int ZOrder { get; set; }

		// Token: 0x170004F3 RID: 1267
		// (get) Token: 0x06001AD2 RID: 6866
		// (set) Token: 0x06001AD3 RID: 6867
		bool IsDirty { get; set; }

		// Token: 0x06001AD4 RID: 6868
		GeneralTransform GetDesiredTransform(GeneralTransform transform);

		// Token: 0x170004F4 RID: 1268
		// (get) Token: 0x06001AD5 RID: 6869
		UIElement AnnotatedElement { get; }

		// Token: 0x06001AD6 RID: 6870
		void AddAttachedAnnotation(IAttachedAnnotation attachedAnnotation);

		// Token: 0x06001AD7 RID: 6871
		void RemoveAttachedAnnotation(IAttachedAnnotation attachedAnnotation);

		// Token: 0x06001AD8 RID: 6872
		void ModifyAttachedAnnotation(IAttachedAnnotation attachedAnnotation, object previousAttachedAnchor, AttachmentLevel previousAttachmentLevel);
	}
}
