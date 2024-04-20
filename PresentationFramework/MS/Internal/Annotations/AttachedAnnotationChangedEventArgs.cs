using System;

namespace MS.Internal.Annotations
{
	// Token: 0x020002BF RID: 703
	internal class AttachedAnnotationChangedEventArgs : EventArgs
	{
		// Token: 0x06001A30 RID: 6704 RVA: 0x00163321 File Offset: 0x00162321
		internal AttachedAnnotationChangedEventArgs(AttachedAnnotationAction action, IAttachedAnnotation attachedAnnotation, object previousAttachedAnchor, AttachmentLevel previousAttachmentLevel)
		{
			Invariant.Assert(attachedAnnotation != null);
			this._action = action;
			this._attachedAnnotation = attachedAnnotation;
			this._previousAttachedAnchor = previousAttachedAnchor;
			this._previousAttachmentLevel = previousAttachmentLevel;
		}

		// Token: 0x170004D2 RID: 1234
		// (get) Token: 0x06001A31 RID: 6705 RVA: 0x0016334F File Offset: 0x0016234F
		public AttachedAnnotationAction Action
		{
			get
			{
				return this._action;
			}
		}

		// Token: 0x170004D3 RID: 1235
		// (get) Token: 0x06001A32 RID: 6706 RVA: 0x00163357 File Offset: 0x00162357
		public IAttachedAnnotation AttachedAnnotation
		{
			get
			{
				return this._attachedAnnotation;
			}
		}

		// Token: 0x170004D4 RID: 1236
		// (get) Token: 0x06001A33 RID: 6707 RVA: 0x0016335F File Offset: 0x0016235F
		public object PreviousAttachedAnchor
		{
			get
			{
				return this._previousAttachedAnchor;
			}
		}

		// Token: 0x170004D5 RID: 1237
		// (get) Token: 0x06001A34 RID: 6708 RVA: 0x00163367 File Offset: 0x00162367
		public AttachmentLevel PreviousAttachmentLevel
		{
			get
			{
				return this._previousAttachmentLevel;
			}
		}

		// Token: 0x06001A35 RID: 6709 RVA: 0x0016336F File Offset: 0x0016236F
		internal static AttachedAnnotationChangedEventArgs Added(IAttachedAnnotation attachedAnnotation)
		{
			Invariant.Assert(attachedAnnotation != null);
			return new AttachedAnnotationChangedEventArgs(AttachedAnnotationAction.Added, attachedAnnotation, null, AttachmentLevel.Unresolved);
		}

		// Token: 0x06001A36 RID: 6710 RVA: 0x00163383 File Offset: 0x00162383
		internal static AttachedAnnotationChangedEventArgs Loaded(IAttachedAnnotation attachedAnnotation)
		{
			Invariant.Assert(attachedAnnotation != null);
			return new AttachedAnnotationChangedEventArgs(AttachedAnnotationAction.Loaded, attachedAnnotation, null, AttachmentLevel.Unresolved);
		}

		// Token: 0x06001A37 RID: 6711 RVA: 0x00163397 File Offset: 0x00162397
		internal static AttachedAnnotationChangedEventArgs Deleted(IAttachedAnnotation attachedAnnotation)
		{
			Invariant.Assert(attachedAnnotation != null);
			return new AttachedAnnotationChangedEventArgs(AttachedAnnotationAction.Deleted, attachedAnnotation, null, AttachmentLevel.Unresolved);
		}

		// Token: 0x06001A38 RID: 6712 RVA: 0x001633AB File Offset: 0x001623AB
		internal static AttachedAnnotationChangedEventArgs Unloaded(IAttachedAnnotation attachedAnnotation)
		{
			Invariant.Assert(attachedAnnotation != null);
			return new AttachedAnnotationChangedEventArgs(AttachedAnnotationAction.Unloaded, attachedAnnotation, null, AttachmentLevel.Unresolved);
		}

		// Token: 0x06001A39 RID: 6713 RVA: 0x001633BF File Offset: 0x001623BF
		internal static AttachedAnnotationChangedEventArgs Modified(IAttachedAnnotation attachedAnnotation, object previousAttachedAnchor, AttachmentLevel previousAttachmentLevel)
		{
			Invariant.Assert(attachedAnnotation != null && previousAttachedAnchor != null);
			return new AttachedAnnotationChangedEventArgs(AttachedAnnotationAction.AnchorModified, attachedAnnotation, previousAttachedAnchor, previousAttachmentLevel);
		}

		// Token: 0x04000DAB RID: 3499
		private AttachedAnnotationAction _action;

		// Token: 0x04000DAC RID: 3500
		private IAttachedAnnotation _attachedAnnotation;

		// Token: 0x04000DAD RID: 3501
		private object _previousAttachedAnchor;

		// Token: 0x04000DAE RID: 3502
		private AttachmentLevel _previousAttachmentLevel;
	}
}
