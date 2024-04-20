using System;
using System.Windows;
using System.Windows.Annotations;
using System.Windows.Annotations.Storage;

namespace MS.Internal.Annotations.Anchoring
{
	// Token: 0x020002CF RID: 719
	internal class AttachedAnnotation : IAttachedAnnotation, IAnchorInfo
	{
		// Token: 0x06001B0E RID: 6926 RVA: 0x00166B13 File Offset: 0x00165B13
		internal AttachedAnnotation(LocatorManager manager, Annotation annotation, AnnotationResource anchor, object attachedAnchor, AttachmentLevel attachmentLevel) : this(manager, annotation, anchor, attachedAnchor, attachmentLevel, null)
		{
		}

		// Token: 0x06001B0F RID: 6927 RVA: 0x00166B23 File Offset: 0x00165B23
		internal AttachedAnnotation(LocatorManager manager, Annotation annotation, AnnotationResource anchor, object attachedAnchor, AttachmentLevel attachmentLevel, DependencyObject parent)
		{
			this._annotation = annotation;
			this._anchor = anchor;
			this._locatorManager = manager;
			this.Update(attachedAnchor, attachmentLevel, parent);
		}

		// Token: 0x06001B10 RID: 6928 RVA: 0x00105F35 File Offset: 0x00104F35
		public bool IsAnchorEqual(object o)
		{
			return false;
		}

		// Token: 0x17000505 RID: 1285
		// (get) Token: 0x06001B11 RID: 6929 RVA: 0x00166B4C File Offset: 0x00165B4C
		public Annotation Annotation
		{
			get
			{
				return this._annotation;
			}
		}

		// Token: 0x17000506 RID: 1286
		// (get) Token: 0x06001B12 RID: 6930 RVA: 0x00166B54 File Offset: 0x00165B54
		public AnnotationResource Anchor
		{
			get
			{
				return this._anchor;
			}
		}

		// Token: 0x17000507 RID: 1287
		// (get) Token: 0x06001B13 RID: 6931 RVA: 0x00166B5C File Offset: 0x00165B5C
		public object AttachedAnchor
		{
			get
			{
				return this._attachedAnchor;
			}
		}

		// Token: 0x17000508 RID: 1288
		// (get) Token: 0x06001B14 RID: 6932 RVA: 0x00166B64 File Offset: 0x00165B64
		public object ResolvedAnchor
		{
			get
			{
				return this.FullyAttachedAnchor;
			}
		}

		// Token: 0x17000509 RID: 1289
		// (get) Token: 0x06001B15 RID: 6933 RVA: 0x00166B6C File Offset: 0x00165B6C
		public object FullyAttachedAnchor
		{
			get
			{
				if (this._attachmentLevel == AttachmentLevel.Full)
				{
					return this._attachedAnchor;
				}
				return this._fullyAttachedAnchor;
			}
		}

		// Token: 0x1700050A RID: 1290
		// (get) Token: 0x06001B16 RID: 6934 RVA: 0x00166B84 File Offset: 0x00165B84
		public AttachmentLevel AttachmentLevel
		{
			get
			{
				return this._attachmentLevel;
			}
		}

		// Token: 0x1700050B RID: 1291
		// (get) Token: 0x06001B17 RID: 6935 RVA: 0x00166B8C File Offset: 0x00165B8C
		public DependencyObject Parent
		{
			get
			{
				return this._parent;
			}
		}

		// Token: 0x1700050C RID: 1292
		// (get) Token: 0x06001B18 RID: 6936 RVA: 0x00166B94 File Offset: 0x00165B94
		public Point AnchorPoint
		{
			get
			{
				Point anchorPoint = this._selectionProcessor.GetAnchorPoint(this._attachedAnchor);
				if (!double.IsInfinity(anchorPoint.X) && !double.IsInfinity(anchorPoint.Y))
				{
					this._cachedPoint = anchorPoint;
				}
				return this._cachedPoint;
			}
		}

		// Token: 0x1700050D RID: 1293
		// (get) Token: 0x06001B19 RID: 6937 RVA: 0x00166BDC File Offset: 0x00165BDC
		public AnnotationStore Store
		{
			get
			{
				return this.GetStore();
			}
		}

		// Token: 0x06001B1A RID: 6938 RVA: 0x00166BE4 File Offset: 0x00165BE4
		internal void Update(object attachedAnchor, AttachmentLevel attachmentLevel, DependencyObject parent)
		{
			this._attachedAnchor = attachedAnchor;
			this._attachmentLevel = attachmentLevel;
			this._selectionProcessor = this._locatorManager.GetSelectionProcessor(attachedAnchor.GetType());
			if (parent != null)
			{
				this._parent = parent;
				return;
			}
			this._parent = this._selectionProcessor.GetParent(this._attachedAnchor);
		}

		// Token: 0x06001B1B RID: 6939 RVA: 0x00166C38 File Offset: 0x00165C38
		internal void SetFullyAttachedAnchor(object fullyAttachedAnchor)
		{
			this._fullyAttachedAnchor = fullyAttachedAnchor;
		}

		// Token: 0x06001B1C RID: 6940 RVA: 0x00166C44 File Offset: 0x00165C44
		private AnnotationStore GetStore()
		{
			if (this.Parent != null)
			{
				AnnotationService service = AnnotationService.GetService(this.Parent);
				if (service != null)
				{
					return service.Store;
				}
			}
			return null;
		}

		// Token: 0x04000DEF RID: 3567
		private Annotation _annotation;

		// Token: 0x04000DF0 RID: 3568
		private AnnotationResource _anchor;

		// Token: 0x04000DF1 RID: 3569
		private object _attachedAnchor;

		// Token: 0x04000DF2 RID: 3570
		private object _fullyAttachedAnchor;

		// Token: 0x04000DF3 RID: 3571
		private AttachmentLevel _attachmentLevel;

		// Token: 0x04000DF4 RID: 3572
		private DependencyObject _parent;

		// Token: 0x04000DF5 RID: 3573
		private SelectionProcessor _selectionProcessor;

		// Token: 0x04000DF6 RID: 3574
		private LocatorManager _locatorManager;

		// Token: 0x04000DF7 RID: 3575
		private Point _cachedPoint;
	}
}
